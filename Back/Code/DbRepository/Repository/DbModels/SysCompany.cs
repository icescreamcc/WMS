using SqlSugar;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysCompany", "企业信息表")]
  public  class SysCompany
    {
        [SugarColumn(Length =50, IsPrimaryKey = true, ColumnDescription = "企业ID")]
        public string CompanyId { get; set; }

        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "企业编号")]
        public string CompanyNo { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "企业名称")]
        public string CompanyName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "企业邮箱")]
        public string Email { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "企业电话")]
        public string Phone { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "企业网址")]
        public string Url { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "企业联系人姓名")]
        public string User { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( Length = 200,IsNullable =true, ColumnDescription = "企业Logo路径")]
        public string Logo { get; set; }
    }
}
