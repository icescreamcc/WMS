using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseSupplierAccountCredited", "供应商收款账户表")]
   public class BaseSupplierAccountCredited
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity =true, ColumnDescription = "账户ID")]
        public int AccountId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "供应商ID")]
        public string SupplierId { get; set; }

        [SugarColumn( ColumnDescription = "账户类型ID")]
        public int AccountTypeId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "账户名")]
        public string AccountName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "账户号")]
        public string AccountNumber { get; set; }

        [SugarColumn( ColumnDescription = "是否常用账户")]
        public bool IsCommonAccount { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "开户行")]
        public string OpeningBank { get; set; }

        [SugarColumn(Length = 200,IsNullable =true, ColumnDescription = "账户备注")]
        public string Remark { get; set; }
    }
}
