using SqlSugar;
using System;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SendingOrderDetail", "发货计划明细表")]
    public class SendingOrderDetail
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "明细ID")]
        public int DetialId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "发货单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品编码")]
        public string GoodsNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "客户料号")]
        public string CustomerGoodsNo { get; set; }

        [SugarColumn(Length = 200, ColumnDescription = "客户识别码")]
        public string CustomerIdentificationCode { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "计划发货数量")]
        public float Quantity { get; set; }

        [SugarColumn(ColumnDescription = "发货托数")]
        public float PalletsQuantity { get; set; }

        [SugarColumn(ColumnDescription = "数量单位ID")]
        public int QuantityUnitId { get; set; }

        [SugarColumn(Length = 10, ColumnDescription = "数量单位")]
        public string QuantityUnitName { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "状态")]
        public string DetailStatus { get; set; }

    }
}
