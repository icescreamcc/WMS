using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysVipUserPermissions", "Vip用户权限表")]
    class SysVipUserPermissions
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "用户权限ID")]
        public int PermissionId { get; set; }

        [SugarColumn(ColumnDescription = "用户ID")]
        public string UserId { get; set; }

        [SugarColumn(ColumnDescription = "权限菜单")]
        public string MenuId { get; set; }
    }
}
