using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Pub
    {
        /// <summary>
        ///     数据库默认读取行数
        /// </summary>
        public static int DEFAULT_DBFETCHCOUNT = 10000;

        /// <summary>
        /// Excel导出Sheet页最大输出记录数
        /// </summary>
        public static int DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT = 50000;

        /// <summary>
        /// Excel导入最大记录数
        /// </summary>
        public static int DEFAULT_EXCEL_MAXCOUNT_FORINFO = 100000;

        /// <summary>
        /// 条件部分单选框默认状态
        /// </summary>
        public static bool DEFAULT_CHECKBOX_STATES = false;

        /// <summary>
        /// Excel导入最大搜索行数
        /// </summary>
        public static int DEFAULT_EXCEL_SEARCHROWCOUNT = 10;

        public static string AppName = "GROS2.0";

        public static string ProcessPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 每页默认显示行数
        /// </summary>
        public static int PageLineCount = 5000;


        private static readonly string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        /// <summary>
        ///     配置文件目录
        /// </summary>
        public static string ConfigPath
        {
            get { return System.IO.Path.Combine(ProcessPath, @"Configs\"); }
        }

        /// <summary>
        /// 打开Tab选项卡的最大数量
        /// </summary>
        public static int OpenTabMaxCount = 10;
    }
}
