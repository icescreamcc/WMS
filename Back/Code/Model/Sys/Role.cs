using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
  public  class Role
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleNo { get; set; }

        public string ParentId { get; set; }

        public string ParentType { get; set; }

        public int Rank { get; set; }

        public string DeptId { get; set; }

        public bool IsVaild { get; set; }

    }
}
