using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class PermissionMenus
    {
        public string MenuId { get; set; }

        public string MenuName { get; set; }

        public string MenuType { get; set; }

        public int Rank { get; set; }

        public bool IsAuth { get; set; }

        public string ParentId { get; set; }

        public List<PermissionMenus> MenuChildren { get; set; }
    }
}
