using Framework.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Configuration
{
    /// <summary>
    /// 初始化时调用ConfigManager.Instance.Init(workPath)
    /// </summary>
    public class ConfigManager
    {
        private const string Ext = ".json";
        private readonly Dictionary<Type, object> _register;

        private ConfigManager()
        {
            _register = new Dictionary<Type, object>();
        }

        /// <summary>
        /// ConfigManger初始化时调用
        /// 默认存储路径程序根目录下“Configs”文件夹下
        /// </summary>
        /// <param name="workPath"></param>
        public void Init(string configsDir = null)
        {
            if (string.IsNullOrWhiteSpace(configsDir))
            {
                _configsDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configs\");
            }
            else
            {
                _configsDir = configsDir;
            }
            Load();
        }



        private static string _configsDir;

        public string ConfigsDir
        {
            get
            {
                return _configsDir;
            }
        }

        private void Load()
        {
            if (FileHelper.IsFileOrDirectoryExists(ConfigsDir))
            {
                FileHelper.CreateDirectoryByFilePath(ConfigsDir);
            }
        }

        private static volatile ConfigManager _configManager;
        private static readonly object SyncRoot = new object();

        public static ConfigManager Instance
        {
            get
            {
                if (_configManager == null)
                {
                    lock (SyncRoot)
                    {
                        if (_configManager == null)
                        {
                            _configManager = new ConfigManager();
                        }
                    }
                }
                return _configManager;
            }
        }

        public T Get<T>() where T : class, new()
        {
            Type type = typeof(T);
            var typeName = type.ToString();
            string fullName = Path.Combine(ConfigsDir, typeName + Ext);
            if (_register.ContainsKey(type))
            {
                return _register[type] as T;
            }

            if (!System.IO.File.Exists(fullName))
            {
                return new T();
            }

            FileToJsonSerializationHelper<T> jsonSerializationHelper = new FileToJsonSerializationHelper<T>();
            T result = jsonSerializationHelper.DeserializeForPath(fullName);
            _register.Add(type, result);
            return result;
        }

        public void Set<T>(T t)
        {
            if (t == null) return;
            var type = typeof(T);
            if (_register.ContainsKey(type))
            {
                _register[type] = t;
            }
            else
            {
                _register.Add(type, t);
            }
            string typeName = type.ToString();
            string fullName = Path.Combine(ConfigsDir, typeName + Ext);
            var streamSerializationHelper = new FileToJsonSerializationHelper<T>();
            streamSerializationHelper.SerializeForPath(fullName, t);
        }
    }
}
