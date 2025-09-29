using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 审批数据类型
    /// </summary>
    public enum ApprovalDataType
    {
        [Description("入库单审批")]
        InStorage, 

        [Description("出库单审批")]
        OutStorage,

        [Description("收货计划审批")]
        ReceivingPlan,

        [Description("采购订单审批")]
        Purchase,

        [Description("安全库存审批")]
        SafetyInventory
    }
}
