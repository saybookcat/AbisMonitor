using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class StatisticsModel : DbModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long SuccessCount { get; set; }

        public string SuccessPercent { get; set; }

        public long AllCount { get; set; }


        public long InvalidCount { get; set; }


        public string InvalidCountStr
        {
            get
            {
                if (AllCount == 0 && InvalidCount == 0) return "--";
                return InvalidCount.ToString();
            }
        }

        public string SuccessCountStr
        {
            get
            {
                if (AllCount == 0)
                    return "--";
                return SuccessCount.ToString();
            }
        }

        public string AllCountStr
        {
            get
            {
                if (AllCount == 0) return "--";
                return AllCount.ToString();
            }
        }

        public long FailedCount
        {
            get { return AllCount - SuccessCount; }
        }

        public string FailedCountStr
        {
            get
            {
                if (AllCount == 0) return "--";
                return FailedCount.ToString();
            }
        }


        public string Remarks { get; set; }


        public long? Tag1 { get; set; }

        public string Tag1Str
        {
            get
            {
                if (AllCount == 0) return "--";
                if (Tag1 == null) return "0";
                return Tag1.ToString();
            }
        }

        public string Tag1Percent
        {
            get
            {
                if (AllCount == 0 || Tag1 == null) return null;
                else
                {
                    var value = Tag1 / (double)AllCount;
                    return string.Format("{0:F2%}", value * 100.0);
                }
            }
        }

        public long? Tag2 { get; set; }

        public string Tag2Str
        {
            get
            {
                if (AllCount == 0) return "--";
                if (Tag2 == null) return "0";
                return Tag2.ToString();
            }
        }

        public string Tag2Percent
        {
            get
            {
                if (AllCount == 0 || Tag2 == null) return null;
                else
                {
                    var value = Tag2 / (double)AllCount;
                    return string.Format("{0:F2%}", value * 100.0);
                }
            }
        }
    }
}
