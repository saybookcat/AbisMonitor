using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework.File
{
    /// <summary>
    /// 检测文件是否被占用
    /// </summary>
    public class FileOccupy
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        public String FilePath { get; set; }

        public FileOccupy(String filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        public Boolean IsOccupy
        {
            get
            {
                Boolean result = false;
                IntPtr vHandle = _lopen(FilePath, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)//文件被占用
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                CloseHandle(vHandle); //关闭_lopen

                return result;
            }
        }
    }
}
