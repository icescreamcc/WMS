using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysMessageSubscribe", "系统消息订阅")]
    public class SysMessageSubscribe
    {
        [SugarColumn(Length = 20,IsPrimaryKey =true, ColumnDescription = "用户ID")]
        public string UserId { get; set; }

        [SugarColumn(Length = 20, IsPrimaryKey = true, ColumnDescription = "消息类型")]
        public string MessageType { get; set; } 
    }
}
