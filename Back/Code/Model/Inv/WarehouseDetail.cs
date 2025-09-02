using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class WarehouseDetail
    {
        public string WarehouseId { get; set; }
         
        public string WarehouseNo { get; set; }
         
        public string WarehouseName { get; set; }

        public string WarehouseType { get; set; }

        public string ChargePerson { get; set; }
         
        public string ChargePersonPhone { get; set; }

        public string Province { get; set; }
         
        public string City { get; set; }

        public string Address { get; set; }
         
        public bool IsAbandon { get; set; }
         
        public string InventoryDateOfLast { get; set; }

        public string InventoryOperatorIdOfLast { get; set; }
         
        public string InventoryOperatorNameOfLast { get; set; }

        public string Remark { get; set; }
         
        public string SpareField1 { get; set; }
         
        public string SpareField2 { get; set; }
         
        public string SpareField3 { get; set; }
         
        public string SpareField4 { get; set; }
         
        public string SpareField5 { get; set; }
         
    }
}
