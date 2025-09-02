using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("LabelPrintRecord", "标签打印记录")]
    public class LabelPrintRecord:CreateModel
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "包装ID")]
        public string PackageId { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "标签ID")]
        public string LabelId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品分组")]
        public string  GoodsClassifyGroup { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品名称")]
        public string GoodsName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品编码")]
        public string GoodsNo { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "物品型号")]
        public string GoodsModel { get; set; }

        [SugarColumn(ColumnDescription = "标准包装数量")]
        public float PackageCount { get; set; }

        [SugarColumn(Length = 20, ColumnDescription = "标准包装单位")]
        public string PackageUnitName { get; set; }

    }
}
