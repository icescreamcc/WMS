using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum RequisitionStatus
    {
        [Description("待发货")]
        Receiving,

        [Description("已发货")]
        Delivered,

        [Description("已收货")]
        Received
    }
}
