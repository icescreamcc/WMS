using Models.Model.Baseinfo;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class InStorage
    {
        public List<FileInfoDto> GoodsPicture { get; set; }
        public string OrderNo { get; set; }

        public string SourceOrderNo { get; set; }

        public string InStorageType { get; set; }
         
        public string InStorageTypeDesc { get; set; }

        public string GoodsClassify { get; set; }

        public string GoodsClassifyDesc { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public string ResponsibleId { get; set; }
         
        public string Responsible { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string CreateUserId { get; set; }
         
        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }

        public DateTime InstorageDate { get; set; }

        public string Remark { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }

        public bool IsApproval { get; set; }

        public int ApprovalLastRank { get; set; }

        public int ApproverRank { get; set; }

        public DateTime ApprovalDate { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalStatusDesc { get; set; }
        public string TransportOrderNo { get; set; }
        public string LicensePlateNo { get; set; }

        public List<InStorageDetail> Details { get; set; }
    }
}
