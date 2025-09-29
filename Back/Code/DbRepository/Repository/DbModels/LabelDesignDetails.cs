using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("LabelDesignDetails", "物料标签内容明细")]
    public class LabelDesignDetails
    {
        [SugarColumn( Length =50, ColumnDescription ="标签ID")]
        public string LabelId { get; set; }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ItemId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "名称")]
        public string ItemName { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "分类")]
        public string ItemType { get; set; }

        [SugarColumn( ColumnDescription = "是否已应用到模板")]
        public bool IsUsed { get; set; }

        [SugarColumn(Length = 1000,IsNullable =true, ColumnDescription = "默认值")]
        public string DeftValue { get; set; }

        [SugarColumn(ColumnDescription = "是否显示名称")]
        public bool ShowName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "值类型")]
        public string ItemValueType { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "绑定字段")]
        public string ItemValueField { get; set; }

        [SugarColumn(Length = 1000, IsNullable = true, ColumnDescription = "绑定字段")]
        public string ItemValueFieldInfo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "值的前缀")]
        public string ValuePerfix { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "值的后缀")]
        public string ValueSuffix { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "字段分隔符")]
        public string FieldSeparator { get; set; }

        [SugarColumn(Length = 1000, IsNullable = true, ColumnDescription = "格式化值")]
        public string ItemValueFormat { get; set; }

        [SugarColumn(Length = 1000, ColumnDescription = "样式")]
        public string Style { get; set; }

        [SugarColumn(Length = 1000, ColumnDescription = "其他参数")]
        public string Args { get; set; }

        [SugarColumn(Length = 500, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
