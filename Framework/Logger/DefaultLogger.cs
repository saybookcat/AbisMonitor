using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Logger
{
    public static class DefaultLogger
    {

        private static readonly ILog Log;
        static DefaultLogger()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4net.xml");
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static void Info(object message)
        {
            if (Log.IsInfoEnabled)
                Log.Info(message);
        }

        public static void Error(object message)
        {
            if (Log.IsErrorEnabled)
                Log.Error(message);
        }
        public static void Fatal(object message)
        {
            if (Log.IsFatalEnabled)
                Log.Fatal(message);
        }
        public static void Debug(object message)
        {
            if (Log.IsDebugEnabled)
                Log.Debug(message);
        }
        public static void Warn(object message)
        {
            if (Log.IsWarnEnabled)
                Log.Warn(message);
        }

        public static void Info(object message, System.Exception ex)
        {
            if (Log.IsInfoEnabled)
                Log.Info(message, ex);
        }
        public static void Error(object message, System.Exception ex)
        {
            if (Log.IsErrorEnabled)
                Log.Error(message, ex);
        }
        public static void Fatal(object message, System.Exception ex)
        {
            if (Log.IsFatalEnabled)
                Log.Fatal(message, ex);
        }
        public static void Debug(object message, System.Exception ex)
        {
            if (Log.IsDebugEnabled)
                Log.Debug(message, ex);
        }
        public static void Warn(object message, System.Exception ex)
        {
            if (Log.IsWarnEnabled)
                Log.Warn(message, ex);
        }
    }
}
