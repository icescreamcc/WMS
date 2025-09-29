using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.BaseModels
{
   public  class CreateModel
    {
        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人ID")]
        public string CreateUserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人姓名")]
        public string CreateUserName { get; set; }

        [SugarColumn( ColumnDescription = "创建时间")]
        public DateTime CreateDate { get; set; }
    }
}
