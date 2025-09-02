using Models.Model.Baseinfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class BinTakeStockDto
    {
        public int BinId { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public string ShelfId { get; set; }

        public string ShelfName { get; set; }

        public string BinNo { get; set; }

        public string BinName { get; set; }

        public int WorkbinId { get; set; }

        public string WorkbinNo { get; set; }

        public int SpecId { get; set; }

        public string SpecName { get; set; }

        public bool HasWorkbin { get; set; }

        public string Property { get; set; }

        public string Specification { get; set; }

        public string Remark { get; set; } 

        public int Rank { get; set; }

        public bool IsTakeStockLock { get; set; }

        public string LastInventoryDate { get; set; }

        public string LastInventoryOperator { get; set; }

        public bool IsTaskStockCurDate { get; set; }
    }
}
