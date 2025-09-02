using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("InvWarehouse", "仓库信息表")]
    public class InvWarehouse
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "仓库ID")]
        public string WarehouseId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "仓库编码")]
        public string WarehouseNo { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "仓库名称")]
        public string WarehouseName { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "仓库类型")]
        public string WarehouseType { get; set; } 

        [SugarColumn(Length = 1000, IsNullable = true, ColumnDescription = "负责人")]
        public string ChargePerson { get; set; }

        [SugarColumn(Length = 1000, IsNullable = true, ColumnDescription = "负责人电话")]
        public string ChargePersonPhone { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "所在省份")]
        public string Province { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "所在城市")]
        public string City { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "所在地址")]
        public string Address { get; set; }

        [SugarColumn( ColumnDescription = "是否已弃用")]
        public bool IsAbandon { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "上次盘点日期")]
        public string InventoryDateOfLast { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "上次盘点人员ID")]
        public string InventoryOperatorIdOfLast { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "上次盘点人员姓名")]
        public string InventoryOperatorNameOfLast { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string SpareField1 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string SpareField2 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string SpareField3 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string SpareField4 { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "预留字段")]
        public string SpareField5 { get; set; }
    }
}
