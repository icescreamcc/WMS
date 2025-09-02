using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 审批模式
    /// </summary>
    public enum ApprovalModel
    { 

        [Description("流程审批")]
        Process,

        [Description("任意审批")]
        Any
    }
}
