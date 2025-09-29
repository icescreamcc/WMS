using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Purchase
{
    public class ReceivingOrderDto
    {
        public string OrderNo { get; set; }
         
        public float TotalPrice { get; set; }

        public int PriceUnitId { get; set; }

        public string PriceUnitName { get; set; }
         
        public DateTime ExpectDate { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }
         
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

        public bool IsAutoEmailToReceiving { get; set; }

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

        public int ApprovalLastRank { get; set; }

        public bool IsApproval { get; set; }

        public List<ReceivingOrderDetailDto> Details { get; set; }
    }
}
