using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    public class PurchaseOrder: CreateModifyModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "明细ID")]
        public int DetialId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "采购单号")]
        public string OrderNo { get; set; }

        [SugarColumn(ColumnDescription = "采购方式")]
        public int PurchaseTypeId { get; set; }

        [SugarColumn(ColumnDescription = "是否申请料号")]
        public bool IsApplyMaterialNumber { get; set; }

        [SugarColumn(ColumnDescription = "是否采买")]
        public bool IsPurchaseBuy { get; set; }

        [SugarColumn(ColumnDescription = "是否一次性采买")]
        public bool IsOnceBuy { get; set; }


        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品编码(SAP编码)")]
        public string GoodsNo { get; set; }
         
        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "物品分类")]
        public string GoodsClassifyGroup { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "NPM采购")]
        public string NPMBuyer { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "NPM采购-邮箱")]
        public string NPMBuyerEMail { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品名称(中文)")]
        public string GoodsNameZH { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品名称(英文)")]
        public string GoodsNameEN { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "订货号")]
        public string ExternalOrderNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "GR号")]
        public string GRNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品型号")]
        public string GoodsModel { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "流程状态")]
        public string FlowStatus { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "数据状态")]
        public string Status { get; set; }
         
        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "MNA编号")]
        public string MNANo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "PO号")]
        public string PONumber { get; set; }

        [SugarColumn(ColumnDescription = "PO日期")]
        public DateTime PODate { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "PR号")]
        public string PRNumber { get; set; }

        [SugarColumn(ColumnDescription = "PR日期")]
        public DateTime PRDate { get; set; }

        [SugarColumn(ColumnDescription = "下单实际数量")]
        public float QuantityActual { get; set; }

        [SugarColumn(ColumnDescription = "单价")]
        public float Price { get; set; }

        [SugarColumn(ColumnDescription = "总价")]
        public float DetailTotalPrice { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "计价单位")]
        public string PriceUnitName { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "PO描述")]
        public string Description { get; set; }

        [SugarColumn(ColumnDescription = "物品存放规格")]
        public int GoodsSpecificationId { get; set; }

        [SugarColumn(ColumnDescription = "最大堆积数量")]
        public float MaxStock { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "实物到货状态")]
        public string ArrivalStatus { get; set; }

        [SugarColumn(ColumnDescription = "实物到货日期")]
        public DateTime ArrivalDate { get; set; }

        [SugarColumn(ColumnDescription = "实际到货数量")]
        public float QuantityArrival { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "实物到货异常原因")]
        public string ArrivalAbnormalReason { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "WK收货状态")]
        public string ReceivingStatus { get; set; }

        [SugarColumn(ColumnDescription = "WK收货日期")]
        public DateTime ReceivingDate { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "WK收货异常原因")]
        public string ReceivingAbnormalReason { get; set; }


        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "供应商ID")]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "供应商代码")]
        public string SupplierNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "供应商名称")]
        public string SupplierName { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "生产厂家")]
        public string Manufactor { get; set; }

        [SugarColumn(ColumnDescription = "下单数量")]
        public float Quantity { get; set; }
         
        [SugarColumn(ColumnDescription = "数量单位ID")]
        public int QuantityUnitId { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "数量单位")]
        public string QuantityUnitName { get; set; }
         
        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "报价链接")]
        public string QuoteLink { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "PM类型")]
        public string PMType { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "申请原因")]
        public string ApplyReason { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "CPMG")]
        public string CPMG { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "科目号")]
        public string AccountNumber { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "收货人(需求人)")]
        public string Consignee { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "收货人邮箱")]
        public string ConsigneeEmail { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "适用产线")]
        public string Line { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "产线编号")]
        public string LineNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "类别编号")]
        public string ClassesNumber { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "类别描述")]
        public string ClassesNumberDesc { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "类别关键词")]
        public string ClassesKey { get; set; }

        [SugarColumn(ColumnDescription = "物品类型ID")]
        public int GoodsClassifyId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品类型")]
        public string GoodsClassifyName { get; set; }

        [SugarColumn(ColumnDescription = "安全库存")]
        public float SafetyInventory { get; set; }

        [SugarColumn(ColumnDescription = "最小订货量")]
        public float MinLotSize { get; set; }

        [SugarColumn(ColumnDescription = "回货周期")]
        public float ReturnCycle { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }


        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "收货操作人")]
        public string ConsigneeOperator { get; set; }
          
        [SugarColumn(ColumnDescription = "邮件通知状态")]
        public bool IsEmailNotification { get; set; }

        [SugarColumn(ColumnDescription = "审批时间")]
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
