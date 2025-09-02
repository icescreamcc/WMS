using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{

    /// <summary>
    /// SysArgs表ArgsType字段值选项
    /// </summary>
    public enum ArgsType
    {
        [Description("普通文本")]
        Text,

        [Description("数字文本")]
        Number,

        [Description("单选框")]
        Check,

        [Description("数字范围")]
        Range,

        [Description("日期")]
        Date,

        [Description("单选项")]
        SingleSelect,

        [Description("多选项")]
        MultipleSelect
    }
}
