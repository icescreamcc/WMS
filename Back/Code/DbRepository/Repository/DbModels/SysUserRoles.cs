using SqlSugar;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysUserRoles", "用户角色表")]
   public class SysUserRoles
    {
        [SugarColumn( IsPrimaryKey = true, ColumnDescription = "角色ID")]
        public string RoleId { get; set; }

        [SugarColumn( IsPrimaryKey = true, ColumnDescription = "用户ID")]
        public string UserId { get; set; }
    }
}
