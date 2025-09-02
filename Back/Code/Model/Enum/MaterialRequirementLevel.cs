using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum MaterialRequirementLevel
    {
        [Description("一般")]
        Common,

        [Description("紧急")]
        Urgent
    }
}
