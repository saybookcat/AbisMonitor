using System;
using AbisMonitor.Domain;
using Common.Helper;
using Framework;

namespace AbisMonitor.UI.ClientSocket.TryParse
{
    public class UserIdParse 
    {
        public UserTrack TryParse(byte[] buffers)
        {
            int index;
            return TryParse(buffers, out index);
        }

        public UserTrack TryParse(byte[] buffers, out int index)
        {
            index = 0;
            try
            {
                if (buffers == null) return null;
                index = 0;
                if (buffers.Length < 20) return null;
                byte[] imsiBytes = ByteUtil.InterceptByteArray(buffers, index, 20);
                index += 20;
                long imsi = BitConverter.ToInt64(imsiBytes, 0);
                byte devByte = buffers[index++];
                int dev = Convert.ToInt16(devByte);
                byte portByte = buffers[index++];
                int port = Convert.ToInt16(portByte);
                byte[] arfcnBytes = ByteUtil.InterceptByteArray(buffers, index, 2);
                index += 2;
                short arfcn = BitConverter.ToInt16(arfcnBytes, 0);
                byte chanNumByte = buffers[index++];

                int chanNum = Convert.ToInt16(chanNumByte);

                byte[] crcBytes = ByteUtil.InterceptByteArray(buffers, index, 2);
                short crc = BitConverter.ToInt16(crcBytes, 0);

                var userId = new UserId
                {
                    IMSI=imsi,
                    Device=dev,
                    Port=port,
                    ARFCN=arfcn,
                    ChanNum=chanNum
                };
                var userTrack = new UserTrack
                {
                    UserId=userId,
                    CRC = crc,

                };
                return userTrack;
            }
            catch (Exception ex)
            {
                Log.Error("userTraced parse error", ex);
                return null;
            }

        }
    }
}
