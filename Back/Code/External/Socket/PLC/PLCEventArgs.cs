using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Socket.PLC
{
    public class PLCEventArgs: EventArgs
    {
        public string? Message { get;}

        public Plc? PLCClient { get; }

        public object? Value { get; }

        public string? Db { get; set; }

        public int BinRank { get; set; }

        public PLCEventArgs(string message)
        {
            Message = message;
        }

        public PLCEventArgs(Plc plcClient)
        {
            PLCClient=plcClient;
        }

        public PLCEventArgs(Plc plcClient,  string message)
        {
            Message = message;
            PLCClient = plcClient; 
        }

        public PLCEventArgs(Plc plcClient, string db, string message)
        {
            Message = message;
            PLCClient = plcClient;
            Db = db;
        }

        public PLCEventArgs(Plc plcClient, object value,string db, string message)
        {
            Message = message;
            PLCClient = plcClient;
            Value = value;
            Db = db;
        }

        public PLCEventArgs(Plc plcClient, object? value, string db, int binRank)
        {
            PLCClient = plcClient;
            Value = value; 
            Db = db;
            BinRank = binRank;
        }
    }
}
