using System;

namespace DuinoCoin
{
    public class JobCompletedEventArgs : JobEventArgs
    {
        public TimeSpan Ellapsed { get; private set; }

        public double HashRate { get; private set; }

        public string Response { get; private set; }

        public DucoJobStatistics JobStatistics { get; private set; }

        internal JobCompletedEventArgs(string source, DucoJob job, TimeSpan ellapsed, double hashRate, string response, DucoJobStatistics jobStatistics)
            : base(source, job)
        {
            this.Ellapsed = ellapsed;
            this.HashRate = hashRate;
            this.Response = response;
            this.JobStatistics = jobStatistics;
        }
    }
}
