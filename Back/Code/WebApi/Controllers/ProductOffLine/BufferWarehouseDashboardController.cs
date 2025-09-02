
using Logic.ProductOffLine;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Inv;
using Models.Model.Prod;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.ProductOffLine
{
    /// <summary>
    /// 缓存仓看板API
    /// </summary>
    public class BufferWarehouseDashboardController : AuthTokenController
    {
        private readonly BufferWarehouseDashboardMgr _bufferWarehouseDashboardMgr;

        public BufferWarehouseDashboardController(BufferWarehouseDashboardMgr bufferWarehouseDashboardMgr)
        {
            _bufferWarehouseDashboardMgr = bufferWarehouseDashboardMgr;
        }

        /// <summary>
        /// 查询所有配对小车信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<TableModel<MatchingCarDto>> GetMatchingInfo(int pgSize, int pgIndex)
        {
            return await _bufferWarehouseDashboardMgr.GetMatchingInfo(pgSize, pgIndex);
        }

        /// <summary>
        /// 查询缓存仓库位信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<WarehouseElement>> GetBufferWarehouseElement()
        {
            return await _bufferWarehouseDashboardMgr.GetBufferWarehouseElement();
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
            return await _bufferWarehouseDashboardMgr.GetStockInfoByBin(binId);
        }
    }
}
