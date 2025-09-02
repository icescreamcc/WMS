using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Socket.Socket
{
    public class WebSocketArgs : EventArgs
    {
        public string ClientAddress { get; set; }

        public string ClientPort { get; set; }

        public string Message { get; set; }
    }
}
