using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysArgsOptions", "系统参数选项表")]
    public class SysArgsOptions
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true,ColumnDescription ="参数选项ID")]
        public int OptionId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "参数ID")]
        public string ArgsKey { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "参数选项编码")]
        public string OptionKey { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "参数选项编码名称")]
        public string OptionName { get; set; }

        [SugarColumn( Length = 50,IsNullable =true, ColumnDescription = "参数选项编码英文名")]
        public string OptionNameEn { get; set; } 

        [SugarColumn( Length = 300, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }
    }
}
