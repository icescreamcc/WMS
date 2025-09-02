using Logic.ProductOffLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Prod;
using Models.Model;
using System.Threading.Tasks;

namespace WebApi.Controllers.ProductOffLine
{ 
    public class ProductPackageController : AuthTokenController
    {
        private readonly ProductPackageMgr _productPackageMgr;

        public ProductPackageController(ProductPackageMgr productPackageMgr)
        {
            _productPackageMgr = productPackageMgr;
        }

        /// <summary>
        /// 配对信息分页查询
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="isPackage"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableModel<MatchingCarDto>> GetMatchingInfo(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, bool isPackage)
        {
            return await _productPackageMgr.GetMatchingInfo(pgSize,pgIndex, orderFiled, ConvertOrderType(orderType), searchKey, isPackage);
        }

        /// <summary>
        /// 包装完成提交
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="matchingCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task SubmitMatchPackage(string userName, string matchingCode)
        {
            await _productPackageMgr.SubmitMatchPackage(userName, matchingCode);
        }
    }
}
