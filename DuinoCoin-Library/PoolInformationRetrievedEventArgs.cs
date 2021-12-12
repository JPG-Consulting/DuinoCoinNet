namespace DuinoCoin
{
    public class PoolInformationRetrievedEventArgs : DucoEventArgs
    {
        public DucoPoolInformation PoolInformation { get; private set; }

        internal PoolInformationRetrievedEventArgs(string source, DucoPoolInformation poolInformation)
            : base(source)
        {
            this.PoolInformation = poolInformation;
        }
    }
}
