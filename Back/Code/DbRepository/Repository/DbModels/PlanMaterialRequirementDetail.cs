using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("PlanMaterialRequirementDetail", "物料需求计划-明细")]
    public class PlanMaterialRequirementDetail
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int DetailId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50,IsIgnore =true, ColumnDescription = "成品型号")]
        public string ProductNo { get; set; }
          
        [SugarColumn( Length = 20, ColumnDescription = "物料分类")]
        public string GoodsClassifyGroup { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "物料ID")]
        public string GoodsId { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "物料编码")]
        public string GoodsNo { get; set; }

        [SugarColumn(IsNullable = true, Length = 100, ColumnDescription = "物料名称")]
        public string GoodsName { get; set; }

        [SugarColumn(ColumnDescription = "需求数量")]
        public float Quantity { get; set; }

        [SugarColumn(IsNullable = true, Length = 10, ColumnDescription = "单位")]
        public string UnitName { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "需求等级")]
        public string RequirementLevel { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "需求班次")]
        public string Shift { get; set; }

        [SugarColumn(ColumnDescription = "需求日期")]
        public DateTime ReqDate { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "状态")]
        public string  Status { get; set; }
    }
}
