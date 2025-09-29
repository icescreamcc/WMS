using Models.Model.Baseinfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Plan
{
    public class PlanMaterialRequirementOrderDto
    {
        public string OrderNo { get; set; }
         
        public int PlanId { get; set; }

        public int Year { get; set; }
         
        public int Week { get; set; }
         
        public float Version { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string SupplierId { get; set; }

        public string SupplierName { get; set; } 

        public bool IsSendMail { get; set; }
         
        public string Remark { get; set; }
         
        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string CreateUserId { get; set; }
         
        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }
         
        public DateTime UpdateDate { get; set; }

        public List<SupplierContactDto> SupplierContactList { get; set; }

        public List<PlanMaterialRequirementDetailDto> Details { get; set; } 
    }
}
