using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public  class RequisitionOrderDto
    {
        public string OrderNo { get; set; }
         
        public string OrderType { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string Line { get; set; }
         
        public string Purpose { get; set; }
         
        public string Remark { get; set; }
         
        public string Status { get; set; }

        public string CreateUserId { get; set; }
         
        public string CreateUserName { get; set; }

        public string CreateUserCard { get; set; }
         
        public DateTime CreateDate { get; set; }

        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }
         
        public DateTime UpdateDate { get; set; }

        public bool UseAgv { get; set; }

        public List<RequisitionOrderDetailDto> Details { get; set; }
    }
}
