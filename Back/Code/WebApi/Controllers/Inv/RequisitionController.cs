using Logic.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Inv;
using Models.Model.Sys;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using WebApi.Filter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Models.Model.Enum;
using NPOI.POIFS.Crypt.Dsig;
using Logic.LogicCommon;
using Models.Model.Baseinfo;
using Models.Model;
using Models.Model.AutomationDevice;
using External.Common;
using Logic.LogicBase;

namespace WebApi.Controllers.Inv
{ 
    public class RequisitionController : AuthTokenController
    {
        private readonly RequisitionMgr _requisitionMgr;

        private readonly SysArgsService  _sysArgsService;

        private const string _moduleName = "物品领用";

        public RequisitionController(RequisitionMgr requisitionMgr, SysArgsService  sysArgsService)
        {
            _requisitionMgr = requisitionMgr; 
            _sysArgsService = sysArgsService;
        }

        [HttpGet]
        [Skip]
        public async Task<UserAuthorizationDto> SwipingCardAuth(string cardNo)
        {
            return await _requisitionMgr.SwipingCardAuth(cardNo);
        }

        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetRequisitionTypes(string goodsClassifyGroup)
        {
            if (goodsClassifyGroup == BaseTypeGroup.SparePart.ToString())
                return await _sysArgsService.GetArgsOptions(BusinessConst.SparePartRequisitionType);
            else
                return await _sysArgsService.GetArgsOptions(BusinessConst.GeneralRequisitionType);
        }

        [HttpGet]
        public async Task<List<RequisitionGoodsDetail>> GetOrderList(string userId, string goodsClassifyGroup, DateTime date)
        {
            return await _requisitionMgr.GetOrderList(userId, goodsClassifyGroup, date);
        }

        [HttpGet]
        [Skip]
        public async Task<TableModel<AutoProdTaskTrackingDto>> GetAGVList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _requisitionMgr.GetAGVList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey);
        }

        [HttpPost]
        [Skip]
        [BusinessLog("关闭执行中的AVG任务", LogType.Update, _moduleName)]
        public async Task UpdateAGVisExecuting(AutoProdTaskTrackingDto data)
        {
            await _requisitionMgr.UpdateAGVisExecuting(data);
        }

        [HttpPost]
        [BusinessLog("添加领用单", LogType.Add, _moduleName)]
        public async Task<string> AddReqisitionOrder(RequisitionOrderDto data)
        {
            return await _requisitionMgr.AddReqisitionOrder(data);
        }

        [HttpPost]
        [BusinessLog("修改领用单", LogType.Update, _moduleName)]
        public async Task UpdateReqisitionOrder(RequisitionOrderDto data)
        {
            await _requisitionMgr.UpdateReqisitionOrder(data);
        }

        [HttpGet]
        [BusinessLog("删除领用单", LogType.Del, _moduleName)]
        public async Task DelReqisitionOrder(string orderNo, int detailId, string userCardNo)
        {
            await _requisitionMgr.DelReqisitionOrder(orderNo, detailId, userCardNo);
        }

        [HttpPost]
        [BusinessLog("确认收货", LogType.Other, _moduleName)]
        public async Task ConfirmReceived(RequisitionOrderDto data)
        {
            await _requisitionMgr.ConfirmReceived(data);
        }
    }
}
