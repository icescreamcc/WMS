using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed
{
    internal interface IDbInitData
    {
        void CreateTableInitData(SqlSugarClient db);
    }
}
