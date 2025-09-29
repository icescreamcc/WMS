using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class WorkbinSpecificationDto
    {
        public int SpecId { get; set; }
           
        public string SpecName { get; set; }
         
        public string Size { get; set; }
         
        public string LoadWeight { get; set; }
         
        public int CellCount { get; set; }

        public int Rank { get; set; }
    }
}
