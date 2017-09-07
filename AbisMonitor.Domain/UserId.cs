using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.Domain
{
    public class UserId
    {
        public long IMSI { get; set; }

        public int Device { get; set; }

        public int Port { get; set; }

        public int Slot { get; set; }

        public int ARFCN { get; set; }

        public int ChanNum;
    }
}
