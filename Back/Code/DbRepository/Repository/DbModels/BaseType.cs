using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseType", "物品类型表")]
    public class BaseType
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity =true, ColumnDescription = "类型ID")]
        public int TypeId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "类型编码")]
        public string TypeNo { get; set; }

        [SugarColumn(Length =50, ColumnDescription = "类型名称")]
        public string TypeName { get; set; }

        [SugarColumn(Length = 50, IsNullable =true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( ColumnDescription = "父级ID")]
        public int ParentId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "类型分组")]
        public string Group { get; set; }

        [SugarColumn(ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }
    }
}
