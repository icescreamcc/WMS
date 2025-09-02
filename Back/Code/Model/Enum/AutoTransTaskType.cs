using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum AutoTransTaskType
    { 
        [Description("入库")]
        InStorage,
         
        [Description("出库")]
        OutStorage
    }
}
