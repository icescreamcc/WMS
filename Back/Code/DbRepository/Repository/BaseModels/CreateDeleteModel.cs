using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.BaseModels
{
    public class CreateDeleteModel:CreateModel
    {  
        [SugarColumn(ColumnDescription = "是否已删除")]
        public bool IsDeleted { get; set; }
    }
}
