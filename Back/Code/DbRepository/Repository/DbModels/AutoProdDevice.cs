using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("AutoProdDevice", "车间设备")]
    public class AutoProdDevice
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true, ColumnDescription = "设备ID")]
        public int DeviceId { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "设备编码")]
        public string DeviceNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "设备名称")]
        public string DeviceName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "设备类型")]
        public string DeviceType { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "连接地址")]
        public string ConnectAddress { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "所属仓库")]
        public string WarehouseId { get; set; }

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsActive { get; set; }

        [SugarColumn(ColumnDescription = "是否在使用中")]
        public bool IsUsing { get; set; }

        [SugarColumn(ColumnDescription = "当前正在执行的AGV任务ID")]
        public int AGVTaskId { get; set; }
    }
}
