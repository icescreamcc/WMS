using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("PlanMaterialRequirementOrder", "物料需求计划")]
    public class PlanMaterialRequirementOrder:CreateModifyModel
    {
        [SugarColumn(Length =50,IsPrimaryKey =true, ColumnDescription = "单号")]
        public string OrderNo { get; set; }

        [SugarColumn(ColumnDescription = "计划订单ID")]
        public int PlanId { get; set; }

        [SugarColumn(ColumnDescription = "年")]
        public int Year { get; set; }

        [SugarColumn(ColumnDescription = "周")]
        public int Week { get; set; }

        [SugarColumn(ColumnDescription = "发布计划版本")]
        public float Version { get; set; }

        [SugarColumn(Length = 20, ColumnDescription = "物料分类")]
        public string GoodsClassifyGroup { get; set; }

        [SugarColumn(IsNullable = true, Length = 100, ColumnDescription = "供应商")]
        public string SupplierId { get; set; }
         
        [SugarColumn(ColumnDescription = "是否已发送邮件")]
        public bool IsSendMail { get; set; } 

        [SugarColumn(IsNullable = true, Length = 200, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "状态")]
        public string Status { get; set; }
    }
}
