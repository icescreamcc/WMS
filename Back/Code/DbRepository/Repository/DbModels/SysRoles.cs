using SqlSugar;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysRoles", "角色表")]
   public class SysRoles
    {
        [SugarColumn( IsPrimaryKey = true, ColumnDescription = "角色ID")]
        public string RoleId { get; set; }

        [SugarColumn(Length = 50, IsNullable =true, ColumnDescription = "角色编码")]
        public string RoleNo { get; set; } 

        [SugarColumn( Length = 50, ColumnDescription = "角色名称")]
        public string RoleName { get; set; }

        [SugarColumn(ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }

        [SugarColumn( ColumnDescription = "所属部门ID",IsNullable =true)]
        public string DeptId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "上级岗位ID")]
        public string ParentId { get; set; }

        [SugarColumn( ColumnDescription = "是否有效")]
        public bool IsVaild { get; set; }
    }
}
