using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace External.Socket.TCP
{
    public class TCPClientState
    {
        public TcpClient TcpClient { get; private set; }

        public byte[]? Buffer { get; private set; }

        private Encoding _encoding = Encoding.UTF8;

        public NetworkStream NetworkStream
        {
            get { return TcpClient.GetStream(); }
        }

        public TCPClientState(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
        }

        public TCPClientState(TcpClient tcpClient, byte[]? buffer)
        {
            TcpClient = tcpClient;
            if (buffer != null)
                Buffer = buffer;
        }

        public string GetData()
        {
            if (Buffer != null && Buffer.Length > 0)
            {
                return _encoding.GetString(Buffer, 0, Buffer.Length);
            }
            return "";
        }
         

        public void Close()
        {
            TcpClient.Close();
            Buffer = null;
        }
    }
}
