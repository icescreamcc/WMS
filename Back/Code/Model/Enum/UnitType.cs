using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum UnitType
    {
        [Description("货币")]
        Currency,

        [Description("尺寸")]
        Size,

        [Description("面积")]
        Acreage,

        [Description("体积")]
        Volume,

        [Description("重量")]
        Weight,

        [Description("包装")]
        Pack

    }
}
