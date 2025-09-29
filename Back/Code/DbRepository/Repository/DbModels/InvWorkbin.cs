using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvWorkbin", "货架料箱信息表")]
    public class InvWorkbin
    { 
        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架ID")]
        public string ShelfId { get; set; }

        [SugarColumn(ColumnDescription = "库位")]
        public int BinId { get; set; }

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true, ColumnDescription = "料箱ID")]
        public int WorkbinId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "料箱编号")]
        public string WorkbinNo { get; set; }

        [SugarColumn( ColumnDescription = "料箱规格")]
        public int SpecId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "单元格状态")]
        public string Status { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; } 
    }
}
