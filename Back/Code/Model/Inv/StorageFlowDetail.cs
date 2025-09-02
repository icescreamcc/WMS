using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
   public class StorageFlowDetail
    {
        public int FlowId { get; set; }
         
        public string GoodsId { get; set; }

        public string GoodsNo { get; set; }

        public string GoodsName { get; set; }

        public string GoodsClassifyName { get; set; }

        public string GoodsTypeName { get; set; }

        public string GoodsModel { get; set; }

        public string GoodsLevel { get; set; }

        public string GoodsProperty { get; set; }
         
        public string FlowType { get; set; }

        public string FlowTypeDesc { get; set; }

        public string SourceOrderNo { get; set; }
         
        public string SourceStorageType { get; set; }

        public string SourceStorageTypeDesc { get; set; }

        public string SourceStorageSubType { get; set; }

        public string SourceStorageSubTypeDesc { get; set; }

        public float Quantity { get; set; }

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

        public int CellId { get; set; }

        public string CellNo { get; set; }

        public string OperateDate { get; set; }
         
        public string OperatorId { get; set; }
         
        public string OperatorName { get; set; }

        public bool IsStatistics { get; set; }
    }
}
