using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdStation", "工位信息")]
    public class ProdStation
    {
        [SugarColumn(Length = 50)]
        public string PlantNo { get; set; }

        [SugarColumn(Length = 50)]
        public string AreaNo { get; set; }

        [SugarColumn( Length = 50)]
        public string LineNo { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 50)]
        public string StationNo { get; set; }

        [SugarColumn(Length = 100)]
        public string StationName { get; set; }
    }
}
