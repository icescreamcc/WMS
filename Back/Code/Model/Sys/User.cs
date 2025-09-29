using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class User
    {
        public string UserId { get; set; }
         
        public string UserName { get; set; }
         
        public string NickName { get; set; }

        public string UserCode { get; set; }
           
        public string DomainName { get; set; }
         
        public string AuthAccount { get; set; }

        public string DeptId { get; set; }

        public string DeptName { get; set; }

        public string Email { get; set; }

        public string Wechat { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public bool IsVaild { get; set; }

    }
}
