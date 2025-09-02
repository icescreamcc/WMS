using Logic.ProductOffLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Prod;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.ProductOffLine
{ 
    /// <summary>
    /// PDA扫码上架
    /// </summary>
    public class PDAPutawayController : AuthTokenController
    {
        private readonly PDAPutawayMgr _pdaPutawayMgr;
        public PDAPutawayController(PDAPutawayMgr pdaPutawayMgr) 
        {
            _pdaPutawayMgr=pdaPutawayMgr;
        }

        /// <summary>
        /// 根据小车唯一码查询已开始装车的生产交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
           return await _pdaPutawayMgr.GetProductionOrderInfo(carSoleCode);
        }

        /// <summary>
        /// 扫码上架,校验小车唯一码和库位码
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <param name="binNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<bool> CheckPutaway(string carSoleCode, string binNo)
        {
            return await _pdaPutawayMgr.CheckPutaway(carSoleCode, binNo);
        }

        /// <summary>
        /// 提交上架
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost] 
        public async Task SubmitPutaway(CarLoadDto data)
        {
             await _pdaPutawayMgr.SubmitPutaway(data);
        } 
    }
}
