using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Models.Model.Purchase
{
    public class SendingOrderExpandDto
    {
        public string OrderNo { get; set; }

        public DateTime SendingDate { get; set; }

        public DateTime RequestDate { get; set; }

        public string PackageUnitName { get; set; }

        public string Status { get; set; }

        public string IsUrgentShipment { get; set; }

        public string IsSufficientStock { get; set; }

        public int YearAndMonth { get; set; }

        public bool IsInBaseFiles { get; set; }

        public string SpecialRequest { get; set; }

        public string Remark { get; set; }

        public string SendingResponsableUserId { get; set; }

        public string SendingResponsableUserName { get; set; }

        public string SendingResponsableUserEmail { get; set; }

        public string ReceivingResponsableUserInfo { get; set; }

        public string SendingAddress { get; set; }

        public string GoodsClassify { get; set; }

        public string GoodsClassifyName { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }

        public DateTime UpdateDate { get; set; }

        public string SupplierId { get; set; }

        public string SupplierName { get; set; }

        /// <summary>
        /// 运输供应商联系人邮箱
        /// </summary>
        public string SendingSupplierUserEmail { get; set; }

        public bool IsEmailNotification { get; set; }

        public int DetialId { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string CustomerGoodsNo { get; set; }

        public string CustomerIdentificationCode { get; set; }

        public float Quantity { get; set; }

        public float PalletsQuantity { get; set; }

        public int QuantityUnitId { get; set; }

        public string QuantityUnitName { get; set; }

        public string DetailStatus { get; set; }

        public string GoodsName { get; set; }


        public string StatusDesc { get; set; }

        public string DetailStatusDesc { get; set; }

        public string UrgentShipmentDesc { get; set; }

        public string SufficientStockDesc { get; set; }

        //public string GoodsClassifyName { get; set; }

        public float PackageCount { get; set; }

        public float MaxPackageCount { get; set; }
    }
}
