using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysLogs", "系统日志表")]
    public class SysLogs
    {
        [SugarColumn( IsPrimaryKey = true, IsIdentity = true)]
        public int LogId { get; set; }

        [SugarColumn( Length = 20, ColumnDescription = "操作人ID")]
        public string UserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "标题")]
        public string Title { get; set; }

        [SugarColumn( Length = 200, ColumnDescription = "操作信息")]
        public string Message { get; set; }

        [SugarColumn( Length = -1, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 20, ColumnDescription = "日志类型")]
        public string LogType { get; set; }

        [SugarColumn( ColumnDescription = "记录时间")]
        public string DateTime { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "模块名称")]
        public string ModuleName { get; set; }
    }
}
