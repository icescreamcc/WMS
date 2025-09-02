using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdCacheWarehouseFlow", "成品缓存仓进出流水表")]
    public class ProdCacheWarehouseFlow
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "ID")]
        public int FlowId { get; set; }

        [SugarColumn(Length = 20, ColumnDescription = "类型")]
        public string FlowType { get; set; }

        [SugarColumn(ColumnDescription = "数量")]
        public double ActualTotal { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "来源订单号")] 
        public string SourceOrderNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "来源订单类型")]
        public string SourceOrderType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "货架ID")]
        public string ShelfId { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "库位ID")]
        public int BinId { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "产品")]
        public string Product { get; set; }
         
        [SugarColumn( ColumnDescription = "创建时间")]
        public DateTime CreateDate { get; set; }

        [SugarColumn(Length = 20, IsNullable =true, ColumnDescription = "创建人")]
        public string CreateUser { get; set; }

        [SugarColumn(ColumnDescription = "是否已流转In->Out")]
        public bool IsStatistical { get; set; } 
    }
}
