using Logic.LogicBase;
using Logic.ProductOffLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Prod;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.ProductOffLine
{
    /// <summary>
    /// PDA扫码下架
    /// </summary>
    public class PDAUnstackController : AuthTokenController
    {
        private readonly PDAUnstackMgr _pdaUnstackMgr;

        public PDAUnstackController(PDAUnstackMgr pdaUnstackMgr)
        {
            _pdaUnstackMgr = pdaUnstackMgr;
        }

        /// <summary>
        /// 根据小车唯一码查询已下架的生产交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
            return await _pdaUnstackMgr.GetProductionOrderInfo(carSoleCode);
        }

        /// <summary>
        /// 上架拆垛机检查 
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <param name="binNo"></param>
        /// <returns></returns> 
        [HttpGet]
        [Skip]
        public async Task<bool> CheckOnUnstack(string carSoleCode, string binNo)
        {
            return await _pdaUnstackMgr.CheckOnUnstack(carSoleCode, binNo);
        }

        /// <summary>
        /// 上架拆垛机提交 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SubmitOnUnStack(CarLoadDto data)
        {
            await _pdaUnstackMgr.SubmitOnUnStack(data);
        }
    }
}
