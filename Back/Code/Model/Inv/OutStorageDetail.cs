using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class OutStorageDetail
    {
        public string GoodsClassify { get; set; }
        public string OutStorageType { get; set; }
        public int DetailId { get; set; }
         
        public string OrderNo { get; set; }
         
        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsLevel { get; set; }

        public string GoodsProperty { get; set; }

        public float PackageCount { get; set; }

        public float MaxPackageCount { get; set; }

        public string PackageUnitName { get; set; }

        public string MinPackageUnitName { get; set; }

        public string MaxPackageUnitName { get; set; }

        public string GoodsPicture { get; set; }

        public float Quantity { get; set; }

        public float ActualQuantity { get; set; }
         
        public int UnitId { get; set; }

        public string UnitName { get; set; }

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

        public int WorkbinCellId { get; set; }

        public string WorkbinCellNo { get; set; }

        public string Remark { get; set; }

        public double TotalPrice { get; set; }

        public double UnitPrice { get; set; }

        public string PriceUnit { get; set; }
    }
}
