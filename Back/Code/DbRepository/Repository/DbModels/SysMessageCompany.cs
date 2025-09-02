using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysMessageCompany", "企业公告表")]
   public class SysMessageCompany
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "消息ID")]
        public int MessageId { get; set; }

        [SugarColumn(Length = 5000, ColumnDescription = "消息体")]
        public string Content { get; set; }

        [SugarColumn( ColumnDescription = "是否为当前发布")]
        public bool IsPublishCurrent { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "记录时间")]
        public string DateTime { get; set; }
    }
}
