using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseBOM", "BOM信息表")]
    public class BaseBOM
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "物品ID")]
        public string MaterialId { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "物品名称")]
        public string MaterialName { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "物品分类")]
        public string MaterialClassifyGroup { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "父级ID")]
        public string ParentId { get; set; }

        [SugarColumn(ColumnDescription = "所需数量")]
        public float Quantity { get; set; }

        [SugarColumn(ColumnDescription = "单位")]
        public string Unit { get; set; }
    }
}
