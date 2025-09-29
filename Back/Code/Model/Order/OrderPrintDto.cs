using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Models.Model.Order
{
    public class OrderPrintDto
    {
        /// <summary>
        ///发货单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        ///  订单号
        /// </summary>
        public string CustomerOrderNo { get; set; }
        /// <summary>
        /// 发货日期
        /// </summary>
        public string SendingDate { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public string SendingAddress { get; set; }
        /// <summary>
        /// 物品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 计划发货数量
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 合同号
        /// </summary>
        public string ContractNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 客户联系人
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户联系电话
        /// </summary>
        public string CustomerTelephone { get; set; }
        /// <summary>
        /// 我方公司信息
        /// </summary>
        public string OurCompany { get; set; }
        /// <summary>
        /// 我方联系人信息
        /// </summary>
        public string OurCompanyName { get; set; }
        /// <summary>
        /// 我方联系电话
        /// </summary>
        public string OurCompanyTelephone { get; set; }
        /// <summary>
        /// 运输公司
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// 货船编号
        /// </summary>
        public string SupplierNo { get; set; }
        /// <summary>
        /// 船长姓名
        /// </summary>
        public string SupplierName { get; set; }
 

    }
}
