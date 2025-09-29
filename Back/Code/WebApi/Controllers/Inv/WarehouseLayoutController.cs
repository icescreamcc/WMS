using Logic.Inventory;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Prod;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Inv
{
    public class WarehouseLayoutController: AuthTokenController
    {
        private readonly LayoutMgr _layoutMgr;

        private readonly ShelfBinMgr _shelfBinMgr;

        private const string _moduleName = "仓库货位布局";

        public WarehouseLayoutController(LayoutMgr layoutMgr, ShelfBinMgr shelfBinMgr)
        {
            _layoutMgr = layoutMgr;
            _shelfBinMgr = shelfBinMgr;
        }

        /// <summary>
        /// 仓库、货架、库位树形结构数据查看
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看仓库货位布局", LogType.Read, _moduleName)]
        public async Task<List<TreeModel>> GetWarehouseTree()
        {
            return await _shelfBinMgr.GetWarehouseTree();
        }

        /// <summary>
        /// 获取仓库下的所有货架或货位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetElementByWarehouse(string warehouseId)
        {
            return await _layoutMgr.GetElementByWarehouse(warehouseId);
        }

        /// <summary>
        /// 获取货架下所有货位
        /// </summary>
        /// <param name="shelfId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetElementByShelf(string shelfId)
        {
            return await _layoutMgr.GetElementByShelf(shelfId);
        }

        /// <summary>
        /// 获取货位对应的料箱单元格
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetWorkbinCells(int binId)
        {
            return await _layoutMgr.GetWorkbinCells(binId);
        }

        /// <summary>
        /// 根据库位查询存放信息
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<BinStockInfo> GetStockInfoByBin(int binId)
        {
            return await _layoutMgr.GetStockInfoByBin(binId);
        }

        /// <summary>
        /// 解锁库位
        /// </summary>
        /// <param name="binId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task UnlockBin(int binId, string userName)
        {
            await _layoutMgr.UnlockBin(binId, userName);
        }
    }
}
