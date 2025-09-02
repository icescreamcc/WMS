using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class StorageDetailDto
    {
        public int StorageId { get; set; }

        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsTypeName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsLevel { get; set; }

        public string GoodsProperty { get; set; }

        public string SupplierNo { get; set; }

        public string SupplierName { get; set; }

        public float PackageCount { get; set; }

        public float MaxPackageCount { get; set; }

        public string PackageUnitName { get; set; }

        public string MinPackageUnitName { get; set; }

        public string MaxPackageUnitName { get; set; }

        public float CostPrice { get; set; }

        public float RefPurchPrice { get; set; }

        public float RefSellingPrice { get; set; }

        public float SafetyInventory { get; set; }

        public string SafetyInventoryUnitName { get; set; }

        public string LastStatisticsDate { get; set; }
         
        public string LastOperatorId { get; set; }

        public string LastOperatorName { get; set; }

        public string LastInventoryDate { get; set; }

        public string LastInventoryOperatorId { get; set; }

        public string LastInventoryOperatorName { get; set; }

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

        public float Stock { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; }
    }
}
