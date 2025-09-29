using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    public class SysMessageReadUser
    {
        [SugarColumn(Length = 20, IsPrimaryKey = true, ColumnDescription = "用户ID")]
        public string UserId { get; set; }

        [SugarColumn(IsPrimaryKey = true,  ColumnDescription = "消息ID")]
        public int MessageId { get; set; }  
    }
}
