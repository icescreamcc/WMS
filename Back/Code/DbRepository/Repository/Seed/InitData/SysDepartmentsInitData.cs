using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysDepartmentsInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        { 
            var dept = new List<SysDepartments>() {
                 //new SysDepartments() { CompanyId = "1000000001", DeptName = "生产部", DeptNo = "PRODDEPT", DeptId="PRODDEPT", IsVaild = true,Rank=5 }
            };
            db.Insertable(dept).AddQueue();
        }
    }
}
