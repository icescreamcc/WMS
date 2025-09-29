using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// 库存数据来源类型
    /// </summary>
    public enum SourceStorageType
    {
        [Description("入库单")]
        InStorage,

        [Description("出库单")]
        OutStorage,
         
        [Description("库存调拨")]
        Allocation 
    }
}
