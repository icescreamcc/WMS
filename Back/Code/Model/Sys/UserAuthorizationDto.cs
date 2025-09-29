using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class UserAuthorizationDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string DeptId { get; set; }

        public string DeptName { get; set; }

        public string Email { get; set; }

        public string AccessToken { get; set; }

        public string CardId { get; set; }

        public bool IsVaild { get; set; }

        /// <summary>
        /// 系统信息
        /// </summary>
        public List<Args> SystemInfo { get; set; }

    }
}
