using Logic.BaseInfo;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Baseinfo;
using Models.Model;
using System.Threading.Tasks;
using WebApi.Filter;
using System.Collections.Generic;
using Models.Model.EchartsModel;
using Models.Model.Enum;

namespace WebApi.Controllers.Baseinfo
{
    public class GoodsController : AuthTokenController
    {
        private readonly GoodsMgr _goodsMgr; 

        public GoodsController(GoodsMgr goodsMgr)
        {
            _goodsMgr = goodsMgr; 
        }

        /// <summary>
        /// 查询货品列表
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsGroup"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="shelfId"></param>
        /// <param name="binId"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableModel<GoodsSimple>> GetGoodsList(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId, string shelfId, int binId, string searchKey)
        {
            return await _goodsMgr.GetGoodsList(pgSize, pgIndex, orderFiled, orderType, goodsGroup, goodsClassifyId, shelfId, binId, searchKey);
        }
         
        /// <summary>
        /// 获取货品明细
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<GoodsDetailDto> GetGoodsDetail(string goodsId)
        {
            return await _goodsMgr.GetGoodsDetail(goodsId);
        }

        /// <summary>
        /// 修改货品图片
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="Photos"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateGoodsPhoto(string goodsId, List<FileInfoDto> Photos)
        {
            await _goodsMgr.UpdateGoodsPhoto(goodsId, Photos);
        }

        /// <summary>
        /// 获取货品分类关系图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<TreeMapModel>> GetGoodsTreeMapData(string classifyGroup)
        {
            return await _goodsMgr.GetGoodsTreeMapData(classifyGroup);
        }


    }
}
