using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class UserDetail
    { 
        public string UserId { get; set; }
         
        public string UserName { get; set; }

        public string DomainName { get; set; }

        public string NickName { get; set; }
         
        public string AuthAccount { get; set; }

        public string UserCode { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
         
        public string Wechat { get; set; }
         
        public string Phone { get; set; }
         
        public string MobilePhone { get; set; }
         
        public string Address { get; set; }
         
        public int ProvinceId { get; set; }

        public string ProvinceName { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public string DeptId { get; set; }

        public string DeptName { get; set; }

        public bool IsVaild { get; set; }
         
        public string Remark { get; set; }
         
        public string CreateDate { get; set; }
         
        public string DepartureDate { get; set; }

        public string EditDate { get; set; }

        public string AvatarImgId { get; set; }

        public string CardId { get; set; }
    }
}
