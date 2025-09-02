using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsAGVStopInput: RCSArgsInput
    {
        public string robotCount { get; set; }

        public string mapShortName { get; set; }

        public string[] robots { get; set; }
    }
}
