using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class Department
    {
        public string DeptId { get; set; }

        public string DeptName { get; set; }

        public string DeptNo { get; set; }

        public int Rank { get; set; }

        public bool IsVaild { get; set; }

        public string CompanyId { get; set; }
    }
}
