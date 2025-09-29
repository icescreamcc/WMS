using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum LabelItemValueType
    {
        [Description("固定值")]
        Fixed,

        [Description("绑定字段")]
        BindingField
    }
}
