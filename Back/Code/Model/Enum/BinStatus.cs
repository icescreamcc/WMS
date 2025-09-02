using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum BinStatus
    {
        [Description("空闲")]
        Free,

        [Description("满库")]
        Full,

        [Description("锁定")]
        Lock
    }
}
