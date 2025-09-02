using System;
using System.Collections.Generic;

namespace Models.Model.Purchase
{
    public class SendingOrderDto
    {
        public string OrderNo { get; set; }

        public DateTime SendingDate { get; set; }

        public DateTime RequestDate { get; set; }

        public string Status { get; set; }

        public string IsUrgentShipment { get; set; }

        public string IsSufficientStock { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateDate { get; set; }

        public int YearAndMonth { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }

        public string UpdateDate { get; set; }

        public string SpecialRequest { get; set; }

        public string Remark { get; set; }

        public string SendingResponsableUserId { get; set; }

        public string SendingResponsableUserName { get; set; }

        public string SendingResponsableUserEmail { get; set; }

        public string ReceivingResponsableUserInfo { get; set; }

        public string SendingAddress { get; set; }

        public string GoodsClassify { get; set; }

        public string SupplierId { get; set; }

        public string SupplierName { get; set; }

        public bool IsEmailNotification { get; set; }

        public List<SendingOrderDetailDto> Details { get; set; }

        public string UrgentShipmentDesc { get; set; }

        public string SufficientStockDesc { get; set; }
    }
}
