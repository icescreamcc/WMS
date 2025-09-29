using SqlSugar;
using System;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysUser","用户表")]
   public class SysUser
    {
        [SugarColumn( Length = 50, IsPrimaryKey = true, ColumnDescription = "用户ID")]
        public string UserId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "用户姓名")]
        public string UserName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "工号")]
        public string UserCode { get; set; }

        [SugarColumn( Length = 50, IsNullable = true, ColumnDescription = "昵称")]
        public string NickName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "域用户名")]
        public string DomainName { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "登录账户")]
        public string AuthAccount { get; set; }

        [SugarColumn( Length = 150, ColumnDescription = "登录密码")]
        public string Password { get; set; }

        [SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "邮箱")]
        public string Email { get; set; }

        [SugarColumn( Length = 50, IsNullable = true, ColumnDescription = "微信号")]
        public string Wechat { get; set; }

        [SugarColumn( Length = 50, IsNullable = true, ColumnDescription = "电话")]
        public string Phone { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "手机号")]
        public string MobilePhone { get; set; }

        [SugarColumn( Length = 200, IsNullable = true, ColumnDescription = "通讯地址")]
        public string Address { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "所在省份")]
        public string ProvinceName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "所在城市")]
        public string CityName { get; set; }

        [SugarColumn( IsNullable = true, Length = 50, ColumnDescription = "所属部门")]
        public string DeptId { get; set; } 

        [SugarColumn( ColumnDescription = "是否有效")]
        public bool IsVaild { get; set; }

        [SugarColumn( Length = 200, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( ColumnDescription = "创建时间")]
        public string CreateDate { get; set; }

        [SugarColumn( IsNullable = true, ColumnDescription = "离职时间")]
        public string DepartureDate { get; set; }

        [SugarColumn( IsNullable = true, ColumnDescription = "上次编辑时间")]
        public string EditDate { get; set; }

        [SugarColumn( IsNullable = true, Length = 50, ColumnDescription = "用户图像")]
        public string AvatarImgId { get; set; }

        [SugarColumn(IsNullable = true,Length =50, ColumnDescription = "员工卡号")]
        public string CardId { get; set; }
    }
}
