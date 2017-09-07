using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.Domain
{
    public class Monitor
    {
        public UserId UserId { get; set; }

        public int EngineId { get; set; }

        public long Kilo { get; set; }

        public int Speed { get; set; }

        public DateTime DateTime { get; set; }

        public long US { get; set; }

        public int DeviceId { get; set; }

        public int CtrlNumber { get; set; }

        public int SlotNumber { get; set; }

        public byte[] Data { get; set; }

        public int CRC { get; set; }

        public MonitorState State { get; set; }
    }

    public enum MonitorState
    {
        Signal,
        SignalOther
    }
}
