using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace External.Socket.TCP
{
    public class TCPClientInfo
    {
        public TCPClientInfo( string ip,int port, string name="") 
        {
            IP = ip;
            Port = port;
            Name = string.IsNullOrEmpty(name) ? ip + ":" + port : name;
            Client = new TcpClient();
        }

        public string Name { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public TcpClient Client { get; set; }

        public bool IsConnected
        {
            get
            {
                return Client.Connected;
            }
        }
         
    }
}
