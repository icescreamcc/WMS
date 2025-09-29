using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class Bin
    {
        public int BinId { get; set; }
         
        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public string ShelfId { get; set; }

        public string ShelfName { get; set; }

        public string BinNo { get; set; }
         
        public string BinName { get; set; }
         
        public string AGVNo { get; set; }
         
        public double Long { get; set; }
         
        public double Width { get; set; } 

        public string Property { get; set; }

        public string Specification { get; set; }

        public string Remark { get; set; }

        public bool IsAbandon { get; set; }

        public bool IsVarietyStock { get; set; }

        public int Rank { get; set; }

        public string Status { get; set; }
    }
}
