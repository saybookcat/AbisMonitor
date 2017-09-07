using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.Domain
{
    public class UserTrack
    {
        public UserId UserId { get; set; }
        public short CRC { get; set; }

        public UserTrackState State { get; set; }
    }

    public enum UserTrackState
    {
        UserTraced,
        UserLose,
        Start,
        Stop
    }
}
