using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseSupplierContact", "供应商联系方式")]
    public class BaseSupplierContact
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "账户ID")]
        public int ContacttId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "供应商ID")]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "联系人")]
        public string ContactPerson { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "电话")]
        public string Telephone { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "手机")]
        public string Mobilephone { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "邮箱")]
        public string Email { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "微信")]
        public string Wechat { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "旺旺")]
        public string Wangwang { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "支付宝")]
        public string Alipay { get; set; }

        [SugarColumn(ColumnDescription = "常用联系方式")]
        public bool IsDeft { get; set; }
    }
}
