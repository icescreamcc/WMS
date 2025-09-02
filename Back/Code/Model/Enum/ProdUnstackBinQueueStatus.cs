using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum ProdUnstackBinQueueStatus
    {
        [Description("已预定")]
        Booking,

        [Description("已上架")]
        Putaway,

        [Description("已下架")]
        Putout 
    }
}
