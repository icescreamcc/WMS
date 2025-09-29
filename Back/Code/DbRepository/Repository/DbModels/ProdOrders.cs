using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdOrders", "生产订单")]
    public class ProdOrders
    {
        [SugarColumn( IsPrimaryKey = true,IsIdentity =true)]
        public int OrderId { get; set; }
         
        [SugarColumn(Length = 50, ColumnDescription = "生产订单号")]
        public string DeliverNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "发货型号")]
        public string ConsignNum { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "产品型号")]
        public string ProdctionTypeNo { get; set; }

        [SugarColumn(Length = 100, IsNullable =true, ColumnDescription = "产品名称")]
        public string ProductName { get; set; }

        [SugarColumn( ColumnDescription = "小车配对数")]
        public int MatchingCount { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "生产订单数量")]
        public double Total { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "已上架数量")]
        public double TotalPutout { get; set; }

        [SugarColumn(ColumnDescription = "装车数量")]
        public double TotalByCar { get; set; }

        [SugarColumn(ColumnDescription = "需求车次")]
        public int CountByCar { get; set; }

        [SugarColumn(IsNullable = true, Length = 10, ColumnDescription = "单位")]
        public string UnitName { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "产线")]
        public string Line { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "创建日期")]
        public DateTime CreateDate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "修改日期")]
        public DateTime ModifyDate { get; set; }

        [SugarColumn(IsNullable =true, Length = 50, ColumnDescription = "创建人")]
        public string CreateUser { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "修改人")]
        public string ModifyUser { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "打印日期")]
        public DateTime PrintDate { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "状态")]
        public string Status { get; set; }

        [SugarColumn(IsNullable = true, Length = 200, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
