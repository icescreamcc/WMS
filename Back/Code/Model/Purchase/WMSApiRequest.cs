using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Purchase
{
    public class WMSApiRequest
    {
        public string customNo { get; set; }

        public string itemCode { get; set; }

        public int qty { get; set; }

        public string operate { get; set; }

        public string itemAreaType { get; set; }
    }
}
