using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class ExportFileInfo
    {
        /// <summary>
        /// Excel类型
        /// </summary>
        public string ExportType { get; set; }

        /// <summary>
        /// Excel标题
        /// </summary>
        public string ExcelTitle { get; set; }

        /// <summary>
        /// 仅文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件的全路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 存储文件夹
        /// </summary>
        public string SaveDirectory { get; set; }
    }
}
