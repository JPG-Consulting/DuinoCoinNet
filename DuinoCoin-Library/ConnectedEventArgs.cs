using System;
using System.Collections.Generic;
using System.Text;

namespace DuinoCoin
{
    public class ConnectedEventArgs : DucoEventArgs
    {
        public DucoPoolInformation PoolInformation { get; private set; }

        public string ServerVersion { get; private set; }

        internal ConnectedEventArgs(string source, DucoPoolInformation poolInformation, string serverVersion)
            : base(source)
        {
            this.PoolInformation = poolInformation;
            this.ServerVersion = serverVersion;
        }
    }
}
