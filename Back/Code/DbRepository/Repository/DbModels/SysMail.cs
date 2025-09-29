using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysMail", "邮件记录表")]
    public class SysMail:CreateModel
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int MailId { get; set; }

        [SugarColumn(IsNullable = true, Length = 100, ColumnDescription = "主数据ID/附件ID")]
        public string PrimaryId { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "附件ID")]
        public string AttachmentId { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "业务类型")]
        public string BusinessType { get; set; }

        [SugarColumn(IsNullable = true, Length = 100, ColumnDescription = "发件人")]
        public string Sender { get; set; }

        [SugarColumn(IsNullable = true, Length = 2000, ColumnDescription = "收件人")]
        public string ToReceiver { get; set; }

        [SugarColumn(IsNullable = true, Length = 2000, ColumnDescription = "抄送人")]
        public string ToCC { get; set; }

        [SugarColumn(IsNullable = true, Length = 1000, ColumnDescription = "主题")]
        public string Subject { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType="text",  ColumnDescription = "内容")]
        public string Body { get; set; } 
    }
}
