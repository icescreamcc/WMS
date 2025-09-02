using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ApprovalHis", "审批记录表")]
    public class ApprovalHis
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "审批记录ID")]
        public int ApprovalId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批数据ID")]
        public string PrimaryId  { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批数据类型")]
        public string DataType { get; set; }

        [SugarColumn( ColumnDescription = "审批时间")]
        public DateTime ApprovalDate { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批人员ID")]
        public string ApproverId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批人员姓名")]
        public string ApproverName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "审批人员角色ID")]
        public string ApproverRoleId { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "审批人员角色名称")]
        public string ApproverRoleName { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批模式")]
        public string ApprovalModel { get; set; }

        [SugarColumn(ColumnDescription = "审批顺序")]
        public int ApprovalRank { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批状态")]
        public string ApprovalStatus { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "审批意见")]
        public string Opinion { get; set; }
    }
}
