using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.BaseModels
{
    public class CreateModifyModel:CreateModel
    { 
        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次修改人ID")]
        public string UpdateUserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次修改人姓名")]
        public string UpdateUserName { get; set; }

        [SugarColumn( ColumnDescription = "上次修改时间")]
        public DateTime UpdateDate { get; set; }
    }
}
