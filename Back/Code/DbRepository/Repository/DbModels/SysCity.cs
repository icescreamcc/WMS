using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysCity","城市表")]
    public class SysCity
    {
        [SugarColumn( ColumnDescription = "省份ID")]
        public int ProvinceId { get; set; }

        [SugarColumn( IsPrimaryKey = true, ColumnDescription = "城市ID")]
        public int CityId { get; set; }

        [SugarColumn( Length = 20, IsNullable = true, ColumnDescription = "城市编号")]
        public string CityCode { get; set; }

        [SugarColumn( Length = 50,  ColumnDescription = "城市名称")]
        public string CityName { get; set; }

        [SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( Length = 2, IsNullable = true, ColumnDescription = "拼音首字母")]
        public string FirstSpell { get; set; }

        [SugarColumn( ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }
    }
}
