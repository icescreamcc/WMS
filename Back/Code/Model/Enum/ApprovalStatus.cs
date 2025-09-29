using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 审批状态
    /// </summary>
    public enum ApprovalStatus
    {
        [Description("无需审批")]
        NoApproval,

        [Description("待审批")]
        Pending,

        [Description("审批中")]
        Approvaling,

        [Description("审批通过")]
        Approve,

        [Description("审批拒绝")]
        Reject
    }
}
