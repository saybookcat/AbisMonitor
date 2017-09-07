using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.UI.Models
{
    public class Device
    {
        public int DeviceNum { get; set; }

        public string DeviceName { get; set; }


        public Dictionary<int,Port> PortDic
        {
            get;
            set;
        }
    }
}
