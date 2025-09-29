using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvSafetyWarningRecord", "安全库存报警记录表")]
    public class InvSafetyWarningRecord: CreateModifyModel
    {
        [SugarColumn(IsIdentity =true, IsPrimaryKey =true)]
        public int DetailId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "流程ID")]
        public string FlowId { get; set; }

        [SugarColumn(ColumnDescription = "年")]
        public int Year { get; set; }

        [SugarColumn(ColumnDescription = "月")]
        public int Month { get; set; }

        [SugarColumn(ColumnDescription = "周")]
        public int Week { get; set; }
         
        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(IsIgnore =true)]
        public string GoodsClassifyGroup { get; set; }
         
        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "状态")]
        public string Status { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "采购单号")]
        public string PurchaseOrderNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "需求数量")]
        public float RequirementQuantity { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "是否需要采购")]
        public string NeedPurchase { get; set; }

        [SugarColumn( ColumnDescription = "是否需要采购")]
        public bool IsNeedPurchase { get; set; }

        [SugarColumn(ColumnDescription = "是否已发送邮件")]
        public bool IsSendMail { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "备注")] 
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "审批时间",IsNullable =true)]
        public DateTime ApprovalDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批人ID")]
        public string ApproverId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批人姓名")]
        public string ApproverName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批人角色")]
        public string ApproverRole { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批状态")]
        public string ApprovalStatus { get; set; }
    }
}
