using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvStorageFlowDetail", "库存流水明细表")]
    public class InvStorageFlowDetail
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "记录ID")]
        public int FlowId { get; set; }

        [SugarColumn(Length =50, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "流水类型")]
        public string FlowType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "数据来源单号")]
        public string SourceOrderNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "数据来源类型")]
        public string SourceStorageType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "数据来源子类型")]
        public string SourceStorageSubType { get; set; }

        [SugarColumn( ColumnDescription = "出入数量")]
        public float Quantity { get; set; }

        [SugarColumn(ColumnDescription = "单位ID")]
        public int UnitId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "仓库")]
        public string WarehouseId { get; set; } 

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "货架")]
        public string ShelfId { get; set; } 

        [SugarColumn(ColumnDescription = "库位")]
        public int BinId { get; set; } 

        [SugarColumn(IsNullable =true, ColumnDescription = "料箱")]
        public int WorkbinId { get; set; }
          
        [SugarColumn(ColumnDescription = "料箱单元格")]
        public int WorkbinCellId { get; set; }
          
        [SugarColumn(Length = 50, ColumnDescription = "出入时间")]
        public string OperateDate { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "操作人ID")]
        public string OperatorId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "操作人姓名")]
        public string OperatorName { get; set; }

        [SugarColumn( ColumnDescription = "是否已计入库存")]
        public bool IsStatistics { get; set; }

        [SugarColumn(ColumnDescription = "年份，格式：2023")]
        public int DateYear { get; set; }

        [SugarColumn(ColumnDescription = "月份，格式：202309")]
        public int DateMonth { get; set; }

        [SugarColumn(ColumnDescription = "月份，格式：20230901")]
        public int DateDay { get; set; }

        [SugarColumn( ColumnDescription = "批次号")]
        public long BatchNumber { get; set; }

        [SugarColumn(IsNullable =true, ColumnDescription = "生产日期")]
        public DateTime ProductionDate { get; set; }

        [SugarColumn(ColumnDescription = "总价")]
        public double TotalPrice { get; set; }

        [SugarColumn(ColumnDescription = "单价")]
        public double UnitPrice { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "价格单位")]
        public string PriceUnit { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "备注类型")]
        public string RemarkType { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
