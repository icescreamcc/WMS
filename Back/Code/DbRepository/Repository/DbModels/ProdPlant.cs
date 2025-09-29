using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdPlant", "工厂信息")]
    public class ProdPlant
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50)]
        public string PlantNo { get; set; }

        [SugarColumn( Length = 50)]
        public string PlantName { get; set; }
    }
}
