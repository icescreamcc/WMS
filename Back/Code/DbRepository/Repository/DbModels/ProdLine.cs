using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdLine", "产线信息")]
    public class ProdLine
    {
        [SugarColumn(Length = 50)]
        public string PlantNo { get; set; }

        [SugarColumn( Length = 50)]
        public string AreaNo { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 50)]
        public string LineNo { get; set; }

        [SugarColumn(Length = 50)]
        public string LineName { get; set; }
    }
}
