using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class StorageWarehouseDetail
    {
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

        public string GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsNo { get; set; }

        public float Stock { get; set; }

        public string UnitId { get; set; }

        public string UnitName { get; set; } 
    }
}
