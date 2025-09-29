using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysProvince",  "省份表")]
    public class SysProvince
    {
        [SugarColumn( IsPrimaryKey = true, ColumnDescription = "省份ID")]
        public int ProvinceId { get; set; }

        [SugarColumn( Length = 20,IsNullable =true, ColumnDescription = "省份编号")]
        public string ProvinceCode { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "省份名称")]
        public string ProvinceName { get; set; }

        [SugarColumn( Length = 10, IsNullable = true, ColumnDescription = "省份简称")]
        public string ProvinceShotName { get; set; }

        [SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( Length = 2, IsNullable = true, ColumnDescription = "拼音首字母")]
        public string FirstSpell { get; set; }

        [SugarColumn( ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }
    }
}
