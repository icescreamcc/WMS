using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class SocketMessageData
    {
        public string BusinessType { get; set; }

        public object Value { get; set; } 

        public string AGVDockDeviceNo { get; set; }
         
        public bool IsDelived { get; set; }

        public string TaskStatus { get; set; }

        public bool IsFinished { get; set; }

        public string TaskType { get; set; }

        public string OrderNo { get; set; }
    }
}
