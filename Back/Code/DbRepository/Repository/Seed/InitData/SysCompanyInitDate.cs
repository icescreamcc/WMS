using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysCompanyInitDate : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var company = new SysCompany() { CompanyId = "1000000001", CompanyNo = "Conti", CompanyName = "Continental-ChangSha" };
            db.Insertable(company).AddQueue();
        }
    }
}
