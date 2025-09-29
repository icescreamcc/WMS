using DbRepository.Repository.DbModels;
using External.Common.Extension;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysUserInitDate : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var user = new List<SysUser> {
                new SysUser{ UserId = "admin", UserName = "管理员", AuthAccount = "admin", Password = "c4ca4238a0b923820dcc509a6f75849b", MobilePhone = "13818461927",IsVaild = true, CreateDate = DateTime.Now.ToStringExtension() }

            };
            db.Insertable(user).AddQueue();
        }
    }
}
