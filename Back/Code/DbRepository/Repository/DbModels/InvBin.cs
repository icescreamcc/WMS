using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvBin", "仓库货位信息表")]
    public class InvBin
    {
        [SugarColumn(IsIdentity =true, IsPrimaryKey = true, ColumnDescription = "货位ID")]
        public int BinId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架ID")]
        public string ShelfId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "货位编号")]
        public string BinNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true ,ColumnDescription = "货位名称")]
        public string BinName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "AGV地址编码")]
        public string AGVNo { get; set; }

        [SugarColumn( IsNullable = true, ColumnDescription = "货位长度")]
        public double Long { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "货位宽度")]
        public double Width { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货位布局")]
        public string Property { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "货位规格")]
        public string Specification { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "是否已弃用")]
        public bool IsAbandon { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "排序")]
        public int Rank { get; set; }

        [SugarColumn(ColumnDescription = "盘点锁定")]
        public bool IsTakeStockLock { get; set; }

        [SugarColumn(ColumnDescription = "允许多样化存储")]
        public bool IsVarietyStock { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次盘点日期")]
        public string LastInventoryDate { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "上次盘点人员")]
        public string LastInventoryOperator { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "状态")]
        public string Status { get; set; }


    }
}
