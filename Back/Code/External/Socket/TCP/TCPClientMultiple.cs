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
    public class TCPClientMultiple
    {
        public List<TCPClientInfo> ClientCollection;
         
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
        /// 是否已连接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasConnected(string clientName)
        {
            if(ClientCollection!=null)
            {
                return ClientCollection.Single(w => w.Name == clientName).IsConnected;
            }
            return false;
        }

        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <returns></returns>
        public bool ConnectServer()
        {
            if (ClientCollection != null)
            {
                foreach (TCPClientInfo clientInfo in ClientCollection)
                {
                    try
                    {
                        if (!clientInfo.IsConnected)
                        {
                            if (clientInfo.Client == null)
                            {
                                clientInfo.Client = new TcpClient();
                            }
                            clientInfo.Client.SendTimeout = 3000;
                            clientInfo.Client.ReceiveTimeout = 3000;
                            clientInfo.Client.Connect(clientInfo.IP, clientInfo.Port);
                        } 
                    }
                    catch (Exception ex)
                    {
                        if (OnClientConnectFailed != null)
                        {
                            var state = new TCPClientState(clientInfo.Client);
                            OnClientConnectFailed(this, new TCPEventArgs($"与{clientInfo.Name}连接失败,{ex.Message}", state));
                        }
                        return false;
                    } 
                }
                return true;
            }
            return false;
        }

        public bool ConnectServer(string clientName)
        {
            if (ClientCollection != null)
            {
                var clientInfo= ClientCollection.SingleOrDefault(w => w.Name == clientName);
                if (clientInfo != null)
                {
                    try
                    {
                        if (clientInfo != null && !clientInfo.IsConnected)
                        {
                            if (clientInfo.Client == null)
                            {
                                clientInfo.Client = new TcpClient();
                            }
                            clientInfo.Client.SendTimeout = 3000;
                            clientInfo.Client.ReceiveTimeout = 3000;
                            clientInfo.Client.Connect(clientInfo.IP, clientInfo.Port);
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (OnClientConnectFailed != null)
                        {
                            var state = new TCPClientState(clientInfo.Client);
                            OnClientConnectFailed(this, new TCPEventArgs($"与{clientInfo.Name}连接失败,{ex.Message}", state));
                        }
                        return false;
                    }
                }  
            }
            return false;
        }

        public void ConnectServerAsync()
        {
            if (ClientCollection != null)
            {
                foreach (TCPClientInfo clientInfo in ClientCollection)
                {
                    if (!clientInfo.IsConnected)
                    {
                        if (clientInfo.Client == null)
                        {
                            clientInfo.Client = new TcpClient();
                        }
                        clientInfo.Client.SendTimeout = 3000;
                        clientInfo.Client.ReceiveTimeout = 3000;
                        clientInfo.Client.BeginConnect(clientInfo.IP, clientInfo.Port, new AsyncCallback(_connectCallBackMethod), clientInfo.Client);
                    }
                } 
            } 
        }

        public void ConnectServerAsync(string clientName)
        {
            if (ClientCollection != null)
            {
                var clientInfo = ClientCollection.SingleOrDefault(w => w.Name == clientName);
                if (clientInfo != null)
                {
                    if (clientInfo != null && !clientInfo.IsConnected)
                    {
                        if (clientInfo.Client == null)
                        {
                            clientInfo.Client = new TcpClient();
                        }
                        clientInfo.Client.SendTimeout = 3000;
                        clientInfo.Client.ReceiveTimeout = 3000;
                        clientInfo.Client.BeginConnect(clientInfo.IP, clientInfo.Port, new AsyncCallback(_connectCallBackMethod), clientInfo.Client);
                    }
                } 
            }
        }

        private void _connectCallBackMethod(IAsyncResult asyncresult)
        {
            var tcpClient = asyncresult.AsyncState as TcpClient;
            if(tcpClient != null)
            {
                try
                {
                    if (tcpClient?.Client != null)
                    {
                        byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                        TCPClientState state = new TCPClientState(tcpClient, buffer);
                        tcpClient.EndConnect(asyncresult);
                        if (OnClientConnected != null)
                        {
                            OnClientConnected(this, new TCPEventArgs(state));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (OnClientConnectFailed != null)
                    {
                        var state = new TCPClientState(tcpClient);
                        var endPoint = state?.TcpClient.Client.RemoteEndPoint as IPEndPoint;
                        var addrress = $"{endPoint?.Address.ToString().Replace("::ffff:", "")}:{endPoint?.Port}";
                        OnClientConnectFailed(this, new TCPEventArgs($"与{addrress}连接失败,{ex.Message}", state));
                    }
                }
            } 
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReceiveDataAsync()
        {
            if (ClientCollection != null)
            {
                foreach (TCPClientInfo clientInfo in ClientCollection)
                {
                    if (clientInfo.Client != null && clientInfo.IsConnected)
                    {
                        TCPClientState state = new TCPClientState(clientInfo.Client, new byte[clientInfo.Client.ReceiveBufferSize]);
                        NetworkStream stream = state.NetworkStream;
                        if (stream.CanRead && state.Buffer != null)
                        {
                            stream.BeginRead(state.Buffer, 0, state.Buffer.Length, _dataReceivedHandle, state);
                        }
                    }
                }
            } 
        }

        public void ReceiveDataAsync(string clientName)
        {
            if (ClientCollection != null)
            {
                var clientInfo = ClientCollection.SingleOrDefault(w => w.Name == clientName);
                if (clientInfo != null && clientInfo.IsConnected)
                {
                    TCPClientState state = new TCPClientState(clientInfo.Client, new byte[clientInfo.Client.ReceiveBufferSize]);
                    NetworkStream stream = state.NetworkStream;
                    if (stream.CanRead && state.Buffer != null)
                    {
                        stream.BeginRead(state.Buffer, 0, state.Buffer.Length, _dataReceivedHandle, state);
                    }
                } 
            }
        }

        /// <summary>
        /// 数据接受回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void _dataReceivedHandle(IAsyncResult ar)
        {
            if (ar.AsyncState != null)
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
                    if (state.TcpClient != null)
                    {
                        lock (state.TcpClient)
                        {
                            //触发客户端连接断开事件
                            if (OnDataReceivedFailed != null)
                            {
                                OnDataReceivedFailed(this, new TCPEventArgs($"接收数据失败,{e.Message}", state));
                            }
                            return;
                        }
                    }
                }
                if (state.Buffer != null&& recv>0)
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

        public void SendAsync(string data,string clientName)
        {
            var dataByte = Encoding.UTF8.GetBytes(data);
            if (ClientCollection != null)
            {
                var clientInfo= ClientCollection.SingleOrDefault(s=>s.Name== clientName);
                if(clientInfo != null&& clientInfo.IsConnected)
                {
                    try
                    {
                        NetworkStream st = clientInfo.Client.GetStream();
                        if (st != null)
                        {
                            st.BeginWrite(dataByte, 0, data.Length, _sendCallBack, clientInfo.Client);
                        }
                    }
                    catch (Exception ex)
                    {
                        //触发客户端连接断开事件
                        if (OnSendFailed != null)
                        {
                            var state = new TCPClientState(clientInfo.Client);
                            OnSendFailed(this, new TCPEventArgs($"发送数据失败,{ex.Message}", state));
                        }
                    }
                }
            } 
        }
         
        /// <summary>
        /// 发送数据完成回调函数
        /// </summary>
        /// <param name="ar">目标客户端Socket</param>
        private void _sendCallBack(IAsyncResult ar)
        {
            if (ar.AsyncState != null)
            {
                var tcpClient = (TcpClient)ar.AsyncState;
                tcpClient.GetStream().EndWrite(ar);
                if (OnCompletedSend != null)
                {
                    TCPClientState state = new TCPClientState(tcpClient, null);
                    OnCompletedSend(this, new TCPEventArgs(state));
                }
            }
        }

        public void CloseConnect()
        {
            if (ClientCollection != null)
            {
                foreach (var clientInfo in ClientCollection)
                {
                    if (clientInfo.IsConnected)
                    {
                        clientInfo.Client.Close();
                        clientInfo.Client.Dispose();
                        if (OnClientDisconnected != null)
                        {
                            OnClientDisconnected(this, new TCPEventArgs($"已关闭{clientInfo.Name}的连接"));
                        }
                    }
                } 
            }
        }

        public void CloseConnect(string clientName)
        {
            if (ClientCollection != null)
            {
                var clientInfo = ClientCollection.SingleOrDefault(s => s.Name == clientName);
                if (clientInfo!=null && clientInfo.IsConnected)
                {
                    clientInfo.Client.Close();
                    clientInfo.Client.Dispose();
                    if (OnClientDisconnected != null)
                    {
                        OnClientDisconnected(this, new TCPEventArgs($"已关闭{clientInfo.Name}的连接"));
                    }
                }
            }
        }
    }
}
