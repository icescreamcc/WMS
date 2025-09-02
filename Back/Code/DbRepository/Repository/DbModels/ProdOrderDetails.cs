using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdOrderDetails", "生产订单明细")]
    public class ProdOrderDetails
    {
        [SugarColumn(Length = 50, ColumnDescription = "生产订单号")]
        public string DeliverNo { get; set; }

        [SugarColumn(IsPrimaryKey = true,IsIdentity =true,ColumnDescription = "明细ID")]
        public int DetailId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "交接单号")]
        public string DetailNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "小车唯一码")]
        public string CarSoleCode { get; set; }

        [SugarColumn(IsNullable =true, ColumnDescription = "配送车次")]
        public int CarRank { get; set; } 

        [SugarColumn(ColumnDescription = "计划装车数量")]
        public double PlanTotalByCar { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "状态")]
        public string Status { get; set; }
    }
}
