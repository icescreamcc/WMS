using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum LabelItemType
    {
        [Description("文本")]
        Text,
          
        [Description("二维码")]
        QRCode,

        [Description("一维码")]
        BarCode
    }
}
