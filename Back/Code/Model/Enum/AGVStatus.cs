using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum AGVStatus
    {
        [Description("闲置中")]
        Free, 

        [Description("使用中")]
        Using,

        [Description("维修中")]
        Repair,

        [Description("已报废")]
        Abandon,
    }
}
