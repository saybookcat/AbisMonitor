using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbisMonitor.Domain;

namespace AbisMonitor.UI.Messager
{
    public class DeviceTrackModelChangedMessage
    {
        public AbisDeviceSimple AbisDeviceSimple { get; set; }
    }
}
