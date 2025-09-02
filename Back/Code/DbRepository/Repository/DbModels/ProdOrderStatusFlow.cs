using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdOrderStatusFlow", "生产订单状态变化流水记录表"), SugarIndex("ProdOrderStatusFlow_Index",nameof(CarSoleCode),OrderByType.Asc,nameof(Status),OrderByType.Asc,nameof(CreateDate),OrderByType.Asc)]
    public class ProdOrderStatusFlow
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int FlowId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "小车唯一码")]
        public string CarSoleCode { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "生产订单号")]
        public string DeliverNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "发货单号")]
        public string ConsignNum { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "交接单号")]
        public string DetailNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "产品型号")]
        public string ProdctionTypeNo { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "产线")]
        public string Line { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "状态")]
        public string Status { get; set; }

        [SugarColumn(IsNullable = true, InsertServerTime=true,  ColumnDescription = "创建日期")]
        public DateTime CreateDate { get; set; }

        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "创建人")]
        public string CreateUser { get; set; }

    }
}
