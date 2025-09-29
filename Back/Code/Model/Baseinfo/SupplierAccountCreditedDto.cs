using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class SupplierAccountCreditedDto
    {
        public int AccountId { get; set; }
         
        public string SupplierId { get; set; }
         
        public int AccountTypeId { get; set; }

        public string AccountTypeName { get; set; }

        public string AccountName { get; set; }
         
        public string AccountNumber { get; set; }
         
        public bool IsCommonAccount { get; set; }
         
        public string OpeningBank { get; set; }
         
        public string Remark { get; set; }
    }
}
