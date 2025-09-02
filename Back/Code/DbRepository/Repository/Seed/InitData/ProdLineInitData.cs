using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class ProdLineInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var user = new List<ProdLine> {
                new ProdLine{PlantNo="CS",AreaNo="FA",LineNo="FA_FDC_Line_1_VW_Seamless",LineName="FA_FDC_Line_1_VW_Seamless"},
                new ProdLine{PlantNo="CS",AreaNo="FA",LineNo="FA_DS_Line_1_VW_ABT_E3",LineName="FA_DS_Line_1_VW_ABT_E3"},
                new ProdLine{PlantNo="CS",AreaNo="FA",LineNo="FA_PA_Line_1_VW_ABT_E3",LineName="FA_PA_Line_1_VW_ABT_E3FA"},

                new ProdLine{PlantNo="CS",AreaNo="SMT",LineNo="SMT_Line1",LineName="SMT_Line1"},
                new ProdLine{PlantNo="CS",AreaNo="SMT",LineNo="SMT_Line2",LineName="SMT_Line2"},

                new ProdLine{PlantNo="CS",AreaNo="DPA",LineNo="DPA_Bonding_Line1",LineName="DPA_Bonding_Line1"},
                new ProdLine{PlantNo="CS",AreaNo="DPA",LineNo="DPA_Gluing_Line1",LineName="DPA_Gluing_Line1"},
                new ProdLine{PlantNo="CS",AreaNo="DPA",LineNo="DPA_Backlight_Line1",LineName="DPA_Backlight_Line1"},

                new ProdLine{PlantNo="CS",AreaNo="PCBA",LineNo="PCBA_Line1", LineName = "PCBA_Line1"},
                new ProdLine{PlantNo="CS",AreaNo="PCBA",LineNo="PCBA_Line2", LineName = "PCBA_Line2"}
            };
            db.Insertable(user).AddQueue();
        }
    }
}
