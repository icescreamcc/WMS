using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum ProdShift
    {
        [Description("白班")]
        Day,

        [Description("晚班")]
        Night
    }
}
