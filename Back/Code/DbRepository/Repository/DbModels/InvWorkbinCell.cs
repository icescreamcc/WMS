using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvWorkbinCell", "料箱单元格信息表")]
    public class InvWorkbinCell
    {
        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架ID")]
        public string ShelfId { get; set; }

        [SugarColumn(ColumnDescription = "库位")]
        public int BinId { get; set; }

        [SugarColumn(ColumnDescription = "胶箱")]
        public int WorkbinId { get; set; }

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true, ColumnDescription = "单元格ID")]
        public int CellId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "单元格编号")]
        public string CellNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "单元格状态")]
        public string Status { get; set; }
    }
}
