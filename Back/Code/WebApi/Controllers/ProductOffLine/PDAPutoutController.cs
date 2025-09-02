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
    public class PDAPutoutController : AuthTokenController
    {
        private readonly PDAPutoutMgr _pdaPutoutMgr;

        public PDAPutoutController(PDAPutoutMgr pdaPutoutMgr)
        {
            _pdaPutoutMgr = pdaPutoutMgr;
        }

        /// <summary>
        /// 根据小车唯一码查询已配对的生产交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
            return await _pdaPutoutMgr.GetProductionOrderInfo(carSoleCode);
        }

        /// <summary>
        /// 扫码下架
        /// 校验小车唯一码和库位码
        /// 校验配对码
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <param name="binNo"></param>
        /// <returns></returns> 
        [HttpGet]
        [Skip]
        public async Task<bool> CheckPutout(string carSoleCode, string binNo)
        {
            return await _pdaPutoutMgr.CheckPutout(carSoleCode, binNo);
        }

        /// <summary>
        /// 提交下架 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SubmitPutout(CarLoadDto data)
        {
            await _pdaPutoutMgr.SubmitPutout(data);
        }
    }
}
