using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvStorage", "库存表")]
    public class InvStorage
    { 
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "商品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(ColumnDescription = "库存总量")]
        public float Stock { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "上次汇总日期")]
        public string LastStatisticsDate { get; set; }

        [SugarColumn(Length = 50, IsNullable =true, ColumnDescription = "上次汇总操作人ID")]
        public string LastOperatorId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "上次汇总操作人姓名")]
        public string LastOperatorName { get; set; }
         
        [SugarColumn(Length = 50, IsNullable = true, IsIgnore = true, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
