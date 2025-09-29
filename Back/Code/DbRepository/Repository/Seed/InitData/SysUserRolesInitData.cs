using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysUserRolesInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var userRole = new List<SysUserRoles>() {
                new SysUserRoles { RoleId = "ADMIN", UserId = "admin" }
            };
            db.Insertable(userRole).AddQueue();
        }
    }
}
