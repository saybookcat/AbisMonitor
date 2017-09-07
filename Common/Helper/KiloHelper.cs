using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class KiloHelper
    {
        /// <summary>
        /// 数字类型的公里标转为字符串规则公里标
        /// </summary>
        /// <param name="kilo"></param>
        /// <returns></returns>
        public static string ConvertToKiloStr(int kilo)
        {
            switch (kilo)
            {
                case FixedParamsPub.DEFAULT_KILO_INVALID:
                    return string.Empty;
                case FixedParamsPub.DEFAULT_KILO_SPECIAL:
                    return "K9999+999";
                default:
                    if (kilo < FixedParamsPub.DEFAULT_KILO_LIMIT1 || kilo > FixedParamsPub.DEFAULT_KILO_LIMIT2)
                        return string.Empty;

                    int k = Math.Abs(kilo);
                    return string.Format(((kilo < 0) ? "-" : "") + "K{0:d}+{1:d03}", k / 1000, k % 1000);

            }
        }

        /// <summary>
        /// 验证字符串类型的公里标，并转化成字符串规则公里标
        /// </summary>
        /// <param name="strKilo"></param>
        /// <returns></returns>
        public static int ConvertToKilo(string strKilo)
        {
            if (string.IsNullOrWhiteSpace(strKilo)) return FixedParamsPub.DEFAULT_KILO_INVALID;
            if (strKilo.Equals("K9999+999", StringComparison.InvariantCultureIgnoreCase))
            {
                return FixedParamsPub.DEFAULT_KILO_SPECIAL;
            }

            Regex re = new Regex(@"^(-?)[kK](\d{1,4})\+(\d{1,3})$");
            Match m = re.Match(strKilo);
            if (m.Success)
            {
                int sgn = 1;
                if (m.Groups[1].Value.Equals("-"))
                    sgn = -1;

                int part1 = 0;
                bool ret = int.TryParse(m.Groups[2].Value, out part1);
                if (ret == false)
                    return 0;

                int part2 = 0;
                ret = int.TryParse(m.Groups[3].Value, out part2);
                if (ret == false)
                    return 0;

                return sgn * (part1 * 1000 + part2);
            }

            return FixedParamsPub.DEFAULT_KILO_INVALID;

        }
    }
}
