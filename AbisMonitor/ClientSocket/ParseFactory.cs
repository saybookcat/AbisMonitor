using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbisMonitor.Service.TryParse;
using CatchUserParse = AbisMonitor.UI.ClientSocket.TryParse.CatchUserParse;
using ITryParse = AbisMonitor.UI.ClientSocket.TryParse.ITryParse;
using LoseUserParse = AbisMonitor.UI.ClientSocket.TryParse.LoseUserParse;
using SignalMonitorParse = AbisMonitor.UI.ClientSocket.TryParse.SignalMonitorParse;
using SignalOtherMonitorParse = AbisMonitor.UI.ClientSocket.TryParse.SignalOtherMonitorParse;
using StartUserTrack = AbisMonitor.UI.ClientSocket.TryParse.StartUserTrack;
using StopUserTrack = AbisMonitor.UI.ClientSocket.TryParse.StopUserTrack;

namespace AbisMonitor.UI.ClientSocket
{
    public class ParseFactory
    {
        public ITryParse CreateParser(byte type)
        {
            switch (type)
            {
                case 0x82:
                    //Create 用户起呼
                    return new CatchUserParse();
                case 0x83:
                    //用户挂机
                    return new LoseUserParse();
                case 0x04:
                    //开始跟踪用户信令
                    return new StartUserTrack();
                case 0x05:
                    //停止跟踪用户信令
                    return new StopUserTrack();
                case 0x86:
                    //服务器向客户端输出某个信令，只有客户端打开该用户的跟踪后才有
                    return new SignalMonitorParse();
                case 0x87:
                    //服务器想客户端输出与用户无关的信息
                    return new SignalOtherMonitorParse();
                default:
                    return null;

            }
            return null;
        }
    }
}
