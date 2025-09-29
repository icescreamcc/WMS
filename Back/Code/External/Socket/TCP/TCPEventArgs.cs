using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Socket.TCP
{
    public class TCPEventArgs : EventArgs
    {
        public string Msg;

        public TCPClientState? ClientState;

        public bool IsHandled { get; set; }

        public TCPEventArgs(string msg)
        {
            Msg = msg;
            IsHandled = false;
        }
        public TCPEventArgs(TCPClientState state)
        {
            ClientState = state;
            IsHandled = false;
        }
        public TCPEventArgs(string msg, TCPClientState state)
        {
            Msg = msg;
            ClientState = state;
            IsHandled = false;
        }
    }
}
