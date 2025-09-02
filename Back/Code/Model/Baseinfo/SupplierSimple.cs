using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class SupplierSimple
    {
        public string SupplierId { get; set; }

        public string SupplierNo { get; set; }

        public string SupplierName { get; set; }

        public int SupplierTypeId { get; set; } 

        public string SupplierTypeName { get; set; }

        public int SupplierPropertyId { get; set; }

        public string SupplierPropertyName { get; set; }

        public string Province { get; set; }

        public string City { get; set; }
    }
}
