using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace DuinoCoin
{
    internal class DucoMiner
    {
        private DucoSocket _socket = null;

        private readonly DucoJobStatistics _statistics = new DucoJobStatistics();

        internal string DucoUser { get; set; }

        internal string Dificulty { get; set; }

        internal string Identifier { get; set; }

        
        internal DucoPoolInformation PoolInformation { get; private set; }

        internal string ServerVersion { get; private set; }

        
        internal event EventHandler<ConnectedEventArgs> Connected;
        internal event EventHandler<JobCompletedEventArgs> JobCompleted;
        internal event EventHandler<JobEventArgs> JobReceived;
        internal event EventHandler<NodeMessageEventArgs> NodeMessageReceived;
        internal event EventHandler<PoolInformationRetrievedEventArgs> PoolInformationRetrieved;

        internal void Connect()
        {
            int retries = 0;
            int connectionRetries = 0;
            bool successfulConnection = false;

            this._statistics.Reset();

            while (!successfulConnection && (retries < 10))
            {
                this.PoolInformation = DucoPoolInformation.GetPool();

                if (this.PoolInformation != null)
                    OnPoolInformationRetrieved(new PoolInformationRetrievedEventArgs(this.Identifier, this.PoolInformation));

                this._socket = new DucoSocket();

                while (!successfulConnection && (connectionRetries < 3))
                {
                    try
                    {
                        if (!this._socket.Connected)
                            this._socket.Connect(this.PoolInformation.Ip, this.PoolInformation.Port);

                        // We receive the version.
                        this.ServerVersion = this._socket.ReceiveString();

                        successfulConnection = true;
                    }
                    catch
                    {
                        successfulConnection = false;
                        connectionRetries++;

                        if (this._socket.Connected)
                            this._socket.Disconnect(true);

                        Thread.Sleep(1000);
                    }
                }

                if (!successfulConnection)
                {
                    connectionRetries = 0;
                    retries++;

                    if (this._socket.Connected)
                        this._socket.Disconnect(false);

                    this._socket.Dispose();

                    Thread.Sleep(500);
                }
            }
            

            OnConnected(new ConnectedEventArgs(this.Identifier, this.PoolInformation, this.ServerVersion));
        }

        internal DucoJob GetJob()
        {
            DucoJob job = null;

            string request;

            if (!String.IsNullOrWhiteSpace(this.Dificulty))
                request = string.Format("JOB,{0},{1}", this.DucoUser, this.Dificulty);
            else
                request = string.Format("JOB,{0}", this.DucoUser);

            this._socket.SendString(request);
            
            string responseString = this._socket.ReceiveString();

            if (!String.IsNullOrEmpty(responseString))
            {
                job = new DucoJob(responseString);

                if (job != null)
                    OnJobReceived(new JobEventArgs(this.Identifier, job));
            }

            return job;
        }


        private void Hash(DucoJob job)
        {
            DateTime startTime = DateTime.UtcNow;

            Hasher hasher = new Hasher(job);

            uint ducoResult;

            if (hasher.Run(out ducoResult))
            {
                TimeSpan ellapsed = DateTime.UtcNow.Subtract(startTime);

                double hashRate = hasher.ComputedHashes / ellapsed.TotalSeconds;

                String resultResponse = SendResponse(job, ducoResult, hashRate);

                switch (resultResponse.Trim().ToUpperInvariant())
                {
                    case "BAD":
                        this._statistics.Bad++;
                        break;
                    case "BLOCK":
                        this._statistics.Block++;
                        break;
                    case "GOOD":
                        this._statistics.Good++;
                        break;
                    default:
                        OnNodeMessageReceived(new NodeMessageEventArgs(this.Identifier, resultResponse));
                        return;
                }

                OnJobCompleted(new JobCompletedEventArgs(this.Identifier, job, ellapsed, hashRate, resultResponse, this._statistics.Clone()));
            }
        }

        private string SendResponse(DucoJob job, uint response, double hashRate)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(response.ToString());
            sb.Append(",");
            sb.Append(hashRate.ToString(System.Globalization.CultureInfo.InvariantCulture));
            sb.Append(",");
            sb.Append("NET Miner v1.0.0");
            sb.Append(",");
            sb.Append("Net Miner");
            //sb.Append(",");
            //sb.Append("ID");

            this._socket.SendString(sb.ToString());

            return this._socket.ReceiveString();
        }

        internal void StartMining()
        {
            while (1 == 1)
            {
                this.Connect();

                while (this._socket.Connected)
                {
                    try
                    {
                        DucoJob job = GetJob();

                        if (job != null)
                            Hash(job);
                    }
                    catch
                    {
                        if (this._socket.Connected)
                            this._socket.Disconnect(false);

                        this.Connect();
                    }
                }
            }
        }

        protected virtual void OnConnected(ConnectedEventArgs e)
        {
            EventHandler<ConnectedEventArgs> handler = Connected;
            if (handler != null)
                handler.Invoke(this, e);
        }

        protected virtual void OnJobCompleted(JobCompletedEventArgs e)
        {
            EventHandler<JobCompletedEventArgs> handler = JobCompleted;
            if (handler != null)
                handler.Invoke(this, e);
        }

        protected virtual void OnJobReceived(JobEventArgs e)
        {
            EventHandler<JobEventArgs> handler = JobReceived;
            if (handler != null)
                handler.Invoke(this, e);
        }

        protected virtual void OnNodeMessageReceived(NodeMessageEventArgs e)
        {
            EventHandler<NodeMessageEventArgs> handler = NodeMessageReceived;
            if (handler != null)
                handler.Invoke(this, e);
        }

        protected virtual void OnPoolInformationRetrieved(PoolInformationRetrievedEventArgs e)
        {
            EventHandler<PoolInformationRetrievedEventArgs> handler = PoolInformationRetrieved;
            if (handler != null)
                handler.Invoke(this, e);
        }

        internal DucoMiner(string identifier)
        {
            this.Identifier = identifier;
            this.Dificulty = "MEDIUM";
        }
    }
}
