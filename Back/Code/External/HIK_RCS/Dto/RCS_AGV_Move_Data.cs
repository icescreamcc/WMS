using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCS_AGV_Move_Data
    {
        public string cooX { get; set; }

        public string cooY { get; set; }

        public string dataTyp { get; set; }

        public string direction { get; set; }

        public string mapCode { get; set; }

        public string mapDataCode { get; set; }

        public string positionCode { get; set; }

        public string berthType { get; set; }
    }
}
