using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum ReceivingLevel
    {
        [Description("正常")]
        Common,

        [Description("一般紧急")]
        Urgent_General,

        [Description("特别紧急")]
        Urgent_Especial
    }
}
