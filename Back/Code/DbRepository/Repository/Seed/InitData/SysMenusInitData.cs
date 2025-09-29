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
    internal class SysMenusInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        { 
            var menus = new List<SysMenus>() {
                 new SysMenus(){ MenuId ="RECEIVINGDELIVERYPLAN", ParentId="ROOT", MenuName="计划管理", Rank=2, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-caigou", MenuType="Menu",MenuDisplay="PC",IsValid=true,IsVisible=true, RoutePath="/purchase"},
                    new SysMenus(){ MenuId ="RECEIVINGMGR", ParentId="RECEIVINGDELIVERYPLAN", MenuName="收货计划", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",MenuDisplay="PC",IsValid=true,IsVisible=true,RoutePath="receiving",ComponentPath="purchase/receiving/index"},
                    new SysMenus(){ MenuId ="RECEIVINGREAD", ParentId="RECEIVINGMGR", MenuName="查看收货计划", Rank=1, CtrlName="ReceivingOrder",ActionName="GetOrders" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGADD", ParentId="RECEIVINGMGR", MenuName="添加收货计划", Rank=2, CtrlName="ReceivingOrder",ActionName="AddReceivingOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGUPDATE", ParentId="RECEIVINGMGR", MenuName="修改收货计划", Rank=3, CtrlName="ReceivingOrder",ActionName="UpdateReceivingOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGDEL", ParentId="RECEIVINGMGR", MenuName="删除收货计划", Rank=4, CtrlName="ReceivingOrder",ActionName="DelReceivingOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGAPPROVAL", ParentId="RECEIVINGMGR", MenuName="审批收货计划", Rank=5, CtrlName="ReceivingOrder",ActionName="ApprovalReceivingOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGNOTIFICATION", ParentId="RECEIVINGMGR", MenuName="邮件通知收货", Rank=6, CtrlName="ReceivingOrder",ActionName="AdviceReceiving" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGAFFIRM", ParentId="RECEIVINGMGR", MenuName="确认收货", Rank=7, CtrlName="ReceivingOrder",ActionName="SubmitReceived" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGABNORMAL", ParentId="RECEIVINGMGR", MenuName="修改异常到货", Rank=8, CtrlName="ReceivingOrder",ActionName="UpdateReceivingAbnormal" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGURGENCY", ParentId="RECEIVINGMGR", MenuName="修改紧急数量", Rank=9, CtrlName="ReceivingOrder",ActionName="UpdateReceivingUrgency" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGSAPIMPORT", ParentId="RECEIVINGMGR", MenuName="导入收货计划", Rank=10, CtrlName="ReceivingOrder",ActionName="ImportReceivingData" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGSAPEXPORT", ParentId="RECEIVINGMGR", MenuName="导出收货计划", Rank=11, CtrlName="ReceivingOrder",ActionName="ExportReceivingData" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGDELIVERYORDERADD", ParentId="RECEIVINGMGR", MenuName="创建送货单", Rank=12, CtrlName="ReceivingOrder",ActionName="AddDeliveryOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGDELIVERYORDERPRINT", ParentId="RECEIVINGMGR", MenuName="打印送货单", Rank=13, CtrlName="ReceivingOrder",ActionName="PrintDeliveryOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGMATLABELPRINT", ParentId="RECEIVINGMGR", MenuName="打印MAT-LABEL", Rank=14, CtrlName="ReceivingOrder",ActionName="PrintMatLabel" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGUPLOAD", ParentId="RECEIVINGMGR", MenuName="上传收货计划异常图片", Rank=15, CtrlName="ReceivingOrder",ActionName="UploadReceivingDocument" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGREADUPLOAD", ParentId="RECEIVINGMGR", MenuName="查看收货计划异常图片", Rank=16, CtrlName="ReceivingOrder",ActionName="GetBaseFiles" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RECEIVINGDELSENDINGFILES", ParentId="RECEIVINGMGR", MenuName="删除收货计划图片", Rank=17, CtrlName="ReceivingOrder",ActionName="DelSendingFiles" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="SENDINGMGR", ParentId="RECEIVINGDELIVERYPLAN", MenuName="发货计划", Rank=2, CtrlName="",ActionName="" ,MenuType="Menu",MenuDisplay="PC",IsValid=true,IsVisible=true,RoutePath="sending",ComponentPath="purchase/sending/index"},
                    new SysMenus(){ MenuId ="SENDINGREAD", ParentId="SENDINGMGR", MenuName="查看发货计划", Rank=1, CtrlName="SendingOrder",ActionName="GetSending" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGADD", ParentId="SENDINGMGR", MenuName="添加发货计划", Rank=2, CtrlName="SendingOrder",ActionName="AddSending" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGUPDATE", ParentId="SENDINGMGR", MenuName="修改发货计划", Rank=3, CtrlName="SendingOrder",ActionName="UpdateSending" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGDEL", ParentId="SENDINGMGR", MenuName="删除发货计划", Rank=4, CtrlName="SendingOrder",ActionName="DelSending" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGIMPORT", ParentId="SENDINGMGR", MenuName="导入发货计划", Rank=5, CtrlName="SendingOrder",ActionName="ImportSendingData" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGEXPORT", ParentId="SENDINGMGR", MenuName="导出发货计划", Rank=6, CtrlName="SendingOrder",ActionName="ExportSendingData" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGUPLOAD", ParentId="SENDINGMGR", MenuName="上传发货计划附件", Rank=7, CtrlName="SendingOrder",ActionName="UploadSendingDocument" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGREADUPLOAD", ParentId="SENDINGMGR", MenuName="查看发货计划附件", Rank=8, CtrlName="SendingOrder",ActionName="GetBaseFiles" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="DELSENDINGFILES", ParentId="SENDINGMGR", MenuName="删除发货计划附件", Rank=9, CtrlName="SendingOrder",ActionName="DelSendingFiles" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SENDINGNOTIFICATION", ParentId="SENDINGMGR", MenuName="发送邮件通知", Rank=10, CtrlName="SendingOrder",ActionName="SendingNotification" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},


                    new SysMenus(){ MenuId ="MATERIALREQUIREMENTPLAN", ParentId="RECEIVINGDELIVERYPLAN", MenuName="物料需求计划", Rank=3, CtrlName="",ActionName="" ,MenuType="Menu",MenuDisplay="PC",IsValid=true,IsVisible=true,RoutePath="mrp",ComponentPath="purchase/mrp/index"},
                    new SysMenus(){ MenuId ="MATERIALREQUIREMENTPLANREAD", ParentId="MATERIALREQUIREMENTPLAN", MenuName="查看物料需求计划", Rank=1, CtrlName="MaterialRequirementPlan",ActionName="GetPlanOrders" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true}, 
                    new SysMenus(){ MenuId ="MATERIALREQUIREMENTPLANEXPORT", ParentId="MATERIALREQUIREMENTPLAN", MenuName="导出物料需求计划", Rank=5, CtrlName="MaterialRequirementPlan",ActionName="ExportPlanOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="MATERIALREQUIREMENTPLANMAIL", ParentId="MATERIALREQUIREMENTPLAN", MenuName="发送物料需求计划邮件", Rank=5, CtrlName="MaterialRequirementPlan",ActionName="SendMailToSupplier" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="PURCHASEMGR", ParentId="RECEIVINGDELIVERYPLAN", MenuName="采购订单", Rank=4, CtrlName="",ActionName="" ,MenuType="Menu",MenuDisplay="PC",IsValid=true,IsVisible=true,RoutePath="purchase-order",ComponentPath="purchase/purchase-order/index"},
                    new SysMenus(){ MenuId ="PURCHASEREAD", ParentId="PURCHASEMGR", MenuName="查看采购订单", Rank=1, CtrlName="PurchaseOrder",ActionName="GetOrders" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASEADD", ParentId="PURCHASEMGR", MenuName="添加采购订单", Rank=2, CtrlName="PurchaseOrder",ActionName="AddPurchaseOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASEUPDATE", ParentId="PURCHASEMGR", MenuName="修改采购订单", Rank=3, CtrlName="PurchaseOrder",ActionName="UpdatePurchaseOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASEDEL", ParentId="PURCHASEMGR", MenuName="删除采购订单", Rank=4, CtrlName="PurchaseOrder",ActionName="DelPurchaseOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASEAPPROVAL", ParentId="PURCHASEMGR", MenuName="审批采购订单", Rank=5, CtrlName="PurchaseOrder",ActionName="ApprovalPurchaseOrder" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASERECEIVEDWH", ParentId="PURCHASEMGR", MenuName="仓库确认收货", Rank=6, CtrlName="PurchaseOrder",ActionName="SubmitReceivedWH" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASERECEIVEDWK", ParentId="PURCHASEMGR", MenuName="WK确认收货", Rank=7, CtrlName="PurchaseOrder",ActionName="SubmitReceivedWK" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASEUPDATEFLOWSTATUS", ParentId="PURCHASEMGR", MenuName="修改流程状态", Rank=8, CtrlName="PurchaseOrder",ActionName="UpdateFlowStatus" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PURCHASEEXPORT", ParentId="PURCHASEMGR", MenuName="导出采购订单", Rank=9, CtrlName="PurchaseOrder",ActionName="ExportPurchaseData" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},

                //仓库管理 
                new SysMenus(){ MenuId ="INVENTORYMGR", ParentId="ROOT", MenuName="仓储管理", Rank=3, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-cangkucangchu", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true, RoutePath="/inventory"},
                    new SysMenus(){ MenuId ="WAREHOUSEMGR", ParentId="INVENTORYMGR", MenuName="仓库管理", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="warehouse",ComponentPath="inventory/warehouse/index"},
                    new SysMenus(){ MenuId ="WAREHOUSEREAD", ParentId="WAREHOUSEMGR", MenuName="查看仓库", Rank=1, CtrlName="WarehouseBin",ActionName="GetWarehouses" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WAREHOUSEADD", ParentId="WAREHOUSEMGR", MenuName="添加仓库", Rank=2, CtrlName="WarehouseBin",ActionName="AddWarehouseBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WAREHOUSEUPDATE", ParentId="WAREHOUSEMGR", MenuName="修改仓库", Rank=3, CtrlName="WarehouseBin",ActionName="UpdateWarehouseBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WAREHOUSEDEL", ParentId="WAREHOUSEMGR", MenuName="删除仓库", Rank=4, CtrlName="WarehouseBin",ActionName="DelWarehouseBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="BINMGR", ParentId="INVENTORYMGR", MenuName="货位管理", Rank=2, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="bin",ComponentPath="inventory/bin/index"},
                    new SysMenus(){ MenuId ="BINREAD", ParentId="BINMGR", MenuName="查看货位", Rank=1, CtrlName="ShelfBin",ActionName="GetWarehouseTree" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SHELFADD", ParentId="BINMGR", MenuName="添加货架", Rank=2, CtrlName="ShelfBin",ActionName="AddShelf" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SHELFUPDATE", ParentId="BINMGR", MenuName="修改货架", Rank=3, CtrlName="ShelfBin",ActionName="UpdateShelf" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SHELFDEL", ParentId="BINMGR", MenuName="删除货架", Rank=4, CtrlName="ShelfBin",ActionName="DelShelf" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="BINADD", ParentId="BINMGR", MenuName="添加货位", Rank=5, CtrlName="ShelfBin",ActionName="AddBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="BINUPDATE", ParentId="BINMGR", MenuName="修改货位", Rank=6, CtrlName="ShelfBin",ActionName="UpdateBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="BINDEL", ParentId="BINMGR", MenuName="删除货位", Rank=7, CtrlName="ShelfBin",ActionName="DelBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="WORKBINMGR", ParentId="INVENTORYMGR", MenuName="料箱管理", Rank=3, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="workbin",ComponentPath="inventory/workbin/index"},
                    new SysMenus(){ MenuId ="WORKBINREAD", ParentId="WORKBINMGR", MenuName="查看料箱信息", Rank=1, CtrlName="Workbin",ActionName="GetWorkbins" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKBINUPDATE", ParentId="WORKBINMGR", MenuName="编辑料箱信息", Rank=1, CtrlName="Workbin",ActionName="UpdateWorkbin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKBINSPECADD", ParentId="WORKBINMGR", MenuName="添加料箱规格", Rank=1, CtrlName="Workbin",ActionName="AddWorkbinSpeci" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKBINSPECUPDATE", ParentId="WORKBINMGR", MenuName="修改料箱规格", Rank=1, CtrlName="Workbin",ActionName="UpdateWorkbinSpeci" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKBINSPECDEL", ParentId="WORKBINMGR", MenuName="删除料箱规格", Rank=1, CtrlName="Workbin",ActionName="DelWorkbinSpeci" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="WAREHOUSELAYOUT", ParentId="INVENTORYMGR", MenuName="仓库布局", Rank=3, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="layout",ComponentPath="inventory/layout/index"},
                    new SysMenus(){ MenuId ="LAYOUTREAD", ParentId="WAREHOUSELAYOUT", MenuName="查看布局", Rank=1, CtrlName="WarehouseLayout",ActionName="GetWarehouseTree" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="LAYOUTUNLOCK", ParentId="WAREHOUSELAYOUT", MenuName="货位解锁", Rank=1, CtrlName="WarehouseLayout",ActionName="UnlockBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                //库存
                new SysMenus(){ MenuId ="STOCKMGR", ParentId="ROOT", MenuName="库存管理", Rank=4, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-zhongzhuanzhan", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true, RoutePath="/stock"},
                  
                    new SysMenus(){ MenuId ="INSTORAGEMGR", ParentId="STOCKMGR", MenuName="入库管理", Rank=4, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,MenuDisplay="PC",RoutePath="instorage",ComponentPath="inventory/instorage/index"},
                    new SysMenus(){ MenuId ="INSTORAGEORDERREAD", ParentId="INSTORAGEMGR", MenuName="查看入库单", Rank=1, CtrlName="InStorage",ActionName="GetOrders" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="INSTORAGEORDERADD", ParentId="INSTORAGEMGR", MenuName="添加入库单", Rank=2, CtrlName="InStorage",ActionName="AddInStorage" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="INSTORAGEORDERUPDATE", ParentId="INSTORAGEMGR", MenuName="修改入库单", Rank=3, CtrlName="InStorage",ActionName="UpdateInStorage" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="INSTORAGEORDERDEL", ParentId="INSTORAGEMGR", MenuName="删除入库单", Rank=4, CtrlName="InStorage",ActionName="DelInStorage" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true}, 
                    new SysMenus(){ MenuId ="INSTORAGEORDEREXPORT", ParentId="INSTORAGEMGR", MenuName="导出入库单", Rank=6, CtrlName="InStorage",ActionName="ExportInStorage" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="INSTORAGEORDERCONFIRM", ParentId="INSTORAGEMGR", MenuName="入库确认", Rank=7, CtrlName="InStorage",ActionName="ConfirmInStorage" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="INSTORAGEORDERAGVSCHED", ParentId="INSTORAGEMGR", MenuName="调度AGV", Rank=8, CtrlName="InStorage",ActionName="AGVScheduling" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="OUTSTORAGEMGR", ParentId="STOCKMGR", MenuName="出库管理", Rank=5, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,MenuDisplay="PC",RoutePath="outstorage",ComponentPath="inventory/outstorage/index"},
                    new SysMenus(){ MenuId ="OUTSTORAGEORDERREAD", ParentId="OUTSTORAGEMGR", MenuName="查看出库单", Rank=1, CtrlName="OutStorage",ActionName="GetOrders" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="OUTSTORAGEORDERADD", ParentId="OUTSTORAGEMGR", MenuName="添加出库单", Rank=2, CtrlName="OutStorage",ActionName="AddOutStorage" ,MenuType="Action",IsValid=true,MenuDisplay = "PC", IsVisible=true},
                    new SysMenus(){ MenuId ="OUTSTORAGEORDERUPDATE", ParentId="OUTSTORAGEMGR", MenuName="修改出库单", Rank=3, CtrlName="OutStorage",ActionName="UpdateOutStorage" ,MenuType="Action",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="OUTSTORAGEORDERDEL", ParentId="OUTSTORAGEMGR", MenuName="删除出库单", Rank=4, CtrlName="OutStorage",ActionName="DelOutStorage" ,MenuType="Action",IsValid=true,MenuDisplay = "PC", IsVisible=true}, 
                    new SysMenus(){ MenuId ="OUTSTORAGEORDEREXPORT", ParentId="OUTSTORAGEMGR", MenuName="导出出库单", Rank=6, CtrlName="OutStorage",ActionName="ExportOutStorage" ,MenuType="Action",IsValid=true,MenuDisplay = "PC", IsVisible=true},
                    new SysMenus(){ MenuId ="OUTSTORAGEORDERCONFIRM", ParentId="OUTSTORAGEMGR", MenuName="出库确认", Rank=7, CtrlName="OutStorage",ActionName="ConfirmOutStorage" ,MenuType="Action",IsValid=true,MenuDisplay = "PC", IsVisible=true},
                    new SysMenus(){ MenuId ="OUTSTORAGEORDERAGVSCHED", ParentId="OUTSTORAGEMGR", MenuName="调度AGV", Rank=8, CtrlName="OutStorage",ActionName="AGVScheduling" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="STORAGEMGR", ParentId="STOCKMGR", MenuName="库存信息", Rank=6, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,RoutePath="storage",MenuDisplay = "PC", ComponentPath="inventory/storage/index"},
                    new SysMenus(){ MenuId ="STORAGEREAD", ParentId="STORAGEMGR", MenuName="查看库存汇总", Rank=1, CtrlName="Storage",ActionName="GetStorageList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="STORAGEDETAILREAD", ParentId="STORAGEMGR", MenuName="查看库存明细", Rank=2, CtrlName="Storage",ActionName="GetStorageDetailList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="STORAGEFLOWREAD", ParentId="STORAGEMGR", MenuName="查看库存流水", Rank=3, CtrlName="Storage",ActionName="GetStorageFlowList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="STORAGEALLOCATION", ParentId="STORAGEMGR", MenuName="添加库存调拨", Rank=4, CtrlName="Storage",ActionName="AddAllocationStorage" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="STORAGEEXPORT", ParentId="STORAGEMGR", MenuName="导出库存汇总信息", Rank=5, CtrlName="Storage",ActionName="ExportStorage" ,MenuType="Action",IsValid=true,MenuDisplay = "PC", IsVisible=true},
                    new SysMenus(){ MenuId ="STORAGEDETAILEXPORT", ParentId="STORAGEMGR", MenuName="导出库存明细信息", Rank=6, CtrlName="Storage",ActionName="ExportStorageDetail" ,MenuType="Action",IsValid=true,MenuDisplay = "PC", IsVisible=true},
                    new SysMenus(){ MenuId ="STORAGEFLOWEXPORT", ParentId="STORAGEMGR", MenuName="导出库存流水记录", Rank=7, CtrlName="Storage",ActionName="ExportStorageFlow" ,MenuType="Action",IsValid=true,MenuDisplay = "PC", IsVisible=true},

                    new SysMenus(){ MenuId ="TAKESTOCKMGR", ParentId="STOCKMGR", MenuName="库存盘点", Rank=7, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,RoutePath="takestock",MenuDisplay = "PC", ComponentPath="inventory/takestock/index"},
                    new SysMenus(){ MenuId ="TAKESTOCKREAD", ParentId="TAKESTOCKMGR", MenuName="查看盘点记录", Rank=1, CtrlName="TakeStock",ActionName="GetTakeStockHis" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="TAKESTOCKEXPORT", ParentId="TAKESTOCKMGR", MenuName="导出盘点记录", Rank=2, CtrlName="TakeStock",ActionName="ExportTakeStockHis" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="TAKESTOCKUPDATEREASON", ParentId="TAKESTOCKMGR", MenuName="修改原因分析", Rank=3, CtrlName="TakeStock",ActionName="UpdateReason" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="SAFETYINVENTORYMGR", ParentId="STOCKMGR", MenuName="安全库存", Rank=8, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,RoutePath="safety-inventory",MenuDisplay = "PC", ComponentPath="inventory/safety-inventory/index"},
                    new SysMenus(){ MenuId ="SAFETYINVENTORYREAD", ParentId="SAFETYINVENTORYMGR", MenuName="查看库存预警", Rank=1, CtrlName="SafetyWarningRecord",ActionName="GetWarningInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAFETYINVENTORYUPDATE", ParentId="SAFETYINVENTORYMGR", MenuName="修改预警信息", Rank=2, CtrlName="SafetyWarningRecord",ActionName="UpdateWarningInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAFETYINVENTORYEXPORT", ParentId="SAFETYINVENTORYMGR", MenuName="导出预警信息", Rank=3, CtrlName="SafetyWarningRecord",ActionName="ExportWarningInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAFETYINVENTORYAPPROVAL", ParentId="SAFETYINVENTORYMGR", MenuName="审批预警信息", Rank=4, CtrlName="SafetyWarningRecord",ActionName="ApprovalSafetyInventory" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true}, 
                    //领料
                    new SysMenus(){ MenuId ="REQUISITIONMGR", ParentId="ROOT", MenuName="物品领用", Rank=1, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-yewu-xianxing",MenuType="Menu",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true,RoutePath="requisition",ComponentPath="inventory/requisition/index"},
                    new SysMenus(){ MenuId ="REQUISITIONEREAD", ParentId="REQUISITIONMGR", MenuName="查看领用单", Rank=1, CtrlName="Requisition",ActionName="GetOrderList" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONADD", ParentId="REQUISITIONMGR", MenuName="添加领用单", Rank=2, CtrlName="Requisition",ActionName="AddReqisitionOrder" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONUPDATE", ParentId="REQUISITIONMGR", MenuName="修改领用单", Rank=3, CtrlName="Requisition",ActionName="UpdateReqisitionOrder" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONDEL", ParentId="REQUISITIONMGR", MenuName="删除领用单", Rank=4, CtrlName="Requisition",ActionName="DelReqisitionOrder" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONRECEIVE", ParentId="REQUISITIONMGR", MenuName="确认收货", Rank=5, CtrlName="Requisition",ActionName="ConfirmReceived" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},

                    new SysMenus(){ MenuId ="REQUISITIONCONSUMABLESMGR", ParentId="ROOT", MenuName="耗材领用", Rank=1, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-yewu-xianxing",MenuType="Menu",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true,RoutePath="requisition-consumables",ComponentPath="inventory/requisition-consumables/index"},
                    new SysMenus(){ MenuId ="REQUISITIONECONSUMABLESREAD", ParentId="REQUISITIONCONSUMABLESMGR", MenuName="查看领用单", Rank=1, CtrlName="Requisition",ActionName="GetOrderList" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONCONSUMABLESADD", ParentId="REQUISITIONCONSUMABLESMGR", MenuName="添加领用单", Rank=2, CtrlName="Requisition",ActionName="AddReqisitionOrder" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONCONSUMABLESUPDATE", ParentId="REQUISITIONCONSUMABLESMGR", MenuName="修改领用单", Rank=3, CtrlName="Requisition",ActionName="UpdateReqisitionOrder" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONCONSUMABLESDEL", ParentId="REQUISITIONCONSUMABLESMGR", MenuName="删除领用单", Rank=4, CtrlName="Requisition",ActionName="DelReqisitionOrder" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="REQUISITIONCONSUMABLESRECEIVE", ParentId="REQUISITIONCONSUMABLESMGR", MenuName="确认收货", Rank=5, CtrlName="Requisition",ActionName="ConfirmReceived" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    //仓库看板
                    new SysMenus(){ MenuId ="BUFFERWAREHOUSEDASHBOARD", ParentId="ROOT", MenuName="缓存仓看板", Rank=1, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-baobiao",MenuType="Menu",IsValid=false,MenuDisplay="DASHBOARD",IsVisible=false, RoutePath="bufferWarehouseDashboard",ComponentPath="production/bufferWarehouseDashboard/index"},

                  //生产订单管理
                  new SysMenus(){ MenuId ="PRODUCTIONMGR", ParentId="ROOT", MenuName="生产管理", Rank=2, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-createtask", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=false, RoutePath="/production"},
                    new SysMenus(){ MenuId ="PRODORDERMGR", ParentId="PRODUCTIONMGR", MenuName="生产订单管理", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="orders",ComponentPath="production/orders/index"},
                    new SysMenus(){ MenuId ="PRODORDERREAD", ParentId="PRODORDERMGR", MenuName="查看订单", Rank=1, CtrlName="ProductionOrder",ActionName="GetOrders" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PRODORDERADD", ParentId="PRODORDERMGR", MenuName="添加订单", Rank=2, CtrlName="ProductionOrder",ActionName="AddOrder" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PRODORDERUPDATE", ParentId="PRODORDERMGR", MenuName="修改订单", Rank=3, CtrlName="ProductionOrder",ActionName="UpdateOrder" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PRODORDERDEL", ParentId="PRODORDERMGR", MenuName="删除订单", Rank=4, CtrlName="ProductionOrder",ActionName="DelOrder" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PRODORDERCARCODECREATE", ParentId="PRODORDERMGR", MenuName="创建小车唯一码", Rank=5, CtrlName="ProductionOrder",ActionName="CreateCarCode" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PRODORDERPRINT", ParentId="PRODORDERMGR", MenuName="打印订单", Rank=6, CtrlName="ProductionOrder",ActionName="PrintOrder" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PRODORDERCLOSED", ParentId="PRODORDERMGR", MenuName="关闭订单", Rank=7, CtrlName="ProductionOrder",ActionName="UpdateOrderClosed" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="PRODORDERPRINTPAGE", ParentId="ROOT", MenuName="生产订单打印预览", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="OTHER",IsVisible=true,HideMenu=true, RoutePath="printOrder",ComponentPath="production/orders/printOrder"},

                    //成品包装 
                    new SysMenus(){ MenuId ="PRODPACKAGEMGR", ParentId="ROOT", MenuName="成品包装管理", Rank=1, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-chaibaoguoqujian",MenuType="Menu",IsValid=false,MenuDisplay="SEPARATE",IsVisible=false,RoutePath="package",ComponentPath="production/package/index"},
                    new SysMenus(){ MenuId ="PRODPACKAGEREAD", ParentId="PRODPACKAGEMGR", MenuName="查看包装配对", Rank=1, CtrlName="ProductPackage",ActionName="GetMatchingInfo" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                    new SysMenus(){ MenuId ="PRODPACKAGECLEARMATCH", ParentId="PRODPACKAGEMGR", MenuName="配对包装提交", Rank=2, CtrlName="ProductPackage",ActionName="SubmitMatchPackage" ,MenuType="Action",IsValid=true,MenuDisplay="SEPARATE",IsVisible=true},
                  
                    //缓存仓看板
                  new SysMenus(){ MenuId ="WAREHOUSEPICKDASHBOARD", ParentId="ROOT", MenuName="仓库拣货看板", Rank=1, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-jiankongshexiangtou",MenuType="Menu",IsValid=false,MenuDisplay="DASHBOARD",IsVisible=true, RoutePath="warehousePickDashboard",ComponentPath="inventory/warehousePickDashboard/index"},
                 
                    //车间设备管理
                new SysMenus(){ MenuId ="WORKSHOPDEVICE", ParentId="ROOT", MenuName="车间设备管理", Rank=5, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-PDAshouchigongzuoshebei", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true, RoutePath="/workshopdevice"},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEINFO", ParentId="WORKSHOPDEVICE", MenuName="设备与仓位", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="device-info",ComponentPath="workshopdevice/device-info/index"},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEINFOREAD", ParentId="WORKSHOPDEVICEINFO", MenuName="查看设备信息", Rank=1, CtrlName="WorkShopDeviceInfo",ActionName="GetDeviceInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEINFOADD", ParentId="WORKSHOPDEVICEINFO", MenuName="添加设备信息", Rank=2, CtrlName="WorkShopDeviceInfo",ActionName="AddDeviceInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEINFOUPDATE", ParentId="WORKSHOPDEVICEINFO", MenuName="修改设备信息", Rank=3, CtrlName="WorkShopDeviceInfo",ActionName="UpdateDeviceInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEINFODEL", ParentId="WORKSHOPDEVICEINFO", MenuName="删除设备信息", Rank=4, CtrlName="WorkShopDeviceInfo",ActionName="DelDeviceInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEBINADD", ParentId="WORKSHOPDEVICEINFO", MenuName="添加设备仓位", Rank=5, CtrlName="WorkShopDeviceInfo",ActionName="AddDeviceBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEBINUPDATE", ParentId="WORKSHOPDEVICEINFO", MenuName="修改设备仓位", Rank=6, CtrlName="WorkShopDeviceInfo",ActionName="UpdateDeviceBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="WORKSHOPDEVICEBINDEL", ParentId="WORKSHOPDEVICEINFO", MenuName="删除设备仓位", Rank=7, CtrlName="WorkShopDeviceInfo",ActionName="DelDeviceBin" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="AGVINFOMGR", ParentId="WORKSHOPDEVICE", MenuName="AGV配置", Rank=2, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="agv-info",ComponentPath="workshopdevice/agv-info/index"},
                    new SysMenus(){ MenuId ="AGVINFOREAD", ParentId="AGVINFOMGR", MenuName="查看AGV配置", Rank=1, CtrlName="AGVSetting",ActionName="GetAGVInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="AGVINFOADD", ParentId="AGVINFOMGR", MenuName="添加AGV配置", Rank=2, CtrlName="AGVSetting",ActionName="AddAGVInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="AGVINFOUPDATE", ParentId="AGVINFOMGR", MenuName="修改AGV配置", Rank=3, CtrlName="AGVSetting",ActionName="UpdateAGVInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="AGVINFODEL", ParentId="AGVINFOMGR", MenuName="删除AGV配置", Rank=4, CtrlName="AGVSetting",ActionName="DelAGVInfo" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                   
                  //基础信息管理 
                 new SysMenus(){ MenuId ="GOODSINFOMGR", ParentId="ROOT", MenuName="货品信息", Rank=6, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-tijikongjian", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true, RoutePath="/goodsinfo"},
                    new SysMenus(){ MenuId ="SAMPLEPIECEMGR", ParentId="GOODSINFOMGR", MenuName="样件管理", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="sample-piece",ComponentPath="goodsinfo/sample-piece/index"},
                    new SysMenus(){ MenuId ="SAMPLEPIECEREAD", ParentId="SAMPLEPIECEMGR", MenuName="查看样件信息", Rank=1, CtrlName="SamplePiece",ActionName="GetSamplePieceList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAMPLEPIECEADD", ParentId="SAMPLEPIECEMGR", MenuName="添加样件信息", Rank=2, CtrlName="SamplePiece",ActionName="AddSamplePiece" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAMPLEPIECEUPDATE", ParentId="SAMPLEPIECEMGR", MenuName="修改样件信息", Rank=3, CtrlName="SamplePiece",ActionName="UpdateSamplePiece" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAMPLEPIECEDEL", ParentId="SAMPLEPIECEMGR", MenuName="删除样件信息", Rank=4, CtrlName="SamplePiece",ActionName="DelSamplePiece" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAMPLEPIECEEXPORT", ParentId="SAMPLEPIECEMGR", MenuName="导出样件信息", Rank=5, CtrlName="SamplePiece",ActionName="ExportSamplePiece" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SAMPLEPIECETYPEADD", ParentId="SAMPLEPIECEMGR", MenuName="添加样件类型", Rank=6, CtrlName="SamplePiece",ActionName="AddSamplePieceClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SAMPLEPIECETYPEUPDATE", ParentId="SAMPLEPIECEMGR", MenuName="编辑样件类型", Rank=7, CtrlName="SamplePiece",ActionName="UpdateSamplePieceClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SAMPLEPIECETYPEDEL", ParentId="SAMPLEPIECEMGR", MenuName="删除样件类型", Rank=8, CtrlName="SamplePiece",ActionName="DelSamplePieceClassify" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="SPAREPARTMGR", ParentId="GOODSINFOMGR", MenuName="备件管理", Rank=2, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="spare-part",ComponentPath="goodsinfo/spare-part/index"},
                    new SysMenus(){ MenuId ="SPAREPARTREAD", ParentId="SPAREPARTMGR", MenuName="查看备件信息", Rank=1, CtrlName="SparePart",ActionName="GetSparePartList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SPAREPARTADD", ParentId="SPAREPARTMGR", MenuName="添加备件信息", Rank=2, CtrlName="SparePart",ActionName="AddSparePart" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SPAREPARTUPDATE", ParentId="SPAREPARTMGR", MenuName="修改备件信息", Rank=3, CtrlName="SparePart",ActionName="UpdateSparePart" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SPAREPARTDEL", ParentId="SPAREPARTMGR", MenuName="删除备件信息", Rank=4, CtrlName="SparePart",ActionName="DelSparePart" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SPAREPARTEXPORT", ParentId="SPAREPARTMGR", MenuName="导出备件信息", Rank=5, CtrlName="SparePart",ActionName="ExportSparePart" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SPAERPARTTYPEADD", ParentId="SPAREPARTMGR", MenuName="添加备件类型", Rank=6, CtrlName="SparePart",ActionName="AddSparePartType" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SPAERPARTTYPEUPDATE", ParentId="SPAREPARTMGR", MenuName="编辑备件类型", Rank=7, CtrlName="SparePart",ActionName="UpdateSparePartType" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SPAERPARTTYPEDEL", ParentId="SPAREPARTMGR", MenuName="删除备件类型", Rank=8, CtrlName="SparePart",ActionName="DelSparePartType" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="SEPARATORMGR", ParentId="GOODSINFOMGR", MenuName="辅材管理", Rank=3, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="separator",ComponentPath="goodsinfo/separator/index"},
                    new SysMenus(){ MenuId ="SEPARATORREAD", ParentId="SEPARATORMGR", MenuName="查看辅材信息", Rank=1, CtrlName="Separator",ActionName="GetSeparatorList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SEPARATORADD", ParentId="SEPARATORMGR", MenuName="添加辅材信息", Rank=2, CtrlName="Separator",ActionName="AddSeparator" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SEPARATORUPDATE", ParentId="SEPARATORMGR", MenuName="修改辅材信息", Rank=3, CtrlName="Separator",ActionName="UpdateSeparator" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SEPARATORDEL", ParentId="SEPARATORMGR", MenuName="删除辅材信息", Rank=4, CtrlName="Separator",ActionName="DelSeparator" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SEPARATOREXPORT", ParentId="SEPARATORMGR", MenuName="导出辅材信息", Rank=5, CtrlName="Separator",ActionName="ExportSeparator" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SEPARATORTYPEADD", ParentId="SEPARATORMGR", MenuName="添加辅材类型", Rank=6, CtrlName="Separator",ActionName="AddSeparatorClassify" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SEPARATORTYPEUPDATE", ParentId="SEPARATORMGR", MenuName="编辑辅材类型", Rank=7, CtrlName="Separator",ActionName="UpdateSeparatorClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="SEPARATORTYPEDEL", ParentId="SEPARATORMGR", MenuName="删除辅材类型", Rank=8, CtrlName="Separator",ActionName="DelSeparatorClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="PACKINGMATERIALMGR", ParentId="GOODSINFOMGR", MenuName="包材管理", Rank=4, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="packing-material",ComponentPath="goodsinfo/packing-material/index"},
                    new SysMenus(){ MenuId ="PACKINGMATERIALREAD", ParentId="PACKINGMATERIALMGR", MenuName="查看包材信息", Rank=1, CtrlName="PackingMaterial",ActionName="GetPackingMaterialList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PACKINGMATERIALADD", ParentId="PACKINGMATERIALMGR", MenuName="添加包材信息", Rank=2, CtrlName="PackingMaterial",ActionName="AddPackingMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PACKINGMATERIALUPDATE", ParentId="PACKINGMATERIALMGR", MenuName="修改包材信息", Rank=3, CtrlName="PackingMaterial",ActionName="UpdatePackingMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PACKINGMATERIALDEL", ParentId="PACKINGMATERIALMGR", MenuName="删除包材信息", Rank=4, CtrlName="PackingMaterial",ActionName="DelPackingMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PACKINGMATERIALEXPORT", ParentId="PACKINGMATERIALMGR", MenuName="导出包材信息", Rank=5, CtrlName="PackingMaterial",ActionName="ExportPackingMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PACKINGMATERIALTYPEADD", ParentId="PACKINGMATERIALMGR", MenuName="添加包材类型", Rank=6, CtrlName="PackingMaterial",ActionName="AddPackingMaterialClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PACKINGMATERIALTYPEUPDATE", ParentId="PACKINGMATERIALMGR", MenuName="编辑包材类型", Rank=7, CtrlName="PackingMaterial",ActionName="UpdatePackingMaterialClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="PACKINGMATERIALTYPEDEL", ParentId="PACKINGMATERIALMGR", MenuName="删除包材类型", Rank=8, CtrlName="PackingMaterial",ActionName="DelPackingMaterialClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="FINISHEDPRODUCTMGR", ParentId="GOODSINFOMGR", MenuName="成品管理", Rank=5, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="finished-product",ComponentPath="goodsinfo/finished-product/index"},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTREAD", ParentId="FINISHEDPRODUCTMGR", MenuName="查看成品信息", Rank=1, CtrlName="FinishedProduct",ActionName="GetFinishedProductList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTADD", ParentId="FINISHEDPRODUCTMGR", MenuName="添加成品信息", Rank=2, CtrlName="FinishedProduct",ActionName="AddFinishedProduct" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTUPDATE", ParentId="FINISHEDPRODUCTMGR", MenuName="修改成品信息", Rank=3, CtrlName="FinishedProduct",ActionName="UpdateFinishedProduct" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTDEL", ParentId="FINISHEDPRODUCTMGR", MenuName="删除成品信息", Rank=4, CtrlName="FinishedProduct",ActionName="DelFinishedProduct" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTEXPORT", ParentId="FINISHEDPRODUCTMGR", MenuName="导出成品信息", Rank=5, CtrlName="FinishedProduct",ActionName="ExportFinishedProduct" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTBOM", ParentId="FINISHEDPRODUCTMGR", MenuName="BOM维护", Rank=5, CtrlName="FinishedProduct",ActionName="UpdateBOM" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTTYPEADD", ParentId="FINISHEDPRODUCTMGR", MenuName="添加成品类型", Rank=6, CtrlName="FinishedProduct",ActionName="AddFinishedProductClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTTYPEUPDATE", ParentId="FINISHEDPRODUCTMGR", MenuName="编辑成品类型", Rank=7, CtrlName="FinishedProduct",ActionName="UpdateFinishedProductClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="FINISHEDPRODUCTTYPEDEL", ParentId="FINISHEDPRODUCTMGR", MenuName="删除成品类型", Rank=8, CtrlName="FinishedProduct",ActionName="DelFinishedProductClassify" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},

                    new SysMenus(){ MenuId ="RAWMATERIALMGR", ParentId="GOODSINFOMGR", MenuName="原材料管理", Rank=5, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="raw-material",ComponentPath="goodsinfo/raw-material/index"},
                    new SysMenus(){ MenuId ="RAWMATERIALREAD", ParentId="RAWMATERIALMGR", MenuName="查看原材料信息", Rank=1, CtrlName="RawMaterial",ActionName="GetRawMaterialList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="RAWMATERIALADD", ParentId="RAWMATERIALMGR", MenuName="添加原材料信息", Rank=2, CtrlName="RawMaterial",ActionName="AddRawMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="RAWMATERIALUPDATE", ParentId="RAWMATERIALMGR", MenuName="修改原材料信息", Rank=3, CtrlName="RawMaterial",ActionName="UpdateRawMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="RAWMATERIALDEL", ParentId="RAWMATERIALMGR", MenuName="删除原材料信息", Rank=4, CtrlName="RawMaterial",ActionName="DelRawMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="RAWMATERIALEXPORT", ParentId="RAWMATERIALMGR", MenuName="导出原材料信息", Rank=5, CtrlName="RawMaterial",ActionName="ExportRawMaterial" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true}, 
                    new SysMenus(){ MenuId ="RAWMATERIALTYPEADD", ParentId="RAWMATERIALMGR", MenuName="添加原材料类型", Rank=6, CtrlName="RawMaterial",ActionName="AddRawMaterialClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RAWMATERIALTYPEUPDATE", ParentId="RAWMATERIALMGR", MenuName="编辑原材料类型", Rank=7, CtrlName="RawMaterial",ActionName="UpdateRawMaterialClassify" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},
                    new SysMenus(){ MenuId ="RAWMATERIALTYPEDEL", ParentId="RAWMATERIALMGR", MenuName="删除原材料类型", Rank=8, CtrlName="RawMaterial",ActionName="DelRawMaterialClassify" ,MenuType="Action",MenuDisplay = "PC", IsValid=true,IsVisible=true},
                    
                    new SysMenus(){ MenuId ="CONSUMABLESMGR", ParentId="GOODSINFOMGR", MenuName="耗材管理", Rank=6, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="consumables",ComponentPath="goodsinfo/consumables/index"},
                    new SysMenus(){ MenuId ="CONSUMABLESREAD", ParentId="CONSUMABLESMGR", MenuName="查看耗材信息", Rank=1, CtrlName="Consumables",ActionName="GetConsumablesList" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CONSUMABLESADD", ParentId="CONSUMABLESMGR", MenuName="添加耗材信息", Rank=2, CtrlName="Consumables",ActionName="AddConsumables" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CONSUMABLESUPDATE", ParentId="CONSUMABLESMGR", MenuName="修改耗材信息", Rank=3, CtrlName="Consumables",ActionName="UpdateConsumables" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CONSUMABLESDEL", ParentId="CONSUMABLESMGR", MenuName="删除耗材信息", Rank=4, CtrlName="Consumables",ActionName="DelConsumables" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CONSUMABLESEXPORT", ParentId="CONSUMABLESMGR", MenuName="导出耗材信息", Rank=5, CtrlName="Consumables",ActionName="ExportConsumables" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CONSUMABLESTYPEADD", ParentId="CONSUMABLESMGR", MenuName="添加耗材类型", Rank=6, CtrlName="Consumables",ActionName="AddConsumablesType" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CONSUMABLESTYPEUPDATE", ParentId="CONSUMABLESMGR", MenuName="编辑耗材类型", Rank=7, CtrlName="Consumables",ActionName="UpdateConsumablesType" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CONSUMABLESTYPEDEL", ParentId="CONSUMABLESMGR", MenuName="删除耗材类型", Rank=8, CtrlName="Consumables",ActionName="DelConsumablesType" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="UNITMGR", ParentId="GOODSINFOMGR", MenuName="计量单位", Rank=7, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="unit",ComponentPath="goodsinfo/unit/index"},
                    new SysMenus(){ MenuId ="UNITREAD", ParentId="UNITMGR", MenuName="查看单位信息", Rank=1, CtrlName="Unit",ActionName="GetUnits" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="UNITADD", ParentId="UNITMGR", MenuName="添加单位信息", Rank=2, CtrlName="Unit",ActionName="AddUnit" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="UNITUPDATE", ParentId="UNITMGR", MenuName="修改单位信息", Rank=3, CtrlName="Unit",ActionName="UpdateUnit" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="UNITDEL", ParentId="UNITMGR", MenuName="删除单位信息", Rank=4, CtrlName="Unit",ActionName="DelUnit" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                   //标签管理
                  new SysMenus(){ MenuId ="LABELMGR", ParentId="ROOT", MenuName="标签管理", Rank=7, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-label", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true, RoutePath="/label"},
                    new SysMenus(){ MenuId ="LABELDESIGNMGR", ParentId="LABELMGR", MenuName="标签设计", Rank=7, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="label-design",ComponentPath="label/label-design/index"},
                    new SysMenus(){ MenuId ="LABELDESIGNREAD", ParentId="LABELDESIGNMGR", MenuName="查看标签", Rank=1, CtrlName="MaterialLabelDesign",ActionName="GetLabelDesign" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="LABELDESIGNADD", ParentId="LABELDESIGNMGR", MenuName="添加标签", Rank=2, CtrlName="MaterialLabelDesign",ActionName="AddLabelDesign" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="LABELDESIGNUPDATE", ParentId="LABELDESIGNMGR", MenuName="修改标签", Rank=3, CtrlName="MaterialLabelDesign",ActionName="UpdateLabelDesign" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="LABELDESIGNUPDATEDEFT", ParentId="LABELDESIGNMGR", MenuName="修改默认模板", Rank=4, CtrlName="MaterialLabelDesign",ActionName="UpdateLabelDeft" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="LABELDESIGNDEL", ParentId="LABELDESIGNMGR", MenuName="删除标签", Rank=5, CtrlName="MaterialLabelDesign",ActionName="DelLabelDesign" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="LABELPRINTMGR", ParentId="LABELMGR", MenuName="标签打印", Rank=8, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="label-print",ComponentPath="label/label-print/index"},
                    new SysMenus(){ MenuId ="LABELPRINT", ParentId="LABELPRINTMGR", MenuName="打印标签", Rank=1, CtrlName="LabelPrint",ActionName="AddPrintRecord" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                
                 //客户供应商管理
                 new SysMenus(){ MenuId ="CLIENTSUPPLIERMGR", ParentId="ROOT", MenuName="客户与供应商", Rank=6, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-hezuoguanxi", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true, RoutePath="/clientsupplier"},
                    new SysMenus(){ MenuId ="CLIENTMGR", ParentId="CLIENTSUPPLIERMGR", MenuName="客户管理", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="client",ComponentPath="clientsupplier/client/index"},
                    new SysMenus(){ MenuId ="CLIENTREAD", ParentId="CLIENTMGR", MenuName="查看客户信息", Rank=1, CtrlName="Client",ActionName="GetClients" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CLIENTADD", ParentId="CLIENTMGR", MenuName="添加客户信息", Rank=2, CtrlName="Client",ActionName="AddClient" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CLIENTUPDATE", ParentId="CLIENTMGR", MenuName="修改客户信息", Rank=3, CtrlName="Client",ActionName="UpdateClient" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CLIENTDEL", ParentId="CLIENTMGR", MenuName="删除客户信息", Rank=4, CtrlName="Client",ActionName="DelClient" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="CLIENTEXPORT", ParentId="CLIENTMGR", MenuName="导出客户信息", Rank=5, CtrlName="Client",ActionName="ExportClients" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="SUPPLIERMGR", ParentId="CLIENTSUPPLIERMGR", MenuName="供应商管理", Rank=2, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="supplier",ComponentPath="clientsupplier/supplier/index"},
                    new SysMenus(){ MenuId ="SUPPLIERREAD", ParentId="SUPPLIERMGR", MenuName="查看供应商信息", Rank=1, CtrlName="Supplier",ActionName="GetSuppliers" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SUPPLIERADD", ParentId="SUPPLIERMGR", MenuName="添加供应商信息", Rank=2, CtrlName="Supplier",ActionName="AddSupplier" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SUPPLIERUPDATE", ParentId="SUPPLIERMGR", MenuName="修改供应商信息", Rank=3, CtrlName="Supplier",ActionName="UpdateSupplier" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SUPPLIERDEL", ParentId="SUPPLIERMGR", MenuName="删除供应商信息", Rank=4, CtrlName="Supplier",ActionName="DelSupplier" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SUPPLIEREXPORT", ParentId="SUPPLIERMGR", MenuName="导出供应商信息", Rank=5, CtrlName="Supplier",ActionName="ExportSuppliers" ,MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                 //报表管理
                 new SysMenus(){ MenuId ="REPORTSMGR", ParentId="ROOT", MenuName="报表管理", Rank=2, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-bingtu", MenuType="Menu",MenuDisplay="PC",IsValid=true,IsVisible=true, RoutePath="/reports"},
                    new SysMenus(){ MenuId ="REPORTSPAREPART", ParentId="REPORTSMGR", MenuName="备件报表", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",MenuDisplay="EXTERNAL",IsValid=true,IsVisible=true,RoutePath="sparepart",ComponentPath="reports/sparepart/index",Url="https://app.powerbi.com/groups/914ae554-ec29-40a1-bd4c-8e5e62970ce2/reports/fb6380fc-7c82-4e1b-a501-8a1be431c200/ReportSection?experience=power-bi"},
                    new SysMenus(){ MenuId ="REPORTSPAREPARTREAD", ParentId="REPORTSPAREPART", MenuName="备件报表查看", Rank=1, CtrlName="ReportSparepart",ActionName="" ,MenuType="Action",MenuDisplay="EXTERNAL",IsValid=true,IsVisible=true},
                    
                    new SysMenus(){ MenuId ="REPORTSAMPLEPIECE", ParentId="REPORTSMGR", MenuName="样件报表", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",MenuDisplay="PC",IsValid=true,IsVisible=true,RoutePath="samplepiece",ComponentPath="reports/samplepiece/index"},
                    new SysMenus(){ MenuId ="REPORTSAMPLEPIECEREAD", ParentId="REPORTSAMPLEPIECE", MenuName="样件报表查看", Rank=1, CtrlName="ReportSamplePiece",ActionName="" ,MenuType="Action",MenuDisplay="PC",IsValid=true,IsVisible=true},

                //系统信息管理(用户管理 角色管理 部门管理 企业管理)
                new SysMenus(){ MenuId ="SYSINFO", ParentId="ROOT", MenuName="系统信息", Rank=9, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-jiaosequnti", MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true, RoutePath="/sysinfo"},
                    new SysMenus(){ MenuId ="USERMGR", ParentId="SYSINFO", MenuName="用户管理", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="users",ComponentPath="sysinfo/users/index"},
                    new SysMenus(){ MenuId ="USERREAD", ParentId="USERMGR", MenuName="查看用户", Rank=1, CtrlName="User",ActionName="GetUsers" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="USERADD", ParentId="USERMGR", MenuName="添加用户", Rank=2, CtrlName="User",ActionName="AddUser" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="USERUPDATE", ParentId="USERMGR", MenuName="编辑用户", Rank=3, CtrlName="User",ActionName="UpdateUser" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="USERDEL", ParentId="USERMGR", MenuName="删除用户", Rank=4, CtrlName="User",ActionName="DelUser" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="USEREXPORT", ParentId="USERMGR", MenuName="导出用户", Rank=5, CtrlName="User",ActionName="ExportUsers" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="USERINITPASSWORD", ParentId="USERMGR", MenuName="初始化用户密码", Rank=6, CtrlName="User",ActionName="InitUserPassword" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="USERUPDATESTATUS", ParentId="USERMGR", MenuName="修改用户状态", Rank=7, CtrlName="User",ActionName="UpdateUserStatus" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="USERUPDATEROLE", ParentId="USERMGR", MenuName="修改用户角色", Rank=8, CtrlName="User",ActionName="UpdateUserRole" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="ORGANIZATIONMGR", ParentId="SYSINFO", MenuName="组织架构管理", Rank=2, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="deptroles",ComponentPath="sysinfo/deptroles/index"},
                    new SysMenus(){ MenuId ="ORGANIZATIONREAD", ParentId="ORGANIZATIONMGR", MenuName="查看组织架构", Rank=1, CtrlName="Organization",ActionName="GetOrganizationData" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="ROLEADD", ParentId="ORGANIZATIONMGR", MenuName="添加角色", Rank=2, CtrlName="Organization",ActionName="AddRole" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="ROLEUPDATE", ParentId="ORGANIZATIONMGR", MenuName="编辑角色", Rank=3, CtrlName="Organization",ActionName="UpdateRole" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="ROLEDEL", ParentId="ORGANIZATIONMGR", MenuName="删除角色", Rank=4, CtrlName="Organization",ActionName="DelRole" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="DEPTADD", ParentId="ORGANIZATIONMGR", MenuName="添加部门", Rank=5, CtrlName="Organization",ActionName="AddDept" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="DEPTUPDATE", ParentId="ORGANIZATIONMGR", MenuName="编辑部门", Rank=6, CtrlName="Organization",ActionName="UpdateDept" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="DEPTDEL", ParentId="ORGANIZATIONMGR", MenuName="删除部门", Rank=7, CtrlName="Organization",ActionName="DelDept" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="COMPANYMGR", ParentId="SYSINFO", MenuName="企业信息", Rank=4, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="company",ComponentPath="sysinfo/company/index"},
                    new SysMenus(){ MenuId ="COMPANYREAD", ParentId="COMPANYMGR", MenuName="查看企业信息", Rank=1, CtrlName="Organization",ActionName="GetCompanyInfo" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="COMPANYUPDATE", ParentId="COMPANYMGR", MenuName="编辑企业信息", Rank=2, CtrlName="Organization",ActionName="UpdateCompany" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="COMPANYANNOUNCEMGR", ParentId="SYSINFO", MenuName="企业公告", Rank=5, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="companyannounce",ComponentPath="sysinfo/companyannounce/index"},
                    new SysMenus(){ MenuId ="COMPANYANNOUNCEREAD", ParentId="COMPANYANNOUNCEMGR", MenuName="查看公告", Rank=1, CtrlName="MessageCompany",ActionName="GetMessages" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="COMPANYANNOUNCEADD", ParentId="COMPANYANNOUNCEMGR", MenuName="发布公告", Rank=2, CtrlName="MessageCompany",ActionName="AddMessage" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="COMPANYANNOUNCEUPDATE", ParentId="COMPANYANNOUNCEMGR", MenuName="修改公告", Rank=3, CtrlName="MessageCompany",ActionName="UpdateMessage" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="COMPANYANNOUNCEDEL", ParentId="COMPANYANNOUNCEMGR", MenuName="删除公告", Rank=4, CtrlName="MessageCompany",ActionName="DelMessage" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="BUSINESSDICMGR", ParentId="SYSINFO", MenuName="企业字典", Rank=6, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="businessdic",ComponentPath="sysinfo/businessdic/index"},
                    new SysMenus(){ MenuId ="BUSINESSDICREAD", ParentId="BUSINESSDICMGR", MenuName="查看字典", Rank=1, CtrlName="BusinessDic",ActionName="GetArgs" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="BUSINESSDICADD", ParentId="BUSINESSDICMGR", MenuName="添加字典", Rank=2, CtrlName="BusinessDic",ActionName="AddArgs" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="BUSINESSDICUPDATE", ParentId="BUSINESSDICMGR", MenuName="修改字典", Rank=3, CtrlName="BusinessDic",ActionName="UpdateArgs" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="BUSINESSDICDEL", ParentId="BUSINESSDICMGR", MenuName="删除字典", Rank=4, CtrlName="BusinessDic",ActionName="DelArgs" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                 //系统设置(菜单管理 系统权限设置 系统参数管理 数据导入管理 数据导出管理 操作日志 数据权限设置)
                 new SysMenus(){ MenuId ="SYSSETUP", ParentId="ROOT", MenuName="系统设置", Rank=10, CtrlName="",ActionName="" ,Icon="iconfont ionfont-md icon-shezhi",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="/syssetup"},

                    new SysMenus(){ MenuId ="MENUSMGR", ParentId="SYSSETUP", MenuName="菜单管理", Rank=1, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",RoutePath="menu",ComponentPath="syssetup/menu/index",IsVisible=false},
                    new SysMenus(){ MenuId ="MENUSREAD", ParentId="MENUSMGR", MenuName="查看菜单", Rank=1, CtrlName="Menu",ActionName="GetMenusData" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=false},
                    new SysMenus(){ MenuId ="MENUSADD", ParentId="MENUSMGR", MenuName="添加菜单", Rank=1, CtrlName="Menu",ActionName="AddMenu" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=false},
                    new SysMenus(){ MenuId ="MENUSUPDATE", ParentId="MENUSMGR", MenuName="编辑菜单", Rank=2, CtrlName="Menu",ActionName="UpdateMenu" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=false},
                    new SysMenus(){ MenuId ="MENUSDEL", ParentId="MENUSMGR", MenuName="删除菜单", Rank=3, CtrlName="Menu",ActionName="DelMenu" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=false},

                    new SysMenus(){ MenuId ="PROVIDERMGR", ParentId="SYSINFO", MenuName="第三方账号管理", Rank=1, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",RoutePath="externalProvider",ComponentPath="sysinfo/externalProvider/index",IsVisible=true},
                    new SysMenus(){ MenuId ="PROVIDERREAD", ParentId="PROVIDERMGR", MenuName="查看第三方账号", Rank=1, CtrlName="ExternalProvider",ActionName="GetProviders" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PROVIDERADD", ParentId="PROVIDERMGR", MenuName="添加第三方账号", Rank=1, CtrlName="ExternalProvider",ActionName="AddProvider" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PROVIDERUPDATE", ParentId="PROVIDERMGR", MenuName="编辑第三方账号", Rank=2, CtrlName="ExternalProvider",ActionName="UpdateProvider" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PROVIDERDEL", ParentId="PROVIDERMGR", MenuName="删除第三方账号", Rank=3, CtrlName="ExternalProvider",ActionName="DelProvider" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="PERMISSIONSETUP", ParentId="SYSSETUP", MenuName="系统权限设置", Rank=2, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="syspermission",ComponentPath="syssetup/syspermission/index"},
                    new SysMenus(){ MenuId ="PERMISSIONREAD", ParentId="PERMISSIONSETUP", MenuName="查看系统权限", Rank=1, CtrlName="UserPermission",ActionName="GetOrganizationData" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="PERMISSIONUPDATE", ParentId="PERMISSIONSETUP", MenuName="修改系统权限", Rank=2, CtrlName="UserPermission",ActionName="UpdatePermissions" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="SYSARGSSETUP", ParentId="SYSSETUP", MenuName="系统参数设置", Rank=3, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="args",ComponentPath="syssetup/args/index"},
                    new SysMenus(){ MenuId ="SYSARGSREAD", ParentId="SYSARGSSETUP", MenuName="查看系统参数", Rank=1, CtrlName="SysArgs",ActionName="GetArgs" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="SYSARGSUPDATE", ParentId="SYSARGSSETUP", MenuName="编辑系统参数", Rank=2, CtrlName="SysArgs",ActionName="UpdateArgs" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="IMPORTMGR", ParentId="SYSSETUP", MenuName="数据导入设置", Rank=4, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=false,MenuDisplay="PC",IsVisible=true,RoutePath="dataimport",ComponentPath="syssetup/dataimport/index"},
                    new SysMenus(){ MenuId ="IMPORTREAD", ParentId="IMPORTMGR", MenuName="查看数据导入设置", Rank=1, CtrlName="DataImport",ActionName="Index" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="IMPORTUPDATE", ParentId="IMPORTMGR", MenuName="编辑数据导入设置", Rank=2, CtrlName="DataImport",ActionName="Update" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="APPROVALSETUP", ParentId="SYSSETUP", MenuName="审批设置", Rank=5, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="approval",ComponentPath="syssetup/approval/index"},
                    new SysMenus(){ MenuId ="APPROVALREAD", ParentId="APPROVALSETUP", MenuName="查看审批设置", Rank=1, CtrlName="Approval",ActionName="GetApprovalSubject" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="APPROVALUPDATE", ParentId="APPROVALSETUP", MenuName="编辑审批设置", Rank=2, CtrlName="Approval",ActionName="UpdateProcess" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="FIELDSMGR", ParentId="SYSSETUP", MenuName="表字段管理", Rank=6, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=false,MenuDisplay="PC",IsVisible=true,RoutePath="fieldsmanage",ComponentPath="syssetup/fieldsmanage/index"},
                    new SysMenus(){ MenuId ="FIELDSREAD", ParentId="FIELDSMGR", MenuName="查看表字段", Rank=1, CtrlName="FieldsManage",ActionName="GetTableFieldList" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FIELDSUPDATE", ParentId="FIELDSMGR", MenuName="编辑表字段", Rank=2, CtrlName="FieldsManage",ActionName="SetSpareField" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="FIELDSPERMISSIONSETUP", ParentId="SYSSETUP", MenuName="字段权限管理", Rank=7, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="fieldspermission",ComponentPath="syssetup/fieldspermission/index"},
                    new SysMenus(){ MenuId ="FIELDSPERMISSIONREAD", ParentId="FIELDSPERMISSIONSETUP", MenuName="查看字段权限", Rank=1, CtrlName="FieldsPermission",ActionName="GetPermissionTbFields" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},
                    new SysMenus(){ MenuId ="FIELDSPERMISSIONUPDATE", ParentId="FIELDSPERMISSIONSETUP", MenuName="编辑字段权限", Rank=2, CtrlName="FieldsPermission",ActionName="SetPermissioFieldRole" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    new SysMenus(){ MenuId ="LOGINFO", ParentId="SYSSETUP", MenuName="操作日志", Rank=8, CtrlName="",ActionName="" ,Icon="",MenuType="Menu",IsValid=true,MenuDisplay="PC",IsVisible=true,RoutePath="loginfo",ComponentPath="syssetup/loginfo/index"},
                    new SysMenus(){ MenuId ="LOGREAD", ParentId="LOGINFO", MenuName="查看操作日志", Rank=1, CtrlName="LogInfo",ActionName="GetLogs" ,Icon="",MenuType="Action",IsValid=true,MenuDisplay="PC",IsVisible=true},

                    //移动端
                     
                    new SysMenus(){ MenuId ="PDAMOBILEMGR", ParentId="ROOT", MenuName="移动端操作", Rank=11, CtrlName="",ActionName="" ,Icon="", MenuType="Menu",IsValid=true,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true, RoutePath="mobilepda",ComponentPath="mobilepda/index"},
                    new SysMenus(){ MenuId ="TRUCKLOADING", ParentId="PDAMOBILEMGR", MenuName="扫码装车", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",Icon="zhuangche.png",IsValid=true,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout=MenuLayout.Primary.ToString(), RoutePath="truckLoading",ComponentPath="mobilepda/truckLoading/index"},
                    new SysMenus(){ MenuId ="TRUCKQUERY", ParentId="TRUCKLOADING", MenuName="扫码查询", Rank=1, CtrlName="PDACarLoad",ActionName="GetProductionOrderInfo" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},
                    new SysMenus(){ MenuId ="TRUCKSUBMIT", ParentId="TRUCKLOADING", MenuName="确认装车", Rank=3, CtrlName="PDACarLoad",ActionName="SubmitCarLoad" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="PUTAWAY", ParentId="PDAMOBILEMGR", MenuName="缓存仓上架", Rank=2, CtrlName="",ActionName="" ,MenuType="Menu",Icon="shangjia.png",IsValid=false,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout=MenuLayout.Primary.ToString(),RoutePath="putaway",ComponentPath="mobilepda/putaway/index"},
                    new SysMenus(){ MenuId ="PUTAWAYQUERY", ParentId="PUTAWAY", MenuName="扫码查询", Rank=1, CtrlName="PDAPutaway",ActionName="GetProductionOrderInfo" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},
                    new SysMenus(){ MenuId ="PUTAWAYSUBMIT", ParentId="PUTAWAY", MenuName="确认上架", Rank=2, CtrlName="PDAPutaway",ActionName="SubmitPutaway" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="PUTOUT", ParentId="PDAMOBILEMGR", MenuName="缓存仓下架", Rank=3, CtrlName="",ActionName="" ,MenuType="Menu",Icon = "xiajia.png", IsValid=false,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout=MenuLayout.Primary.ToString(),RoutePath="putout",ComponentPath="mobilepda/putout/index"},
                    new SysMenus(){ MenuId ="PUTOUTQUERY", ParentId="PUTOUT", MenuName="扫码查询", Rank=1, CtrlName="PDAPutout",ActionName="GetProductionOrderInfo" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},
                    new SysMenus(){ MenuId ="PUTOUTSUBMIT", ParentId="PUTOUT", MenuName="确认下架", Rank=2, CtrlName="PDAPutout",ActionName="SubmitPutout" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},

                    new SysMenus(){ MenuId ="UNSTACK", ParentId="PDAMOBILEMGR", MenuName="拆垛机上架", Rank=4, CtrlName="",ActionName="" ,MenuType="Menu",Icon = "chaiduo.png", IsValid=false,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout=MenuLayout.Primary.ToString(),RoutePath="unstack",ComponentPath="mobilepda/unstack/index"},
                    new SysMenus(){ MenuId ="UNSTACKQUERY", ParentId="UNSTACK", MenuName="扫码查询", Rank=1, CtrlName="PDAUnstack",ActionName="GetProductionOrderInfo" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},
                    new SysMenus(){ MenuId ="UNSTACKSUBMIT", ParentId="UNSTACK", MenuName="确认上架", Rank=2, CtrlName="PDAUnstack",ActionName="SubmitOnUnStack" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},

                    new SysMenus(){ MenuId ="QUERYORDER", ParentId="PDAMOBILEMGR", MenuName="交接单查询", Rank=5, CtrlName="",ActionName="" ,MenuType="Menu",Icon = "danjuchaxun.png", IsValid=false,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout = MenuLayout.Primary.ToString(), RoutePath="queryorder",ComponentPath="mobilepda/queryorder/index"},
                    new SysMenus(){ MenuId ="QUERYORDERQUERY", ParentId="QUERYORDER", MenuName="扫码查询", Rank=1, CtrlName="",ActionName="" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},
                    new SysMenus(){ MenuId ="QUERYORDERUNLOCK", ParentId="QUERYORDER", MenuName="货位解锁", Rank=2, CtrlName="PDAOrderQuery",ActionName="UnlockBin" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="SCANINSTORAGE", ParentId="PDAMOBILEMGR", MenuName="扫码入库", Rank=5, CtrlName="",ActionName="" ,MenuType="Menu",Icon = "rukudan.png", IsValid=true,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout = MenuLayout.Primary.ToString(), RoutePath="scan-instorage",ComponentPath="mobilepda/scan-instorage/index"},
                    new SysMenus(){ MenuId ="SCANINSTORAGESUBMITSCAN", ParentId="SCANINSTORAGE", MenuName="扫码登记", Rank=1, CtrlName="InStorageLabels",ActionName="SubmitScan" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},
                    new SysMenus(){ MenuId ="SCANINSTORAGESUBMITCODE", ParentId="SCANINSTORAGE", MenuName="提交入库", Rank=2, CtrlName="InStorageLabels",ActionName="SubmitCode" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="SCANINSTORAGESELECTGOODS", ParentId="PDAMOBILEMGR", MenuName="扫码入库-选择物品", Rank=1, CtrlName="",ActionName="" ,MenuType="Menu",Icon = "", IsValid=true,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true, RoutePath="select-instorage-goods",ComponentPath="mobilepda/scan-instorage/select-instorage-goods"},
                    new SysMenus(){ MenuId ="SCANINSTORAGEGOODSQUERY", ParentId="SCANINSTORAGE", MenuName="查看物品列表", Rank=1, CtrlName="InStorageLabels",ActionName="GetGoodsByKey" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},

                    new SysMenus(){ MenuId ="MESSAGE", ParentId="PDAMOBILEMGR", MenuName="消息", Rank=6, CtrlName="",ActionName="" ,MenuType="Menu",Icon = "", IsValid=true,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout = MenuLayout.Bottom.ToString(), RoutePath="pdamsg",ComponentPath="mobilepda/message"},
                    new SysMenus(){ MenuId ="MESSAGEREAD", ParentId="MESSAGE", MenuName="查看消息", Rank=1, CtrlName="PDAMessage",ActionName="GetMessages" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},

                    new SysMenus(){ MenuId ="MYSETTING", ParentId="PDAMOBILEMGR", MenuName="我的", Rank=7, CtrlName="",ActionName="" ,MenuType="Menu",Icon = "", IsValid=true,MenuDisplay="MOBILE",IsVisible=true,HideMenu=true,MenuLayout = MenuLayout.Bottom.ToString(), RoutePath="pdausersetting",ComponentPath="mobilepda/usersetting"},
                    new SysMenus(){ MenuId ="MYSETTINGREAD", ParentId="MYSETTING", MenuName="我的设置", Rank=1, CtrlName="PDAMessage",ActionName="" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},
                    new SysMenus(){ MenuId ="MESSAGESUBSCRIBE", ParentId="MYSETTING", MenuName="订阅消息", Rank=2, CtrlName="PDAMessage",ActionName="SubmitSubscribeMessageType" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE", IsVisible=true},

                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEMGR", ParentId="PDAMOBILEMGR", MenuName="库存盘点", Rank=8, CtrlName="",ActionName="" ,MenuType="Menu" ,Icon="kucundan.png",IsValid=true,IsVisible=true,HideMenu=true,MenuLayout = MenuLayout.Primary.ToString(),RoutePath="takestock-mobile",MenuDisplay = "MOBILE", ComponentPath="mobilepda/takestock/index-mobile"},
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEREAD", ParentId="TAKESTOCKMOBILEMGR", MenuName="查看盘点信息", Rank=1, CtrlName="TakeStock",ActionName="GetExpendTrend" ,MenuType="Action",IsValid=true,IsVisible=true,MenuDisplay = "MOBILE"},
                     
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEBYGOODS", ParentId="PDAMOBILEMGR", MenuName="库存盘点-按物品盘点", Rank=9, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,HideMenu=true,RoutePath="takestockbygoods",MenuDisplay = "MOBILE", ComponentPath="mobilepda/takestock/takegoods-mobile"},
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEBYGOODSREAD", ParentId="TAKESTOCKMOBILEBYGOODS", MenuName="查看物品列表", Rank=1, CtrlName="TakeStock",ActionName="GetGoodsByKey" ,MenuType="Action",IsValid=true,IsVisible=true,MenuDisplay = "MOBILE"},
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILELOCKGOODS", ParentId="TAKESTOCKMOBILEBYGOODS", MenuName="锁定盘点物品", Rank=5, CtrlName="TakeStock",ActionName="SetTakeStockLockByGoods" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEBYBIN", ParentId="PDAMOBILEMGR", MenuName="库存盘点-按货位盘点", Rank=10, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,HideMenu=true,RoutePath="takestockbybin",MenuDisplay = "MOBILE", ComponentPath="mobilepda/takestock/takebin-mobile"},
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEBYBINREAD", ParentId="TAKESTOCKMOBILEBYBIN", MenuName="查看货位列表", Rank=1, CtrlName="TakeStock",ActionName="GetBins" ,MenuType="Action",IsValid=true,IsVisible=true,MenuDisplay = "MOBILE"},
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILELOCKBIN", ParentId="TAKESTOCKMOBILEBYBIN", MenuName="锁定盘点货位", Rank=6, CtrlName="TakeStock",ActionName="SetTakeStockLockByBin" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEINVENTORYBYGOODS", ParentId="PDAMOBILEMGR", MenuName="库存盘点-按物品清点", Rank=11, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,HideMenu=true,RoutePath="takegoodssubmit",MenuDisplay = "MOBILE", ComponentPath="mobilepda/takestock/takegoodssubmit-mobile"},
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEINVENTORYBYGOODSREAD", ParentId="TAKESTOCKMOBILEINVENTORYBYGOODS", MenuName="查看仓储明细", Rank=1, CtrlName="TakeStock",ActionName="GetGoodsInventoryDetail" ,MenuType="Action",IsValid=true,IsVisible=true,MenuDisplay = "MOBILE"},
                    new SysMenus(){ MenuId ="TAKESTOCKBYGOODSMOBILEADD", ParentId="TAKESTOCKMOBILEINVENTORYBYGOODS", MenuName="盘点提交", Rank=7, CtrlName="TakeStock",ActionName="AddTaskStockOrderByGoods" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEINVENTORYBYBIN", ParentId="PDAMOBILEMGR", MenuName="库存盘点-按货位清点", Rank=12, CtrlName="",ActionName="" ,MenuType="Menu",IsValid=true,IsVisible=true,HideMenu=true,RoutePath="takebinsubmit",MenuDisplay = "MOBILE", ComponentPath="mobilepda/takestock/takebinsubmit-mobile"},
                    new SysMenus(){ MenuId ="TAKESTOCKMOBILEINVENTORYBYBINREAD", ParentId="TAKESTOCKMOBILEINVENTORYBYBIN", MenuName="查看仓储明细", Rank=1, CtrlName="TakeStock",ActionName="GetBinInventoryDetail" ,MenuType="Action",IsValid=true,IsVisible=true,MenuDisplay = "MOBILE"}, 
                    new SysMenus(){ MenuId ="TAKESTOCKBYBINMOBILEADD", ParentId="TAKESTOCKMOBILEINVENTORYBYBIN", MenuName="盘点提交", Rank=8, CtrlName="TakeStock",ActionName="AddTaskStockOrderByBin" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

                    new SysMenus(){ MenuId ="GOODSMOBILEMGR", ParentId="PDAMOBILEMGR", MenuName="货品信息", Rank=13, CtrlName="",ActionName="" ,MenuType="Menu" ,Icon="beijianguanli.png",IsValid=true,IsVisible=true,HideMenu=true,MenuLayout = MenuLayout.Primary.ToString(),RoutePath="goodslist-mobile",MenuDisplay = "MOBILE", ComponentPath="mobilepda/goods/index-mobile"},
                    new SysMenus(){ MenuId ="GOODSMOBILEREAD", ParentId="GOODSMOBILEMGR", MenuName="查看货品列表", Rank=1, CtrlName="Goods",ActionName="GetGoodsList" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},
                     
                    new SysMenus(){ MenuId ="GOODSMOBILEDETAIL", ParentId="PDAMOBILEMGR", MenuName="货品信息-货品明细", Rank=14, CtrlName="",ActionName="" ,MenuType="Menu" ,IsValid=true,IsVisible=true,HideMenu=true,RoutePath="goodsdetail-mobile",MenuDisplay = "MOBILE", ComponentPath="mobilepda/goods/detail-mobile"},
                    new SysMenus(){ MenuId ="GOODSMOBILEDETAILREAD", ParentId="GOODSMOBILEDETAIL", MenuName="查看货品明细", Rank=1, CtrlName="Goods",ActionName="GetGoodsDetail" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},
                    new SysMenus(){ MenuId ="GOODSMOBILEDETAILUPDATE", ParentId="GOODSMOBILEDETAIL", MenuName="修改货品明细", Rank=1, CtrlName="Goods",ActionName="UpdateGoodsPhoto" ,MenuType="Action",IsValid=true,MenuDisplay="MOBILE",IsVisible=true},

            };
            db.Insertable(menus).AddQueue();
        }
    }
}
