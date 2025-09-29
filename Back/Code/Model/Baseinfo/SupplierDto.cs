using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class SupplierDto
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
         
        public string Address { get; set; }
          
        public bool IsImportant { get; set; }
         
        public string Remark { get; set; }
         
        public string CreateUser { get; set; }
         
        public string CreateDate { get; set; }
         
        public string Consignee { get; set; }
         
        public string ConsigneeTel { get; set; }
         
        public string ConsigneeAddress { get; set; }

        public bool IsValid { get; set; }
         
        public string SpareField1 { get; set; }
         
        public string SpareField2 { get; set; }
         
        public string SpareField3 { get; set; }
         
        public string SpareField4 { get; set; }
         
        public string SpareField5 { get; set; }

        public List<SupplierAccountCreditedDto> AccountDetails { get; set; }

        public List<SupplierContactDto> ContactDetails { get; set; }
    }
}
