using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientSocketEngine.Core;
using System.Threading;
using Common.Helper;
using Framework;

namespace AbisMonitor.UI.ClientSocket
{
    public class Client
    {
        #region Fields
        private volatile System.Threading.Timer _keepAliveTimer;

        private readonly object LockObj_IsConnection = new object();
        private readonly object LockObj_Reconnect = new object();

        private const int Timeout = 3; //超时3秒
        private AsyncTcpSession _clientSocket;

        public Action ConnectionEvent;
        public Action DisconnectedEvent;

        private const int MinReconnectTime = 3; //最小重连时间  3秒
        private const int MaxReconnectTime = 10*60;//最大重连时间 10分钟
        private volatile int _reconnectInterval = MinReconnectTime;  //重连间隙，默认等于最小重连时间
        private byte[] _keepAliveBuffer;

        private Task _stopAndReconnectTask;

        public Client(IPAddress ipa,int port,IParseEngine parseEngine,byte[] keepAliveBuffer)
        {
            this.IpAddress = ipa;
            this.Port = port;
            this._parseEngine = parseEngine;
            _keepAliveBuffer = keepAliveBuffer;
        }

        public bool IsConnection
        {
            get
            {
                if (_clientSocket == null) return false;
                return _clientSocket.IsSocketConnected();
            }
        }

        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }

        private IParseEngine _parseEngine;

        #endregion 

        #region public methods
        public bool Start()
        {
            lock (LockObj_IsConnection)
            {
                if (IsConnection) return true;

                _clientSocket = new AsyncTcpSession();
                RegistrationSocketEvent(_clientSocket);

                var ipEndPoint = new IPEndPoint(this.IpAddress, this.Port);
                _clientSocket.Connect(ipEndPoint);
                return true;
            }
        }

        public bool Stop()
        {
            lock (LockObj_IsConnection)
            {
                StopKeepAliveTimer();
                if (_parseEngine != null)
                {
                    _parseEngine.Stop();
                    Log.Info(string.Format("socket[{0}:{1}] parseEngine is stop", IpAddress, Port));
                }
                if (IsConnection)
                {
                    _clientSocket.Close();
                    LogOffSocketEvent(_clientSocket);
                    _clientSocket = null;
                }
                return true;
            }
        }

        public void Send(byte[] buffer)
        {
            if (IsConnection)
            {
                _clientSocket.Send(buffer, 0, buffer.Length);
            }
            else
            {
                throw new SocketException(1000);
            }
        }

        public void SendCommand(string msg)
        {
            if (IsConnection)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.Default.GetBytes(msg));
                _clientSocket.Send(buffer);
            }
            else
            {
                throw new SocketException(1000);
            }
        }
        #endregion

        #region private methods

        private void StopAndReconect()
        {
            if (_stopAndReconnectTask != null) return;
            lock (LockObj_Reconnect)
            {
                if (_stopAndReconnectTask != null) return;
                _stopAndReconnectTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Stop();
                        Reconnect();

                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                    finally
                    {
                        _stopAndReconnectTask = null;
                    }
                });
            }
        }

        private void Reconnect()
        {

            lock (LockObj_IsConnection)
            {

                if (!App.IsLogin) return; //未登陆状态下，不重连

                if (IsConnection)
                    return;

                Start();
                if (IsConnection) return;

                Log.Debug(string.Format("socket[{0}:{1}] reconnect interval :{2}", IpAddress, Port,
                    _reconnectInterval*1000));
                Thread.Sleep(_reconnectInterval*1000);

                _reconnectInterval = _reconnectInterval*2;
                if (_reconnectInterval > MaxReconnectTime)
                {
                    _reconnectInterval = MinReconnectTime;
                }

                Reconnect();
            }
        }

        private void StartupKeepAliveTimer()
        {
            if (_keepAliveTimer == null)
            {
                TcpKeepAlive();
            }
        }

        private void StopKeepAliveTimer()
        {
            if (_keepAliveTimer != null)
            {
                _keepAliveTimer.Dispose();
                _keepAliveTimer = null;
            }
        }

        private void TcpKeepAlive()
        {
            if (_keepAliveTimer == null)
            {
                _keepAliveTimer = new System.Threading.Timer(SeedKeepAlive, null, Timeout * 1000, Timeout * 1000);
            }
        }

        private void SeedKeepAlive(object o)
        {
            try
            {
                if (IsConnection)
                {
                    byte[] bytes = _keepAliveBuffer;
                    Send(bytes);
                    _keepAliveTimer.Change(Timeout * 1000, Timeout * 1000);
                }
                else
                {
                    Log.Error(string.Format("[{0}:{1}] Keep Alive Dispose", IpAddress, Port));
                    StopAndReconect();
                }
            }
            catch(Exception ex)
            {
                Log.Error(string.Format("Seed(To [{0}:{1}]) Keep Alive Error  - Keep Alive Dispose", IpAddress, Port),
                    ex);
                StopAndReconect();
            }
        }

        #endregion 

        #region socket event

        private void RegistrationSocketEvent(AsyncTcpSession clientSocket)
        {
            if (clientSocket == null) return;
            clientSocket.Closed += _clientSocket_Closed;
            clientSocket.DataReceived += _clientSocket_DataReceived;
            clientSocket.Connected += _clientSocket_Connected;
            clientSocket.Error += _clientSocket_Error;
        }

        public void LogOffSocketEvent(AsyncTcpSession clientSocket)
        {
            if (clientSocket == null) return;
            clientSocket.Closed -= _clientSocket_Closed;
            clientSocket.DataReceived -= _clientSocket_DataReceived;
            clientSocket.Connected -= _clientSocket_Connected;
            clientSocket.Error -= _clientSocket_Error;
        }

        void _clientSocket_Error(object sender, ErrorEventArgs e)
        {
            Log.Error(string.Format("socket[{0}:{1}] error:{2}", IpAddress, Port, e.Exception));
            if (DisconnectedEvent != null)
            {
                DisconnectedEvent.Invoke();
            }
        }

        void _clientSocket_Connected(object sender, EventArgs e)
        {
            _parseEngine.Start();
            Log.Info(string.Format("socket[{0}:{1}] parseEngine is started", IpAddress, Port));
            StartupKeepAliveTimer();
            Log.Info(string.Format("socket[{0}:{1}] connected", IpAddress, Port));
            _reconnectInterval = MinReconnectTime;
            if (ConnectionEvent != null)
            {
                ConnectionEvent.Invoke();
            }
        }

        void _clientSocket_DataReceived(object sender, DataEventArgs e)
        {
            try
            {
                byte[] buffers = ByteUtil.InterceptByteArray(e.Data, e.Offset, e.Length);
                if (_parseEngine != null)
                {
                    _parseEngine.Append(buffers);
                }
            }
            catch (Exception ex)
            {
                Log.Error("_clientSocket_DataReceived error :" + ex);
            }
        }

        void _clientSocket_Closed(object sender, EventArgs e)
        {
            if (DisconnectedEvent != null)
            {
                DisconnectedEvent.Invoke();
            }
            Log.Debug(string.Format("socket[{0}:{1}] closed", IpAddress, Port));
            
        }

        #endregion 

    }
}
