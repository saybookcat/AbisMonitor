using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.File
{
    public class FileHelper
    {
        /// <summary>
        /// 判断文件或文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileOrDirectoryExists(string path)
        {
            return System.IO.File.Exists(path) || System.IO.Directory.Exists(path);
        }

        /// <summary>
        ///     创建目录，支持文件夹和文件
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectoryByFilePath(string path)
        {
            if (IsFileOrDirectoryExists(path))
                return;
            System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(path);
            parent.Create();
        }
    }
}
