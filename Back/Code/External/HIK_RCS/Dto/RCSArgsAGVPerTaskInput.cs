using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsAGVPerTaskInput: RCSArgsInput
    {
        public string positionCode { get; set; }

        public string nextTask { get; set; }

        public string agvTyp { get; set; }

        public string priority { get; set; }

        public string useableLayers { get; set; }

        public string cacheCount { get; set; }

        public string update { get; set; }
    }
}
