using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class BaseTypeDto
    {
        public int TypeId { get; set; }
         
        public string TypeNo { get; set; }
         
        public string TypeName { get; set; }
         
        public string Remark { get; set; }
         
        public int ParentId { get; set; } 

        public int Rank { get; set; }

        public string Group { get; set; }
    }
} 
