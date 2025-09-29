using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class InvSafetyWarningRecordDto
    {
        public int DetailId { get; set; }

        public string FlowId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Week { get; set; }
          
        public string Supplier { get; set; }

        public string GoodsId { get; set; }
         
        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string GoodsClassifyTypeName { get; set; }
         
        public string GoodsModel { get; set; }
         
        public string AreaName { get; set; } 

        public bool HasInventory { get; set; }

        public float CurInvenstory { get; set; }

        public float SafetyInventory { get; set; }
         
        public string SafetyInventoryUnitName { get; set; }
         
        public float PurchaseMinimum { get; set; }
         
        public string PurchaseMinimumUnitName { get; set; }
         
        public float CostPrice { get; set; }
         
        public float CostTotal { get; set; }
         
        public string CostPriceUnitName { get; set; }

        public string PriceUnitName { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string PurchaseOrderNo { get; set; }
         
        public float RequirementQuantity { get; set; }
         
        public bool IsNeedPurchase { get; set; }

        public string NeedPurchase { get; set; }

        public bool IsSendMail { get; set; }

        public string Remark { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }

        public bool IsValid { get; set; }

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

        public List<ApprovalHisModel> ApprovalHis { get; set; }
    }
}
