using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum MessageType
    { 
        [Description("成品装车")]
        CarLoading,

        [Description("成品上架")]
        Putaway,

        [Description("配对成功")]
        Matching,

        [Description("成品下架")]
        PutOut,

        [Description("成品拆垛")]
        Unstack,

        [Description("成品包装")]
        Packed,

        [Description("入库操作")]
        InStorage,

        [Description("出库操作")]
        OutStorage,

        [Description("库存调拨")]
        AllocationStorage,

        [Description("物品领用")]
        Requisition
    }
}
