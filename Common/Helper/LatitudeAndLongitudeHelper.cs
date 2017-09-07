using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class LatitudeAndLongitudeHelper
    {
        /// <summary>
        /// 纬度转换为String
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static string LatitudeToString(double latitude)
        {
            try
            {
                if ((latitude < FixedParamsPub.DEFAULT_LONGLAT_LIMIT1) || (latitude > FixedParamsPub.DEFAULT_LONGLAT_LIMIT2))
                    return "";

                int du = (int)latitude;
                int fen = (int)((latitude - du) * 60);
                double sec = latitude * 3600 - du * 3600 - fen * 60;

                string result = du + "°" + fen + "′" + String.Format("{0:f2}″N", sec);
                return result;
            }
            catch { return ""; }
        }

        /// <summary>
        /// 经度转发为字符串
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static string LongitudeToString(double longitude)
        {
            try
            {
                if ((longitude < FixedParamsPub.DEFAULT_LONGLAT_LIMIT1) || (longitude > FixedParamsPub.DEFAULT_LONGLAT_LIMIT2))
                    return "";

                int du = (int)longitude;
                int fen = (int)((longitude - du) * 60);
                double sec = longitude * 3600 - du * 3600 - fen * 60;

                string result = du + "°" + fen + "′" + String.Format("{0:f2}″E", sec);
                return result;
            }
            catch { return ""; }
        }

        /// <summary>
        /// 度分秒经纬度(必须含有'°')和数字经纬度转换
        /// </summary>
        /// <param name="degrees">度分秒经纬度</param>
        /// <return>数字经纬度</return>
        public static double? ConvertDegreesToDigital(string degrees)
        {
            try
            {
                const double num = 60;
                double digitalDegree = 0;
                int d = degrees.IndexOf('°'); //度的符号对应的 Unicode 代码为：00B0[1]（六十进制），显示为°。
                if (d < 0)
                {
                    return null;
                }
                string degree = degrees.Substring(0, d);
                digitalDegree += Convert.ToDouble(degree);

                int m = degrees.IndexOf('′'); //分的符号对应的 Unicode 代码为：2032[1]（六十进制），显示为′。
                if (m < 0)
                {
                    return digitalDegree;
                }
                string minute = degrees.Substring(d + 1, m - d - 1);
                digitalDegree += (Convert.ToDouble(minute)) / num;

                int s = degrees.IndexOf('″'); //秒的符号对应的 Unicode 代码为：2033[1]（六十进制），显示为″。
                if (s < 0)
                {
                    return digitalDegree;
                }
                string second = degrees.Substring(m + 1, s - m - 1);
                digitalDegree += Convert.ToDouble(second) / (num * num);

                return Math.Round(digitalDegree, 6);
            }
            catch
            {
                return null;
            }
        }
    }
}
