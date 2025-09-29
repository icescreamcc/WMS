using System;

namespace Models.Model.Purchase
{
    public class SendingOrderDetailDto
    {
        public int DetialId { get; set; }

        public string OrderNo { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string CustomerGoodsNo { get; set; }

        public string CustomerIdentificationCode { get; set; }

        public float Quantity { get; set; }
        public float? ActualQuantity { get; set; }

        public float PalletsQuantity { get; set; }

        public int QuantityUnitId { get; set; }

        public string QuantityUnitName { get; set; }

        public string DetailStatus { get; set; }

        public string GoodsName { get; set; }


    }
}
