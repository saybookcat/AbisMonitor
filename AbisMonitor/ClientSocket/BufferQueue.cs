using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Helper;

namespace AbisMonitor.UI.ClientSocket
{
    public class BufferQueue
    {
        private static readonly object LockObject = new object();
        private byte[] _data = new byte[0];

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public int Length
        {
            get { return Data.Length; }
        }

        /// <summary>
        /// 从头部截取指定长度，并从缓存byte[]集合中移除
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] DequeueBytes(int length)
        {
            lock (LockObject)
            {
                byte[] lastBuffer;
                byte[] tmpBuffer = new byte[0];
                if (Data.Length == 0)
                {
                    lastBuffer = Data;
                    Monitor.Wait(LockObject);
                }
                else
                {
                    tmpBuffer = ByteUtil.InterceptByteArray(_data, 0, length);

                    lastBuffer = ByteUtil.InterceptByteArray(Data, length, _data.Length - length);

                    Monitor.PulseAll(LockObject);
                }
                Data = lastBuffer;
                return tmpBuffer;
            }
        }


        public void Add(byte[] buffers)
        {
            lock (LockObject)
            {
                if (buffers == null || buffers.Length == 0) return;
                Data = ByteUtil.AppendByteArray(Data, buffers);
                Monitor.PulseAll(LockObject);
            }
        }

        public void Remove(int index, int bufferLength)
        {
            lock (LockObject)
            {
                if (bufferLength == 0) return;
                if (Length < index + bufferLength) return;
                byte[] beforeBytes = ByteUtil.InterceptByteArray(Data, 0, index);
                byte[] afterBytes = ByteUtil.InterceptByteArray(Data, index + bufferLength, Length - index - bufferLength);
                Data = ByteUtil.AppendByteArray(beforeBytes, afterBytes);
                Monitor.PulseAll(LockObject);
            }
        }
    }
}
