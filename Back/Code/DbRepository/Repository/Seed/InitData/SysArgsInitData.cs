using DbRepository.Repository.DbModels;
using Models.Model.Enum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysArgsInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var sysArgs = new List<SysArgs> {
                new SysArgs() {ArgsKey="SysVersion", ArgsKeyName="系统版本", ArgsValue="v1.10", Remark= "", ArgsType= "Text",ArgsGroup="Args", Rank=1 ,ArgsSubGroup="Sys"},
                new SysArgs() {ArgsKey="AdminAccount", ArgsKeyName="开发者", ArgsValue="developer", Remark= "11C9C475F21FE001", ArgsType= "Text",ArgsGroup="Args", Rank=1 ,ArgsSubGroup="Sys"},
                new SysArgs() {ArgsKey="InitialPassword", ArgsKeyName="用户初始密码", ArgsValue="123456",  ArgsType= "Text",ArgsGroup="Args", Rank=2,IsVisible=true,ArgsSubGroup="Sys" },
                new SysArgs() {ArgsKey="GoodsPhotoLimit", ArgsKeyName="物品图片上传上限", ArgsValue="3",  ArgsType= "Number",ArgsGroup="Args", Rank=3,IsVisible=true,ArgsSubGroup="Goods" }, 
                new SysArgs() {ArgsKey="IsInStorageApproval", ArgsKeyName="启用入库审批", ArgsValue="false",  ArgsType= "Check",ArgsGroup="Args", Rank=7,IsVisible=false,ArgsSubGroup="Approval" },
                new SysArgs() {ArgsKey="IsOutStorageApproval", ArgsKeyName="启用出库审批", ArgsValue="false",  ArgsType= "Check",ArgsGroup="Args", Rank=8,IsVisible=false,ArgsSubGroup="Approval" },
                new SysArgs() {ArgsKey="IsReceivingApproval", ArgsKeyName="启用收货计划审批", ArgsValue="false",  ArgsType= "Check",ArgsGroup="Args", Rank=8,IsVisible=true,ArgsSubGroup="Approval" },
                new SysArgs() {ArgsKey="IsPurchaseOrderApproval", ArgsKeyName="启用采购订单审批", ArgsValue="false",  ArgsType= "Check",ArgsGroup="Args", Rank=8,IsVisible=true,ArgsSubGroup="Approval" },
                new SysArgs() {ArgsKey="IsSafetyInventoryApproval", ArgsKeyName="启用安全库存审批", ArgsValue="false",  ArgsType= "Check",ArgsGroup="Args", Rank=8,IsVisible=true,ArgsSubGroup="Approval" },
                new SysArgs() {ArgsKey="CarCodePrefix", ArgsKeyName="小车唯一码前缀", ArgsValue="CAR",  ArgsType= "Text",ArgsGroup="Args", Rank=9,IsVisible=false,ArgsSubGroup="Production" },
                new SysArgs() {ArgsKey="IsFIFO", ArgsKeyName="启用先进先出", ArgsValue="true",  ArgsType= "Check",ArgsGroup="Args", Rank=10,IsVisible=false,ArgsSubGroup="Production" },
                new SysArgs() {ArgsKey="ProductBufferWarehouse", ArgsKeyName="指定成品下线缓存仓类型", ArgsValue="ProductBuffer",  ArgsType= "SingleSelect",ArgsGroup="Args", Rank=11,IsVisible=false,ArgsSubGroup="Production" ,Remark="WarehouseType"},
                new SysArgs() {ArgsKey="UnstackWarehouse", ArgsKeyName="指定拆垛机器仓库类型", ArgsValue="Unstack",  ArgsType= "SingleSelect",ArgsGroup="Args", Rank=12,IsVisible=false,ArgsSubGroup="Production" ,Remark="WarehouseType"},
                new SysArgs() {ArgsKey="IsWorkbinNoSameBinNo", ArgsKeyName="料箱编号等同货位编号", ArgsValue="true",  ArgsType= "Check",ArgsGroup="Args", Rank=12,IsVisible=false,ArgsSubGroup="Inventory" },
                new SysArgs() {ArgsKey="ReceivingAddress", ArgsKeyName="物料收发货地址", ArgsValue="湖南省长沙市长沙县蓝田路318号",  ArgsType= "Text",ArgsGroup="Args", Rank=13,IsVisible=true,ArgsSubGroup="Inventory" },
                new SysArgs() {ArgsKey="IsAutoSendMailByMaterialRequirement", ArgsKeyName="自动邮件通知（物料需求计划）", ArgsValue="false",  ArgsType= "Check",ArgsGroup="Args", Rank=14,IsVisible=true,ArgsSubGroup="Plan" },
                new SysArgs() {ArgsKey="IsAutoSendMailBySavetyInvenstory", ArgsKeyName="自动邮件通知（安全库存预警）", ArgsValue="true",  ArgsType= "Check",ArgsGroup="Args", Rank=15,IsVisible=true,ArgsSubGroup="Plan" },
                new SysArgs() {ArgsKey="InStorageIsolateDays", ArgsKeyName="入库隔离（间隔天数）", ArgsValue="7",  ArgsType= "Number",ArgsGroup="Args", Rank=16,IsVisible=true,ArgsSubGroup="Plan" },

                new SysArgs() {ArgsKey="ClientType", ArgsKeyName="客户类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=1,IsVisible=true },
                new SysArgs() {ArgsKey="ClientLevel", ArgsKeyName="客户等级", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=2,IsVisible=true },
                new SysArgs() {ArgsKey="SupplierType", ArgsKeyName="供应商类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=3,IsVisible=true },
                new SysArgs() {ArgsKey="SupplierProperty", ArgsKeyName="供应商性质", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=4,IsVisible=true },
                new SysArgs() {ArgsKey="CreditType", ArgsKeyName="收款方式", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=5,IsVisible=true },
                new SysArgs() {ArgsKey="ProductLevel", ArgsKeyName="产品等级", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=6,IsVisible=true },
                new SysArgs() {ArgsKey="ProductProperty", ArgsKeyName="产品属性", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=7,IsVisible=true }, 
                new SysArgs() {ArgsKey="WarehouseType", ArgsKeyName="仓库类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=9,IsVisible=true }, 
                new SysArgs() {ArgsKey="SparePartType", ArgsKeyName="备件类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=11,IsVisible=true },
                new SysArgs() {ArgsKey="ConsumablesType", ArgsKeyName="耗材类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=11,IsVisible=true },
                new SysArgs() {ArgsKey="SamplePieceType", ArgsKeyName="样件类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=12,IsVisible=true },
                new SysArgs() {ArgsKey="SeparatorType", ArgsKeyName="辅材类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=12,IsVisible=true },
                new SysArgs() {ArgsKey="PackingMaterialType", ArgsKeyName="包材类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=12,IsVisible=true },
                new SysArgs() {ArgsKey="RawMaterialType", ArgsKeyName="原材料类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=12,IsVisible=true },
                new SysArgs() {ArgsKey="PurchaseChanne", ArgsKeyName="采购渠道", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=8,IsVisible=true },
                new SysArgs() {ArgsKey="PurchaseType", ArgsKeyName="采购方式", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=9,IsVisible=true },
                new SysArgs() {ArgsKey="Purchase-CPMG", ArgsKeyName="CPMG", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=9,IsVisible=true },
                new SysArgs() {ArgsKey="Purchase-Account", ArgsKeyName="采购科目", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=9,IsVisible=true },
                new SysArgs() {ArgsKey="Purchase-Classes", ArgsKeyName="采购类别", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=9,IsVisible=true },
                new SysArgs() {ArgsKey="SparePartRequisitionType", ArgsKeyName="备件领用类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=9,IsVisible=true,ArgsSubGroup="Requisition" },
                new SysArgs() {ArgsKey="GeneralRequisitionType", ArgsKeyName="通用领用类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=10,IsVisible=true,ArgsSubGroup="Requisition" },
                new SysArgs() {ArgsKey="ReceivingAbnormalType", ArgsKeyName="发货异常类型", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=11,IsVisible=true,ArgsSubGroup="Requisition" },
                new SysArgs() {ArgsKey="TakeStockReasonType", ArgsKeyName="盘点盈亏原因分类", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=12,IsVisible=true,ArgsSubGroup="TakeStock" },
                new SysArgs() {ArgsKey="Line", ArgsKeyName="产线", ArgsValue="",  ArgsType= "SingleSelect",ArgsGroup="Dic", Rank=13,IsVisible=true,ArgsSubGroup="TakeStock" },
            };
            db.Insertable(sysArgs).AddQueue();
        }
    }
}
