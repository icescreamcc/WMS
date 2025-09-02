using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysFieldsPermissionsRole", "字段权限表")]
    public class SysFieldsPermissionsRole
    {
        [SugarColumn( Length = 50,IsPrimaryKey =true, ColumnDescription = "数据权限ID")]
        public string FieldsManageId { get; set; }

        [SugarColumn( Length = 20, IsPrimaryKey = true, ColumnDescription = "拒绝访问的角色")]
        public string DeniedRoleId { get; set; }
    }
}
