using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("AutoProdDeviceWarehouse", "车间设备仓位")]
    public class AutoProdDeviceWarehouse
    {
        [SugarColumn(Length = 50, ColumnDescription = "设备ID")]
        public int DeviceId { get; set; }

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true, ColumnDescription = "仓位ID")]
        public int BinId { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "仓位编码")]
        public string BinNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "AGV仓位编码-送料点")]
        public string AGVBinCode_Delivery { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "AGV仓位编码-取料点")]
        public string AGVBinCode_Receive { get; set; }

        [SugarColumn( ColumnDescription = "排列")]
        public int Sort { get; set; }

        [SugarColumn(ColumnDescription = "层")]
        public int Tier { get; set; }

        [SugarColumn(ColumnDescription = "列")]
        public int Column { get; set; }

        [SugarColumn(ColumnDescription = "排")]
        public int Row { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "仓位状态")]
        public string BinStatus { get; set; }
    }
}
