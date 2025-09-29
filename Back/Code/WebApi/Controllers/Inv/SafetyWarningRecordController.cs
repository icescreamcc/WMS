using Logic.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Inv;
using Models.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApi.Filter;
using Models.Model.Enum;
using External.Common;
using Logic.LogicCommon;
using Models.Model.Sys;
using Logic.LogicBase;

namespace WebApi.Controllers.Inv
{ 
    public class SafetyWarningRecordController : AuthTokenController
    {
        private const string _moduleName = "安全库存预警信息";

        private readonly SafetyWarningRecordMgr _safetyWarningRecordMgr;

        private readonly SelectOptionsService _selectOptionsService;

        private readonly SysArgsService _sysArgsHelper;

        public SafetyWarningRecordController(SafetyWarningRecordMgr safetyWarningRecordMgr, SelectOptionsService selectOptionsService, SysArgsService sysArgsService)
        {
            _safetyWarningRecordMgr = safetyWarningRecordMgr;
            _selectOptionsService = selectOptionsService;
            _sysArgsHelper = sysArgsService; 
        }

        [HttpGet, BusinessLog("查看安全库存预警信息", LogType.Read, _moduleName)] 
        public async Task<TableModel<InvSafetyWarningRecordDto>> GetWarningInfo(string userId, int pgSize, int pgIndex, string orderField, string orderType, string searchKey,  string goodsGroup, int goodsClassifyType, string status)
        {
            return await _safetyWarningRecordMgr.GetWarningInfo(userId,pgSize, pgIndex, orderField, ConvertOrderType(orderType), searchKey, goodsGroup, goodsClassifyType, status);
        }

        [HttpGet, Skip] 
        public async Task<object> GetArgs()
        {
            var hasApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsSafetyInventoryApproval)).Value.ToString());
            return new { HasApproval = hasApproval };
        }

        [HttpGet, Skip] 
        public object GetOptions()
        { 
            var statusData = EnumHelper.GetEnumValNames<SafetyWarningRecordStatus>();
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();  
            return new
            {  
                goodsClassifyData, 
                statusData
            };
        }

        [HttpGet, Skip] 
        public async Task<InvSafetyWarningRecordDto> GetSafetyInfoDetail(int detailId)
        {
            return await _safetyWarningRecordMgr.GetSafetyInfoDetail(detailId);
        }

        [HttpGet, Skip]
        public async Task<List<ApprovalHisModel>> GetWarningApprovalHis(int detailId)
        {
            return await _safetyWarningRecordMgr.GetWarningApprovalHis(detailId);
        }

        [HttpPost, BusinessLog("修改安全库存预警信息", LogType.Update, _moduleName)]
        public async Task UpdateWarningInfo(InvSafetyWarningRecordDto data)
        {
            await _safetyWarningRecordMgr.UpdateWarningInfo(data);
        }

        [HttpPut, Skip, BusinessLog("修改安全库存状态", LogType.Update, _moduleName)]
        public async Task UpdateReceived(int[] detailId, string userId, string userName)
        {
            await _safetyWarningRecordMgr.UpdateReceived(detailId, userId, userName); 
        }

        [HttpPut, BusinessLog("审批安全库存预警", LogType.Update, _moduleName)]
        public async Task ApprovalSafetyInventory(string[] flowId, bool isApprove, string opinion, string goodsClassifyGroup, string userId, string userName)
        {
            await _safetyWarningRecordMgr.ApprovalSafetyInventory(flowId, isApprove, opinion, goodsClassifyGroup, userId, userName);
        }

        [HttpGet, Skip] 
        public List<KeyValueModel> GetExportFields()
        {
            return _safetyWarningRecordMgr.GetExportFields();
        }

        [HttpPut, BusinessLog("导出安全库存预警信息", LogType.Export, _moduleName)]
        public async Task<string> ExportWarningInfo(string orderField, string orderType, string searchKey, string goodsGroup, int goodsClassifyType, string status, List<KeyValueModel> fields)
        {
            return await _safetyWarningRecordMgr.ExportWarningInfo(orderField, ConvertOrderType(orderType), searchKey, goodsGroup, goodsClassifyType, status, fields);
        }
    }
}
