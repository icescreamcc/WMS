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
    internal class ProdPlantInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var user = new List<ProdPlant> {
                new ProdPlant{PlantNo="CS",PlantName="Plant Changsha"} 
            };
            db.Insertable(user).AddQueue();
        }
    }
}
