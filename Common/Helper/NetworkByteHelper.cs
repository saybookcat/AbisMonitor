using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class NetworkHelper
    {
        public static byte[] HostToNetworkOrder(short value)
        {
            short temp = IPAddress.HostToNetworkOrder(value);
            return BitConverter.GetBytes((Int16)temp);
        }

        public static byte[] NetworkToHostOrder(short value)
        {
            short temp = IPAddress.NetworkToHostOrder(value);

            return BitConverter.GetBytes((Int16)temp);
        }

        public static byte[] LittleBytesByShort(short value)
        {
            var buffer = BitConverter.GetBytes((Int16)value);
            if (!BitConverter.IsLittleEndian)
            {
                return buffer.Reverse().ToArray();
            }
            return buffer;
        }

        public static byte[] LittleBytesByInt32(int value)
        {
            var buffer = BitConverter.GetBytes((Int32)value);
            if (!BitConverter.IsLittleEndian)
            {
                return buffer.Reverse().ToArray();
            }
            return buffer;
        }

        public static byte[] LittleBytesByUInt32(UInt32 value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                return buffer.Reverse().ToArray();
            }
            return buffer;
        }

        public static int LittleBtyesToInt32(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            int i = BitConverter.ToInt32(bytes, 0);
            return i;
        }

        public static int LittleBtyesToInt16(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            int i = BitConverter.ToInt16(bytes, 0);
            return i;
        }


    }
}
