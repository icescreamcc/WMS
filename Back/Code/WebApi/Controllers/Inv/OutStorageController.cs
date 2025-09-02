using DbRepository.Repository.DbModels;
using External.Common;
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon; 
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Inv
{ 
    public class OutStorageController : AuthTokenController
    {

        private readonly OutStorageMgr _outStorageMgr; 

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "出库管理";

        public OutStorageController(OutStorageMgr outStorageMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _outStorageMgr = outStorageMgr; 
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        /// <summary>
        /// 出库单分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看出库单", LogType.Read, _moduleName)]
        public async Task<TableModel<OutStorage>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup)
        {
            return await _outStorageMgr.GetOrders( userId,  pgSize,  pgIndex,  orderFiled,  ConvertOrderType(orderType,false),  searchKey,  dateStart,  dateEnd, goodsGroup); 
        }

        /// <summary>
        /// 获取出库单相关参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetArgs()
        { 
            var isOutStorageApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsOutStorageApproval)).Value.ToString());
            return new {  IsOutStorageApproval = isOutStorageApproval };
        }

        /// <summary>
        /// 获取出库单相关选项及参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        { 
            var unitData = await _selectOptionsServer.GetUnits();
            var warehouseData = await _selectOptionsServer.GetWarehouses();
            var outStorageTypeData = EnumHelper.GetEnumValNames<OutStorageType>();
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            var lineData = await _selectOptionsServer.GetLines(BusinessConst.PlantNo);
            return new { UnitOptions = unitData, WarehouseOptions = warehouseData , OutStorageTypeOptions = outStorageTypeData , GoodsClassifyOptions = goodsClassifyData, LineOptions = lineData }; 
        }

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<OutStorageDetail>> GetOrderDetail(string userId, string orderNo)
        {
            return await _outStorageMgr.GetOrderDetail(userId,orderNo); 
        } 
          
        /// <summary>
        /// 添加出库单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加出库单", LogType.Add, _moduleName)]
        public async Task AddOutStorage(OutStorage data)
        {
            await _outStorageMgr.AddOutStorage(data); 
        }

        /// <summary>
        /// 修改仓库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改出库单", LogType.Update, _moduleName)]
        public async Task UpdateOutStorage(OutStorage data)
        {
            await _outStorageMgr.UpdateOutStorage(data); 
        }

        /// <summary>
        /// 删除出库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除出库单", LogType.Del, _moduleName)]
        public async Task DelOutStorage(string[] orderNo)
        {
            await _outStorageMgr.DelOutStorage(orderNo); 
        }

        [HttpPost]
        [Skip]
        [BusinessLog("修改出库单实际数量", LogType.Update, _moduleName)]
        public async Task UpdateActualQuantity(InvOutStorageDetail detail)
        {
            await _outStorageMgr.UpdateActualQuantity(detail);
        }

        /// <summary>
        /// 出库确认
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("出库确认", LogType.Del, _moduleName)]
        public async Task ConfirmOutStorage(string orderNo)
        {
            await _outStorageMgr.ConfirmOutStorage(orderNo); 
        }

        /// <summary>
        /// 导出出库单
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("导出出库单", LogType.Export, _moduleName)]
        public async Task<string> ExportOutStorage(string searchKey, string orderField, string orderType, string dateStart, string dateEnd, string goodsGroup)
        {
            var fileUrl = await _outStorageMgr.ExportOutStorage(searchKey, orderField, ConvertOrderType(orderType), dateStart, dateEnd, goodsGroup); 
            return fileUrl;
        }

        /// <summary>
        /// 获取当前用户导出出库单表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldPermission>> GetAllowField(string userId)
        {
            var outStorageFields = await _outStorageMgr.GetAllowField<InvOutStorage>(userId);
            var outStorageDetailFields = await _outStorageMgr.GetAllowField<InvOutStorageDetail>(userId);
            var data = new List<FieldPermission>();
            data.AddRange(outStorageFields);
            data.AddRange(outStorageDetailFields);
            return data;
        }

        /// <summary>
        /// 审批出库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isApprove"></param>
        /// <param name="opinion"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("审批出库单", LogType.Update, _moduleName)]
        public async Task ApprovalOutStorage(string[] orderNo, bool isApprove,string opinion, string userId, string userName)
        {
            await _outStorageMgr.ApprovalOutStorage(orderNo, isApprove, opinion, userId, userName); 
        }
         
    }
}
