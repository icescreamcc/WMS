using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    /// <summary>
    /// 字段权限业务模型
    /// </summary>
   public class FieldPermission
    {  
        public string TableName { get; set; }
         
        public string TableDesc { get; set; } 
         
        public string FieldName { get; set; }
         
        public string FieldDesc { get; set; }

        public string RoleId { get; set; }
    }
}
