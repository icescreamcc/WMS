using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class ClientDetail
    {
        public string ClientId { get; set; }
         
        public string ClientNo { get; set; }
         
        public string ClientName { get; set; }
         
        public int ClientTypeId { get; set; }
         
        public string ClientTypeName { get; set; }
         
        public string ClientProperty { get; set; }
         
        public string ClientLevel { get; set; }
         
        public string Province { get; set; }
         
        public string City { get; set; }
         
        public string Address { get; set; }
         
        public string Telephone { get; set; }
         
        public string Mobilephone { get; set; }
         
        public string Email { get; set; }
         
        public string Wechat { get; set; }
         
        public string Wangwang { get; set; }
         
        public string Alipay { get; set; }
         
        public string CommonContact { get; set; }
         
        public bool IsImportant { get; set; }
         
        public string Remark { get; set; } 

        public string CreateUser { get; set; }
         
        public string CreateDate { get; set; }

        public string Consignee { get; set; }

        public string ConsigneeTel { get; set; }

        public string ConsigneeAddress { get; set; }

        public string SpareField1 { get; set; }
         
        public string SpareField2 { get; set; }
         
        public string SpareField3 { get; set; }
         
        public string SpareField4 { get; set; } 

        public string SpareField5 { get; set; }
    }
}
