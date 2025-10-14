using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("OrderPlan", "订单号主表")]
    public class OrderPlan
    {
        //[SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "订单号")]
        [SugarColumn(IsPrimaryKey = true)]
        public string OrderNo { get; set; }
         
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "合同号")]
        public string ContractNo { get; set; }

        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "签约日期")]
        public string SigningDate { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "客户名称")]
        public string CustomerName { get; set; }
          
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "物品名称")]
        public string GoodsName { get; set; }
         
        [SugarColumn(Length = 100,IsNullable =true, ColumnDescription = "订单量")]
        public double OrderNum { get; set; }
          
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "单位")]
        public string Unit { get; set; }

        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "订单金额")]
        public string OrderAmount { get; set; }
         
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "交货截止日期")]
        public string DeliveryDate { get; set; } 

        [SugarColumn(Length = 255, IsNullable = true, ColumnDescription = "备注")]
        public string Remarks { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "已发重量")]
        public double ShippedNum { get; set; }
         
        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "完成比列(已发重量/订单量)")]
        public double Belial { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "开票信息")]
        public string Invoice { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "回款状态")]
        public string PaymentState { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "订单状态")]
        public string OrderState { get; set; }
         
        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "签字文件")]
        public string OrderUrl { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "订单状态")]
        public string CreateDate { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "签字文件")]
        public string ModifyDate { get; set; }

    }
}
