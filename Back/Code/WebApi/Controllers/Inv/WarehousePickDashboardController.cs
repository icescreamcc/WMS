using Logic.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Inv
{ 
    public class WarehousePickDashboardController : AuthTokenController
    {
        private readonly WarehousePickDashboardMgr _warehousePickDashboardMgr;

        public WarehousePickDashboardController(WarehousePickDashboardMgr warehousePickDashboardMgr)
        {
            _warehousePickDashboardMgr = warehousePickDashboardMgr;
        }

        [HttpGet]
        [Skip]
        public async Task<List<GoodsPickData>> GetData(string goodsClassifyGroup, int goodsClassifyId, string searchKey)
        {
            return await _warehousePickDashboardMgr.GetData( goodsClassifyGroup,  goodsClassifyId,  searchKey);
        }
    }
}
