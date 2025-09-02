using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 库存出入标识
    /// </summary>
    public enum FlowType
    {
        [Description("入库")]
        In,

        [Description("出库")]
        Out,

        [Description("等待中")]
        Waiting
    }
}
