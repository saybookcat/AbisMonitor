using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.UI.Models
{
    public class Port
    {
        public int PortNum { get; set; }


        public Dictionary<int,Cell> CellDic
        {
            get;
            set;
        }
    }
}
