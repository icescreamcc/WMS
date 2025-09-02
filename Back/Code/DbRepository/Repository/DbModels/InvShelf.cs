using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvShelf", "仓库货架信息表")]
    public class InvShelf
    {
        [SugarColumn( IsPrimaryKey = true, Length = 50, ColumnDescription = "货架ID")]
        public string ShelfId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "货架编号")]
        public string ShelfNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架编号")]
        public string ShelfName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架尺寸")]
        public string Size { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "货架布局")]
        public string Property { get; set; }

        [SugarColumn(Length =100, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "是否已弃用")]
        public bool IsAbandon { get; set; }

        [SugarColumn(ColumnDescription = "是否启用料箱")]
        public bool HasWorkbin { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "排序")]
        public int Rank { get; set; }
    }
}
