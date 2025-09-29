using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Prod
{
    public class CarLoadDto
    {
        public string DetailNo { get; set; }

        public string ConsignNum { get; set; }

        public string CarSoleCode { get; set; }

        public string DeliverNo { get; set; }

        public string ProdctionTypeNo { get; set; }

        public int CountByCar { get; set; }

        public int MatchingCount { get; set; }

        public double Total { get; set; }

        public double PlanTotalByCar { get; set; }

        public double ActualTotal { get; set; }

        public string UnitName { get; set; }

        public int BinId { get; set; }

        public string BinNo { get; set; }

        public int UnstackBinId { get; set; } 

        public string UnstackBinNo { get; set; }

        public string ScanBinNo { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public string StatusDisplay { get; set; }
    }
}
