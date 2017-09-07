using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class DateTimeHelper
    {
        public static string GetDateTimeUsecToString(DateTime dateTime, int microsecond)
        {
            return GetDateTimeUsec(dateTime, microsecond) == new DateTime() ? null : GetDateTimeUsec(dateTime, microsecond).ToString(FixedParamsPub.TIME_FORMAT_YMDHMSF3);
        }

        /// <summary>
        /// 时间部分附加微妙部分
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="microsecond"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeUsec(DateTime dateTime, int microsecond)
        {
            try
            {
                if (dateTime == new DateTime())
                    return new DateTime();
                if (dateTime.Date.Equals(FixedParamsPub.DefaultDate))
                    return new DateTime();
                if (dateTime == FixedParamsPub.DefaultNullDtDb)
                    return new DateTime();
                if (dateTime == FixedParamsPub.DefaultNullDtRt)
                    return new DateTime();

                return dateTime.AddTicks(microsecond * 10);
            }
            catch
            {
                return new DateTime();
            }
        }

        public static int GetMillisecond(DateTime dateTime)
        {
            string str = dateTime.ToString(FixedParamsPub.TIME_COMPFORMAT_YMDHMSF6);
            return int.Parse(str.Substring(str.Length - 6));
        }

        public static string FormatDateTimeOfYMDHMS(DateTime? dateTime)
        {
            return FormatDateTime(dateTime, FixedParamsPub.TIME_FORMAT_YMDHMS);
        }

        public static string FormatDateTimeOfYMDHMS(string dateTime)
        {
            if (string.IsNullOrWhiteSpace(dateTime)) return string.Empty;
            DateTime tempDateTime;
            if (DateTime.TryParse(dateTime, out tempDateTime))
            {
                return FormatDateTime(tempDateTime, FixedParamsPub.TIME_FORMAT_YMDHMS);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatDateTimeOfYMDHMSF3(DateTime? dateTime)
        {
            return FormatDateTime(dateTime, FixedParamsPub.TIME_FORMAT_YMDHMSF3);
        }

        public static string FormatDateTime(DateTime? dt, string timeFormat)
        {
            if ((dt == null) || string.IsNullOrWhiteSpace(timeFormat))
                return string.Empty;

            try
            {
                DateTime dtWork = (DateTime)dt;
                if (dtWork.Equals(new DateTime()))
                    return string.Empty;
                if (dtWork.Date.Equals(FixedParamsPub.DefaultDate))
                    return string.Empty;
                if (dtWork.Equals(FixedParamsPub.DefaultNullDtDb))
                    return string.Empty;
                if (dtWork.Equals(FixedParamsPub.DefaultNullDtRt))
                    return string.Empty;

                return dtWork.ToString(timeFormat);
            }
            catch { return string.Empty; }
        }

        public static string SecondToTimeStr(int second)
        {
            TimeSpan ts = new TimeSpan(0, 0, second);
            string str = "";
            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分" + ts.Seconds + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString() + "分" + ts.Seconds + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = ts.Seconds + "秒";
            }
            return str;
        }


        public static string SecondToDateStr(int second)
        {
            TimeSpan ts = new TimeSpan(0, 0, second);
            string str = "";
            if (ts.Days > 0)
            {
                str = ts.Days.ToString() + "天";
            }
            if (ts.Hours > 0)
            {
                str += ts.Hours.ToString() + "小时";
            }
            if (ts.Minutes > 0)
            {
                str += ts.Minutes.ToString() + "分";
            }

            return str;
        }

        /// <summary>
        /// 从格林尼治时间转为本地时间，
        /// 自动判断本地时区
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static DateTime GetUTC2Local(int second)
        {

            TimeSpan serverOffset = TimeZoneInfo.Local.GetUtcOffset(DateTimeOffset.Now);
            DateTime datetime = _dt1970.AddSeconds(second);
            datetime = datetime.AddTicks(serverOffset.Ticks);
            return datetime;
        }


        private static readonly DateTime _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 获取当前时间到1970年1月1日的秒数
        /// </summary>
        /// <returns></returns>
        public static int GetUTCDateTimeSec()
        {
            return GetUTCDateTimeSec(DateTime.Now);
        }


        public static int GetUTCDateTimeSec(DateTime dateTime)
        {
            if (dateTime == default(DateTime)) return 0;
            TimeSpan serverOffset = TimeZoneInfo.Local.GetUtcOffset(DateTimeOffset.Now);
            TimeSpan timeSpan = dateTime - _dt1970.AddTicks(serverOffset.Ticks);
            return Convert.ToInt32((int)timeSpan.TotalSeconds);
        }
    }
}
