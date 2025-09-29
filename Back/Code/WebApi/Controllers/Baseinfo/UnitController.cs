using External.Common;
using Logic.BaseInfo;
using Logic.LogicCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter;
using WebApi.Response;

namespace WebApi.Controllers.Baseinfo
{ 
    public class UnitController : AuthTokenController
    {

        private readonly UnitMgr _unitMgr; 

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "单位信息管理";

        public UnitController(UnitMgr unitMgr,  SelectOptionsService selectOptionsServer)
        {
            _unitMgr = unitMgr; 
            _selectOptionsServer = selectOptionsServer;
        }

        /// <summary>
        /// 获取所有单位列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看单位信息", Models.Model.Enum.LogType.Read, _moduleName)]
        public async Task<TableModel<UnitDetail>> GetUnits(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _unitMgr.GetUnits(pgSize,  pgIndex,  orderFiled,  ConvertOrderType(orderType),  searchKey); 
        }

        /// <summary>
        /// 获取单位相关选项数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public object GetAboutUnitOptions()
        { 
            var unitTypes = EnumHelper.GetEnumValNames<UnitType>();
            return new {UnitTypeOptions = unitTypes }; 
        }

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加单位", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task AddUnit(UnitDetail data)
        {
            await _unitMgr.AddUnit(data); 
        }

        /// <summary>
        /// 修改单位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改单位", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateUnit(UnitDetail data)
        {
            await _unitMgr.UpdateUnit(data); 
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="unitsId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除单位", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelUnit(int[] unitsId)
        {
            await _unitMgr.DelUnit(unitsId); 
        }
    }
}
