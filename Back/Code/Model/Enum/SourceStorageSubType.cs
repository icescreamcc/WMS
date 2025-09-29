using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 库存数据来源子类型
    /// </summary>
    public enum SourceStorageSubType
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

        [Description("调拨入库")]
        AllocationIn,

        [Description("其他入库")]
        OtherIn,

        [Description("销售出库")]
        SalesOut,

        [Description("生产领用")]
        ReceiveOut,

        [Description("借用出库")]
        LeaseOut,

        [Description("退货出库")]
        ReturnOut,

        [Description("调拨出库")]
        AllocationOut,

        [Description("盘点入库")]
        TakeStockIn,

        [Description("盘点出库")]
        TakeStockOut,

        [Description("盘平记录")]
        TakeStockNoProfitOrLoss,

        [Description("其他出库")]
        OtherOut
    }
}
