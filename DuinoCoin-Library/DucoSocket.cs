using System;
using System.Net.Sockets;
using System.Text;

namespace DuinoCoin
{
    /// <summary>
    /// Represents a socket to interact with the Duco-Coin server.
    /// </summary>
    internal class DucoSocket : IDisposable
    {
        private bool _disposedValue;
        private Encoding _encoding = new UTF8Encoding(false);
        private int _receiveBufferSize = 1024;
        private readonly Socket _socket;

        internal bool Connected { get { return ((this._socket != null) && (this._socket.Connected)); } }

        internal void Connect(string host, int port)
        {
            this._socket.Connect(host, port);
        }

        internal void Disconnect(bool reuseSocket)
        {
            this._socket.Disconnect(reuseSocket);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this._socket.Dispose();
                }

                this._disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        internal void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        
        internal string ReceiveString()
        {
            int byteCount = 0;
            byte[] buffer = new byte[this._receiveBufferSize];

            byteCount = this._socket.Receive(buffer, this._receiveBufferSize, SocketFlags.None);

            if (byteCount == 0)
            {
                System.Threading.Thread.Sleep(50);

                if (this._socket.Available > 0)
                    byteCount = this._socket.Receive(buffer, this._receiveBufferSize, SocketFlags.None);
            }

            // ToDo: if the buffer is too small read more.

            return this._encoding.GetString(buffer, 0, byteCount);
        }

        internal int SendString(string data)
        {
            int byteCount = 0;

            byte[] buffer = Encoding.Convert(Encoding.Default, this._encoding, Encoding.Default.GetBytes(data));
            
            byteCount = this._socket.Send(buffer, SocketFlags.None);

            return byteCount;
        }

        internal DucoSocket()
        {
            this._socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        }        
    }
}
