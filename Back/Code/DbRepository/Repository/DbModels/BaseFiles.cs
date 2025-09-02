using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseFiles", "文件存储信息表")]
    public class BaseFiles
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true, ColumnDescription = "文件ID")]
        public int FileId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "文件名称")]
        public string FileName { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "文件类型")]
        public string FileInfoType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "对应主表ID")]
        public string PrimaryId { get; set; }

        [SugarColumn(Length = 200, ColumnDescription = "文件地址")]
        public string Url { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "存储路径")]
        public string Path { get; set; }

        [SugarColumn( ColumnDescription = "是否默认文件")]
        public bool IsDeft { get; set; }

        [SugarColumn(Length = 200, IsNullable =true, ColumnDescription = "备注")] 
        public string Remark { get; set; }
    }
}
