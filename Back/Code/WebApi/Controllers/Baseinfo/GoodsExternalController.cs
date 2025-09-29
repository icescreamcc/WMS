using Logic.BaseInfo;
using Logic.LogicCommon;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Baseinfo;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Baseinfo
{
    public class GoodsExternalController : ExternalApiController
    {
        private readonly SelectOptionsService _selectOptionsServer;

        private readonly GoodsMgr _goodsMgr;

        public GoodsExternalController(SelectOptionsService selectOptionsServer, GoodsMgr goodsMgr)
        {
            _selectOptionsServer = selectOptionsServer;
            _goodsMgr = goodsMgr;
        }

        [HttpGet]
        [ExternalApi]
        public async Task<List<GoodsSimple>> GetGoodsByKeyAndClassify(string goodsClassify, int goodsClassifyId, string isSAP, string keyword, int limit = 40)
        {
            return await _selectOptionsServer.GetGoodsByKey(goodsClassify, goodsClassifyId, isSAP, keyword, limit);
        }

        [HttpGet]
        [ExternalApi]
        public async Task<List<GoodsExternalDto>> GetAllGoods(string goodsGroup)
        {
            return await _goodsMgr.GetAllGoods(goodsGroup);
        }

    }
}
