using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 出库类型
    /// </summary>
    public enum OutStorageType
    {
        [Description("销售出库")]
        SalesOut,

        [Description("领用出库")]
        ReceiveOut,

        [Description("借用出库")]
        LeaseOut,

        [Description("退货出库")]
        ReturnOut,

        [Description("库存调拨")]
        AllocationOut,

        [Description("库存盘点")]
        TakeStockOut,

        [Description("其他出库")]
        OtherOut,
    }
}
