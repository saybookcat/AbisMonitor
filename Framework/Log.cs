
using Framework.Logger;

namespace Framework
{
    public static class Log
    {
        public static void Init(LogEngineTypeEnum logEngineType = LogEngineTypeEnum.DefaultLogger)
        {
            LogEngineType = logEngineType;
            if (logEngineType == LogEngineTypeEnum.FlashLogger)
            {
                FlashLogger.Instance.Register();
            }
        }

        private static LogEngineTypeEnum LogEngineType;

        public static void Info(object message)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Info(message);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Info(message);
                    break;
            }
        }

        public static void Error(object message)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Error(message);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Error(message);
                    break;
            }
        }
        public static void Fatal(object message)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Fatal(message);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Fatal(message);
                    break;
            }
        }
        public static void Debug(object message)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Debug(message);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Debug(message);
                    break;
            }
        }
        public static void Warn(object message)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Warn(message);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Warn(message);
                    break;
            }
        }

        public static void Info(object message, System.Exception ex)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Info(message, ex);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Info(message, ex);
                    break;
            }
        }
        public static void Error(object message, System.Exception ex)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Error(message, ex);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Error(message, ex);
                    break;
            }
        }
        public static void Fatal(object message, System.Exception ex)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Fatal(message, ex);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Fatal(message, ex);
                    break;
            }
        }
        public static void Debug(object message, System.Exception ex)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Debug(message, ex);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Debug(message, ex);
                    break;
            }
        }
        public static void Warn(object message, System.Exception ex)
        {
            switch (LogEngineType)
            {
                case LogEngineTypeEnum.DefaultLogger:
                    DefaultLogger.Warn(message, ex);
                    break;
                case LogEngineTypeEnum.FlashLogger:
                    FlashLogger.Warn(message, ex);
                    break;
            }
        }
    }
}
