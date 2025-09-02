using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum AGVType
    {
        [Description("CTU")]
        CTU,

        [Description("潜伏车")]
        Lurk,

        [Description("叉车")]
        Forklift
    }
}
