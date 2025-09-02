using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum PlanMaterialRequirementStatus
    {
        [Description("有效的")]
        Created,

        [Description("已过期")]
        Exceed,

        [Description("已废弃")]
        Obsolete
    }
}
