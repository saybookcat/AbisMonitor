using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Domain;
using Framework.DataExtension;

namespace AbisMonitor.Domain
{
    public class AbisDeviceSimple : DbModel
    {
        [DbField("DeviceName")]
        public byte[] DeviceName { get; set; }

        [DbField("DeviceNum")]
        public int DeviceNum { get; set; }

        [DbField("PortNum")]
        public int PortNum { get; set; }

        [DbField("IPAddress")]
        public string IpAddress { get; set; }

        [DbField("BTS_Name")]
        public byte[] BtsName { get; set; }

        [DbField("SlotNum")]
        public int SlotNum { get; set; }

        public string DeviceNameStr
        {
            get { return System.Text.Encoding.Default.GetString(DeviceName); }
        }

        public string BtsNameStr
        {
            get { return System.Text.Encoding.Default.GetString(BtsName); }
        }
    }
}

