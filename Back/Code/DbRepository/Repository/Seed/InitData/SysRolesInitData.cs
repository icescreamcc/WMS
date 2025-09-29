using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysRolesInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var role = new List<SysRoles>() {
                new SysRoles { RoleId = "ADMIN", RoleNo="ADMIN", RoleName = "管理员", ParentId = "1000000001", IsVaild = true,Rank=1 },
                //new SysRoles { RoleId = "PRODM",RoleNo="PRODM", RoleName = "生产部主管", DeptId = "PRODDEPT",ParentId = "PRODDEPT", IsVaild = true , Rank=8},
                //    new SysRoles { RoleId = "PRODGL1",RoleNo="PRODGL1", RoleName = "一组组长", DeptId = "PRODDEPT",ParentId = "PRODM", IsVaild = true ,Rank=1}
            };
            db.Insertable(role).AddQueue();
        }
    }
}
