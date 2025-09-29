using DbRepository.Repository.DbModels;
using Logic.BaseInfo;
using Logic.LogicBase;
using Logic.LogicCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Sys;
using Models.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using WebApi.Filter;
using System.Linq;
using static Dm.parser.LVal;
using Models.Model.Inv;

namespace WebApi.Controllers.Baseinfo
{ 
    public class ConsumablesController : AuthTokenController
    {
        private readonly ConsumablesMgr  _consumablesMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "耗材信息管理";

        public ConsumablesController(ConsumablesMgr consumablesMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _consumablesMgr = consumablesMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        #region 耗材类型
        /// <summary>
        /// 获取所有耗材类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip] 
        public async Task<List<TreeModel>> GetConsumablesClassifyData()
        {
            return await _consumablesMgr.GetConsumablesClassifyData();
        }

        /// <summary>
        /// 添加耗材类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加耗材类型", LogType.Add, _moduleName)]
        public async Task AddConsumablesType(BaseTypeDto data)
        {
            await _consumablesMgr.AddConsumablesType(data);
        }

        /// <summary>
        /// 修改耗材类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改耗材类型", LogType.Update, _moduleName)]
        public async Task UpdateConsumablesType(BaseTypeDto data)
        {
            await _consumablesMgr.UpdateConsumablesType(data);
        }

        /// <summary>
        /// 删除耗材类型
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除耗材类型", LogType.Del, _moduleName)]
        public async Task DelConsumablesType(int typeId)
        {
            await _consumablesMgr.DelConsumablesType(typeId);
        }
        #endregion

        #region 耗材
        /// <summary>
        /// 获取所有耗材列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看耗材信息", LogType.Read, _moduleName)]
        public async Task<TableModel<ConsumablesDto>> GetConsumablesList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, int goodsClassifyId, string searchKey)
        {
            return await _consumablesMgr.GetConsumablesList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), goodsClassifyId, searchKey);
        }
         
        /// <summary>
        /// 获取耗材表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _consumablesMgr.GetEnableSpareFields(typeof(BaseGoods).Name);
        }

        /// <summary>
        /// 获取耗材明细
        /// </summary>
        /// <param name="spareId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<ConsumablesDto> GetConsumablesDetail(string spareId)
        {
            return await _consumablesMgr.GetConsumablesDetail(spareId);
        }

        /// <summary>
        /// 获取耗材相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units = await _selectOptionsServer.GetUnits(); 
            var spareTypes = await _selectOptionsServer.GetDictionaryOption(BusinessConst.ConsumablesType);
            var areas = await _selectOptionsServer.GetAreas(BusinessConst.PlantNo);
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            var workbinSpec = await _selectOptionsServer.GetWorkbinSpec();
            return new { ConsumablesTypeOptions = spareTypes, UnitsOptions = units,AreasOptions= areas, PhotoLimit = photoLimit.Value,WorkbinSpecOptions= workbinSpec };
        }

        /// <summary>
        /// 添加耗材
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加耗材", LogType.Add, _moduleName)]
        public async Task AddConsumables(ConsumablesDto data)
        {
            await _consumablesMgr.AddConsumables(data);
        }

        /// <summary>
        /// 修改耗材
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改耗材", LogType.Update, _moduleName)]
        public async Task UpdateConsumables(ConsumablesDto data)
        {
            await _consumablesMgr.UpdateConsumables(data);
        }
         
        /// <summary>
        /// 删除耗材
        /// </summary>
        /// <param name="ConsumablesId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除耗材", LogType.Del, _moduleName)]
        public async Task DelConsumables(string[] ConsumablesId)
        {
            await _consumablesMgr.DelConsumables(ConsumablesId);
        }
         
        /// <summary>
        /// 导出耗材
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出耗材", LogType.Export, _moduleName)]
        public async Task<string> ExportConsumables(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _consumablesMgr.ExportConsumables(searchKey, orderField, ConvertOrderType(orderType), goodsClassifyId, fields); 
        }

        /// <summary>
        /// 获取当前用户导出用户列表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public List<KeyValueModel> GetAllowField()
        {
            return  _consumablesMgr.GetExportField(); 
        }
        #endregion
    }
}
