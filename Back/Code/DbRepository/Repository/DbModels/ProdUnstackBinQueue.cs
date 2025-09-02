using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ProdUnstackBinQueue", "拆垛机库位队列信息")]
    public class ProdUnstackBinQueue
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "小车唯一码")]
        public string CarSoleCode { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "小车配对码")]
        public string MatchingCode { get; set; }

        [SugarColumn( ColumnDescription = "拆垛机库位ID")]
        public int UnstackBinId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "拆垛机库位编号")]
        public string UnstackBinNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "订单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "状态")]
        public string Status { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
