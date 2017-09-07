using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class LacAndCiHelper
    {
        #region Lac Ci  Rac  LacCi
        public static string LacToStr(int lac, bool isHex)
        {
            if ((lac < FixedParamsPub.DEFAULT_LAC_LIMIT1) || (lac > FixedParamsPub.DEFAULT_LAC_LIMIT2))
                return string.Empty;
            return String.Format(isHex ? "{0:X04}H" : "{0}", lac);
        }

        public static string RacToStr(int rac, bool isHex)
        {
            if ((rac < FixedParamsPub.DEFAULT_CI_LIMIT1) || (rac > FixedParamsPub.DEFAULT_CI_LIMIT2))
                return string.Empty;
            return String.Format(isHex ? "{0:X04}H" : "{0}", rac);
        }

        public static string CiToStr(int ci, bool isHex)
        {
            if ((ci < FixedParamsPub.DEFAULT_CI_LIMIT1) || (ci > FixedParamsPub.DEFAULT_CI_LIMIT2))
                return string.Empty;
            return String.Format(isHex ? "{0:X04}H" : "{0}", ci);
        }

        public static int ParseCi(string strRaw)
        {
            if (string.IsNullOrWhiteSpace(strRaw))
                return FixedParamsPub.DEFAULT_CI_INVALID;

            int result = FixedParamsPub.DEFAULT_CI_INVALID;

            Regex re = new Regex(@"^(?<num>\d+)$");
            Match m = re.Match(strRaw);

            if (m.Success)
            {
                result = int.Parse(m.Result("${num}"));
            }
            else
            {
                re = new Regex(@"^(0x)?(?<num>[0-9a-f]{1,4})(h)?$", RegexOptions.IgnoreCase);
                m = re.Match(strRaw);

                if (m.Success)
                    result = int.Parse(m.Result("${num}"), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                else
                    return FixedParamsPub.DEFAULT_CI_INVALID;

            }

            if ((result < FixedParamsPub.DEFAULT_CI_LIMIT1) || (result > FixedParamsPub.DEFAULT_CI_LIMIT2))
                return FixedParamsPub.DEFAULT_CI_INVALID;
            else
                return result;
        }


        public static int ParseLac(string strRaw)
        {
            if (string.IsNullOrWhiteSpace(strRaw))
                return FixedParamsPub.DEFAULT_LAC_INVALID;

            int result = FixedParamsPub.DEFAULT_LAC_INVALID;

            Regex re = new Regex(@"^(?<num>\d+)$");
            Match m = re.Match(strRaw);

            if (m.Success)
            {
                result = int.Parse(m.Result("${num}"));
            }
            else
            {
                re = new Regex(@"^(0x)?(?<num>[0-9a-f]{1,4})(h)?$", RegexOptions.IgnoreCase);
                m = re.Match(strRaw);

                if (m.Success)
                    result = int.Parse(m.Result("${num}"), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                else
                    return FixedParamsPub.DEFAULT_LAC_INVALID;

            }

            if ((result < FixedParamsPub.DEFAULT_LAC_LIMIT1) || (result > FixedParamsPub.DEFAULT_LAC_LIMIT2))
                return FixedParamsPub.DEFAULT_LAC_INVALID;
            else
                return result;
        }

        /// <summary>
        /// 任何异常均返回Pub.DEFAULT_LAC_INVALID
        /// </summary>
        /// <param name="strRaw">原始字符</param>
        /// <param name="isHex">当前是否是Hex格式</param>
        /// <returns></returns>
        public static int ParseLac(string strRaw, bool isHex)
        {
            if (string.IsNullOrWhiteSpace(strRaw))
                return FixedParamsPub.DEFAULT_LAC_INVALID;

            Regex re = null;
            Match m = null;
            int result = FixedParamsPub.DEFAULT_LAC_INVALID;
            if (isHex)
            {
                re = new Regex(@"^(0x)?(?<num>[0-9a-f]{1,4})(h)?$", RegexOptions.IgnoreCase);
                m = re.Match(strRaw);

                if (m.Success == false)
                    return FixedParamsPub.DEFAULT_LAC_INVALID;

                if (int.TryParse(m.Result("${num}"), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result) == false)
                    return FixedParamsPub.DEFAULT_LAC_INVALID;
            }
            else
            {
                re = new Regex(@"^(?<num>\d+)$");
                m = re.Match(strRaw);

                if (m.Success == false)
                    return FixedParamsPub.DEFAULT_LAC_INVALID;

                if (int.TryParse(m.Result("${num}"), out result) == false)
                    return FixedParamsPub.DEFAULT_LAC_INVALID;
            }

            if ((result < FixedParamsPub.DEFAULT_LAC_LIMIT1) || (result > FixedParamsPub.DEFAULT_LAC_LIMIT2))
                return FixedParamsPub.DEFAULT_LAC_INVALID;
            return result;
        }

        /// <summary>
        /// 任何异常均返回Pub.DEFAULT_CI_INVALID
        /// </summary>
        /// <param name="strRaw"></param>
        /// <param name="isHex"></param>
        /// <returns></returns>
        public static int ParseCi(string strRaw, bool isHex)
        {
            if (string.IsNullOrWhiteSpace(strRaw))
                return FixedParamsPub.DEFAULT_CI_INVALID;

            Regex re = null;
            Match m = null;
            int result = FixedParamsPub.DEFAULT_CI_INVALID;
            if (isHex)
            {
                re = new Regex(@"^(0x)?(?<num>[0-9a-f]{1,4})(h)?$", RegexOptions.IgnoreCase);
                m = re.Match(strRaw);

                if (m.Success == false)
                    return FixedParamsPub.DEFAULT_CI_INVALID;

                if (int.TryParse(m.Result("${num}"), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result) == false)
                    return FixedParamsPub.DEFAULT_CI_INVALID;
            }
            else
            {
                re = new Regex(@"^(?<num>\d+)$");
                m = re.Match(strRaw);

                if (m.Success == false)
                    return FixedParamsPub.DEFAULT_CI_INVALID;

                if (int.TryParse(m.Result("${num}"), out result) == false)
                    return FixedParamsPub.DEFAULT_CI_INVALID;
            }

            if ((result < FixedParamsPub.DEFAULT_CI_LIMIT1) || (result > FixedParamsPub.DEFAULT_CI_LIMIT2))
                return FixedParamsPub.DEFAULT_CI_INVALID;
            return result;
        }

        public static long GetLacCi(int lac, int ci)
        {
            return lac * 65536L + ci;
        }

        #endregion 
    }
}
