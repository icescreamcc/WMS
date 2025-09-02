using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 入库类型
    /// </summary>
    public enum InStorageType
    {
        [Description("初期库存")]
        InitialIn,

        [Description("采购入库")]
        PurchaseIn,

        [Description("生产入库")]
        ProductIn,

        [Description("借用退还")]
        LeaseIn,

        [Description("退货入库")]
        ReturnIn,

        [Description("库存调拨")]
        AllocationIn,

        [Description("库存盘点")]
        TakeStockIn,

        [Description("其他入库")]
        OtherIn
    }
}
