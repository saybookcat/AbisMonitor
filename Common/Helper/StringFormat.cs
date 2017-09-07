using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class StringFormat
    {
        /// <summary>
        /// 规范显示名称，编号类型的字符串
        /// </summary>
        /// <param name="value">名称</param>
        /// <param name="num">编号值</param>
        /// <param name="startingFromNum">编号开始数，默认编号从1开始，小于1的编号将返回空</param>
        /// <returns></returns>
        public static string DisplayFormat(string value, int num, int startingFromNum = 1)
        {
            if (num < startingFromNum) return null;
            return string.Format("{0}({1})", string.IsNullOrWhiteSpace(value) ? "未知" : value,
                num);
        }

        public static string DisplayFormat(string value, long num, int startingFromNum = 1)
        {
            if (num < startingFromNum) return null;
            return string.Format("{0}({1})", string.IsNullOrWhiteSpace(value) ? "未知" : value,
                num);
        }

        public static string DisplayFormat(string value, uint num, int startingFromNum = 1)
        {
            if (num < startingFromNum) return null;
            return string.Format("{0}({1})", string.IsNullOrWhiteSpace(value) ? "未知" : value,
                num);
        }
    }
}
