using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class AllocationOrder
    {
        public string InWarehouseId { get; set; }

        public string OutWarehouseId { get; set; }

        public string GoodsClassify { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; } 

        public string Remark { get; set; } 

        public List<AllocationOrderDetail> Details { get; set; }
    }
}
