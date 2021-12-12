using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DuinoCoin
{
    internal class Hasher
    {
        internal struct TaskArgs
        {
            internal uint Start;

            internal uint End;

            internal byte[] LastBlockHash;

            internal byte[] ExpectedHash;
        }

        private UInt32 _ducoResult = 0;
        private bool _ducoResultFound = false;

        private System.Text.Encoding _encoding = new System.Text.UTF8Encoding(false);

        internal UInt32 ComputedHashes = 0;

        internal DucoJob Job { get; private set; }

        /// <summary>
        /// Checks if the hashes are the same.
        /// </summary>
        /// <param name="hash">Has to test.</param>
        /// <param name="expected">Expected hash.</param>
        /// <returns>Return <c>true</c> if the hashes are the same; else <c>false</c>.</returns>
        private bool CheckHash(byte[] hash, byte[] expected)
        {
            if (((hash == null) || (expected == null)) || (!hash.Length.Equals(expected.Length)))
                return false;

            for(int i=0; i<hash.Length; i++)
            {
                if (!hash[i].Equals(expected[i]))
                    return false;
            }

            return true;
        }

        private void RunTask(object state)
        {
            if (state == null)
                return;

            TaskArgs args = (TaskArgs)state;

            int lastBlockHashLength = args.LastBlockHash.Length;
            byte[] buffer = new byte[lastBlockHashLength + 1];
            Array.Copy(args.LastBlockHash, 0, buffer, 0, lastBlockHashLength);
            
            using (SHA1 sha1 = SHA1.Create())
            {
                for (uint ducoResult = args.Start; ducoResult <= args.End; ducoResult++)
                {
                    this.ComputedHashes++;

                    if (this._ducoResultFound)
                        break;

                    byte[] ducoResultBuffer = System.Text.Encoding.Convert(System.Text.Encoding.Default, this._encoding, System.Text.Encoding.Default.GetBytes(ducoResult.ToString()));

                    if (buffer.Length < lastBlockHashLength + ducoResultBuffer.Length)
                        Array.Resize(ref buffer, lastBlockHashLength + ducoResultBuffer.Length);

                    Array.Copy(ducoResultBuffer, 0, buffer, lastBlockHashLength, ducoResultBuffer.Length);

                    byte[] hash = sha1.ComputeHash(buffer);

                    // Check if hash is OK.
                    if (CheckHash(hash, args.ExpectedHash))
                    {
                        this._ducoResult = ducoResult;
                        this._ducoResultFound = true;
                    }
                }
            }
        }

        internal bool Run(out uint result)
        {
            // Two tasks
            int numTasks = 4;

            this._ducoResultFound = false;
            this._ducoResult = 0;
            this.ComputedHashes = 0;

            Task[] tasks = new Task[numTasks];

            uint maxHashes = this.Job.Difficulty * 100 + 1;

            uint taskLength = Convert.ToUInt32(Math.Ceiling(Convert.ToDecimal(maxHashes) / Convert.ToDecimal(numTasks)));

            uint startPosition = 0;

            for (int i = 0; i < numTasks; i++)
            {
                uint maxPosition = startPosition + taskLength;
                if (maxPosition > maxHashes)
                    maxPosition = maxHashes;

                if (maxPosition > maxHashes)
                    continue;

                if ((i == numTasks - 1) && (maxPosition < maxHashes))
                    maxPosition = maxHashes;

                TaskArgs arg = new TaskArgs() {
                    Start = startPosition,
                    End = maxPosition,
                    LastBlockHash = this.Job.HashForJob,
                    ExpectedHash = this.Job.ExpectedHash
                };

                Action<object> action = new Action<object>(RunTask);

                tasks[i] = new Task(action, (object)arg);
                tasks[i].Start();

                startPosition = maxPosition;
            }

            Task.WaitAll(tasks);

            result = this._ducoResult;

            return this._ducoResultFound;
        }

        internal Hasher(DucoJob job)
        {
            this.Job = job;
        }

    }
}
