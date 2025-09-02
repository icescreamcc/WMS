using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvInStorage", "入库单主表")]
   public class InvInStorage
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "入库单号")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "入库类型")]
        public string InStorageType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "物品类型")]
        public string GoodsClassify { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "数据来源单号")]
        public string SourceOrderNo { get; set; } 

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "入库仓库")]
        public string WarehouseId { get; set; } 
         
        [SugarColumn(Length = 50,  ColumnDescription = "单据状态")]
        public string Status { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人ID")]
        public string CreateUserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人姓名")]
        public string CreateUserName { get; set; }

        [SugarColumn( ColumnDescription = "创建时间")]
        public DateTime CreateDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次修改人ID")]
        public string UpdateUserId { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "上次修改人姓名")]
        public string UpdateUserName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "上次修改时间")]
        public DateTime UpdateDate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "入库时间")]
        public DateTime InstorageDate { get; set; }

        [SugarColumn(Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "责任人ID")]
        public string ResponsibleId { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "责任人")]
        public string Responsible { get; set; }

        [SugarColumn( IsNullable = true, ColumnDescription = "审批时间")]
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
