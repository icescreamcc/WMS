using Models.Model.Baseinfo;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Purchase
{
    public class PurchaseOrderDto
    {
        public int DetialId { get; set; }
         
        public string OrderNo { get; set; }
         
        public int PurchaseTypeId { get; set; }

        public string PurchaseTypeDesc { get; set; }

        public bool IsApplyMaterialNumber { get; set; }
         
        public bool IsPurchaseBuy { get; set; }

        public bool IsOnceBuy { get; set; }

        public string GoodsNameZH { get; set; }
         
        public string GoodsNameEN { get; set; }
         
        public string GoodsModel { get; set; }
         
        public string GoodsId { get; set; }
         
        public string GoodsNo { get; set; }
         
        public string ExternalOrderNo { get; set; }

        public string GRNo { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string NPMBuyer { get; set; }

        public string NPMBuyerEMail { get; set; }

        public string MNANo { get; set; }

        public string PRNumber { get; set; }

        public DateTime PRDate { get; set; }

        public string ArrivalStatus { get; set; }

        public string ArrivalStatusDesc { get; set; }

        public DateTime ArrivalDate { get; set; }

        public float QuantityArrival { get; set; }

        public string ReceivingStatus { get; set; }

        public string ReceivingStatusDesc { get; set; }

        public DateTime ReceivingDate { get; set; }

        public string ReceivingAbnormalReason { get; set; }

        public string ArrivalAbnormalReason { get; set; }

        public int GoodsSpecificationId { get; set; }
         
        public float MaxStock { get; set; }

        public int GoodsClassifyId { get; set; }

        public string GoodsClassifyName { get; set; }
         
        public float Quantity { get; set; }
         
        public float QuantityActual { get; set; }
         
        public int QuantityUnitId { get; set; }
         
        public string QuantityUnitName { get; set; }
         
        public float Price { get; set; }
         
        public float DetailTotalPrice { get; set; }
         
        public string PriceUnitName { get; set; }

        public string FlowStatus { get; set; }

        public string FlowStatusDesc { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string Description { get; set; }
         
        public DateTime ExpectDate { get; set; }
           
        public string Line { get; set; }

        public string LineNo { get; set; }

        public string ApplyReason { get; set; }
         
        public string SupplierId { get; set; }

        public string SupplierNo { get; set; }

        public string SupplierName { get; set; }

        public string Manufactor { get; set; }

        public string PMType { get; set; }

        public string CPMG { get; set; }

        public string CPMGDesc { get; set; }

        public string PONumber { get; set; }

        public DateTime PODate { get; set; }

        public string AccountNumber { get; set; }

        public string AccountNumberDesc { get; set; }

        public string ClassesNumber { get; set; }

        public string ClassesNumberDesc { get; set; }

        public string ClassesKey { get; set; }

        public float MinLotSize { get; set; }
          
        public float ReturnCycle { get; set; }

        public string QuoteLink { get; set; }
         
        public string Consignee { get; set; }

        public string ConsigneeOperator { get; set; }

        public string ConsigneeEmail { get; set; }
         
        public bool IsAutoEmailToReceiving { get; set; }
         
        public bool IsEmailNotification { get; set; }

        public float SafetyInventory { get; set; }

        public DateTime ApprovalDate { get; set; }
         
        public string ApproverId { get; set; }
         
        public string ApproverName { get; set; }
         
        public string ApproverRole { get; set; }
         
        public string ApprovalStatus { get; set; }

        public string ApprovalStatusDesc { get; set; }

        public bool IsApproval { get; set; }

        public int ApprovalLastRank { get; set; }

        public string ApprovalResult { get; set; }

        public string ApprovalOpinion { get; set; }

        public bool IsLastApproval { get; set; }

        public string CreateUserId { get; set; }
         
        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }
         
        public DateTime UpdateDate { get; set; }

        public string Remark { get; set; }

        public List<FileInfoDto> QuoteLinkFileList { get; set; }

        public List<FileInfoDto> GoodsPictureList { get; set; }

        public List<ApprovalHisModel> ApprovalHis { get; set; }
    }
}
