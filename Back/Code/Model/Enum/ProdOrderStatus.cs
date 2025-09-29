using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 生产订单业务流程状态
    /// </summary>
    public enum ProdOrderStatus
    {
         
        [Description("订单已创建")]
        Create,
         
        [Description("已分配小车")]
        Allot,

        [Description("已开始装车")]
        CarLoading,

        [Description("已开始上架")]
        Putaway,
         
        [Description("已开始下架")]
        PutOut, 

        [Description("已开始包装")]
        Packed,

        [Description("已完成包装")]
        PackageFinished,

        [Description("已入库")]
        InStorage,

        [Description("已关闭")]
        Closed
    }
}
