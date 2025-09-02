using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum ReceivingOrderStatus
    {
        [Description("待审批")]
        Pending,

        [Description("审批中")]
        Approvaling,

        [Description("审批拒绝")]
        Reject,

        [Description("待收货")]
        WaitReceiving,

        [Description("部分收货完成")]
        ReceivedPart,

        [Description("全部收货完成")]
        ReceivedAll 
    }
}
