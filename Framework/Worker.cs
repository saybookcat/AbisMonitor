using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    public class Worker : IDisposable
    {
        private static readonly object LockObject = new object();
        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(false);
        Thread _thread;

        public Worker(Action worker, bool once = false)
        {
            _thread = new Thread(() => DoWork(worker, once))
            {
                IsBackground = true
            };
            State = WorkerState.Cteated;
        }

        public WorkerState State { get; set; }

        public void Start()
        {
            if (State == WorkerState.Running) return;
            lock (LockObject)
            {
                if (State == WorkerState.Cteated)
                {
                    _thread.Start();
                    State = WorkerState.Running;
                }
            }
        }

        public void Pause()
        {
            if (State == WorkerState.Paused) return;
            lock (LockObject)
            {
                if (State == WorkerState.Running)
                {
                    _pauseEvent.Reset();
                    State = WorkerState.Paused;
                }
            }
        }

        public void Resume()
        {
            if (State == WorkerState.Running) return;
            lock (LockObject)
            {
                if (State == WorkerState.Paused)
                {
                    _pauseEvent.Set();
                    State = WorkerState.Running;
                }
            }
        }

        public void Stop()
        {
            if (State == WorkerState.Stop) return;
            lock (LockObject)
            {
                if (State != WorkerState.Stop)
                {
                    _shutdownEvent.Set();
                    _thread.Join();
                    Dispose();
                    State = WorkerState.Stop;
                }
            }
        }

        private void DoWork(Action worker,bool once)
        {
            while (true)
            {
                _pauseEvent.WaitOne(Timeout.Infinite);
                if (_shutdownEvent.WaitOne(0))
                    continue;
                if (worker != null)
                {
                    worker.Invoke();
                }

                if (once)
                {
                    Stop();
                    break;
                }
            }
        }

        public void Dispose()
        {
            if (_pauseEvent != null)
            {
                _pauseEvent.Close();
                _pauseEvent = null;
            }
            if (_shutdownEvent != null)
            {
                _shutdownEvent.Close();
                _shutdownEvent = null;
            }
        }

    }

    public enum WorkerState
    {
        Cteated,
        Running,
        Paused,
        Stop
    }
}
