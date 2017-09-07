using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class StringIsNumberHelper
    {
        public static bool StringIsNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            Regex re = new Regex(@"^[\d]+$");
            if (re.IsMatch(value))
            {
                return true;
            }
            return false;
        }
    }
}
