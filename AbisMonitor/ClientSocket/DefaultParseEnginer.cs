using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Helper;
using Framework;

namespace AbisMonitor.UI.ClientSocket
{
    public class DefaultParseEnginer:IParseEngine
    {
        private static readonly object RootLock = new object();
        private readonly BufferQueue _bufferQueue;
        private readonly Framework.Worker _worker;

        private byte[] _headBytes = {0xFF, 0x7E};

        public DefaultParseEnginer()
        {
            _bufferQueue = new BufferQueue();
            _worker = new Framework.Worker(DoWork,false);
        }

        private void DoWork()
        {
            if (_bufferQueue.Length <= 5) Pause();
            int startIndex = ByteUtil.IndexOf(_bufferQueue.Data, _headBytes);
            if (startIndex > 0)
            {
                var removeBuffers = ByteUtil.InterceptByteArray(_bufferQueue.Data, 0, startIndex);
                Log.Info("\nSingle header error,startIndex : " + startIndex);
                Log.Info(string.Format("Buffer Queue[{0}]:{1}", _bufferQueue.Data.Length,
                                   DataContentHelper.DataContentConvert(_bufferQueue.Data)));
                Log.Info(string.Format("Remove buffers[{0}]:{1}", removeBuffers.Length,
                    DataContentHelper.DataContentConvert(removeBuffers)));
                _bufferQueue.Remove(0, startIndex);
                Log.Info(string.Format("Remove buffers after[{0}]:{1}\n", _bufferQueue.Length,
                    DataContentHelper.DataContentConvert(_bufferQueue.Data)));
                return;
            }
            if (startIndex == -1)
            {
                Pause();
                return;
            }

            byte[] lengthBytes = ByteUtil.InterceptByteArray(_bufferQueue.Data, 2, 2);
            int singleLength = BitConverter.ToUInt16(lengthBytes, 0);
            if (_bufferQueue.Length < singleLength)
            {
                Pause();
                return;
            }

            //信令长度足够时
            int index = 4;
            byte type = _bufferQueue.Data[index];
            var buffers = _bufferQueue.DequeueBytes(singleLength);
            if (buffers == null || buffers.Length==0) return;

            var parser = new ParseFactory().CreateParser(type);
            if (parser != null)
            {
                try
                {
                    bool isSuccess = parser.TryParse(buffers);
                    if (isSuccess)
                    {
                        Log.Info(string.Format("Type:{0} parse {1}", type, "failed"));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(DataContentHelper.DataContentConvert(buffers));
                    Log.Error(parser.ToString());
                    Log.Error(ex);
                }
            }
        }

        public void Start()
        {
            lock (RootLock)
            {
                _worker.Start();
            }
        }

        public void Stop()
        {
            lock (RootLock)
            {
                _worker.Stop();
            }
        }

        public void Pause()
        {
            lock (RootLock)
            {
                _worker.Pause();
            }
        }

        public void Resume()
        {
            lock (RootLock)
            {
                _worker.Resume();
            }
        }

        public void Append(byte[] buffer)
        {
            if (State == WorkerState.Stop) return;
            lock (RootLock)
            {
                if (State == WorkerState.Stop) return;
                _bufferQueue.Add(buffer);
                _worker.Resume();
            }
        }

        public int Count
        {
            get
            {
                if (_bufferQueue == null) return 0;
                return _bufferQueue.Length;
            }
        }

        public WorkerState State
        {
            get { return _worker.State; }
        }
    }
}
