using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum InStorageLabelStatus
    {
        [Description("已扫码")]
        Scan,

        [Description("已提交")]
        Submit,

        [Description("已入库")]
        InStorage,

        [Description("已入库")]
        OutStorage
    }
}
