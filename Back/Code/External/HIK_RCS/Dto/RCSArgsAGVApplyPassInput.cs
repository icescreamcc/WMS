using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsAGVApplyPassInput: RCSArgsInput
    {
        public string taskCode { get; set; }

        public string type { get; set; }
    }
}
