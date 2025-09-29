using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 出库单状态
    /// </summary>
    public enum OutStorageStatus
    {  
        [Description("待审批")]
        Pending,

        [Description("审批进行中")]
        Approvaling,

        [Description("审批未通过")]
        Reject,

        [Description("待出库")]
        WaitOutStorage,

        [Description("已出库")]
        OutStorage
    }
}
