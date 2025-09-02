using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    public class AutoProdDevicePlcConfig
    {
        [SugarColumn(Length = 50,IsPrimaryKey =true, ColumnDescription = "设备编码")]
        public string DeviceNo { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "设备类型")]
        public string DeviceType { get; set; }

        [SugarColumn(ColumnDescription = "库位号")]
        public int BinRank { get; set; }

        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "信号名称")]
        public string SignalName { get; set; }

        [SugarColumn(Length = 50,ColumnDescription = "信号类型（R/W）")]
        public string SignalType { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "内存区域")]
        public string DbRange { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "偏移量")]
        public string DbOffset { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "值类型")]
        public string ValueType { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "默认值")]
        public string DeftVal { get; set; }

        [SugarColumn(Length =300, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "获取到该信号后需要执行的任务类型")]
        public string TaskType { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "获取到该信号后需要执行的动作类型")]
        public string ActionType { get; set; }

        [SugarColumn(Length = 300, IsNullable = true, ColumnDescription = "提示消息")]
        public string Message { get; set; }
    }
}
