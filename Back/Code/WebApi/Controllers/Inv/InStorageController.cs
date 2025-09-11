using DbRepository.Repository.DbModels;
using External.Common;
using Logic.Inventory;
using Logic.LogicBase;
using Logic.LogicCommon; 
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Sys;
using Quartz.Util;
using StackExchange.Redis;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Inv
{ 
    public class InStorageController : AuthTokenController
    {

        private readonly InStorageMgr _inStorageMgr; 

        private readonly SelectOptionsService _selectOptionsServer;

        private readonly SysArgsService _sysArgsHelper;

        private const string _moduleName = "入库管理";

        public InStorageController(InStorageMgr  inStorageMgr, SelectOptionsService selectOptionsServer, SysArgsService sysArgsHelper)
        {
            _inStorageMgr = inStorageMgr;  
            _selectOptionsServer = selectOptionsServer;
            _sysArgsHelper = sysArgsHelper;
        }

        /// <summary>
        /// 入库单分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看入库单", LogType.Read, _moduleName)]
        public async Task<TableModel<InStorage>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup)
        {
            return await _inStorageMgr.GetOrders( userId,  pgSize,  pgIndex,  orderFiled,  ConvertOrderType(orderType,false),  searchKey,  dateStart,  dateEnd, goodsGroup); 
        }

        /// <summary>
        /// 获取入库单相关参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetArgs()
        { 
            var isInStorageApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsInStorageApproval)).Value.ToString());
            return new { IsInStorageApproval = isInStorageApproval };
        }

        /// <summary>
        /// 获取入库单相关选项及参数数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        { 
            var unitData = await _selectOptionsServer.GetUnits();
            var warehouseData = await _selectOptionsServer.GetWarehouses();
            
            var inStorageTypeData = EnumHelper.GetEnumValNames<InStorageType>();
            var goodsClassifyData = EnumHelper.GetEnumValNames<BaseTypeGroup>();
            var invbinData = await _selectOptionsServer.GetInVBin();
            var data = new { UnitOptions = unitData, WarehouseOptions = warehouseData , InStorageTypeOptions = inStorageTypeData,GoodsClassifyOptions=goodsClassifyData,InvBinData= invbinData };
            return data;
        }

        /// <summary>
        /// 获取入库单明细
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<InStorageDetail>> GetOrderDetail(string userId, string orderNo)
        {
            return await _inStorageMgr.GetOrderDetail(orderNo); 
        }

        /// <summary>
        /// 根据物品查询推荐入库料箱
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<RecommendWorkbinCellDto>> GetWorkbinRecommend(string goodsId)
        {
            return await _inStorageMgr.GetWorkbinRecommend(goodsId);
        }

        /// <summary>
        /// 查询指定仓库和规格的货架、货位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="specId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WorkbinDto>> GetBinRecommend(string warehouseId, int specId)
        {
            return await _inStorageMgr.GetBinRecommend(warehouseId, specId);
        }

        /// <summary>
        /// 添加入库单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加入库单", LogType.Add, _moduleName)]
        public async Task AddInStorage(InStorage data)
        {
            await _inStorageMgr.AddInStorage(data); 
        }

        /// <summary>
        /// 修改仓库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改入库单", LogType.Update, _moduleName)]
        public async Task UpdateInStorage(InStorage data)
        {
            await _inStorageMgr.UpdateInStorage(data); 
        }

        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除入库单", LogType.Del, _moduleName)]
        public async Task DelInStorage(string[] orderNo)
        {
            await _inStorageMgr.DelInStorage(orderNo); 
        }

        /// <summary>
        /// 入库确认
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("入库确认", LogType.Update, _moduleName)]
        public async Task ConfirmInStorage(string orderNo)
        {
            await _inStorageMgr.ConfirmInStorage(orderNo); 
        }

        [HttpGet]
        [BusinessLog("AGV调度", LogType.Add, _moduleName)]
        public async Task AGVScheduling(string orderNo)
        {
            await _inStorageMgr.AGVScheduling(orderNo);
        }

        /// <summary>
        /// 导出入库单
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("导出入库单", LogType.Export, _moduleName)]
        public async Task<string> ExportInStorage(string searchKey, string orderField, string orderType, string dateStart, string dateEnd, string goodsGroup)
        {
            var fileUrl = await _inStorageMgr.ExportInStorage(searchKey, orderField, ConvertOrderType(orderType), dateStart, dateEnd, goodsGroup); 
            return fileUrl;
        }

        /// <summary>
        /// 获取当前用户导出入库单表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldPermission>> GetAllowField(string userId)
        {
            var inStorageFields = await _inStorageMgr.GetAllowField<InvInStorage>(userId);
            var inStorageDetailFields = await _inStorageMgr.GetAllowField<InvInStorageDetail>(userId);
            var data = new List<FieldPermission>();
            data.AddRange(inStorageFields);
            data.AddRange(inStorageDetailFields);
            return data;
        }

        /// <summary>
        /// 审批入库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isApprove"></param>
        /// <param name="opinion"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("审批入库单", LogType.Update, _moduleName)]
        public async Task ApprovalInStorage(string[] orderNo, bool isApprove,string opinion, string userId, string userName)
        {
            await _inStorageMgr.ApprovalInStorage(orderNo, isApprove, opinion, userId, userName); 
        }

        [HttpPost]
        [Skip]
        public async Task SparePartExport()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 1024)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        var fileName = file.FileName;
                        using (var stream = file.OpenReadStream())
                        {
                            await _inStorageMgr.SparePartExport(fileName, fileExtension, stream);
                        }
                    }
                    else
                    {
                        throw new BusinessException("导入文件必须是Excel格式");
                    }

                }
                else
                {
                    throw new BusinessException("文件不能大于1G");
                }

            }
            else
            {
                throw new BusinessException("未获取到文件信息");
            }

        }
    }
}
