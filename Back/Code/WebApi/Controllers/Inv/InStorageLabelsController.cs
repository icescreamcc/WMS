using Logic.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Baseinfo;
using Models.Model;
using System.Threading.Tasks;
using WebApi.Filter;
using Models.Model.Inv;
using System.Collections.Generic;

namespace WebApi.Controllers.Inv
{ 
    public class InStorageLabelsController : AuthTokenController
    {
        private readonly InStorageLabelsMgr _inStorageLabelsMgr;

        public InStorageLabelsController(InStorageLabelsMgr  inStorageLabelsMgr)
        {
            _inStorageLabelsMgr = inStorageLabelsMgr;
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
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<TableModel<GoodsSimple>> GetGoodsByKey(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId, string keyword)
        {
            return await _inStorageLabelsMgr.GetGoodsByKey(pgSize, pgIndex, orderFiled, orderType, goodsGroup, goodsClassifyId,  keyword);
        }

        /// <summary>
        /// 根据物品查询推荐入库货位
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<RecommendWorkbinCellDto>> GetWorkbinRecommend(string goodsId)
        {
            return await _inStorageLabelsMgr.GetWorkbinRecommend(goodsId);
        }

        /// <summary>
        /// 查询当前物品已扫码未提交的数据
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<InStorageLabelsDto>> GetUnSubmitCodes(string goodsId)
        {
           return await _inStorageLabelsMgr.GetUnSubmitCodes(goodsId);
        }

        [HttpGet]
        [Skip]
        public async Task<StorageBin> ScanBinCheck(string scanNo, string goodsId)
        {
            return await _inStorageLabelsMgr.ScanBinCheck(scanNo, goodsId);
        }

        /// <summary>
        /// 扫码记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SubmitScan(InStorageLabelsDto data)
        {
            await _inStorageLabelsMgr.SubmitScan(data);
        }

        /// <summary>
        /// 扫码提交
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SubmitCode(InStorageLabelSubmitDto data)
        {
            await _inStorageLabelsMgr.SubmitCode(data);
        }
    }
}
