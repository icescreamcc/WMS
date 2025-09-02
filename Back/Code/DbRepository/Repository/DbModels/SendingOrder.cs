using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SendingOrder", "发货计划主表")]
    public class SendingOrder : CreateModifyModel
    {
        [SugarColumn(Length = 50, IsPrimaryKey = true, ColumnDescription = "发货单号")]
        public string OrderNo { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "计划发货日期")]
        public DateTime SendingDate { get; set; }

        [SugarColumn(ColumnDescription = "要求到货日期")]
        public DateTime RequestDate { get; set; }
         
        [SugarColumn(Length = 50,IsNullable =true, ColumnDescription = "单据状态")]
        public string Status { get; set; }
          
        [SugarColumn(ColumnDescription = "是否紧急发货")]
        public string IsUrgentShipment { get; set; }

        [SugarColumn(ColumnDescription = "是否有足够库存")]
        public string IsSufficientStock { get; set; }
         
        [SugarColumn(ColumnDescription = "发货年月")]
        public int YearAndMonth { get; set; } 

        [SugarColumn(Length = 500, ColumnDescription = "特殊要求")]
        public string SpecialRequest { get; set; }

        [SugarColumn(Length = 500, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "发货责任人ID")]
        public string SendingResponsableUserId { get; set; }
         
        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "发货人责任人名称")]
        public string SendingResponsableUserName { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "发货责任人邮箱")]
        public string SendingResponsableUserEmail { get; set; }

        [SugarColumn(Length = 500, ColumnDescription = "收件人信息")]
        public string ReceivingResponsableUserInfo { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "到货地址")]
        public string SendingAddress { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "物料分类")]
        public string GoodsClassify { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "运输供应商ID")]
        public string SupplierId { get; set; }

        [SugarColumn(ColumnDescription = "邮件通知状态(0:未发送  1:已发送)")]
        public bool IsEmailNotification { get; set; }

    }
}
