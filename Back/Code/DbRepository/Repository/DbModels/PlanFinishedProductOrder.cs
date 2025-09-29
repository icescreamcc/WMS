using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("PlanFinishedProductOrder", "成品生产计划")]
    public class PlanFinishedProductOrder: CreateModel
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true)]
        public int  OrderId { get; set; }

        [SugarColumn(ColumnDescription = "计划订单ID")]
        public int PlanId { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "计划订单名称")]
        public string PlanName { get; set; }

        [SugarColumn(ColumnDescription = "年")]
        public int Year { get; set; }

        [SugarColumn(ColumnDescription = "周")]
        public int Week { get; set; }

        [SugarColumn(ColumnDescription = "发布计划版本")]
        public float Version { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "发货型号")]
        public string ConsignNum { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "产品型号")]
        public string ProdctionTypeNo { get; set; }

        [SugarColumn(ColumnDescription = "生产数量")]
        public float QuantityTotal { get; set; }

        [SugarColumn(IsNullable = true, Length = 10, ColumnDescription = "单位")]
        public string UnitName { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "周一日期")]
        public string DateOfMon { get; set; }

        [SugarColumn(ColumnDescription = "周一白班生产数量")]
        public float QuantityDayOfMon { get; set; }

        [SugarColumn(ColumnDescription = "周一晚班生产数量")]
        public float QuantityNightOfMon { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "周二日期")]
        public string DateOfTues { get; set; }

        [SugarColumn(ColumnDescription = "周二白班生产数量")]
        public float QuantityDayOfTues { get; set; }

        [SugarColumn(ColumnDescription = "周二晚班生产数量")]
        public float QuantityNightOfTues { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "周三日期")]
        public string DateOfWed { get; set; }

        [SugarColumn(ColumnDescription = "周三白班生产数量")]
        public float QuantityDayOfWed { get; set; }

        [SugarColumn(ColumnDescription = "周三晚班生产数量")]
        public float QuantityNightOfWed { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "周四日期")]
        public string DateOfThur { get; set; }

        [SugarColumn(ColumnDescription = "周四白班生产数量")]
        public float QuantityDayOfThur { get; set; }

        [SugarColumn(ColumnDescription = "周四晚班生产数量")]
        public float QuantityNightOfThur { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "周五日期")]
        public string DateOfFri { get; set; }

        [SugarColumn(ColumnDescription = "周五白班生产数量")]
        public float QuantityDayOfFri { get; set; }

        [SugarColumn(ColumnDescription = "周五晚班生产数量")]
        public float QuantityNightOfFri { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "周六日期")]
        public string DateOfSat { get; set; }

        [SugarColumn(ColumnDescription = "周六白班生产数量")]
        public float QuantityDayOfSat { get; set; }

        [SugarColumn(ColumnDescription = "周六晚班生产数量")]
        public float QuantityNightOfSat { get; set; }

        [SugarColumn(IsNullable = true, Length = 20, ColumnDescription = "周日日期")]
        public string DateOfSun { get; set; }

        [SugarColumn(ColumnDescription = "周日白班生产数量")]
        public float QuantityDayOfSun { get; set; }

        [SugarColumn(ColumnDescription = "周日晚班生产数量")]
        public float QuantityNightOfSun { get; set; }

        [SugarColumn(IsNullable = true, Length = 100, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
