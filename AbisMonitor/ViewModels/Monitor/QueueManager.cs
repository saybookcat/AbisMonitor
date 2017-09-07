using System.Threading;
using AbisMonitor.Domain;

namespace AbisMonitor.UI.ViewModels.Monitor
{
    public class QueueManager : IMonitorManager
    {
        private static QueueManager _queueManager;
        private static readonly object QueueManagerLockRoot = new object();
        public QueueViewModel<AbisMonitor.Domain.Monitor> MonitorQueue { get; set; }
        public QueueViewModel<UserTrack> UserTrackQueue { get; set; }

        public QueueViewModel<AbisMonitor.Domain.Monitor> MonitorOtherQueue { get; set; }

        private QueueManager()
        {
            MonitorQueue = new QueueViewModel<Domain.Monitor>();
            UserTrackQueue = new QueueViewModel<UserTrack>();
            MonitorOtherQueue = new QueueViewModel<AbisMonitor.Domain.Monitor>();
        }

        public static QueueManager Instance
        {
            get
            {
                if (_queueManager == null)
                {
                    lock (QueueManagerLockRoot)
                    {
                        if (_queueManager == null)
                        {
                            _queueManager = new QueueManager();
                        }
                    }
                }
                return _queueManager;
            }
        }

        public void Start()
        {
            
        }

        public void Pause()
        {
            
        }

        public void Stop()
        {
            
        }

        public void AddMonitor(Domain.Monitor monitor)
        {
            MonitorQueue.Add(monitor);
        }

        public void AddUserTrack(UserTrack usertrack)
        {
            UserTrackQueue.Add(usertrack);
        }

        public void AddMonitorOther(Domain.Monitor monitor)
        {
            MonitorOtherQueue.Add(monitor);
        }

    }
}
