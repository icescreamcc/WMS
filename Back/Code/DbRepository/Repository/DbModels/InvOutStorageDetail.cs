using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvOutStorageDetail", "出库单明细表")]
   public class InvOutStorageDetail
    {
        [SugarColumn( IsPrimaryKey = true,IsIdentity =true, ColumnDescription = "明细ID")]
        public int DetailId { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "出库单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品名称")]
        public string GoodsName { get; set; }

        [SugarColumn( ColumnDescription = "出库数量")]
        public float Quantity { get; set; }

        [SugarColumn(ColumnDescription = "实际出库数量")]
        public float ActualQuantity { get; set; }

        [SugarColumn( ColumnDescription = "出库单位ID")]
        public int UnitId { get; set; } 

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "出库仓库")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "出库货架")]
        public string ShelfId { get; set; }

        [SugarColumn(ColumnDescription = "出库库位")]
        public int BinId { get; set; }

        [SugarColumn(ColumnDescription = "胶箱")]
        public int WorkbinId { get; set; }

        [SugarColumn(ColumnDescription = "胶箱单元格")]
        public int WorkbinCellId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "备注类型")]
        public string RemarkType { get; set; }

        [SugarColumn(Length = 500,IsNullable =true, ColumnDescription = "出库明细备注")]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "入库总价")]
        public double TotalPrice { get; set; }

        [SugarColumn(ColumnDescription = "入库单价")]
        public double UnitPrice { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "价格单位")]
        public string PriceUnit { get; set; }
    }
}
