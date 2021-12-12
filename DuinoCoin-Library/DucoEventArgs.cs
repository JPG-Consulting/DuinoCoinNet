using System;

namespace DuinoCoin
{
    public class DucoEventArgs : EventArgs
    {
        public string Source { get; private set; }

        internal DucoEventArgs(string source)
        {
            this.Source = source;
        }
    }
}
