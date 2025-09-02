using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Purchase
{
    public class ReceivingOrderDetailDto
    { 
        public string OrderNo { get; set; }

        public int DetialId { get; set; }
         
        public string ReceivingAbnormalType { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsModel { get; set; }

        public string ExternalOrderNo { get; set; }

        public string SupplierId { get; set; }

        public string SupplierName { get; set; }

        public bool IsASN { get; set; }

        public string ASNCheckStatus { get; set; }

        public string ReceivingLevel { get; set; }

        public string ReceivingLevelDesc { get; set; }

        public string ReceivingAbnormalDesc { get; set; }

        public DateTime? DownTime { get; set; }

        public bool IsMakeInvoice { get; set; }

        public string InvoiceNumber { get; set; }

        public string WaybillNo { get; set; }

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
