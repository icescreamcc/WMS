using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class OutStorage
    {
        public string OrderNo { get; set; }

        public string SourceOrderNo { get; set; }

        public string OutStorageType { get; set; }

        public string OutStorageTypeDesc { get; set; }

        public string GoodsClassify { get; set; }

        public string GoodsClassifyDesc { get; set; }

        public string LineNo { get; set; }

        public string LineName { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string CreateUserId { get; set; }
         
        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }
         
        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }
         
        public DateTime UpdateDate { get; set; }
         
        public DateTime OutStorageDate { get; set; }
         
        public string Remark { get; set; }

        public bool IsApproval { get; set; }

        public DateTime ApprovalDate { get; set; }
         
        public string ApproverId { get; set; }
         
        public string ApproverName { get; set; }
         
        public string ApproverRole { get; set; }
         
        public string ApprovalStatus { get; set; } 

        public string ApprovalStatusDesc { get; set; }

        public int ApprovalLastRank { get; set; }
        public string GoodsName { get; set; }
        public float Quantity { get; set; }
        public float ActualQuantity { get; set; }
        public string UnitName { get; set; }
        public List<OutStorageDetail> Details { get; set; }
    }
}
