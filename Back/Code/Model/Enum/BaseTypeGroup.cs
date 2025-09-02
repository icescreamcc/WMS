using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum BaseTypeGroup
    {
        [Description("备件")]
        SparePart,

        [Description("耗材")]
        Consumables,

        [Description("样件")]
        SamplePiece,
         
        [Description("包材")]
        PackingMaterial,

        [Description("辅材")]
        Separator,

        [Description("成品")]
        FinishedProduct,

        [Description("原材料")]
        RawMaterial,

        [Description("半成品")]
        SemiFinishedProduct
    }
}
