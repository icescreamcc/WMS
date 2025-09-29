using Logic.ProductOffLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Prod;
using Models.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApi.Filter;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model.Enum;

namespace WebApi.Controllers.ProductOffLine
{ 

    /// <summary>
    /// 生产订单管理接口
    /// </summary>
    public class ProductionOrderController : AuthTokenController
    {
        private readonly ProductionOrderMgr _productionOrderMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        public ProductionOrderController(ProductionOrderMgr productionOrderMgr, SelectOptionsService selectOptionsServer)
        {
            _productionOrderMgr = productionOrderMgr;
            _selectOptionsServer = selectOptionsServer;
        }

        /// <summary>
        /// 生产订单分页查询
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableModel<ProdOrdersDto>> GetOrders(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _productionOrderMgr.GetOrders(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), searchKey);
        }

        /// <summary>
        /// 生产订单明细查询
        /// </summary>
        /// <param name="deliverNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<ProdOrdersDto> GetOrderDetail(string deliverNo)
        {
            return await _productionOrderMgr.GetOrderDetail(deliverNo);
        }

        /// <summary>
        /// 获取生产订单编辑所需的选项数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var units= await _selectOptionsServer.GetUnits(UnitType.Pack.ToString());
            var lines = await _selectOptionsServer.GetLines(BusinessConst.PlantNo);
            return new {UnitOptions=units,LineOptions=lines};
        } 

        /// <summary>
        /// 添加生产订单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加生产订单", LogType.Add)]
        public async Task AddOrder(ProdOrdersDto data)
        {
           await _productionOrderMgr.AddOrder(data);
        }

        /// <summary>
        /// 修改生产订单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改生产订单", LogType.Update)]
        public async Task UpdateOrder(ProdOrdersDto data)
        {
            await _productionOrderMgr.UpdateOrder(data);
        }

        /// <summary>
        /// 删除生产订单
        /// </summary>
        /// <param name="deliverNoArr"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除生产订单", LogType.Del)]
        public async Task DelOrder(string[] deliverNoArr)
        {
            await _productionOrderMgr.DelOrder(deliverNoArr);
        }

        /// <summary>
        /// 创建小车唯一码
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProdOrderDetailsDto>> CreateCarCode(int orderId)
        {
           return await _productionOrderMgr.CreateCarCode(orderId);
        }

        /// <summary>
        /// 修改打印日期
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet] 
        [Skip]
        public async Task UpdatePrintDate(int orderId)
        {
            await _productionOrderMgr.UpdatePrintDate(orderId);
        }

        /// <summary>
        /// 关闭生产订单
        /// </summary>
        /// <param name="deliverNo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("关闭生产订单", LogType.Update)]
        public async Task UpdateOrderClosed(string deliverNo, string userName)
        {
            await _productionOrderMgr.UpdateOrderClosed(deliverNo, userName);
        }
    }
}
