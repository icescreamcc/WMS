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
    public class RawMaterialController : AuthTokenController
    {
        private readonly RawMaterialMgr _goodsMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "原材料信息管理";

        public RawMaterialController(RawMaterialMgr goodsMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _goodsMgr = goodsMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        #region 原材料类型
        /// <summary>
        /// 获取所有原材料类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<TreeModel>> GetRawMaterialClassifyData()
        {
            return await _goodsMgr.GetRawMaterialClassifyData();
        }

        /// <summary>
        /// 添加原材料类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加原材料类型", LogType.Add, _moduleName)]
        public async Task AddRawMaterialClassify(BaseTypeDto data)
        {
            await _goodsMgr.AddRawMaterialClassify(data);
        }

        /// <summary>
        /// 修改原材料类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改原材料类型", LogType.Update, _moduleName)]
        public async Task UpdateRawMaterialClassify(BaseTypeDto data)
        {
            await _goodsMgr.UpdateRawMaterialClassify(data);
        }

        /// <summary>
        /// 删除原材料类型
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除原材料类型", LogType.Del, _moduleName)]
        public async Task DelRawMaterialClassify(int typeId)
        {
            await _goodsMgr.DelRawMaterialClassify(typeId);
        }
        #endregion

        #region 原材料
        /// <summary>
        /// 获取所有原材料列表
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
        [BusinessLog("查看原材料信息", LogType.Read, _moduleName)]
        public async Task<TableModel<RawMaterialDto>> GetRawMaterialList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, int typeId, string searchKey)
        {
            return await _goodsMgr.GetRawMaterialList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), typeId, searchKey);
        }

        /// <summary>
        /// 获取原材料表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _goodsMgr.GetEnableSpareFields(typeof(BaseGoods).Name);
        }

        /// <summary>
        /// 获取原材料明细
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<RawMaterialDto> GetRawMaterialDetail(string sampleId)
        {
            return await _goodsMgr.GetRawMaterialDetail(sampleId);
        }

        /// <summary>
        /// 获取原材料相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units = await _selectOptionsServer.GetUnits();
            var sampleTypes = await _selectOptionsServer.GetDictionaryOption(BusinessConst.RawMaterialType);
            var abnormalReceiptClassifications=await _selectOptionsServer.GetDictionaryOption(BusinessConst.AbnormalReceiptClassification);
            var areas = await _selectOptionsServer.GetAreas(BusinessConst.PlantNo);
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            var workbinSpec = await _selectOptionsServer.GetWorkbinSpec();
            return new { RawMaterialTypeOptions = sampleTypes, AbnormalReceiptClassificationOptions= abnormalReceiptClassifications, UnitsOptions = units, AreasOptions = areas, PhotoLimit = photoLimit.Value, WorkbinSpecOptions = workbinSpec };
        }


        /// <summary>
        /// 添加原材料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加原材料", LogType.Add, _moduleName)]
        public async Task AddRawMaterial(RawMaterialDto data)
        {
            await _goodsMgr.AddRawMaterial(data);
        }

        /// <summary>
        /// 修改原材料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改原材料", LogType.Update, _moduleName)]
        public async Task UpdateRawMaterial(RawMaterialDto data)
        {
            await _goodsMgr.UpdateRawMaterial(data);
        }

        /// <summary>
        /// 删除原材料
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除原材料", LogType.Del, _moduleName)]
        public async Task DelRawMaterial(string[] productId)
        {
            await _goodsMgr.DelRawMaterial(productId);
        }

        /// <summary>
        /// 导出原材料
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出原材料", LogType.Export, _moduleName)]
        public async Task<string> ExportRawMaterial(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _goodsMgr.ExportRawMaterial(searchKey, orderField, ConvertOrderType(orderType), goodsClassifyId, fields);
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
