using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysArgs","系统参数表")]
    public class SysArgs
    {
        [SugarColumn( IsPrimaryKey = true, IsIdentity = true,ColumnDescription ="参数ID")]
        public int ArgsId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "参数编码")]
        public string ArgsKey { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "参数编码名称")]
        public string ArgsKeyName { get; set; }

        [SugarColumn( Length = 50, IsNullable = true, ColumnDescription = "参数编码英文名")]
        public string ArgsKeyNameEn { get; set; }

        [SugarColumn( Length = 500, IsNullable = true, ColumnDescription = "参数值")]
        public string ArgsValue { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "参数分类")]
        public string ArgsGroup { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "参数子类")]
        public string ArgsSubGroup { get; set; }

        [SugarColumn( Length = 300, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }
         
        [SugarColumn( Length = 20, ColumnDescription = "参数类型")]
        public string ArgsType { get; set; }

        [SugarColumn( ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }

        [SugarColumn(ColumnDescription = "是否可见")]
        public bool IsVisible { get; set; }
    }
}
