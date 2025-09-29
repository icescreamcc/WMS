using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum SocketMessageType
    {
        Tick,
        Test,
        Connect,
        ConnectQuery,
        Read,
        Write,
        Business,
        AGVCallback
    }
}
