using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSOutputData_TaskStatus
    {
        public string taskCode { get; set; }

        public string taskTyp { get; set; }

        public string taskStatus { get; set; }

        public string agvCode { get; set; }
    }
}
