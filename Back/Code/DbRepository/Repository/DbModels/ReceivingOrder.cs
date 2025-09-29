using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ReceivingOrder", "收货计划主表")]
    public class ReceivingOrder:CreateModifyModel
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "收货单号")]
        public string OrderNo { get; set; }
         
        [SugarColumn(ColumnDescription = "收货总价")]
        public float TotalPrice { get; set; }

        [SugarColumn(ColumnDescription = "计价单位ID")]
        public int PriceUnitId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "计价单位名称")]
        public string PriceUnitName { get; set; }
          
        [SugarColumn( ColumnDescription = "预期到货日期")]
        public DateTime ExpectDate { get; set; }
         
        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "单据状态")]
        public string Status { get; set; }
          
        [SugarColumn(ColumnDescription = "是否已付款")]
        public bool IsAccountPaid { get; set; }

        [SugarColumn(ColumnDescription = "已付款金额")]
        public float PaymentAmount { get; set; }
         
        [SugarColumn(ColumnDescription = "收货年月")]
        public int YeareAndMonth { get; set; } 

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "收货责任人ID")]
        public string ReceivingResponsableUserId { get; set; }
         
        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "收货人责任人名称")]
        public string ReceivingResponsableUserName { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "收货责任人邮箱")]
        public string ReceivingResponsableUserEmail { get; set; }

        [SugarColumn(ColumnDescription = "是否自动邮件通知收货责任人")]
        public bool IsAutoEmailToReceiving { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "收货地址")]
        public string ReceivingAddress { get; set; }
         
        [SugarColumn(ColumnDescription = "邮件通知状态")]
        public bool IsEmailNotification { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "物品分类")]
        public string GoodsClassify { get; set; }

        [SugarColumn( ColumnDescription = "审批时间")]
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
