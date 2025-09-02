using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysMessage", "系统消息表")]
    public class SysMessage
    {
        [SugarColumn( IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "消息ID")]
        public int MessageId { get; set; }

        [SugarColumn(Length = 20,IsNullable =true, ColumnDescription = "消息类型")]
        public string MessageType { get; set; }

        [SugarColumn( Length = 20, ColumnDescription = "发送人")]
        public string Sender { get; set; }

        [SugarColumn( Length = 20,IsNullable =true, ColumnDescription = "接收人")]
        public string Receiver { get; set; }

        [SugarColumn( Length = 500, ColumnDescription = "消息体")]
        public string Content { get; set; }

        [SugarColumn( Length = 200, ColumnDescription = "消息对应的前端链接")]
        public string Link { get; set; }

        [SugarColumn( Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( ColumnDescription = "记录时间")]
        public DateTime CreateDate { get; set; } 
    }
}
