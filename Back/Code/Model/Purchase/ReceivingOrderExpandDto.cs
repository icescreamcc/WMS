using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Purchase
{
    public class ReceivingOrderExpandDto
    {
        public string OrderNo { get; set; }

        public string ExternalOrderNo { get; set; }

        public string SupplierId { get; set; }

        public string SupplierName { get; set; }

        public float TotalPrice { get; set; }

        public int PriceUnitId { get; set; } 

        public string PriceUnitName { get; set; }

        public string ReceivingLevel { get; set; }

        public string ReceivingLevelDesc { get; set; }

        public DateTime DownTime { get; set; }

        public DateTime ExpectDate { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public bool IsMakeInvoice { get; set; }

        public string InvoiceNumber { get; set; }

        public bool IsAccountPaid { get; set; }

        public float PaymentAmount { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateDate { get; set; }

        public int YeareAndMonth { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }

        public string UpdateDate { get; set; }

        public string Remark { get; set; }

        public string ReceivingAbnormalDesc { get; set; }

        public bool IsASN { get; set; }

        public string ASNCheckStatus { get; set; }

        public string WaybillNo { get; set; }

        public string ReceivingResponsableUserId { get; set; }
         
        public string ReceivingResponsableUserName { get; set; }
         
        public string ReceivingResponsableUserEmail { get; set; }
          
        public bool IsAutoEmailToDeliver { get; set; }

        public string ReceivingAddress { get; set; }
          
        public bool IsEmailNotification { get; set; }

        public string GoodsClassify { get; set; }

        public string ApprovalDate { get; set; }

        public string ApproverId { get; set; }

        public string ApproverName { get; set; }

        public string ApproverRole { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalStatusDesc { get; set; }

        public bool IsInBaseFiles { get; set; }

        public int ApprovalLastRank { get; set; }

        public bool IsApproval { get; set; }

        public int DetialId { get; set; }
          
        public string ReceivingAbnormalType { get; set; }

        public string ReceivingAbnormalTypeName { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsModel { get; set; }

        public float Quantity { get; set; }

        public float QuantityActual { get; set; }

        public float QuantityUrgency { get; set; }

        public int QuantityUnitId { get; set; }
         
        public string QuantityUnitName { get; set; }
         
        public float Price { get; set; }
         
        public int WorkpieceTray { get; set; }
         
        public int Pallet { get; set; }
         
        public float DetailTotalPrice { get; set; }
         
        public string DetailStatus { get; set; }

        public string DetailStatusDesc { get; set; }

        public DateTime ReceivingDate { get; set; }

        public string ReceivingOperatorId { get; set; }

        public string ReceivingOperatorName { get; set; }

        public int AbnormalDeliveryPallet { get; set; }

    }
}
