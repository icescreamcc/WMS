using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("ApprovalProcess", "审批流程表")]
    public class ApprovalProcess
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "审批流程ID")]
        public int ProcessId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批数据类型")]
        public string DataType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批人员角色")]
        public string ApproverRole { get; set; }

        [SugarColumn(ColumnDescription = "审批顺序")]
        public int Rank { get; set; }

        [SugarColumn(ColumnDescription = "是否终审")]
        public bool IsLastApproval { get; set; }

        [SugarColumn(ColumnDescription = "是否允许跳审")]
        public bool IsJump { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "审批模式")]
        public string ApprovalModel { get; set; }
    }
}
