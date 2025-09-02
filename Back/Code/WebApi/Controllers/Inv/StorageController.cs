using External.Common;
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Inv
{ 
    public class StorageController : AuthTokenController
    {

        private readonly StorageMgr _storageMgr; 

        private readonly SysArgsService _sysArgsHelper;

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "库存管理";

        public StorageController(StorageMgr storageMgr, SysArgsService sysArgsHelper, SelectOptionsService selectOptionsServer)
        {
            _storageMgr = storageMgr;  
            _sysArgsHelper = sysArgsHelper;
            _selectOptionsServer = selectOptionsServer;
        }

        /// <summary>
        /// 库存汇总分页查询
        /// </summary> 
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看库存", LogType.Read, _moduleName)]
        public async Task<TableModel<Storage>> GetStorageList(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string warehouseId,string goodsGroup)
        {
            return await _storageMgr.GetStorageList( pgSize,  pgIndex,  orderFiled,  ConvertOrderType(orderType),  searchKey, warehouseId, goodsGroup); 
        }

        /// <summary>
        /// 库存明细分页查询
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<TableModel<StorageDetailDto>> GetStorageDetailList(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string warehouseId, string goodsGroup)
        {
            return await _storageMgr.GetStorageDetailList(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), searchKey, warehouseId, goodsGroup);
        }

        /// <summary>
        /// 库存流水分页查询
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="flowType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<TableModel<StorageFlowDetail>> GetStorageFlowList(string goodsId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string flowType, string dateStart, string dateEnd)
        {
            return await _storageMgr.GetStorageFlowList(goodsId, pgSize,  pgIndex,  orderFiled, ConvertOrderType(orderType, false),  searchKey,  flowType,  dateStart,  dateEnd); 
        }
         
        /// <summary>
        /// 获取库存相关选项及参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var unitData = await _selectOptionsServer.GetUnits();
            var warehouseData = await _selectOptionsServer.GetWarehouses();
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            return new { UnitOptions = unitData, WarehouseOptions = warehouseData, GoodsClassifyOptions = goodsClassifyData }; 
        }

        /// <summary>
        /// 获取库存明细
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<StorageWarehouseDetail>> GetStorageDetails(string goodsId)
        {
            return await _storageMgr.GetStorageDetails(goodsId); 
        }

        /// <summary>
        /// 库存汇总计算
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip] 
        public async Task StorageStatistics(string userId, string userName)
        {
             await _storageMgr.StorageStatistics(userId, userName); 
        }

        /// <summary>
        /// 添加库存调拨
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加库存调拨", LogType.Add, _moduleName)]
        public async Task AddAllocationStorage(AllocationOrder data)
        {
             await _storageMgr.AddAllocationStorage(data); 
        }

        /// <summary>
        /// 导出库存汇总信息
        /// </summary>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("导出库存汇总信息", LogType.Export, _moduleName)]
        public async Task<string> ExportStorage(string orderFiled, string orderType, string searchKey, string warehouseId, string goodsGroup)
        {
            return await _storageMgr.ExportStorage(orderFiled, ConvertOrderType(orderType), searchKey, warehouseId, goodsGroup); 
        }

        /// <summary>
        /// 导出库存明细信息
        /// </summary>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("导出库存明细信息", LogType.Export, _moduleName)]
        public async Task<string> ExportStorageDetail(string orderFiled, string orderType, string searchKey, string warehouseId, string goodsGroup)
        {
            return await _storageMgr.ExportStorageDetail(orderFiled, ConvertOrderType(orderType), searchKey, warehouseId, goodsGroup);
        }

        /// <summary>
        /// 导出库存流水信息
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="orderFieled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="flowType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("导出库存流水信息", LogType.Export, _moduleName)]
        public async Task<string> ExportStorageFlow(string goodsId, string orderFieled, string orderType, string searchKey, string flowType, string dateStart, string dateEnd)
        {
            return await _storageMgr.ExportStorageFlow( goodsId,  orderFieled, ConvertOrderType(orderType,false),  searchKey,  flowType,  dateStart,  dateEnd);
        }
    }
}
