using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseSuppliers", "供应商表")]
   public class BaseSuppliers: DeletedModel
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "供应商ID")]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "供应商编码")]
        public string SupplierNo { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "供应商名称")]
        public string SupplierName { get; set; }

        [SugarColumn( ColumnDescription = "供应商类型ID")]
        public int SupplierTypeId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "供应商类型名称")]
        public string SupplierTypeName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "供应商性质ID")]
        public int SupplierPropertyId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "供应商性质名称")]
        public string SupplierPropertyName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "所在省份")]
        public string Province { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "所在城市")]
        public string City { get; set; }

        [SugarColumn(Length = 150, IsNullable = true, ColumnDescription = "详细地址")]
        public string Address { get; set; }
         
        [SugarColumn(IsNullable = true, ColumnDescription = "是否重要供应商")]
        public bool IsImportant { get; set; }

        [SugarColumn(Length = 150, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建人")]
        public string CreateUser { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "创建时间")]
        public string CreateDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "收货人")]
        public string Consignee { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "收货人电话")]
        public string ConsigneeTel { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "收货人地址")]
        public string ConsigneeAddress { get; set; }

        [SugarColumn(ColumnDescription = "是否有效")]
        public bool IsValid { get; set; }

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
