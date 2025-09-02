using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseUnits", "统计单位表")]
    public class BaseUnits
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true, ColumnDescription = "单位ID")]
        public int UnitId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "单位编码")]
        public string UnitNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "单位名称")]
        public string UnitName { get; set; }

        [SugarColumn(Length = 20, ColumnDescription = "单位类型")]
        public string UnitType { get; set; }

        [SugarColumn(Length = 100,IsNullable =true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "数据是否生效")]
        public bool IsValid { get; set; }
    }
}
