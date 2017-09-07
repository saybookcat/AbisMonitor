using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service.ExcelServices
{
    public class ExcelFileHelper
    {
        public static string GetExportFileName(string title, string saveDir, int fileIndex, int fileCount)
        {
            if (fileCount <= 0) return null;
            if (fileCount > 1)
            {
                saveDir = System.IO.Path.Combine(saveDir, System.IO.Path.GetFileNameWithoutExtension(title));
                if (!System.IO.Directory.Exists(saveDir))
                {
                    System.IO.Directory.CreateDirectory(saveDir);
                }
            }
            return BuildExcelFileName(saveDir,fileIndex,fileCount,title);

        }

        private static string BuildExcelFileName(string saveDir, int fileIndex, int fileCount, string title)
        {
            string path = fileCount == 1 ? string.Format("{0}.xlsx", title) : string.Format("{0}({1}).xlsx", title, fileIndex);
            path = System.IO.Path.Combine(saveDir, path);
            while (System.IO.File.Exists(path))
            {
                path = string.Format("{0}({1}).xlsx", title, ++fileIndex);
                path = System.IO.Path.Combine(saveDir, path);
            }
            return path;
        }
    }
}
