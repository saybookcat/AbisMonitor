using System;
using AbisMonitor.Service.TryParse;
using AbisMonitor.UI.ViewModels.Monitor;
using Common.Helper;
using Framework;
using MonitorState = AbisMonitor.Domain.MonitorState;

namespace AbisMonitor.UI.ClientSocket.TryParse
{
    public class SignalOtherMonitorParse:UI.ClientSocket.TryParse.ITryParse
    {
        public bool TryParse(byte[] buffers)
        {
            try
            {
                if (buffers == null) return false;
                int index = 0;
                var userId= new UserIdParse().TryParse(buffers, out index);
                if (userId == null) return false;

                var timeBytes = ByteUtil.InterceptByteArray(buffers, index, 4);
                int second = BitConverter.ToInt32(timeBytes, 0);
                DateTime datetime = DateTimeHelper.GetUTC2Local(second);
                index = index + 4;

                var usBytes = ByteUtil.InterceptByteArray(buffers, index, 4);
                int us = BitConverter.ToInt32(usBytes, 0);

                int deviceId = buffers[index++];
                int ctrlNumber = buffers[index++];
                int slotNumber = buffers[index++];

                var dataBytes = ByteUtil.InterceptByteArray(buffers, index, 2048);
                index += 2048;

                var crcBytes = ByteUtil.InterceptByteArray(buffers, index, 2);
                int crc = BitConverter.ToInt16(crcBytes, 0);

                var monitor = new AbisMonitor.Domain.Monitor
                {
                    DateTime = datetime,
                    US = us,
                    DeviceId = deviceId,
                    CtrlNumber = ctrlNumber,
                    SlotNumber = slotNumber,
                    Data = dataBytes,
                    CRC = crc
                };

                monitor.State = MonitorState.SignalOther;

                QueueManager.Instance.AddMonitorOther(monitor);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Signal Other Monitor parse error", ex);
                return false;
            }
        }
    }
}
