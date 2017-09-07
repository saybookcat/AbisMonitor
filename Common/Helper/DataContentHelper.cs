using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class DataContentHelper
    {
        public static string DataContentConvert(byte[] data)
        {
            if (data == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(b.ToString("X2"));
                sb.Append(" ");
            }
            return sb.ToString().Trim();
        }

        public static string DataContentConvert(byte[] data, double width)
        {
            try
            {
                if (data == null || data.Length == 0) return string.Empty;
                int countPerRow = (int)width / 150 * 8;
                if (countPerRow == 0)
                    countPerRow = 8;

                StringBuilder hexStr = new StringBuilder();
                int index = 0;
                foreach (byte b in data)
                {
                    hexStr.Append(b.ToString("X2"));

                    if (index == (data.Length - 1))
                        break;

                    if ((index % countPerRow) == (countPerRow - 1))
                        hexStr.Append("\r\n");
                    else
                    {
                        if ((index % 8) == 7)
                            hexStr.Append("  ");
                        else
                            hexStr.Append(b.ToString(" "));
                    }

                    index++;
                }
                return hexStr.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
