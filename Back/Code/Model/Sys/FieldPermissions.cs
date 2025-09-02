using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class FieldPermissions
    {
        public string FieldManageId { get; set; }

        public string TableName { get; set; }
         
        public string TableDesc { get; set; }
         
        public string FieldName { get; set; }
         
        public string FieldDesc { get; set; }

        public List<FieldsPermissionsRole> PermissionsRoles { get; set; }

    }
}
