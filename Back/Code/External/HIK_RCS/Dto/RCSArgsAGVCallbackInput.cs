using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsAGVCallbackInput: RCSArgsInput
    {
        public float cooX { get; set; }

        public float cooY { get; set; }

        public string currentPositionCode { get; set; }

        public string data { get; set; }

        public string stgBinCode { get; set; }

        public string method { get; set; }

        public string robotCode { get; set; }

        public string taskCode { get; set; } 
    }
}
