using Logic.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Inv;
using Models.Model;
using System.Threading.Tasks;
using Models.Model.Enum;
using WebApi.Filter;
using System.Collections.Generic;
using Logic.LogicCommon;
using Logic.LogicBase;

namespace WebApi.Controllers.Inv
{ 
    public class WorkbinController : AuthTokenController
    {
        private readonly WorkbinMgr _workbinMgr;

        private readonly SysArgsService _sysArgsService;

        private const string _moduleName = "料箱管理";

        public WorkbinController(WorkbinMgr workbinMgr, SysArgsService sysArgsService)
        {
            _workbinMgr = workbinMgr;
            _sysArgsService = sysArgsService;
        }

        [HttpGet]
        [BusinessLog("查看料箱信息", LogType.Read, _moduleName)]
        public async Task<TableModel<WorkbinDto>> GetWorkbins(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _workbinMgr.GetWorkbins(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), searchKey);
        }

        [HttpGet]
        [Skip]
        public async Task<List<WorkbinCellDto>> GetWorkbinCells(int workbinId)
        {
            return await _workbinMgr.GetWorkbinCells(workbinId);
        }

        [HttpGet]
        [Skip]
        public async Task<List<WorkbinCellDto>> GetWorkbinCellsByCellNo(string cellNo)
        {
            return await _workbinMgr.GetWorkbinCells(cellNo);
        }

        [HttpGet]
        [Skip]
        public async Task<List<WorkbinSpecificationDto>> GetWorkbinSpec()
        {
            return await _workbinMgr.GetWorkbinSpec();
        }

        [HttpGet]
        [Skip]
        public async Task<object> GetArgs()
        {
            var isSameBinNo = await _sysArgsService.GetValueByKey(BusinessConst.IsWorkbinNoSameBinNo);
            return new { IsSameBinNo = bool.Parse(isSameBinNo.Value.ToString()) };
        }

        [HttpPost]
        [BusinessLog("修改料箱信息", LogType.Update, _moduleName)]
        public async Task UpdateWorkbin(WorkbinDto data)
        {
            await _workbinMgr.UpdateWorkbin(data);
        }

        [HttpPost]
        [BusinessLog("添加料箱规格", LogType.Add, _moduleName)]
        public async Task AddWorkbinSpeci(WorkbinSpecificationDto data)
        {
            await _workbinMgr.AddWorkbinSpeci(data);
        }

        [HttpPost]
        [BusinessLog("修改料箱规格", LogType.Update, _moduleName)]
        public async Task UpdateWorkbinSpeci(WorkbinSpecificationDto data)
        {
            await _workbinMgr.UpdateWorkbinSpeci(data);
        }

        [HttpGet]
        [BusinessLog("删除料箱规格", LogType.Del, _moduleName)]
        public async Task DelWorkbinSpeci(int specId)
        {
            await _workbinMgr.DelWorkbinSpeci(specId);
        }
    }
}
