using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvInStorageLabels", "扫码入库")]
    public class InvInStorageLabels: CreateModifyModel
    {
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "标签码字符串")]
        public string CodeString { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品类型分组")]
        public string GoodsClassifyGroup { get; set; }
         
        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架ID")]
        public string ShelfId { get; set; }

        [SugarColumn( ColumnDescription = "货位ID")]
        public int BinId { get; set; }

        [SugarColumn( ColumnDescription = "料箱ID")]
        public int WorkbinId { get; set; }

        [SugarColumn( ColumnDescription = "料箱单元格ID")]
        public int WorkbinCellId { get; set; }

        [SugarColumn( ColumnDescription = "单位ID")]
        public int UnitId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "单位")]
        public string UnitName { get; set; }

        [SugarColumn(ColumnDescription = "每个标签对应数量")]
        public float Quantity { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "批次号")]
        public string PatchNum { get; set; }

        [SugarColumn(Length = 20,  ColumnDescription = "状态")]
        public string Status { get; set; }
    }
}
