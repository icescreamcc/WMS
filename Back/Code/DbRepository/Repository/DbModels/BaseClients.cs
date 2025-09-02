using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("BaseClients", "客户表")]
    public class BaseClients
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "客户ID")]
        public string ClientId { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "客户编码")]
        public string ClientNo { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "客户名称")]
        public string ClientName { get; set; }

        [SugarColumn( ColumnDescription = "客户类型ID")]
        public int ClientTypeId { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "客户类型名称")]
        public string ClientTypeName { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "客户属性")]
        public string ClientProperty { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "客户等级")]
        public string ClientLevel { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "所在省份")]
        public string Province { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "所在城市")]
        public string City { get; set; }

        [SugarColumn(Length = 150, IsNullable = true, ColumnDescription = "详细地址")]
        public string Address { get; set; }

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

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "常用联系方式")]
        public string CommonContact { get; set; }

        [SugarColumn( IsNullable = true, ColumnDescription = "是否重要客户")]
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
