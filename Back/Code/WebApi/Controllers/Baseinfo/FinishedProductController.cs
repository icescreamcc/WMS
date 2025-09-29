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
    public class FinishedProductController : AuthTokenController
    {
        private readonly FinishedProductMgr _goodsMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        public FinishedProductController(FinishedProductMgr goodsMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _goodsMgr = goodsMgr;
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        #region 成品类型
        /// <summary>
        /// 获取所有成品类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<TreeModel>> GetFinishedProductClassifyData()
        {
            return await _goodsMgr.GetFinishedProductClassifyData();
        }

        /// <summary>
        /// 添加成品类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加成品类型", LogType.Add)]
        public async Task AddFinishedProductClassify(BaseTypeDto data)
        {
            await _goodsMgr.AddFinishedProductClassify(data);
        }

        /// <summary>
        /// 修改成品类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改成品类型", LogType.Update)]
        public async Task UpdateFinishedProductClassify(BaseTypeDto data)
        {
            await _goodsMgr.UpdateFinishedProductClassify(data);
        }

        /// <summary>
        /// 删除成品类型
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除成品类型", LogType.Del)]
        public async Task DelFinishedProductClassify(int typeId)
        {
            await _goodsMgr.DelFinishedProductClassify(typeId);
        }
        #endregion

        #region 成品
        /// <summary>
        /// 获取所有成品列表
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
        public async Task<TableModel<FinishedProductDto>> GetFinishedProductList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, int typeId, string searchKey)
        {
            return await _goodsMgr.GetFinishedProductList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), typeId, searchKey);
        }

        /// <summary>
        /// 获取成品表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _goodsMgr.GetEnableSpareFields(typeof(BaseGoods).Name);
        }

        /// <summary>
        /// 获取成品明细
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<FinishedProductDto> GetFinishedProductDetail(string sampleId)
        {
            return await _goodsMgr.GetFinishedProductDetail(sampleId);
        }

        /// <summary>
        /// 获取成品相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units = await _selectOptionsServer.GetUnits();
            var sampleTypes = await _selectOptionsServer.GetDictionaryOption(BusinessConst.FinishedProductType);
            var areas = await _selectOptionsServer.GetAreas(BusinessConst.PlantNo);
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            var workbinSpec = await _selectOptionsServer.GetWorkbinSpec();
            return new { FinishedProductTypeOptions = sampleTypes, UnitsOptions = units, AreasOptions = areas, PhotoLimit = photoLimit.Value, WorkbinSpecOptions = workbinSpec };
        }


        /// <summary>
        /// 添加成品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加成品", LogType.Add)]
        public async Task AddFinishedProduct(FinishedProductDto data)
        {
            await _goodsMgr.AddFinishedProduct(data);
        }

        /// <summary>
        /// 修改成品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改成品", LogType.Update)]
        public async Task UpdateFinishedProduct(FinishedProductDto data)
        {
            await _goodsMgr.UpdateFinishedProduct(data);
        }

        /// <summary>
        /// 删除成品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除成品", LogType.Del)]
        public async Task DelFinishedProduct(string[] productId)
        {
            await _goodsMgr.DelFinishedProduct(productId);
        }

        /// <summary>
        /// 查询成品BOM
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<BOMDto>> GetBOMList(string goodsId)
        {
            return await _goodsMgr.GetBOMList(goodsId);
        }

        /// <summary>
        /// 修改成品BOM
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task UpdateBOM(List<BOMDto> data)
        {
            await _goodsMgr.UpdateBOM(data);
        }

        /// <summary>
        /// 导出成品
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出成品", LogType.Export)]
        public async Task<string> ExportFinishedProduct(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _goodsMgr.ExportFinishedProduct(searchKey, orderField, ConvertOrderType(orderType), goodsClassifyId, fields);
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
