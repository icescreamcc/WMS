using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvRequisitionOrder", "物品领用单")]
    public class InvRequisitionOrder: CreateModifyModel
    {
        [SugarColumn(IsPrimaryKey = true, Length =50, ColumnDescription = "单据编号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, IsNullable =true, ColumnDescription = "单据类型")]
        public string OrderType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品类型")]
        public string GoodsClassifyGroup { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "产线")]
        public string Line { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "领用目的")]
        public string Purpose { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "备注")]
        public string  Remark { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "状态")]
        public string Status { get; set; }
    }
}
