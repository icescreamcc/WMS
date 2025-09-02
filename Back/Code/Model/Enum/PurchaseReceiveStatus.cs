using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum PurchaseReceiveStatus
    {
        [Description("未到货")]
        NoArrived,

        [Description("已到货")]
        Arrived,
         
        [Description("到货异常")]
        Abnormal
    }
}
