using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuinoCoin
{
    public class DucoJob
    {
        internal byte[] HashForJob;

        public DucoHash LastBlockHash { get; private set; }

        public DucoHash ExpectedHash { get; private set; }

        public uint Difficulty { get; private set; }

        

        internal DucoJob(string data)
        {
            string[] arrData = data.Split(new char[] { ',' });

            this.LastBlockHash = new DucoHash(arrData[0]);
            this.ExpectedHash = new DucoHash(arrData[1]);
            this.Difficulty = Convert.ToUInt32(arrData[2]);

            this.HashForJob = System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.ASCII, System.Text.Encoding.Default.GetBytes(arrData[0]));
        }
    }
}
