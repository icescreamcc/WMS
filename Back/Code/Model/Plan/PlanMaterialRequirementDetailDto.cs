using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Plan
{
    public class PlanMaterialRequirementDetailDto
    {
        public int DetailId { get; set; }
         
        public string OrderNo { get; set; } 
         
        public string GoodsClassifyGroup { get; set; }
         
        public string GoodsId { get; set; }
         
        public string GoodsNo { get; set; }
         
        public string GoodsName { get; set; }

        public string GoodsType { get; set; }

        public float Quantity { get; set; }
         
        public string UnitName { get; set; }

        public string RequirementLevel { get; set; }

        public string RequirementLevelDesc { get; set; }

        public string Shift { get; set; }
         
        public DateTime ReqDate { get; set; }
         
        public string Status { get; set; }
    }
}
