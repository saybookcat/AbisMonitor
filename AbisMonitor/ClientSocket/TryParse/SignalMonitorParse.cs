using System;
using AbisMonitor.UI.ViewModels.Monitor;
using Common.Helper;
using Framework;
using MonitorState = AbisMonitor.Domain.MonitorState;

namespace AbisMonitor.UI.ClientSocket.TryParse
{
    public class SignalMonitorParse:UI.ClientSocket.TryParse.ITryParse
    {
        public bool TryParse(byte[] buffers)
        {

            try
            {
                if (buffers == null) return false;
                int index = 0;
                var userId = new UserIdParse().TryParse(buffers, out index);
                if (userId == null) return false;
                var engineIdBytes = ByteUtil.InterceptByteArray(buffers, index, 2);
                index += 2;
                int engineId = BitConverter.ToInt16(engineIdBytes, 0);

                var kiloBytes = ByteUtil.InterceptByteArray(buffers, index, 4);
                index += 4;
                long kilo = BitConverter.ToInt64(kiloBytes, 0);

                var speedBytes = ByteUtil.InterceptByteArray(buffers, index, 2);
                int speed = BitConverter.ToInt16(speedBytes, 0);


                var timeBytes = ByteUtil.InterceptByteArray(buffers, index, 4);
                int second = BitConverter.ToInt32(timeBytes, 0);
                DateTime datetime = DateTimeHelper.GetUTC2Local(second);
                index = index + 4;

                var usBytes = ByteUtil.InterceptByteArray(buffers, index, 4);
                int us = BitConverter.ToInt32(usBytes,0);

                int deviceId = buffers[index++];
                int ctrlNumber = buffers[index++];
                int slotNumber = buffers[index++];

                var dataBytes = ByteUtil.InterceptByteArray(buffers, index, 2048);
                index += 2048;

                var crcBytes = ByteUtil.InterceptByteArray(buffers, index, 2);
                int crc = BitConverter.ToInt16(crcBytes, 0);

                var monitor = new AbisMonitor.Domain.Monitor
                {
                    EngineId=engineId,
                    Kilo=kilo,
                    Speed=speed,
                    DateTime=datetime,
                    US=us,
                    DeviceId=deviceId,
                    CtrlNumber=ctrlNumber,
                    SlotNumber=slotNumber,
                    Data=dataBytes,
                    CRC=crc
                };

                monitor.State = MonitorState.Signal;

                QueueManager.Instance.AddMonitor(monitor);
                return true;

            }
            catch (Exception ex)
            {
                Log.Error("Signal Monitor parse error", ex);
                return false;
            }
        }

    }
}
