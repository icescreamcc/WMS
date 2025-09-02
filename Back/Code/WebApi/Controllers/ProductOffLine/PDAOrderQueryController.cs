using Logic.Inventory;
using Logic.ProductOffLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Prod;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.ProductOffLine
{ 
    /// <summary>
    /// PDA扫码查询订单
    /// </summary>
    public class PDAOrderQueryController : AuthTokenController
    {
        private readonly PDAOrderQueryMgr _pdaOrderQueryMgr;

        public PDAOrderQueryController(PDAOrderQueryMgr pdaOrderQueryMgr)
        {
            _pdaOrderQueryMgr = pdaOrderQueryMgr;
        }

        /// <summary>
        /// 根据小车唯一码、交接单码、库位码查询交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<CarLoadDto> GetProductionOrderByCarCode(string carSoleCode)
        {
            return await _pdaOrderQueryMgr.GetProductionOrderByCarCode(carSoleCode);
        }

        /// <summary>
        /// 解锁库位
        /// </summary>
        /// <param name="binNo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task UnlockBin(string binNo, string userName)
        {
            await _pdaOrderQueryMgr.UnlockBin(binNo, userName);
        }
    }
}
