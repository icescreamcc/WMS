using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ReceivingOrderDetail", "收货计划明细表")]
    public class ReceivingOrderDetail
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "明细ID")]
        public int DetialId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "收货单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length =500,IsNullable =true, ColumnDescription = "异常到货类别")]
        public string ReceivingAbnormalType { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "异常到货描述")]
        public string ReceivingAbnormalDesc { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品编码")]
        public string GoodsNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品类型")]
        public string GoodsClassifyName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品型号")]
        public string GoodsModel { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "物品名称")]
        public string GoodsName { get; set; }

        [SugarColumn(Length = 50, IsNullable =true, ColumnDescription = "供应商ID")]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "供应商名称")]
        public string SupplierName { get; set; }

        [SugarColumn(ColumnDescription = "是否ASN收货")]
        public bool IsASN { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "ASN Check状态")]
        public string ASNCheckStatus { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "重要级别")]
        public string ReceivingLevel { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "预计停线时间")]
        public DateTime DownTime { get; set; }

        [SugarColumn(ColumnDescription = "是否开票")]
        public bool IsMakeInvoice { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "发票编号")]
        public string InvoiceNumber { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "外部订单号")]
        public string ExternalOrderNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "运单号")]
        public string WaybillNo { get; set; }

        [SugarColumn(ColumnDescription = "收货数量")]
        public float Quantity { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "实际收货数量")]
        public float QuantityActual { get; set; }

        [SugarColumn(ColumnDescription = "紧急收货数量")]
        public float QuantityUrgency { get; set; }

        [SugarColumn(ColumnDescription = "数量单位ID")]
        public int QuantityUnitId { get; set; }

        [SugarColumn(Length = 10, IsNullable = true, ColumnDescription = "数量单位")]
        public string QuantityUnitName { get; set; }

        [SugarColumn(ColumnDescription = "收货单价")]
        public float Price { get; set; }

        [SugarColumn(ColumnDescription = "料盘数")]
        public int WorkpieceTray { get; set; }

        [SugarColumn(ColumnDescription = "托盘数")]
        public int Pallet { get; set; }

        [SugarColumn(ColumnDescription = "收货总价")]
        public float DetailTotalPrice { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "状态")]
        public string DetailStatus { get; set; }

        [SugarColumn(ColumnDescription = "收货日期")]
        public DateTime ReceivingDate { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "收货人ID")]
        public string ReceivingOperatorId { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "收货人名称")]
        public string ReceivingOperatorName { get; set; }


        [SugarColumn(ColumnDescription = "异常到货托数")]
        public int AbnormalDeliveryPallet { get; set; }

    }
}
