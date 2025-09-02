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
    public class TCPServer
    {
        public List<string> ClientIPAddress;

        private TcpListener _listener;

        public List<TCPClientState> Clients { get; set; }

        public bool IsConnected { get; set; }

        public bool IsRunning { get; private set; }

        public IPAddress Address { get; private set; }

        public int Port { get; private set; }

        /// <summary>
        /// 与客户端的连接已建立事件
        /// </summary>
        public event EventHandler<TCPEventArgs> OnClientConnected;

        /// <summary>
        /// 与客户端的连接已断开事件
        /// </summary>
        public event EventHandler<TCPEventArgs> OnClientDisconnected;

        /// <summary>
        /// 接收到数据事件
        /// </summary>
        public event EventHandler<TCPEventArgs> OnDataReceived;

        /// <summary>
        /// 发送数据前的事件
        /// </summary>
        public event EventHandler<TCPEventArgs> OnPrepareSend;

        /// <summary>
        /// 数据发送完毕事件
        /// </summary>
        public event EventHandler<TCPEventArgs> OnCompletedSend;

        /// <summary>
        /// 网络错误事件
        /// </summary>
        public event EventHandler<TCPEventArgs> OnNetError;

        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<TCPEventArgs> OnOtherException;


        public TCPServer(string localIPAddress, int listenPort)
        {
            Address = IPAddress.Parse(localIPAddress);
            Port = listenPort;

            Clients = new List<TCPClientState>();

            _listener = new TcpListener(Address, Port);
            _listener.AllowNatTraversal(true);
        }

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                ClientIPAddress = new List<string>();
                _listener.Start();
                IAsyncResult res = _listener.BeginAcceptTcpClient(
                    new AsyncCallback(_tcpClientAcceptedHandle), _listener);
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _listener.Stop();
                lock (Clients)
                {
                    //关闭所有客户端连接
                    CloseAllClient();
                }
            }
        }

        /// <summary>
        /// 关闭一个与客户端之间的会话
        /// </summary>
        /// <param name="state">需要关闭的客户端会话对象</param>
        public void Close(TCPClientState state)
        {
            if (state != null)
            {
                string clientIP = state.TcpClient.Client.RemoteEndPoint.ToString();
                ClientIPAddress.Remove(clientIP);
                state.Close();
                Clients.Remove(state);

                //TODO 触发关闭事件
            }
        }
        /// <summary>
        /// 关闭所有的客户端会话,与所有的客户端连接会断开
        /// </summary>
        public void CloseAllClient()
        {
            foreach (TCPClientState client in Clients)
            {
                client.Close();
            }
            ClientIPAddress.Clear();
            Clients.Clear();
        }

        private void _tcpClientAcceptedHandle(IAsyncResult ar)
        {
            if (IsRunning)
            {
                //TcpListener tcpListener = (TcpListener)ar.AsyncState;

                TcpClient client = _listener.EndAcceptTcpClient(ar);
                IsConnected = true;
                string clientIP = client.Client.RemoteEndPoint.ToString();//客户连接到当前服务器的地址
                ClientIPAddress.Add(clientIP);
                byte[] buffer = new byte[client.ReceiveBufferSize];
                TCPClientState state = new TCPClientState(client, buffer);
                lock (Clients)
                {
                    Clients.Add(state);
                    if (OnClientConnected != null)
                    {
                        OnClientConnected(this, new TCPEventArgs(state));
                    }
                }

                NetworkStream stream = state.NetworkStream;
                //开始异步读取数据
                stream.BeginRead(state.Buffer, 0, state.Buffer.Length, _dataReceivedHandle, state);

                _listener.BeginAcceptTcpClient(
                  new AsyncCallback(_tcpClientAcceptedHandle), ar.AsyncState);
            }
        }

        private void _dataReceivedHandle(IAsyncResult ar)
        {
            if (IsRunning)
            {
                TCPClientState state = (TCPClientState)ar.AsyncState;
                NetworkStream stream = state.NetworkStream;
                int recv = 0;
                try
                {
                    recv = stream.EndRead(ar);
                }
                catch (Exception ex)
                {
                    recv = 0;
                    lock (Clients)
                    {
                        Clients.Remove(state);
                        IsConnected = false;
                        //连接断开事件
                        if (OnClientDisconnected != null)
                        {
                            OnClientDisconnected(this, new TCPEventArgs($"连接已断开,{ex.Message}"));
                        }
                        return;
                    }
                }
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

        public void Send(TCPClientState state, byte[] data)
        {
            if (OnPrepareSend != null)
            {
                OnPrepareSend(this, new TCPEventArgs(state));
            }
            NetworkStream st = state.TcpClient.GetStream();
            if (st != null)
            {
                st.BeginWrite(data, 0, data.Length, _sendDataEnd, state.TcpClient);
            }
        }


        /// <summary>
        /// 发送数据完成处理函数
        /// </summary>
        /// <param name="ar">目标客户端Socket</param>
        private void _sendDataEnd(IAsyncResult ar)
        {
            var tcpClient = (TcpClient)ar.AsyncState;
            tcpClient.GetStream().EndWrite(ar);
            if (OnCompletedSend != null)
            {
                TCPClientState state = new TCPClientState(tcpClient, null);
                OnCompletedSend(this, new TCPEventArgs(state));
            }
        }

        public void Dispose()
        {
            Stop();
            if (_listener != null)
            {
                _listener = null;
            }
            GC.SuppressFinalize(this);
        }

    }
}
