using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("Shipment", "发货单主表")]
    public class Shipment
    {
        //[SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "订单号")]
        [SugarColumn(IsPrimaryKey = true)]
        public string ShipmentNo { get; set; }
         
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "发货单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "订单号")]
        public string ShipmentDate { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "发货日期")]
        public string SendingAddress { get; set; }
          
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "目的地")]
        public string ShipmentNum { get; set; }
         
        [SugarColumn(Length = 100,IsNullable =true, ColumnDescription = "发货数量")]
        public string Remarks { get; set; }
          
        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "发货单状态")]
        public string ShipmentState { get; set; }

        [SugarColumn(Length = 100, IsPrimaryKey = true, ColumnDescription = "运输公司")]
        public string Transport { get; set; }
         

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "订单状态")]
        public string CreateDate { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "签字文件")]
        public string ModifyDate { get; set; }

    }
}
