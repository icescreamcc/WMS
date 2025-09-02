using SqlSugar;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysUserPermissions","用户权限表")]
   public class SysUserPermissions
    {
        [SugarColumn( IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "用户权限ID")]
        public int PermissionId { get; set; }

        [SugarColumn( ColumnDescription = "角色ID")]
        public string RoleId { get; set; }

        [SugarColumn( ColumnDescription = "权限菜单")]
        public string MenuId { get; set; }

    }
}
