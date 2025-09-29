using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class ProdAreaInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var user = new List<ProdArea> {
                new ProdArea{PlantNo="CS",AreaNo="FA",AreaName="FA"},
                new ProdArea{PlantNo="CS",AreaNo="SMT",AreaName="SMT"},
                new ProdArea{PlantNo="CS",AreaNo="DPA",AreaName="DPA"},
                new ProdArea{PlantNo="CS",AreaNo="PCBA",AreaName="PCBA"},
            };
            db.Insertable(user).AddQueue();
        }
    }
}
