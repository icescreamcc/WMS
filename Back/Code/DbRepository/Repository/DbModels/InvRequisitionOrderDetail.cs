using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvRequisitionOrderDetail", "物品领用单明细")]
    public class InvRequisitionOrderDetail
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true, ColumnDescription = "ID")]
        public int DetailId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "单据编号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品ID")]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 50, IsNullable =true, ColumnDescription = "物品名称")]
        public string GoodsName { get; set; }

        [SugarColumn(ColumnDescription = "计划领用数量")]
        public float Quantity { get; set; }

        [SugarColumn(ColumnDescription = "实际领用数量")]
        public float ActualQuantity { get; set; }

        [SugarColumn(ColumnDescription = "单位ID(计划)")]
        public int UnitId { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "单位(计划)")]
        public string UnitName { get; set; }

        [SugarColumn(ColumnDescription = "单位ID(实际)")]
        public int ActualUnitId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "单位(实际)")]
        public string ActualUnitName { get; set; }
    }
}
