using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Prod
{
    public class MatchingCarDto
    {
        public int MatchingId { get; set; }
         
        public string DeliverNo { get; set; }

        public string ConsignNum { get; set; }

        public string Prodct { get; set; }
         
        public int CountByCar { get; set; }
         
        public int MatchingCount { get; set; }
         
        public double Total { get; set; }

        public double TotalPutout { get; set; }

        public string UnitName { get; set; }

        public string CarSoleCode { get; set; }
         
        public int MatchingNum { get; set; }
         
        public string MatchingCode { get; set; }
         
        public double PlanTotalByCar { get; set; }
         
        public double ActualTotalByCar { get; set; }

        public int BinId { get; set; }
         
        public string BinNo { get; set; }

        public int UnstackBinId { get; set; }

        public string UnstackBinNo { get; set; }

        public bool IsPackage { get; set; }

        public bool IsMatch { get; set; }

        public DateTime MatchDate { get; set; }
         
        public DateTime PackageDate { get; set; }

        public string Status { get; set; }
    }
}
