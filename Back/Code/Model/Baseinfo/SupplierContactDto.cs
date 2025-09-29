using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
    public class SupplierContactDto
    {
        public int ContacttId { get; set; }
         
        public string SupplierId { get; set; }
         
        public string ContactPerson { get; set; }
         
        public string Telephone { get; set; }
         
        public string Mobilephone { get; set; }
         
        public string Email { get; set; }
         
        public string Wechat { get; set; }
         
        public string Wangwang { get; set; }
         
        public string Alipay { get; set; }
         
        public bool IsDeft { get; set; }
    }
}
