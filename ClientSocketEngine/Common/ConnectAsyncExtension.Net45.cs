using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSocketEngine.Common
{
    public static partial class ConnectAsyncExtension
    {
        public static void ConnectAsync(this EndPoint remoteEndPoint, EndPoint localEndPoint, ConnectedCallback callback, object state)
        {
            var e = CreateSocketAsyncEventArgs(remoteEndPoint, callback, state);

#if NETSTANDARD

            if (localEndPoint != null)
            {
                var socket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.ExclusiveAddressUse = false;
                socket.Bind(localEndPoint);
                socket.ConnectAsync(e);
            }
            else
            {
                Socket.ConnectAsync(SocketType.Stream, ProtocolType.Tcp, e);
            }            
#else
            Socket socket = null;
            if (IPAddress.Any.AddressFamily == AddressFamily.InterNetwork)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else if (IPAddress.Any.AddressFamily == AddressFamily.InterNetworkV6)
            {
                socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }


            if (localEndPoint != null)
            {
                socket.ExclusiveAddressUse = false;
                socket.Bind(localEndPoint);
            }

            socket.ConnectAsync(e);
#endif
        }
    }
}
