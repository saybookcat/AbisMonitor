using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class StatisticsParameter
    {
        public StatisticsParameter()
        {
            SqlList = new List<string>();
        }

        public List<string> SqlList { get; private set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
