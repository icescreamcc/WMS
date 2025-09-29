using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 生产订单配送车流程状态
    /// </summary>
    public enum ProdOrderDetailStatus
    {
        [Description("已分配小车")]
        Allot,

        [Description("已装车")]
        CarLoading,

        [Description("已上架")]
        Putaway,

        [Description("已配对")]
        Matching,

        [Description("已下架")]
        PutOut,
         
        //[Description("待拆垛")]
        //WaitUnstack,

        //[Description("已拆垛")]
        //Unstack,

        [Description("已包装")]
        Packed,

        [Description("已入库")]
        InStorage,

        [Description("已关闭")]
        Closed
    }
}
