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
    public class SparePartController : AuthTokenController
    {
        private readonly SparePartMgr  _sparePartMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "备件信息管理";

        public SparePartController(SparePartMgr sparePartMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _sparePartMgr = sparePartMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        #region 备件类型
        /// <summary>
        /// 获取所有备件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip] 
        public async Task<List<TreeModel>> GetSparePartClassifyData()
        {
            return await _sparePartMgr.GetSparePartClassifyData();
        }

        /// <summary>
        /// 添加备件类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加备件类型", LogType.Add, _moduleName)]
        public async Task AddSparePartType(BaseTypeDto data)
        {
            await _sparePartMgr.AddSparePartType(data);
        }

        /// <summary>
        /// 修改备件类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改备件类型", LogType.Update, _moduleName)]
        public async Task UpdateSparePartType(BaseTypeDto data)
        {
            await _sparePartMgr.UpdateSparePartType(data);
        }

        /// <summary>
        /// 删除备件类型
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除备件类型", LogType.Del, _moduleName)]
        public async Task DelSparePartType(int typeId)
        {
            await _sparePartMgr.DelSparePartType(typeId);
        }
        #endregion

        #region 备件
        /// <summary>
        /// 获取所有备件列表
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
        [BusinessLog("查看备件信息", LogType.Read, _moduleName)]
        public async Task<TableModel<SparePartDto>> GetSparePartList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, int goodsClassifyId, string searchKey)
        {
            return await _sparePartMgr.GetSparePartList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), goodsClassifyId, searchKey);
        }
         
        /// <summary>
        /// 获取备件表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _sparePartMgr.GetEnableSpareFields(typeof(BaseGoods).Name);
        }

        /// <summary>
        /// 获取备件明细
        /// </summary>
        /// <param name="spareId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<SparePartDto> GetSparePartDetail(string spareId)
        {
            return await _sparePartMgr.GetSparePartDetail(spareId);
        }

        /// <summary>
        /// 获取备件相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units = await _selectOptionsServer.GetUnits(); 
            var spareTypes = await _selectOptionsServer.GetDictionaryOption(BusinessConst.SparePartType);
            var areas = await _selectOptionsServer.GetAreas(BusinessConst.PlantNo);
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            var workbinSpec = await _selectOptionsServer.GetWorkbinSpec();
            return new { SparePartTypeOptions = spareTypes, UnitsOptions = units,AreasOptions= areas, PhotoLimit = photoLimit.Value,WorkbinSpecOptions= workbinSpec };
        }
         
        /// <summary>
        /// 添加备件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加备件", LogType.Add, _moduleName)]
        public async Task AddSparePart(SparePartDto data)
        {
            await _sparePartMgr.AddSparePart(data);
        }

        /// <summary>
        /// 修改备件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改备件", LogType.Update, _moduleName)]
        public async Task UpdateSparePart(SparePartDto data)
        {
            await _sparePartMgr.UpdateSparePart(data);
        }
         
        /// <summary>
        /// 删除备件
        /// </summary>
        /// <param name="SparePartId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除备件", LogType.Del, _moduleName)]
        public async Task DelSparePart(string[] SparePartId)
        {
            await _sparePartMgr.DelSparePart(SparePartId);
        }
         
        /// <summary>
        /// 导出备件
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出备件", LogType.Export, _moduleName)]
        public async Task<string> ExportSparePart(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _sparePartMgr.ExportSparePart(searchKey, orderField, ConvertOrderType(orderType), goodsClassifyId, fields); 
        }

        /// <summary>
        /// 获取当前用户导出用户列表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public List<KeyValueModel> GetAllowField()
        {
            return  _sparePartMgr.GetExportField(); 
        }
        #endregion
    }
}
