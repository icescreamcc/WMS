using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class WorkbinCellDto
    {
        public string WarehouseId { get; set; }
         
        public string ShelfId { get; set; }
         
        public int BinId { get; set; }
         
        public int WorkbinId { get; set; }
         
        public int CellId { get; set; }
         
        public string CellNo { get; set; }
         
        public string Status { get; set; }
    }
}
