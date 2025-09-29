using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("LabelDesign", "物料标签")]
    public class LabelDesign: CreateModifyModel
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "标签ID")]
        public string LabelId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "标签名称")]
        public string LabelName { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物料类型分组")]
        public string GoodsClassifyGroup { get; set; }

        [SugarColumn( ColumnDescription = "宽度mm")]
        public float Width { get; set; }

        [SugarColumn(ColumnDescription = "高度mm")]
        public float Height { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "背景色")]
        public string BackgroundColor { get; set; }

        [SugarColumn(ColumnDescription = "是否默认")]
        public bool IsDeft { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsValid { get; set; }

    }
}
