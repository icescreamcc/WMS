using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Prod
{
    public class ProdOrdersDto
    {
        public int OrderId { get; set; }
         
        public string DeliverNo { get; set; }

        public string ConsignNum { get; set; }

        public string ProdctionTypeNo { get; set; }
         
        public string ProductName { get; set; }
         
        public int MatchingCount { get; set; }
         
        public double Total { get; set; }

        public double TotalPutout { get; set; }

        public double TotalByCar { get; set; }

        public int CountByCar { get; set; }

        public string UnitName { get; set; }
         
        public string Line { get; set; }
         
        public DateTime CreateDate { get; set; }
         
        public string CreateUser { get; set; }

        public DateTime ModifyDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime PrintDate { get; set; } 

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string Remark { get; set; }

        public List<ProdOrderDetailsDto> Details { get; set; }
    }
}
