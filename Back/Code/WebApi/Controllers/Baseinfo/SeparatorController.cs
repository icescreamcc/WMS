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
using WebApi.Filter;

namespace WebApi.Controllers.Baseinfo
{ 
    public class SeparatorController : AuthTokenController
    {
        private readonly SeparatorMgr _goodsMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "辅材信息管理";

        public SeparatorController(SeparatorMgr goodsMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _goodsMgr = goodsMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        #region 辅材类型
        /// <summary>
        /// 获取所有辅材类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<TreeModel>> GetSeparatorClassifyData()
        {
            return await _goodsMgr.GetSeparatorClassifyData();
        }

        /// <summary>
        /// 添加辅材类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加辅材类型", LogType.Add, _moduleName)]
        public async Task AddSeparatorClassify(BaseTypeDto data)
        {
            await _goodsMgr.AddSeparatorClassify(data);
        }

        /// <summary>
        /// 修改辅材类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改辅材类型", LogType.Update, _moduleName)]
        public async Task UpdateSeparatorClassify(BaseTypeDto data)
        {
            await _goodsMgr.UpdateSeparatorClassify(data);
        }

        /// <summary>
        /// 删除辅材类型
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除辅材类型", LogType.Del, _moduleName)]
        public async Task DelSeparatorClassify(int typeId)
        {
            await _goodsMgr.DelSeparatorClassify(typeId);
        }
        #endregion

        #region 辅材
        /// <summary>
        /// 获取所有辅材列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="typeId"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看辅材信息", LogType.Read, _moduleName)]
        public async Task<TableModel<SeparatorDto>> GetSeparatorList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, int typeId, string searchKey)
        {
            return await _goodsMgr.GetSeparatorList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), typeId, searchKey);
        }

        /// <summary>
        /// 获取辅材表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _goodsMgr.GetEnableSpareFields(typeof(BaseGoods).Name);
        }

        /// <summary>
        /// 获取辅材明细
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<SeparatorDto> GetSeparatorDetail(string sampleId)
        {
            return await _goodsMgr.GetSeparatorDetail(sampleId);
        }

        /// <summary>
        /// 获取辅材相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units = await _selectOptionsServer.GetUnits();
            var sampleTypes = await _selectOptionsServer.GetDictionaryOption(BusinessConst.SeparatorType);
            var areas = await _selectOptionsServer.GetAreas(BusinessConst.PlantNo);
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            var workbinSpec = await _selectOptionsServer.GetWorkbinSpec();
            return new { SeparatorTypeOptions = sampleTypes, UnitsOptions = units, AreasOptions = areas, PhotoLimit = photoLimit.Value, WorkbinSpecOptions = workbinSpec };
        }


        /// <summary>
        /// 添加辅材
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加辅材", LogType.Add, _moduleName)]
        public async Task AddSeparator(SeparatorDto data)
        {
            await _goodsMgr.AddSeparator(data);
        }

        /// <summary>
        /// 修改辅材
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改辅材", LogType.Update, _moduleName)]
        public async Task UpdateSeparator(SeparatorDto data)
        {
            await _goodsMgr.UpdateSeparator(data);
        }

        /// <summary>
        /// 删除辅材
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除辅材", LogType.Del, _moduleName)]
        public async Task DelSeparator(string[] productId)
        {
            await _goodsMgr.DelSeparator(productId);
        }

        /// <summary>
        /// 导出辅材
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出辅材", LogType.Export, _moduleName)]
        public async Task<string> ExportSeparator(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _goodsMgr.ExportSeparator(searchKey, orderField, ConvertOrderType(orderType), goodsClassifyId, fields);
        }

        /// <summary>
        /// 获取当前用户导出用户列表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public List<KeyValueModel> GetAllowField()
        {
            return _goodsMgr.GetExportField();
        }
        #endregion
    }
}
