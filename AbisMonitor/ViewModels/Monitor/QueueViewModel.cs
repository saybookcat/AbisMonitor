using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using AbisMonitor.UI.Models;
using GalaSoft.MvvmLight.Threading;

namespace AbisMonitor.UI.ViewModels.Monitor
{
    public class QueueViewModel<T> where T:class,new()
    {
        private readonly System.Collections.Concurrent.ConcurrentQueue<T> _cacheMonitorQueue = new System.Collections.Concurrent.ConcurrentQueue<T>();

        private readonly AutoResetEvent _cacheQueueEvent;

        private static object CacheLock = new object();

        public ObservableCollection<T> MonitorList { get; set; }

        public QueueViewModel()
        {
            _cacheQueueEvent = new AutoResetEvent(false);
            Thread cacheQueueThread = new Thread(CacheToMonitorListAction) {IsBackground = true};
            cacheQueueThread.Start();
        }



        public void Add(T model)
        {
            lock (CacheLock)
            {
                _cacheMonitorQueue.Enqueue(model);
                _cacheQueueEvent.Set();
            }
        }

        private void CacheToMonitorListAction()
        {
            while (true)
            {
                lock (CacheLock)
                {
                    if (_cacheMonitorQueue.Count == 0)
                    {
                        _cacheQueueEvent.WaitOne();
                        continue;
                    }
                    T model;
                    var isSuccess = _cacheMonitorQueue.TryDequeue(out model);
                    if (!isSuccess) continue;
                    AddToMonitorList(model);
                }
                Thread.Sleep(1);
            }
        }

        private void AddToMonitorList(T model)
        {
            if (MonitorList == null)
            {
                MonitorList = new ObservableCollection<T>();
            }
            MonitorList.Add(model);
        }

    }
}
