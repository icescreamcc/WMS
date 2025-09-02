using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSOutputData_AgvStatus
    {
        public string robotCode { get; set; }

        public string robotDir { get; set; }

        public string robotIp { get; set; }

        public string battery { get; set; }

        public string posX { get; set; }

        public string posY { get; set; }

        public string mapCode { get; set; }

        public string speed { get; set; }

        public string status { get; set; }

        public string exclType { get; set; }

        public string stop { get; set; }

        public string podCode { get; set; }

        public string podDir { get; set; }

        public string[] path { get; set; }
    }
}
