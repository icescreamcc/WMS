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
    public class PackingMaterialController : AuthTokenController
    {
        private readonly PackingMaterialMgr _goodsMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "包材信息管理";

        public PackingMaterialController(PackingMaterialMgr goodsMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _goodsMgr = goodsMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        #region 包材类型
        /// <summary>
        /// 获取所有包材类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<TreeModel>> GetPackingMaterialClassifyData()
        {
            return await _goodsMgr.GetPackingMaterialClassifyData();
        }

        /// <summary>
        /// 添加包材类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加包材类型", LogType.Add, _moduleName)]
        public async Task AddPackingMaterialClassify(BaseTypeDto data)
        {
            await _goodsMgr.AddPackingMaterialClassify(data);
        }

        /// <summary>
        /// 修改包材类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改包材类型", LogType.Update, _moduleName)]
        public async Task UpdatePackingMaterialClassify(BaseTypeDto data)
        {
            await _goodsMgr.UpdatePackingMaterialClassify(data);
        }

        /// <summary>
        /// 删除包材类型
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除包材类型", LogType.Del, _moduleName)]
        public async Task DelPackingMaterialClassify(int typeId)
        {
            await _goodsMgr.DelPackingMaterialClassify(typeId);
        }
        #endregion

        #region 包材
        /// <summary>
        /// 获取所有包材列表
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
        [BusinessLog("查看包材信息", LogType.Read, _moduleName)]
        public async Task<TableModel<PackingMaterialDto>> GetPackingMaterialList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, int typeId, string searchKey)
        {
            return await _goodsMgr.GetPackingMaterialList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), typeId, searchKey);
        }

        /// <summary>
        /// 获取包材表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _goodsMgr.GetEnableSpareFields(typeof(BaseGoods).Name);
        }

        /// <summary>
        /// 获取包材明细
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<PackingMaterialDto> GetPackingMaterialDetail(string sampleId)
        {
            return await _goodsMgr.GetPackingMaterialDetail(sampleId);
        }

        /// <summary>
        /// 获取包材相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units = await _selectOptionsServer.GetUnits();
            var sampleTypes = await _selectOptionsServer.GetDictionaryOption(BusinessConst.PackingMaterialType);
            var areas = await _selectOptionsServer.GetAreas(BusinessConst.PlantNo);
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            var workbinSpec = await _selectOptionsServer.GetWorkbinSpec();
            return new { PackingMaterialTypeOptions = sampleTypes, UnitsOptions = units, AreasOptions = areas, PhotoLimit = photoLimit.Value, WorkbinSpecOptions = workbinSpec };
        }


        /// <summary>
        /// 添加包材
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加包材", LogType.Add, _moduleName)]
        public async Task AddPackingMaterial(PackingMaterialDto data)
        {
            await _goodsMgr.AddPackingMaterial(data);
        }

        /// <summary>
        /// 修改包材
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改包材", LogType.Update, _moduleName)]
        public async Task UpdatePackingMaterial(PackingMaterialDto data)
        {
            await _goodsMgr.UpdatePackingMaterial(data);
        }

        /// <summary>
        /// 删除包材
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除包材", LogType.Del, _moduleName)]
        public async Task DelPackingMaterial(string[] productId)
        {
            await _goodsMgr.DelPackingMaterial(productId);
        }

        /// <summary>
        /// 导出包材
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出包材", LogType.Export, _moduleName)]
        public async Task<string> ExportPackingMaterial(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _goodsMgr.ExportPackingMaterial(searchKey, orderField, ConvertOrderType(orderType), goodsClassifyId, fields);
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
