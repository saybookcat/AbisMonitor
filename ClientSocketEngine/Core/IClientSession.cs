using ClientSocketEngine.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientSocketEngine.Core
{
    public interface IClientSession
    {
        IProxyConnector Proxy { get; set; }

        int ReceiveBufferSize { get; set; }

        int SendingQueueSize { get; set; }

        bool IsConnected { get; }

        void Connect(EndPoint remoteEndPoint);

        void Send(ArraySegment<byte> segment);

        void Send(IList<ArraySegment<byte>> segments);

        void Send(byte[] data, int offset, int length);

        bool TrySend(ArraySegment<byte> segment);

        bool TrySend(IList<ArraySegment<byte>> segments);

        void Close();

        event EventHandler Connected;

        event EventHandler Closed;

        event EventHandler<ErrorEventArgs> Error;

        event EventHandler<DataEventArgs> DataReceived;
    }
}
