using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("AutoProdDeviceAGV", "车间设备-AGV")]
    public class AutoProdDeviceAGV:ModifyModel
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true, ColumnDescription = "ID")]
        public int AGVId { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "AGV编码")]
        public string AGVNo { get; set; } 

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "AGV类型")]
        public string AGVType { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "执行任务模板")]
        public string TaskType { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "搬运容器类型")]
        public string CtnrType { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "路径位置类型")]
        public string PositionCodeType { get; set; }

        [SugarColumn(ColumnDescription = "是否默认配置")]
        public bool IsDeft { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "小车状态")]
        public string Status { get; set; }

    }
}
