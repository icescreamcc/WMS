using Logic.PlanMaterial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Plan;
using Models.Model;
using System.Threading.Tasks;
using Models.Model.Sys;
using System.Collections.Generic;
using WebApi.Filter;
using Models.Model.Enum;
using External.Common;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model.Baseinfo;

namespace WebApi.Controllers.Plan
{ 
    public class MaterialRequirementPlanController : AuthTokenController
    {
        private readonly MaterialRequirementPlanMgr _materialRequirementPlanMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "物料需求计划";

        public MaterialRequirementPlanController(MaterialRequirementPlanMgr materialRequirementPlanMgr, SelectOptionsService selectOptionsService)
        {
            _materialRequirementPlanMgr = materialRequirementPlanMgr;
            _selectOptionsServer = selectOptionsService;
        }

        [HttpGet]
        [BusinessLog("查看物料需求计划", LogType.Read, _moduleName)]
        public async Task<TableModel<PlanMaterialRequirementOrderDto>> GetPlanOrders(int pgSize, int pgIndex, string orderField, string orderType, string searchKey, int year, int week, string goodsGroup,string status)
        {
            return await _materialRequirementPlanMgr.GetPlanOrders(pgSize, pgIndex, orderField, ConvertOrderType(orderType,false), searchKey, year, week, goodsGroup, status);
        }

        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var unitData = await _selectOptionsServer.GetUnits();
            var requirementLevelData = EnumHelper.GetEnumValNames<MaterialRequirementLevel>(); 
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            var shiftData = EnumHelper.GetEnumValNames<ProdShift>();
            var statusData= EnumHelper.GetEnumValNames<PlanMaterialRequirementStatus>();
            return new
            {
                unitData,
                requirementLevelData,
                goodsClassifyData,
                shiftData,
                statusData
            };
        }

        [HttpGet]
        [Skip]
        public async Task<PlanMaterialRequirementOrderDto> GetPlanOrderDetails(string orderNo)
        {
            return await _materialRequirementPlanMgr.GetPlanOrderDetails(orderNo);
        }

        [HttpGet]
        [Skip]
        public async Task<List<MailDto>> GetOrderMailInfo(string orderNo)
        {
            return await _materialRequirementPlanMgr.GetOrderMailInfo(orderNo);
        }

        [HttpGet]
        [Skip]
        public async Task<List<PlanFinishedProductOrderDto>> GetProductionInfoByReqPlanOrder(int year, int week, float version)
        {
            return await _materialRequirementPlanMgr.GetProductionInfoByReqPlanOrder(year, week, version);
        }

        [HttpPost]
        [BusinessLog("创建物料需求计划", LogType.Add, _moduleName)]
        public async Task AddPlanOrder(PlanMaterialRequirementOrderDto data)
        {
             await _materialRequirementPlanMgr.AddPlanOrder(data);
        }

        [HttpPost]
        [BusinessLog("修改物料需求计划", LogType.Update, _moduleName)]
        public async Task UpdatePlanOrder(PlanMaterialRequirementOrderDto data)
        {
            await _materialRequirementPlanMgr.UpdatePlanOrder(data);
        }

        [HttpGet]
        [BusinessLog("删除物料需求计划", LogType.Del, _moduleName)]
        public async Task DelPlanOrder(string orderNo)
        {
            await _materialRequirementPlanMgr.DelPlanOrder(orderNo);
        }

        [HttpGet]
        [Skip]
        public  List<FieldModel> GetExportFields()
        {
            return  _materialRequirementPlanMgr.GetExportFields();
        }

        [HttpPut]
        [BusinessLog("导出物料需求计划", LogType.Export, _moduleName)]
        public async Task<string> ExportPlanOrder(string orderField, string orderType, string searchKey, int year, int week, string goodsGroup,string status, List<KeyValueModel> fields)
        {
            return await _materialRequirementPlanMgr.ExportPlanOrder(orderField, ConvertOrderType(orderType,false), searchKey, year, week, goodsGroup, status, fields);
        }

        [HttpGet]
        [Skip]
        public async Task<FileInfoDto> CreateAttachment(string orderNo)
        {
            return await _materialRequirementPlanMgr.CreateAttachment(orderNo);
        }

        [HttpPost]
        [BusinessLog("发送物料需求计划邮件", LogType.Add, _moduleName)]
        public async Task SendMailToSupplier(MailDto data)
        {
            await _materialRequirementPlanMgr.SendMailToSupplier(data);
        }
    }
}
