using DbRepository.Repository.DbModels; 
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon; 
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Inv
{ 
    public class WarehouseBinController : AuthTokenController
    {

        private readonly WarehouseMgr _warehouseMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "仓库管理";

        public WarehouseBinController(WarehouseMgr warehouseMgr, SelectOptionsService selectOptionsService)
        {
            _warehouseMgr = warehouseMgr;  
            _selectOptionsServer = selectOptionsService;
        }
         
        /// <summary>
        /// 获取所有仓库列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看仓库信息", LogType.Read, _moduleName)]
        public async Task<TableModel<WarehouseDetail>> GetWarehouses(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _warehouseMgr.GetWarehouses(userId, pgSize,  pgIndex,  orderFiled,  ConvertOrderType(orderType),  searchKey); 
        }

        /// <summary>
        /// 获取仓库表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _warehouseMgr.GetEnableSpareFields(typeof(InvWarehouse).Name); 
        }

        /// <summary>
        /// 获取仓库相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            return await _selectOptionsServer.GetDictionaryOption("WarehouseType");  
        }

        /// <summary>
        /// 根据仓库查询库位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseBin>> GetWarehouseBins(string warehouseId)
        {
            return await _warehouseMgr.GetWarehouseBins(warehouseId); 
        } 
          
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加仓库", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task AddWarehouseBin(WarehouseDetail data)
        {
             await _warehouseMgr.AddWarehouseBin(data); 
        }

        /// <summary>
        /// 修改仓库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改仓库", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateWarehouseBin(WarehouseDetail data)
        {
            await _warehouseMgr.UpdateWarehouseBin(data); 
        }

        /// <summary>
        /// 删除仓库
        /// </summary>
        /// <param name="warehousesId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除仓库", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelWarehouseBin(string[] warehousesId)
        {
            await _warehouseMgr.DelWarehouseBin(warehousesId); 
        } 
    }
}
