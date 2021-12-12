namespace DuinoCoin
{
    public class JobEventArgs : DucoEventArgs
    {
        public DucoJob Job { get; private set; }

        public JobEventArgs(string source, DucoJob job)
            : base(source)
        {
            this.Job = job;
        }
    }
}
