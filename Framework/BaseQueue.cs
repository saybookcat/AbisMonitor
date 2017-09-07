using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Framework
{
    public class BaseQueue<T> where T : class
    {
        private static object rootLock = new object();
        private readonly Queue<T> _queue = new Queue<T>();

        public BaseQueue(int maxLength)
        {
            MaxLength = maxLength;
        }

        /// <summary>
        ///     队列大小
        /// </summary>
        public int MaxLength { get; private set; }

        public int Count
        {
            get
            {
                lock (rootLock)
                {
                    return _queue.Count;
                }
            }
        }

        /// <summary>
        ///     返回第一项，并不移除。
        ///     当不存在项时返回Null，且停止队列。
        ///     当正常取出第一项时，唤醒所有操作。
        /// </summary>
        /// <returns></returns>
        public virtual T Peek()
        {
            lock (rootLock)
            {
                T t = default(T);
                if (_queue.Count == 0)
                {
                    Monitor.Wait(_queue);
                }
                else
                {
                    t = _queue.Peek();
                    Monitor.PulseAll(_queue);
                }
                return t;
            }
        }

        public virtual T Dequeue()
        {
            lock (rootLock)
            {
                T t = default(T);
                if (_queue.Count == 0)
                {
                    Monitor.Wait(_queue);
                }
                else
                {
                    t = _queue.Dequeue();
                    Monitor.PulseAll(_queue);
                }
                return t;
            }
        }


        /// <summary>
        ///     在队列尾部加入新项
        ///     当队列中存在项时，退出操作。
        ///     当队列达到最大上限时，队列执行等待。
        ///     当添加成功时，唤醒所有操作。
        /// </summary>
        /// <param name="t"></param>
        public virtual void Add(T t)
        {
            lock (rootLock)
            {

                if (_queue.Count >= MaxLength)
                {
                    Monitor.Wait(_queue);
                }
                else
                {
                    _queue.Enqueue(t);
                    Monitor.PulseAll(_queue);
                }
            }
        }

        public void Clear()
        {
            lock (rootLock)
            {
                _queue.Clear();
                Monitor.PulseAll(_queue);
            }
        }
    }
}
