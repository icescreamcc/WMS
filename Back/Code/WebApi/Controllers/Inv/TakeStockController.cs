using Logic.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Baseinfo;
using Models.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models.Model.Inv;
using WebApi.Filter;
using External.Common;
using Models.Model.Enum;
using Logic.LogicCommon;
using Models.Model.EchartsModel;
using Logic.LogicBase;
using Logic.Purchase;

namespace WebApi.Controllers.Inv
{
    /// <summary>
    /// 盘点
    /// </summary>
    public class TakeStockController : AuthTokenController
    {
        private readonly TakeStockMgr _takeStockMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "库存盘点";

        public TakeStockController(TakeStockMgr takeStockMgr, SelectOptionsService selectOptionsServer)
        {
            _takeStockMgr = takeStockMgr;
            _selectOptionsServer = selectOptionsServer;
        }

        /// <summary>
        /// 查询消耗趋势
        /// </summary>
        /// <returns></returns>
        [HttpGet] 
        public async Task<RadarModel> GetExpendTrend(string classifyGroup)
        {
            return await _takeStockMgr.GetExpendTrend(classifyGroup);
        }

        /// <summary>
        /// 获取当前盘点被锁定的物品
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="typeId"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableModel<GoodsTakeStockDto>> GetTakeStockLockGoods(int pgSize, int pgIndex, string orderFiled, string orderType, int typeId, string searchKey)
        {
           return await _takeStockMgr.GetTakeStockLockGoods(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), typeId, searchKey);
        }

        /// <summary>
        /// 查询历史盘点记录
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsGroup"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="month"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看历史盘点记录", LogType.Read, _moduleName)]
        public async Task<TableModel<TakeStockReportDto>> GetTakeStockHis(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId, int month, string searchKey)
        {
            return await _takeStockMgr.GetTakeStockHis(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType,false), goodsGroup , goodsClassifyId, month, searchKey);
        }

        [HttpGet]
        [Skip]
        public async Task<TakeStockReasonDto> GetTaskStockDetil(int flowId)
        {
            return await _takeStockMgr.GetTaskStockDetil(flowId);
        }

        [HttpPost]
        [BusinessLog("修改盘点盈亏原因分析", LogType.Update, _moduleName)]
        public async Task UpdateReason(TakeStockReasonDto data)
        {
            await _takeStockMgr.UpdateReason(data);
        }

        /// <summary>
        /// 获取盘点相关选项参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        { 
            var reasonTypeData = await _selectOptionsServer.GetDictionaryOption(BusinessConst.TakeStockReasonType); 
            return new
            { 
                ReasonTypeOptions = reasonTypeData?.ArgsOptions, 
            };
        }

        /// <summary>
        /// 导出盘点记录
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsGroup"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("导出盘点信息", LogType.Export, _moduleName)]
        public async Task<string> ExportTakeStockHis(string orderField, string orderType, string goodsGroup, int goodsClassifyId, int month, string searchKey)
        {
            var fileUrl = await _takeStockMgr.ExportTakeStockHis(searchKey, orderField, ConvertOrderType(orderType), goodsGroup, goodsClassifyId, month);
            return fileUrl;
        }
        /// <summary>
        /// 查询所有物品列表
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsGroup"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="keyword"></param>
        /// <param name="isTakeStockCurDate"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<TableModel<GoodsSimple>> GetGoodsByKey(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId, string warehouseId, string keyword, bool isTakeStockCurDate)
        {
            return await _takeStockMgr.GetGoodsByKey(pgSize,pgIndex, orderFiled, orderType, goodsGroup, goodsClassifyId, warehouseId, keyword, isTakeStockCurDate);
        }

        /// <summary>
        /// 查询当前物品是否在盘点中
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<bool> GetGoodsTakeStockStatus(string goodsId)
        {
            return await _takeStockMgr.GetGoodsTakeStockStatus(goodsId);
        }

        /// <summary>
        /// 盘点锁定物品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SetTakeStockLockByGoods(List<string> goodsId)
        {
            await _takeStockMgr.SetTakeStockLockByGoods(goodsId);
        }

        /// <summary>
        /// 查询指定物品的存储明细
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<GoodsDetailDto> GetGoodsInventoryDetail(string goodsId)
        {
            return await _takeStockMgr.GetGoodsInventoryDetail(goodsId);
        }

        /// <summary>
        /// 盘点保存（按物品盘点）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("保存盘点记录", LogType.Add, _moduleName)]
        public async Task AddTaskStockOrderByGoods(TakeStockGoodsDto data)
        {
            await _takeStockMgr.AddTaskStockOrderByGoods(data);
        }

        /// <summary>
        /// 获取当前盘点被锁定的库位
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<TableModel<BinTakeStockDto>> GetTakeStockLockInvBin(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _takeStockMgr.GetTakeStockLockInvBin(pgSize,pgIndex, orderFiled, ConvertOrderType(orderType), searchKey);
        }

        /// <summary>
        /// 分页查询货位信息
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="warehouseId"></param>
        /// <param name="shelfId"></param>
        /// <param name="workbinSpecId"></param>
        /// <param name="goodsGroup"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="keyword"></param>
        /// <param name="isTakeStockCurDate"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<TableModel<BinTakeStockDto>> GetBins(int pgSize, int pgIndex, string warehouseId, string shelfId, int workbinSpecId, string goodsGroup, string orderFiled, string orderType, string keyword,bool isTakeStockCurDate)
        {
            return await _takeStockMgr.GetBins(pgSize, pgIndex, warehouseId, shelfId, workbinSpecId, goodsGroup,orderFiled, orderType, keyword, isTakeStockCurDate);
        }

        /// <summary>
        /// 查询当前货位是否在盘点中
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<bool> GetBinTakeStockStatus(int binId)
        {
            return await _takeStockMgr.GetBinTakeStockStatus(binId);
        }

        /// <summary>
        /// 盘点锁定库位
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SetTakeStockLockByBin(List<int> binId)
        {
            await _takeStockMgr.SetTakeStockLockByBin(binId);
        }

        /// <summary>
        /// 查询指定库位存储明细
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<BinTakeStockDetailDto> GetBinInventoryDetail(int binId)
        {
           return await _takeStockMgr.GetBinInventoryDetail(binId);
        }

        /// <summary>
        /// 盘点保存（按货位盘点）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("保存盘点记录", LogType.Add, _moduleName)]
        public async Task AddTaskStockOrderByBin(BinTakeStockDetailDto data)
        {
             await _takeStockMgr.AddTaskStockOrderByBin(data);
        }
    }
}
