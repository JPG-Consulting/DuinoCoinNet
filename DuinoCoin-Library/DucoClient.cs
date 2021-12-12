using System;
using System.Threading;

namespace DuinoCoin
{
    public class DucoClient
    {
        private DucoMinerSettings _settings = null;

        private DucoMiner[] _miners = null;

        public event EventHandler<ConnectedEventArgs> Connected;

        public event EventHandler<JobCompletedEventArgs> JobCompleted;

        public event EventHandler<JobEventArgs> JobReceived;

        public event EventHandler<NodeMessageEventArgs> NodeMessageReceived;

        public event EventHandler<PoolInformationRetrievedEventArgs> PoolInformationRetrieved;

        public string DucoUser { get; set; }

        public DucoMinerSettings Settings 
        { 
            get { return this._settings; } 
            set { this._settings = value; }
        }

        public Version ProtocolVersion
        { 
            get { return new Version(2, 7); }
        }


        private void DucoClient_Connected(object sender, ConnectedEventArgs e)
        {
            OnConnected(e);
        }

        private void DucoClient_JobCompleted(object sender, JobCompletedEventArgs e)
        {
            OnJobCompleted(e);
        }

        private void DucoClient_JobReceived(object sender, JobEventArgs e)
        {
            OnJobReceived(e);
        }

        private void DucoClient_NodeMessageReceived(object sender, NodeMessageEventArgs e)
        {
            OnNodeMessageReceived(e);
        }
        private void DucoClient_PoolInformationRetrieved(object sender, PoolInformationRetrievedEventArgs e)
        {
            OnPoolInformationRetrieved(e);
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

        public void StartMining()
        {
            StartMining(1);
        }

        public void StartMining(int numThreads)
        {
            numThreads = 1;

            Array.Resize(ref this._miners, numThreads);

            for (int i = 0; i < numThreads; i++)
            {
                this._miners[i] = new DucoMiner(String.Concat("cpu", i.ToString()));
                this._miners[i].DucoUser = this.Settings.Username;

                this._miners[i].PoolInformationRetrieved += DucoClient_PoolInformationRetrieved;
                this._miners[i].Connected += DucoClient_Connected;
                this._miners[i].JobReceived += DucoClient_JobReceived;
                this._miners[i].JobCompleted += DucoClient_JobCompleted;
                this._miners[i].NodeMessageReceived += DucoClient_NodeMessageReceived;
            }

            //for (int i = 0; i < this._miners.Length; i++)
            //{
            //    ThreadStart threadStart = new ThreadStart(this._miners[i].StartMining);
            //    Thread thread = new Thread(threadStart);

            //    thread.IsBackground = true;

            //    thread.Start();

            //    thread.Join()
            //}

            this._miners[0].StartMining();
        }

        
    }
}
