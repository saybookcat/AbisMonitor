using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Common.Helper
{
    /// <summary>
    /// Dpc和Opc的转换工具类，Dpc和Opc的现实格式为0-0-0
    /// </summary>
    public class DpcAndOpcHelper
    {
        public static string ConvertToStr(int value)
        {
            if (value <= 0) return string.Empty;

            IPAddress tempIpAddress;
            if (IPAddress.TryParse(value.ToString(), out tempIpAddress))
            {
                var str = tempIpAddress.ToString();
                if (str.StartsWith("0."))
                {
                    str = str.Remove(0, 2);
                }
                str = str.Replace('.', '-');
                return str;
            }
            return string.Empty;
        }


        public static int ConvertToInt(string value)
        {
            string tempSgnPointCodeStr = "0-" + value;
            tempSgnPointCodeStr = tempSgnPointCodeStr.Replace("-", ".");

            IPAddress ipa = new IPAddress(0);
            bool flag = IPAddress.TryParse(tempSgnPointCodeStr, out ipa);
            if (!flag || ipa.Equals(IPAddress.Any) || ipa.Equals(IPAddress.Broadcast))
            {
                return 0;
            }
            return IpAndMacHelper.ConvertIpAddressToHost(ipa);
        }
    }
}
