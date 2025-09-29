using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum SafetyWarningRecordStatus
    {
        [Description("预警")]
        Warning,
         
        [Description("不采购")]
        NoPurchase,

        [Description("采购中")]
        Purchasing,

        [Description("已到货")]
        Received
    }
}
