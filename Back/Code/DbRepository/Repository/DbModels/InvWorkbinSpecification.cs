using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvWorkbinSpecification", "料箱规格信息表")]
    public class InvWorkbinSpecification
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int SpecId { get; set; } 

        [SugarColumn(Length = 50)]
        public string SpecName { get; set; }

        [SugarColumn(Length = 50,IsNullable =true)]
        public string Size { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string LoadWeight { get; set; }

        [SugarColumn(DefaultValue = "1")]
        public int CellCount { get; set; }

        public int Rank { get; set; }
    }
}
