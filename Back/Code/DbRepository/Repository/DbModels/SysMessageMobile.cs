using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysMessageMobile","手机验证码消息表")]
    public class SysMessageMobile
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "消息ID")]
        public int MessageId { get; set; }

        [SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "发送方")]
        public string Sender { get; set; }

        [SugarColumn( Length = 20, ColumnDescription = "接收人用户ID")]
        public string Receiver { get; set; }

        [SugarColumn( Length = 200, ColumnDescription = "消息体")]
        public string Content { get; set; }

        [SugarColumn( Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( ColumnDescription = "记录时间")]
        public string DateTime { get; set; }
    }
}
