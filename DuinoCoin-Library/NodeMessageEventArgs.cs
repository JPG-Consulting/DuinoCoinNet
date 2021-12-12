namespace DuinoCoin
{
    public class NodeMessageEventArgs : DucoEventArgs
    {
        public string Message { get; private set; }

        internal NodeMessageEventArgs(string source, string message)
            : base(source)
        {
            this.Message = (message != null) ? message : string.Empty;
        }
    }
}
