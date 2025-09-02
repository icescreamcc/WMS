using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysArea", "区县表")]
    public class SysArea
    {
        [SugarColumn( ColumnDescription = "城市ID")]
        public int CityId { get; set; }

        [SugarColumn( IsPrimaryKey = true, ColumnDescription = "区县ID")]
        public int AreaId { get; set; }

        [SugarColumn( Length = 20, IsNullable = true, ColumnDescription = "区县编号")]
        public string AreaCode { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "区县名称")]
        public string AreaName { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 2, IsNullable = true, ColumnDescription = "拼音首字母")]
        public string FirstSpell { get; set; }

        [SugarColumn(ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }
    }
}
