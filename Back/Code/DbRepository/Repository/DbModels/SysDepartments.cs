using SqlSugar;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysDepartments", "部门表")]
   public class SysDepartments
    {
        [SugarColumn( IsPrimaryKey = true, Length = 50, ColumnDescription = "部门ID")]
        public string DeptId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "部门编号")]
        public string DeptNo { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "部门名称")]
        public string DeptName { get; set; }

        [SugarColumn(ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "企业ID")]
        public string CompanyId { get; set; }

        [SugarColumn( ColumnDescription = "是否有效")]
        public bool IsVaild { get; set; }
    }
}
