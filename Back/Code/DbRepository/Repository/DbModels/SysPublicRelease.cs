using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysPublicRelease", "发布版本管理")]
    public class SysPublicRelease
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "版本号")]
        public string VersionNo { get; set; }

        [SugarColumn(IsNullable =true, ColumnDescription = "SQL脚本迁移版本")]
        public int SQLMigrationNo { get; set; }

        [SugarColumn(ColumnDescription = "发布时间")]
        public DateTime ReleaseDate { get; set; }
    }
}
