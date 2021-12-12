using System;
using System.Collections.Generic;
using System.Text;

namespace DuinoCoin
{
    public class DucoHash
    {
        private readonly string _hexString;

        public static implicit operator byte[](DucoHash d) => d.Value;

        public static implicit operator string(DucoHash d) => d._hexString;

        public byte[] Value { get; private set; }

        public override string ToString()
        {
            return this._hexString;
        }

        internal DucoHash(string hexString)
        {
            this._hexString = hexString;

            // Transform the hash to byte array.
            int len = hexString.Length / 2;

            this.Value = new byte[len];

            for (int i = 0; i < len; i++)
            {
                string temp = hexString.Substring(i * 2, 2);
                this.Value[i] = Convert.ToByte(temp, 16);
            }
        }
    }
}
