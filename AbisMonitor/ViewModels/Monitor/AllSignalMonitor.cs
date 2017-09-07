using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.UI.ViewModels.Monitor
{
    public class AllSignalMonitor:IMonitorManager
    {
        private readonly System.Collections.Concurrent.ConcurrentQueue<Models.MonitorModel> _queue =
            new System.Collections.Concurrent.ConcurrentQueue<Models.MonitorModel>();

        private static AllSignalMonitor _allSignalMonitor;
        private static readonly object LockObject = new object();
        private Framework.Worker _worker;

        private AllSignalMonitor()
        {
            _worker = new Framework.Worker(DoWork);
        }

        public static AllSignalMonitor Instance
        {
            get
            {
                if (_allSignalMonitor == null)
                {
                    lock (LockObject)
                    {
                        if (_allSignalMonitor == null)
                        {
                            _allSignalMonitor = new AllSignalMonitor();
                        }
                    }
                }
                return _allSignalMonitor;
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public int Count { get; set; }
        public MonitorState State { get; set; }

        private void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
