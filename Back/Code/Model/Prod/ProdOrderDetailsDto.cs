using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Prod
{
    public class ProdOrderDetailsDto
    {
        public string DeliverNo { get; set; }

        public int DetailId { get; set; }

        public string DetailNo { get; set; }

        public string CarSoleCode { get; set; }

        public int CarRank { get; set; } 

        public double PlanTotalByCar { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }
    }
}
