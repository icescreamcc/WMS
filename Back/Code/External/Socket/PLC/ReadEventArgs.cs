using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace External.Socket.PLC
{
    public class ReadEventArgs : EventArgs
    {
        public string DB { get; }

        public TimeSpan SignalTime { get; }

        public ReadEventArgs(string db, TimeSpan signalTime)
        {
            DB = db;
            SignalTime = signalTime;
        }
    }
}
