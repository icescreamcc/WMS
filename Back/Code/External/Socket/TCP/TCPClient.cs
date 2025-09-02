using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace External.Socket.TCP
{
    public class TCPClient
    { 
        private TcpClient? _tcpClient;
         
        public bool isConnected
        {
            get
            {
                if (_tcpClient == null)
                {
                    return false;
                }
                else
                {
                    return _tcpClient.Connected;
                }
            }
        }

        /// <summary>
        /// 建立连接事件处理
        /// </summary>
        public event EventHandler<TCPEventArgs>? OnClientConnected;

        /// <summary>
        /// 连接失败事件处理
        /// </summary>
        public event EventHandler<TCPEventArgs>? OnClientConnectFailed;

        /// <summary>
        /// 连接断开事件处理
        /// </summary>
        public event EventHandler<TCPEventArgs>? OnClientDisconnected;

        /// <summary>
        /// 接收数据失败事件处理
        /// </summary>
        public event EventHandler<TCPEventArgs>? OnDataReceivedFailed;

        /// <summary>
        /// 发送数据失败事件处理
        /// </summary>
        public event EventHandler<TCPEventArgs>? OnSendFailed;

        /// <summary>
        /// 接收到数据事件处理
        /// </summary>
        public event EventHandler<TCPEventArgs>? OnDataReceived;

        /// <summary>
        /// 数据发送完毕事件处理
        /// </summary>
        public event EventHandler<TCPEventArgs>? OnCompletedSend;

        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <returns></returns>
        public bool ConnectServer(string ip, int port)
        {
            if (!isConnected)
            {
                if (_tcpClient == null)
                {
                    _tcpClient = new TcpClient();
                }
                try
                {
                    _tcpClient.SendTimeout = 3000;
                    _tcpClient.ReceiveTimeout = 3000;
                    _tcpClient.Connect(ip, port);
                    return true;
                }
                catch (Exception e)
                {
                    if (OnClientConnectFailed != null)
                    {
                        var endPoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                        var addrress = $"{endPoint?.Address.ToString().Replace("::ffff:", "")}:{endPoint?.Port}";
                        OnClientConnectFailed(this, new TCPEventArgs($"与{addrress}连接失败,{e.Message}"));
                    }
                    return false;
                }
            }
            return true;
        }

        public void ConnectServerAsync(string ip, int port)
        {
            if (!isConnected)
            {
                if (_tcpClient == null)
                {
                    _tcpClient = new TcpClient();
                }
                try
                {
                    _tcpClient.SendTimeout = 2500;
                    _tcpClient.ReceiveTimeout = 2500;
                    _tcpClient.BeginConnect(ip, port, new AsyncCallback(_connectCallBackMethod), _tcpClient);
                }
                catch (Exception e)
                {
                    if (OnClientConnectFailed != null)
                    {
                        var endPoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                        var addrress = $"{endPoint?.Address.ToString().Replace("::ffff:", "")}:{endPoint?.Port}";
                        OnClientConnectFailed(this, new TCPEventArgs($"与{addrress}连接失败,{e.Message}"));
                    }
                }
            }
        }
        private void _connectCallBackMethod(IAsyncResult asyncresult)
        {
            _tcpClient = asyncresult.AsyncState as TcpClient;
            if (_tcpClient?.Client != null)
            {
                byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];
                TCPClientState state = new TCPClientState(_tcpClient, buffer);
                _tcpClient.EndConnect(asyncresult);
                if (OnClientConnected != null)
                {
                    var endPoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                    var addrress = $"{endPoint?.Address.ToString().Replace("::ffff:", "")}:{endPoint?.Port}";
                    OnClientConnected(this, new TCPEventArgs($"已与{addrress}建立TCP连接",state));
                }
            }
        }
          
        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReceiveDataAsync()
        {
            if (_tcpClient != null && isConnected)
            {
                TCPClientState state = new TCPClientState(_tcpClient, new byte[_tcpClient.ReceiveBufferSize]);
                NetworkStream stream = state.NetworkStream;
                if (stream.CanRead && state.Buffer != null)
                {
                    stream.BeginRead(state.Buffer, 0, state.Buffer.Length, _dataReceivedHandle, state); 
                }
            }
        }

        /// <summary>
        /// 数据接受回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void _dataReceivedHandle(IAsyncResult ar)
        {
            if (isConnected && ar.AsyncState != null)
            {
                TCPClientState state = (TCPClientState)ar.AsyncState;
                NetworkStream stream = state.NetworkStream;
                int recv = 0;
                try
                {
                    recv = stream.EndRead(ar);
                }
                catch (Exception e)
                {
                    recv = 0;
                    if (_tcpClient != null)
                    {
                        lock (_tcpClient)
                        {
                            //触发客户端连接断开事件
                            if (OnDataReceivedFailed != null)
                            {
                                var endPoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                                var addrress = $"{endPoint?.Address.ToString().Replace("::ffff:", "")}:{endPoint?.Port}";
                                OnDataReceivedFailed(this, new TCPEventArgs( $"接收{addrress}数据失败,{e.Message}", state));
                            }
                            return;
                        }
                    }
                }
                if (state.Buffer != null && recv > 0)
                {
                    byte[] buff = new byte[recv];
                    Buffer.BlockCopy(state.Buffer, 0, buff, 0, recv);
                    //触发数据收到事件
                    if (OnDataReceived != null)
                    {
                        OnDataReceived(this, new TCPEventArgs(state));
                    }
                    stream.BeginRead(state.Buffer, 0, state.Buffer.Length, _dataReceivedHandle, state);
                }
            }
        }

        public void SendAsync(string data)
        {
            var dataByte = Encoding.UTF8.GetBytes(data);
            SendAsync(dataByte);
        }

        public void SendAsync(byte[] data)
        {
            if (_tcpClient != null && isConnected)
            {
                try
                {
                    NetworkStream st = _tcpClient.GetStream();
                    if (st != null)
                    {
                        st.BeginWrite(data, 0, data.Length, SendCallBack, _tcpClient);
                    }
                }
                catch (Exception ex)
                {
                    //触发客户端连接断开事件
                    if (OnSendFailed != null)
                    {
                        var endPoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                        var addrress = $"{endPoint?.Address.ToString().Replace("::ffff:", "")}:{endPoint?.Port}";
                        OnSendFailed(this, new TCPEventArgs($"发送数据到{addrress}失败,{ex.Message}",new TCPClientState (_tcpClient)));
                    }
                }
            }
        }
        /// <summary>
        /// 发送数据完成回调函数
        /// </summary>
        /// <param name="ar">目标客户端Socket</param>
        private void SendCallBack(IAsyncResult ar)
        {
            if (ar.AsyncState != null)
            {
                var tcpClient = (TcpClient)ar.AsyncState;
                tcpClient.GetStream().EndWrite(ar);
                if (OnCompletedSend != null)
                {
                    TCPClientState state = new TCPClientState(_tcpClient, null);
                    OnCompletedSend(this, new TCPEventArgs(state));
                }
            }
        }

        public void CloseConnect()
        {
            if (_tcpClient != null)
            {
                var endPoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                var addrress = $"{endPoint?.Address.ToString().Replace("::ffff:", "")}:{endPoint?.Port}";
                _tcpClient.Close(); 
                if (OnClientDisconnected != null)
                { 
                    OnClientDisconnected(this, new TCPEventArgs($"与{addrress}连接已关闭", new TCPClientState(_tcpClient)));
                } 
                _tcpClient.Dispose();
            }
        }
    }
}
