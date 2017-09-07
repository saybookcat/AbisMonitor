using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class NumberFormatHelper
    {
        public static string NumberToString(object value)
        {
            if (value == null) return string.Empty;
            if (value is int)
            {
                int temp = (int)value;
                if (temp == 0) return string.Empty;
            }
            else if (value is long)
            {
                long temp = (long)value;
                if (temp == 0) return string.Empty;
            }
            return value.ToString();
        }

        public static string NumberToString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            int tempInt;
            if (int.TryParse(value, out tempInt))
            {
                if (tempInt == 0) return string.Empty;
                return tempInt.ToString();
            }
            long tempLong;
            if (long.TryParse(value, out tempLong))
            {
                if (tempLong == 0) return string.Empty;
                return tempLong.ToString();
            }
            return value.ToString();
        }
    }
}
