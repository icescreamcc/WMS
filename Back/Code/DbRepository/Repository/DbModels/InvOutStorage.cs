using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvOutStorage", "出库单主表")]
   public class InvOutStorage
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "出库单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "出库类型")]
        public string OutStorageType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品类型")]
        public string GoodsClassify { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "产线")]
        public string LineNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "数据来源单号")]
        public string SourceOrderNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "出库仓库")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "单据状态")]
        public string Status { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人ID")]
        public string CreateUserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人姓名")]
        public string CreateUserName { get; set; }

        [SugarColumn( ColumnDescription = "创建时间")]
        public DateTime CreateDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次修改人ID")]
        public string UpdateUserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次修改人姓名")]
        public string UpdateUserName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "上次修改时间")]
        public DateTime UpdateDate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "出库时间")]
        public DateTime OutStorageDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "审批时间")]
        public DateTime ApprovalDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批人ID")]
        public string ApproverId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批人姓名")]
        public string ApproverName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批人角色")]
        public string ApproverRole { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批状态")]
        public string ApprovalStatus { get; set; }
    }
}
