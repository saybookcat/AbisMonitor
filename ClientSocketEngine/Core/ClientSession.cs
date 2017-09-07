using ClientSocketEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSocketEngine.Core
{
    public abstract class ClientSession : IClientSession, IBufferSetter
    {
        public const int DefaultReceiveBufferSize = 4096;

        protected Socket Client { get; set; }

        protected EndPoint RemoteEndPoint { get; set; }

#if !SILVERLIGHT
        public virtual EndPoint LocalEndPoint { get; set; }
#endif

        public bool IsConnected { get; private set; }

#if !__IOS__
        public bool NoDelay { get; set; }
#endif

        public ClientSession()
        {

        }

        public int SendingQueueSize { get; set; }

        public abstract void Connect(EndPoint remoteEndPoint);

        public abstract bool TrySend(ArraySegment<byte> segment);

        public abstract bool TrySend(IList<ArraySegment<byte>> segments);

        public void Send(byte[] data, int offset, int length)
        {
            this.Send(new ArraySegment<byte>(data, offset, length));
        }

#if NO_SPINWAIT_CLASS
        public void Send(ArraySegment<byte> segment)
        {
            if (TrySend(segment))
                return;

            while (true)
            {
                Thread.SpinWait(1);

                if (TrySend(segment))
                    return;
            }
        }

        public void Send(IList<ArraySegment<byte>> segments)
        {
            if (TrySend(segments))
                return;

            while (true)
            {
                Thread.SpinWait(1);

                if (TrySend(segments))
                    return;
            }
        }
#else
        public void Send(ArraySegment<byte> segment)
        {
            if (TrySend(segment))
                return;

            var spinWait = new SpinWait();

            while (true)
            {
                spinWait.SpinOnce();

                if (TrySend(segment))
                    return;
            }
        }

        public void Send(IList<ArraySegment<byte>> segments)
        {
            if (TrySend(segments))
                return;

            var spinWait = new SpinWait();

            while (true)
            {
                spinWait.SpinOnce();

                if (TrySend(segments))
                    return;
            }
        }
#endif

        public abstract void Close();

        private EventHandler m_Closed;

        public event EventHandler Closed
        {
            add { m_Closed += value; }
            remove { m_Closed -= value; }
        }

        protected virtual void OnClosed()
        {
            IsConnected = false;

            var handler = m_Closed;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private EventHandler<ErrorEventArgs> m_Error;

        public event EventHandler<ErrorEventArgs> Error
        {
            add { m_Error += value; }
            remove { m_Error -= value; }
        }

        protected virtual void OnError(Exception e)
        {
            var handler = m_Error;
            if (handler == null)
                return;

            handler(this, new ErrorEventArgs(e));
        }

        private EventHandler m_Connected;

        public event EventHandler Connected
        {
            add { m_Connected += value; }
            remove { m_Connected -= value; }
        }

        protected virtual void OnConnected()
        {
            var client = Client;

#if !__IOS__
            if (client != null)
            {
                if (client.NoDelay != NoDelay)
                    client.NoDelay = NoDelay;
            }
#endif

            IsConnected = true;

            var handler = m_Connected;
            if (handler == null)
                return;

            handler(this, EventArgs.Empty);
        }

        private EventHandler<DataEventArgs> m_DataReceived;

        public event EventHandler<DataEventArgs> DataReceived
        {
            add { m_DataReceived += value; }
            remove { m_DataReceived -= value; }
        }

        private DataEventArgs m_DataArgs = new DataEventArgs();

        protected virtual void OnDataReceived(byte[] data, int offset, int length)
        {
            var handler = m_DataReceived;
            if (handler == null)
                return;

            m_DataArgs.Data = data;
            m_DataArgs.Offset = offset;
            m_DataArgs.Length = length;

            handler(this, m_DataArgs);
        }

        public virtual int ReceiveBufferSize { get; set; }

        public IProxyConnector Proxy { get; set; }

        protected ArraySegment<byte> Buffer { get; set; }

        void IBufferSetter.SetBuffer(ArraySegment<byte> bufferSegment)
        {
            SetBuffer(bufferSegment);
        }

        protected virtual void SetBuffer(ArraySegment<byte> bufferSegment)
        {
            Buffer = bufferSegment;
        }


        public bool IsSocketConnected()
        {
            #region remarks
            /******************************************************************************************** 
             * 当Socket.Conneted为false时， 如果您需要确定连接的当前状态，请进行非阻塞、零字节的 Send 调用。 
             * 如果该调用成功返回或引发 WAEWOULDBLOCK 错误代码 (10035)，则该套接字仍然处于连接状态；  
             * 否则，该套接字不再处于连接状态。 
             * Depending on http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.connected.aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2 
            ********************************************************************************************/
            #endregion


            #region 过程
            if (this.Client == null) return false;

            // This is how you can determine whether a socket is still connected.  
            bool connectState = true;

            bool blockingState = this.Client.Blocking;
            try
            {
                if (Client == null) return false;
                byte[] tmp = new byte[1];

                Client.Blocking = false;
                Client.Send(tmp, 0, 0);
                //Console.WriteLine("Connected!");  
                connectState = true; //若Send错误会跳去执行catch体，而不会执行其try体里其之后的代码  
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK  
                if (e.NativeErrorCode.Equals(10035))
                {
                    //Console.WriteLine("Still Connected, but the Send would block");  
                    connectState = true;
                }

                else
                {
                    //Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);  
                    connectState = false;
                }
            }
            finally
            {
                Client.Blocking = blockingState;
            }

            //Console.WriteLine("Connected: {0}", client.Connected);  
            return connectState;
            #endregion  
        }

        /// <summary>
        /// 另一种判断connected的方法，但未检测对端网线断开或ungraceful的情况 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsSocketConnected(Socket s)
        {
            #region remarks
            /* As zendar wrote, it is nice to use the Socket.Poll and Socket.Available, but you need to take into consideration  
             * that the socket might not have been initialized in the first place.  
             * This is the last (I believe) piece of information and it is supplied by the Socket.Connected property.  
             * The revised version of the method would looks something like this:  
             * from：http://stackoverflow.com/questions/2661764/how-to-check-if-a-socket-is-connected-disconnected-in-c */
            #endregion

            #region 过程

            if (s == null)
                return false;
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);

            /* The long, but simpler-to-understand version: 
 
                    bool part1 = s.Poll(1000, SelectMode.SelectRead); 
                    bool part2 = (s.Available == 0); 
                    if ((part1 && part2 ) || !s.Connected) 
                        return false; 
                    else 
                        return true; 
 
            */
            #endregion
        }
    }
}
