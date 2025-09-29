using External.Common;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Enum;
using Models.Model.Purchase;
using Models.Model.Sys;
using Models.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter;
using System;

namespace WebApi.Controllers.Purchase
{ 
    public class PurchaseOrderController : AuthTokenController
    {
        private readonly PurchaseOrderMgr  _purchaseOrderMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "采购订单";

        public PurchaseOrderController(PurchaseOrderMgr purchaseOrderMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _purchaseOrderMgr = purchaseOrderMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        /// <summary>
        /// 采购订单分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看采购订单", LogType.Read, _moduleName)]
        public async Task<TableModel<PurchaseOrderDto>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, int purchaseTypeId, string status, string goodsClassifyGroup, int goodsClassifyId)
        {
            return await _purchaseOrderMgr.GetOrders(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey, dateStart, dateEnd,purchaseTypeId,status,goodsClassifyGroup, goodsClassifyId);
        }

        [HttpGet]
        [Skip]
        public async Task<PurchaseOrderDto> GetOrderDetail(int detailId, string userId)
        {
            return await _purchaseOrderMgr.GetOrderDetail(detailId, userId);
        }

        /// <summary>
        /// 获取采购订单相关参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetArgs()
        {
            var hasApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsPurchaseOrderApproval)).Value.ToString()); 
            return new { HasApproval = hasApproval };
        }


        /// <summary>
        /// 获取采购订单相关选项及参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var unitData = await _selectOptionsServer.GetUnits(); 
            var orderStatusData = EnumHelper.GetEnumValNames<PurchaseOrderStatus>();
            var orderFlowStatusData = EnumHelper.GetEnumValNames<PurchaseOrderFlowStatus>();
            var orderReceiveStatus= EnumHelper.GetEnumValNames<PurchaseReceiveStatus>();
            var purchaseTypeData = await _selectOptionsServer.GetDictionaryOption(BusinessConst.PurchaseType);
            var purchaseCPMGData = await _selectOptionsServer.GetDictionaryOption(BusinessConst.Purchase_CPMG);
            var purchaseAccountData = await _selectOptionsServer.GetDictionaryOption(BusinessConst.Purchase_Account);
            var purchaseClassesData = await _selectOptionsServer.GetDictionaryOption(BusinessConst.Purchase_Classes);
            var pmTypeData = await _selectOptionsServer.GetDictionaryOption(BusinessConst.SparePartRequisitionType);
            var goodsClassifyGroupData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            var lineData=await _selectOptionsServer.GetDictionaryOption(BusinessConst.Line);
            var workbinSpecData = await _selectOptionsServer.GetWorkbinSpec();
            var npmBuyerData = await _purchaseOrderMgr.GetNPMBuyer();
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            return new
            {
                UnitOptions = unitData,
                OrderStatusOptions= orderStatusData,
                OrderFlowStatusOptions= orderFlowStatusData,
                OrderReceiveStatusOptions= orderReceiveStatus,
                PurchaseTypeOptions = purchaseTypeData?.ArgsOptions,
                PurchaseCPMGOptions=purchaseCPMGData?.ArgsOptions,
                PurchaseAccountOptions = purchaseAccountData?.ArgsOptions,
                PurchaseClassesOptions = purchaseClassesData?.ArgsOptions,
                PMTypeOptions= pmTypeData?.ArgsOptions,
                GoodsClassifyGroupOptions = goodsClassifyGroupData,
                LineOptions= lineData.ArgsOptions,
                WorkbinSpecOptions= workbinSpecData,
                NpmBuyerOptions= npmBuyerData,
                PhotoLimit = photoLimit.Value
            };
        }

        /// <summary>
        /// 查询审批明细
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<ApprovalHisModel>> GetApprovalHis(string orderNo)
        {
            return await _purchaseOrderMgr.GetApprovalHis(orderNo);
        }

        [HttpGet]
        [Skip]
        public async Task<bool> IsLastApproval(string userId)
        {
            return await _purchaseOrderMgr.IsLastApproval(userId);
        }

        /// <summary>
        /// 添加采购订单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加采购订单", LogType.Add, _moduleName)]
        public async Task AddPurchaseOrder(PurchaseOrderDto data)
        {
            await _purchaseOrderMgr.AddPurchaseOrder(data);
        }

        /// <summary>
        /// 修改仓库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改采购订单", LogType.Update, _moduleName)]
        public async Task UpdatePurchaseOrder(PurchaseOrderDto data)
        {
            await _purchaseOrderMgr.UpdatePurchaseOrder(data);
        }

        [HttpPost,Skip]
        [BusinessLog("审批保存", LogType.Update, _moduleName)]
        public async Task UpdateApprovalPurchaseOrder(PurchaseOrderDto data)
        {
            await _purchaseOrderMgr.UpdateApprovalPurchaseOrder(data);
        }

        /// <summary>
        /// 删除采购订单
        /// </summary>
        /// <param name="detailsId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除采购订单", LogType.Del, _moduleName)]
        public async Task DelPurchaseOrder(int[] detailsId)
        {
            await _purchaseOrderMgr.DelPurchaseOrder(detailsId);
        }

        /// <summary>
        /// 发送邮件通知收货
        /// </summary>
        /// <param name="data"></param>
        /// <param name="orderNo"></param>
        [HttpPut]
        [Skip]
        public void AdviceReceiving(string orderNo, MailModel data)
        {
            _purchaseOrderMgr.AdviceReceiving(data, orderNo);
        }

        /// <summary>
        /// 仓库确认收货
        /// </summary>
        /// <param name="detailsId"></param>        
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("仓库确认收货", LogType.Other, _moduleName)]
        public async Task SubmitReceivedWH(List<PurchaseReceiveDto> data)
        {
            await _purchaseOrderMgr.SubmitReceivedWH(data);
        }

        /// <summary>
        /// WK确认收货
        /// </summary>
        /// <param name="detailsId"></param>        
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("WK确认收货", LogType.Other, _moduleName)]
        public async Task SubmitReceivedWK(List<PurchaseReceiveDto> data)
        {
            await _purchaseOrderMgr.SubmitReceivedWK(data);
        }

        [HttpGet]
        [BusinessLog("修改流程状态", LogType.Other, _moduleName)]
        public async Task UpdateFlowStatus(int detailsId, string status)
        {
            await _purchaseOrderMgr.UpdateFlowStatus(detailsId, status);
        }

        /// <summary>
        /// 审批采购订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isApprove"></param>
        /// <param name="opinion"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("审批收货计划", LogType.Update, _moduleName)]
        public async Task ApprovalPurchaseOrder(PurchaseOrderDto data)
        {
            await _purchaseOrderMgr.ApprovalPurchaseOrder(data);
        }

        [HttpGet, Skip]
        public List<FieldModel> GetExportFields()
        {
            return _purchaseOrderMgr.GetExportFields();
        }
          
        [HttpPut, BusinessLog("导出收货计划", LogType.Export, _moduleName)]
        public async Task<string> ExportPurchaseData(string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, int purchaseTypeId, string flowStatus, string goodsClassifyGroup, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _purchaseOrderMgr.ExportPurchaseData(orderFiled, ConvertOrderType(orderType, false), searchKey, dateStart, dateEnd, purchaseTypeId, flowStatus, goodsClassifyGroup, goodsClassifyId, fields);
        }
    }
}
