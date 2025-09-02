using Logic.ProductOffLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Inv;
using Models.Model.Prod;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.ProductOffLine
{ 
    /// <summary>
    /// PDA扫码装车
    /// </summary>
    public class PDACarLoadController : AuthTokenController
    {
        private readonly PDACarLoadMgr _pdaCarLoadMgr;
        public PDACarLoadController(PDACarLoadMgr pdaCarLoadMgr)
        {
            _pdaCarLoadMgr=pdaCarLoadMgr;
        }

        /// <summary>
        /// 根据小车唯一码查询已分配小车的交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
            return await _pdaCarLoadMgr.GetProductionOrderInfo(carSoleCode);
        }

        /// <summary>
        /// 根据小车唯一码查询推荐存放的缓存库位
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<BinSimple> GetFreeBin(string carSoleCode)
        {
            return await _pdaCarLoadMgr.GetFreeBin(carSoleCode);
        }

        /// <summary>
        /// 锁定库位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost] 
        public async Task LockBin(CarLoadDto data)
        {
            await _pdaCarLoadMgr.LockBin(data);
        }

        /// <summary>
        /// 确认装车
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SubmitCarLoad(CarLoadDto data)
        {
            await _pdaCarLoadMgr.SubmitCarLoad(data);
        }
    }
}
