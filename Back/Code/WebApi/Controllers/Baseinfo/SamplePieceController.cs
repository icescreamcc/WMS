using DbRepository.Repository.DbModels;
using Logic.BaseInfo;
using Logic.LogicBase;
using Logic.LogicCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Baseinfo
{ 
    public class SamplePieceController : AuthTokenController
    {

        private readonly SamplePieceMgr  _goodsMgr;

        private readonly SelectOptionsService _selectOptionsServer; 

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "样件信息管理";

        public SamplePieceController(SamplePieceMgr goodsMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _goodsMgr = goodsMgr;
            _selectOptionsServer = selectOptionsServer; 
            _sysArgsHelper = sysArgsHelper;
        }

        #region 样件类型
        /// <summary>
        /// 获取所有样件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<TreeModel>> GetSamplePieceClassifyData()
        {
            return await _goodsMgr.GetSamplePieceClassifyData();
        }

        /// <summary>
        /// 添加样件类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加样件类型", LogType.Add, _moduleName)]
        public async Task AddSamplePieceClassify(BaseTypeDto data)
        {
            await _goodsMgr.AddSamplePieceClassify(data);
        }

        /// <summary>
        /// 修改样件类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改样件类型", LogType.Update, _moduleName)]
        public async Task UpdateSamplePieceClassify(BaseTypeDto data)
        {
            await _goodsMgr.UpdateSamplePieceClassify(data);
        }

        /// <summary>
        /// 删除样件类型
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除样件类型", LogType.Del, _moduleName)]
        public async Task DelSamplePieceClassify(int typeId)
        {
            await _goodsMgr.DelSamplePieceClassify(typeId);
        }
        #endregion

        #region 样件
        /// <summary>
        /// 获取所有样件列表
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
        [BusinessLog("查看样件信息", LogType.Read, _moduleName)]
        public async Task<TableModel<SamplePieceDto>> GetSamplePieceList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType,int typeId, string searchKey)
        {
            return await _goodsMgr.GetSamplePieceList(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), typeId, searchKey); 
        }

        /// <summary>
        /// 获取样件表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _goodsMgr.GetEnableSpareFields(typeof(BaseGoods).Name); 
        }

        /// <summary>
        /// 获取样件明细
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<SamplePieceDto> GetSamplePieceDetail(string sampleId)
        {
            return await _goodsMgr.GetSamplePieceDetail(sampleId); 
        }

        /// <summary>
        /// 获取样件相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units = await _selectOptionsServer.GetUnits();
            var sampleTypes = await _selectOptionsServer.GetDictionaryOption(BusinessConst.SamplePieceType);
            var areas = await _selectOptionsServer.GetAreas(BusinessConst.PlantNo);
            var photoLimit = await _sysArgsHelper.GetValueByKey(BusinessConst.GoodsPhotoLimit);
            var workbinSpec = await _selectOptionsServer.GetWorkbinSpec();
            return new { SamplePieceTypeOptions = sampleTypes, UnitsOptions = units, AreasOptions = areas, PhotoLimit = photoLimit.Value, WorkbinSpecOptions = workbinSpec };
        }


        /// <summary>
        /// 添加样件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加样件",LogType.Add, _moduleName)]
        public async Task AddSamplePiece(SamplePieceDto data)
        {
            await _goodsMgr.AddSamplePiece(data); 
        }

        /// <summary>
        /// 修改样件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改样件",LogType.Update, _moduleName)]
        public async Task UpdateSamplePiece(SamplePieceDto data)
        {
            await _goodsMgr.UpdateSamplePiece(data); 
        }

        /// <summary>
        /// 删除样件
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除样件", LogType.Del, _moduleName)]
        public async Task DelSamplePiece(string[] productId)
        {
             await _goodsMgr.DelSamplePiece(productId); 
        }

        /// <summary>
        /// 导出样件
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出样件", LogType.Export, _moduleName)]
        public async Task<string> ExportSamplePiece(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            return await _goodsMgr.ExportSamplePiece(searchKey, orderField, ConvertOrderType(orderType), goodsClassifyId, fields);
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
