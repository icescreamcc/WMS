using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class RecommendWorkbinCellDto
    {
        public string GoodsId { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseNo { get; set; }

        public string WarehouseName { get; set; }

        public string ShelfId { get; set; }

        public string ShelfNo { get; set; }

        public string ShelfName { get; set; }

        public int BinId { get; set; }

        public string BinNo { get; set; }

        public string BinName { get; set; }

        public int WorkbinId { get; set; }

        public string WorkbinNo { get; set; }

        public int CellId { get; set; }

        public string CellNo { get; set; }

        public int SpecId { get; set; }

        public string SpecName { get; set; }

        public float MaxStock { get; set; }

        public int MaxStockUnitId { get; set; }

        public string MaxStockUnitName { get; set; }

        public float Stock { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set;}
    }
}
