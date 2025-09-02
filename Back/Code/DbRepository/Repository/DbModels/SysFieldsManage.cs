using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysFieldsManage", "字段管理表")]
    public class SysFieldsManage
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "主键ID")]
        public string FieldsManageId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "表名")]
        public string TableName { get; set; }

        [SugarColumn( Length = 100, ColumnDescription = "表描述")]
        public string TableDesc { get; set; }

        [SugarColumn(Length =50, ColumnDescription = "字段名")]
        public string FieldName { get; set; }

        [SugarColumn(Length =100, ColumnDescription = "字段描述")]
        public string FieldDesc { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "字段类型")]
        public string FieldType { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "允许保存长度")]
        public int FieldLength { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "对应字典选项")]
        public string FieldArgsKey { get; set; }

        [SugarColumn(ColumnDescription = "是否启用当前字段")]
        public bool IsEnable { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( ColumnDescription = "排序")]
        public int Rank { get; set; }

        [SugarColumn(ColumnDescription = "是否支持导出")]
        public bool IsExport { get; set; }
    }
}
