using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework
{
    public static class StringExtensions
    {
        /// <summary>
        /// 忽略大小写替换字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="search"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceInsensitive(this string input, string search, string replacement)
        {
            return Regex.Replace(input, search, replacement, RegexOptions.IgnoreCase);
        }
    }
}
