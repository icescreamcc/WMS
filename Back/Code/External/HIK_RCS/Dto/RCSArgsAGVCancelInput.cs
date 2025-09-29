using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsAGVCancelInput: RCSArgsInput
    {
        public string forceCancel { get; set; }

        public string matterArea { get; set;}

        public string taskCode { get; set; }
    }
}
