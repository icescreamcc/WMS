using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 参数子类
    /// </summary>
   public enum ArgsSubGroup
    {
        [Description("系统参数")]
        Sys,

        [Description("商品参数")]
        Goods,
         
        [Description("客户参数")]
        Client,

        [Description("仓储参数")]
        Inventory,

        [Description("审批参数")]
        Approval,

        [Description("生产参数")]
        Production,

        [Description("领用参数")]
        Requisition

    }
}
