using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSCallbackArgs
    {
        public string method { get; set; }

        public string taskCode { get; set; }

        public string reqCode { get; set; }

        public string ctnrTyp { get; set; }
    }
}
