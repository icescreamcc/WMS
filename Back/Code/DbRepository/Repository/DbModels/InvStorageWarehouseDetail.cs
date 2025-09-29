using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvStorageWarehouseDetail", "库存明细表")]
    public class InvStorageWarehouseDetail
    { 
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "商品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架ID")]
        public string ShelfId { get; set; }

        [SugarColumn( IsPrimaryKey = true, ColumnDescription = "货位ID")]
        public int BinId { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "料箱ID")]
        public int WorkbinId { get; set; }

        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "料箱单元格ID")]
        public int WorkbinCellId { get; set; }

        [SugarColumn(ColumnDescription = "库存量")]
        public float Stock { get; set; }

        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "库存单位Id")]
        public int UnitId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, IsIgnore = true, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
