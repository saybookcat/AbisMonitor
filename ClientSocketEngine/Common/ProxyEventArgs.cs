using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSocketEngine.Common
{
    public class ProxyEventArgs : EventArgs
    {
        public ProxyEventArgs(Socket socket)
            : this(true, socket, null)
        {

        }

        public ProxyEventArgs(Exception exception)
            : this(false, null, exception)
        {

        }

        public ProxyEventArgs(bool connected, Socket socket, Exception exception)
        {
            Connected = connected;
            Socket = socket;
            Exception = exception;
        }

        public bool Connected { get; private set; }

        public Socket Socket { get; private set; }

        public Exception Exception { get; private set; }
    }
}
