using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdMatchingCar", "小车配对信息表")]
    public class ProdMatchingCar
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int MatchingId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "生产订单号")]
        public string DeliverNo { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "发货单号")]
        public string ConsignNum { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "产品")]
        public string Prodct { get; set; }

        [SugarColumn(ColumnDescription = "需求车次")]
        public int CountByCar { get; set; }

        [SugarColumn(ColumnDescription = "小车配对数")]
        public int MatchingCount { get; set; }

        [SugarColumn(ColumnDescription = "生产订单数量")]
        public double Total { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "小车唯一码")]
        public string CarSoleCode { get; set; }

        [SugarColumn(ColumnDescription = "小车配对序列号")]
        public int MatchingNum { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "小车配对码")]
        public string MatchingCode { get; set; }

        [SugarColumn(ColumnDescription = "计划装车数量")]
        public double PlanTotalByCar { get; set; }

        [SugarColumn(ColumnDescription = "实际装车数量")]
        public double ActualTotalByCar { get; set; }

        [SugarColumn(ColumnDescription = "缓存仓库位ID")]
        public int BinId { get; set; }

        [SugarColumn(IsNullable =true,Length =50, ColumnDescription = "缓存仓库位编号")]
        public string BinNo { get; set; } 

        [SugarColumn(ColumnDescription = "是否完成配对")]
        public bool IsMatch { get; set; }

        [SugarColumn(ColumnDescription = "是否已包装完成")]
        public bool IsPackage { get; set; }

        [SugarColumn(IsNullable =true, ColumnDescription = "配对时间")]
        public DateTime MatchDate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "包装完成时间")]
        public DateTime PackageDate { get; set; }
    }
}
