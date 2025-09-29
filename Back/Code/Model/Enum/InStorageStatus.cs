using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 入库单状态
    /// </summary>
    public enum InStorageStatus
    {
        [Description("待质检")]
        QualityWait,

        [Description("质检未通过")]
        QualityFailed,

        [Description("待审批")]
        Pending,

        [Description("审批进行中")]
        Approvaling,

        [Description("审批未通过")]
        Reject,

        [Description("待入库")]
        WaitInStorage,

        [Description("已入库")]
        InStorage
    }
}
