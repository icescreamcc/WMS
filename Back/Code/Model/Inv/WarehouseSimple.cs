using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class WarehouseSimple
    {
        public string WarehouseId { get; set; }

        public string WarehouseNo { get; set; }

        public string WarehouseName { get; set; }

        public string WarehouseType { get; set; }

        public string ChargePerson { get; set; } 

        public string Province { get; set; }

        public string City { get; set; } 

        public bool IsAbandon { get; set; }
    }
}
