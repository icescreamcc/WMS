using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum AGVTaskModel
    {
        [Description("单任务模式")]
        TransportSingle,

        [Description("多任务模式")]
        TransportMultipl
    }
}
