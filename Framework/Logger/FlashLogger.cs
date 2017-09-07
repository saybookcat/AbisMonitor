using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Logger
{
    /// <summary>
    /// 快速缓存日志写入
    /// </summary>
    public sealed class FlashLogger
    {
        /// <summary>
        /// 记录消息Queue
        /// </summary>
        private readonly ConcurrentQueue<FlashLogMessage> _que;

        /// <summary>
        /// 信号
        /// </summary>
        private readonly ManualResetEvent _mre;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILog _log;

        /// <summary>
        /// 日志
        /// </summary>
        private static FlashLogger _flashLog = new FlashLogger();
        private volatile bool _isRegister=false;
        private static object _registerLock = new object();

        private FlashLogger()
        {

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4net.xml");
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            _que = new ConcurrentQueue<FlashLogMessage>();
            _mre = new ManualResetEvent(false);

        }

        public static FlashLogger Instance
        {
            get { return _flashLog; }
        }


        /// <summary>
        /// 另一个线程记录日志，只在程序初始化时调用一次
        /// </summary>
        public void Register()
        {
            lock (_registerLock)
            {
                if (_isRegister) return;
                _isRegister = true;

                Thread t = new Thread(new ThreadStart(WriteLog)) {IsBackground = false};
                t.Start();
            }
        }

        /// <summary>
        /// 从队列中写日志至磁盘
        /// </summary>
        private void WriteLog()
        {
            while (true)
            {
                // 等待信号通知
                _mre.WaitOne();

                FlashLogMessage msg;
                // 判断是否有内容需要如磁盘 从列队中获取内容，并删除列队中的内容
                while (_que.Count > 0 && _que.TryDequeue(out msg))
                {
                    // 判断日志等级，然后写日志
                    switch (msg.Level)
                    {
                        case FlashLogLevel.Debug:
                            _log.Debug(msg.Message, msg.Exception);
                            break;
                        case FlashLogLevel.Info:
                            _log.Info(msg.Message, msg.Exception);
                            break;
                        case FlashLogLevel.Error:
                            _log.Error(msg.Message, msg.Exception);
                            break;
                        case FlashLogLevel.Warn:
                            _log.Warn(msg.Message, msg.Exception);
                            break;
                        case FlashLogLevel.Fatal:
                            _log.Fatal(msg.Message, msg.Exception);
                            break;
                    }
                }

                // 重新设置信号
                _mre.Reset();
                Thread.Sleep(1);
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志文本</param>
        /// <param name="level">等级</param>
        /// <param name="ex">Exception</param>
        public void EnqueueMessage(object message, FlashLogLevel level, Exception ex = null)
        {
            if ((level == FlashLogLevel.Debug && _log.IsDebugEnabled)
             || (level == FlashLogLevel.Error && _log.IsErrorEnabled)
             || (level == FlashLogLevel.Fatal && _log.IsFatalEnabled)
             || (level == FlashLogLevel.Info && _log.IsInfoEnabled)
             || (level == FlashLogLevel.Warn && _log.IsWarnEnabled))
            {
                _que.Enqueue(new FlashLogMessage
                {
                    Message = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "]\r\n" + message,
                    Level = level,
                    Exception = ex
                });

                // 通知线程往磁盘中写日志
                _mre.Set();
            }
        }

        public static void Debug(object msg, Exception ex = null)
        {
            Instance.EnqueueMessage(msg, FlashLogLevel.Debug, ex);
        }

        public static void Error(object msg, Exception ex = null)
        {
            Instance.EnqueueMessage(msg, FlashLogLevel.Error, ex);
        }

        public static void Fatal(object msg, Exception ex = null)
        {
            Instance.EnqueueMessage(msg, FlashLogLevel.Fatal, ex);
        }

        public static void Info(object msg, Exception ex = null)
        {
            Instance.EnqueueMessage(msg, FlashLogLevel.Info, ex);
        }

        public static void Warn(object msg, Exception ex = null)
        {
            Instance.EnqueueMessage(msg, FlashLogLevel.Warn, ex);
        }
    }
}
