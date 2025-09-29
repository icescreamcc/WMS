using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    /// <summary>
    /// SysLogs表LogType字段值选项
    /// </summary>
    public enum LogType
    {
        [Description("数据导入")]
        Import,

        [Description("数据导出")]
        Export,

        [Description("查看数据")]
        Read,

        [Description("添加数据")]
        Add,

        [Description("更新数据")]
        Update,

        [Description("删除数据")]
        Del,

        [Description("其他操作")]
        Other
    }
}
