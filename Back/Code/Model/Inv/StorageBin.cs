using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class StorageBin
    {
        public string WarehouseId { get; set; }

        public string ShelfId { get; set; }

        public string ShelfNo { get; set; }

        public int BinId { get; set; }

        public string BinNo { get; set; }

        public int WorkbinId { get; set; }

        public int CellId { get; set; }

        public string CellNo { get; set; }

        public bool IsVarietyStock { get; set; }

        public bool IsTakeStockLock { get; set; }
    }
}
