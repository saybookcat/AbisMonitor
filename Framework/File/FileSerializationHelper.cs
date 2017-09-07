
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Logger;

namespace Framework.File
{
    public abstract class FileSerializationHelper<T>
    {

        private static object SerializeLock = new object();
        public abstract void Serialize(System.IO.Stream stream, T data);

        public abstract T Deserialize(System.IO.Stream stream);


        private FileOccupy _fileOccupy;

        /// <summary>
        /// 序列化文件到指定路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public void SerializeForPath(string path, T data)
        {
            lock (SerializeLock)
            {
                try
                {
                    EnsureDirectoryExsit(path);
                    _fileOccupy = new FileOccupy(path);

                    if (_fileOccupy.IsOccupy)
                    {
                        System.IO.File.Create(path).Close();
                    }
                    using (System.IO.Stream fileStream = System.IO.File.Create(path))
                    {
                        if (data != null)
                        {
                            Serialize(fileStream, data);
                        }
                    }
                }
                catch (InvalidOperationException iex)
                {
                    Log.Error(iex);
                }
                catch (UnauthorizedAccessException uex)
                {
                    Log.Error(uex);
                }
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public T DeserializeForPath(string path)
        {
            T data = default(T);
            using (System.IO.Stream fileStream = System.IO.File.OpenRead(path))
                data = Deserialize(fileStream);
            return data;
        }

        /// <summary>
        /// 确保目录存在，不存在时创建目录
        /// </summary>
        /// <param name="path"></param>
        private static void EnsureDirectoryExsit(string path)
        {
            if (!FileHelper.IsFileOrDirectoryExists(path))
                FileHelper.CreateDirectoryByFilePath(path);
        }
    }
}
