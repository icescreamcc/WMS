using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using Logic.LogicBase;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Prod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ProductOffLine
{
    public class PDAOrderQueryMgr : DbOperationHandler
    {
        public PDAOrderQueryMgr(Repository repository) : base(repository)
        {
        }

        public async Task<CarLoadDto> GetProductionOrderByCarCode(string code)
        {
            code = code.Trim();
            var data= await Repository.ClientDb.Queryable<ProdOrders>()
                 .InnerJoin<ProdOrderDetails>((o, d) => o.DeliverNo == d.DeliverNo)
                 .LeftJoin<ProdCacheWarehouseFlow>((o, d, f)=>f.SourceOrderNo==d.CarSoleCode&&f.SourceOrderType==SourceOrderType.Production.ToString()&&f.FlowType==FlowType.In.ToString())
                 .LeftJoin<InvBin>((o, d, f,b)=>b.BinId==f.BinId&&f.IsStatistical==false)
                 .Where((o, d, f, b) => d.CarSoleCode == code||d.DetailNo== code|| b.BinNo == code)
                   .Select((o, d, f, b) => new CarLoadDto
                   {
                       DetailNo = d.DetailNo,
                       ConsignNum = o.ConsignNum,
                       CarSoleCode = d.CarSoleCode,
                       DeliverNo = d.DeliverNo,
                       ProdctionTypeNo = o.ProdctionTypeNo,
                       CountByCar = o.CountByCar,
                       MatchingCount = o.MatchingCount,
                       Total = o.Total,
                       PlanTotalByCar = d.PlanTotalByCar,
                       ActualTotal = f.ActualTotal,
                       BinId = f.BinId,
                       BinNo = b.BinNo,
                       CreateDate = f.CreateDate,
                       UnitName = o.UnitName,
                       Status = d.Status
                   })
                 .SingleAsync();
            if (data== null)
            {
                var bin=await Repository.ClientDb.Queryable<InvBin>().SingleAsync(s=>s.BinNo== code);
                if (bin == null)
                {
                    throw new BusinessException("系统未查询到任何与该编码有关的信息，请检查是否正确");
                }
                return new CarLoadDto
                {
                    BinId = bin.BinId,
                    BinNo = bin.BinNo,
                    Status = bin.Status,
                    StatusDisplay = EnumHelper.GetDescFromEnumVal<BinStatus>(bin.Status)
                }; 
            } 
            data.StatusDisplay = EnumHelper.GetDescFromEnumVal<ProdOrderDetailStatus>(data.Status);
            return data;
        }

        /// <summary>
        /// 解除库位锁定
        /// 1.检查库位状态
        /// 2.检查是否在库存流水InvStockFlow中存在记录
        /// 3.删除库位对应的库存流水记录
        /// 4.修改生产订单明细状态：Allot
        /// 5.当所有生产订单明细都为Allot时，修改订单状态为：Allot
        /// 6.修改库位状态：Free
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task UnlockBin(string binNo, string userName)
        {
            var bin = await Repository.ClientDb.Queryable<InvBin>().SingleAsync(w=>w.BinNo==binNo);
            if (bin != null)
            {
                if (bin.Status != BinStatus.Lock.ToString())
                {
                    throw new BusinessException("当前库位已不在锁定状态");
                }
                var flow = await Repository.ClientDb.Queryable<ProdCacheWarehouseFlow>().SingleAsync(w => w.BinId == bin.BinId && w.FlowType == FlowType.Waiting.ToString());
                if (flow != null)
                {
                    Repository.ClientDb.Deleteable(flow).AddQueue();

                    var detail = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(w => w.CarSoleCode == flow.SourceOrderNo && flow.SourceOrderType == SourceOrderType.Production.ToString());
                    detail.Status = ProdOrderDetailStatus.Allot.ToString();
                    Repository.ClientDb.Updateable(detail).AddQueue();

                    var orderDetails = await Repository.ClientDb.Queryable<ProdOrderDetails>().CountAsync(w => w.DeliverNo == detail.DeliverNo && w.CarSoleCode != detail.CarSoleCode && w.Status == ProdOrderDetailStatus.Allot.ToString());
                    if (orderDetails == 0)
                    {
                        var order = await Repository.ClientDb.Queryable<ProdOrders>().SingleAsync(w => w.DeliverNo == detail.DeliverNo);
                        order.Status = ProdOrderStatus.Allot.ToString();
                        order.ModifyDate = DateTime.Now;
                        order.ModifyUser = userName;
                        Repository.ClientDb.Updateable(order).AddQueue();
                    }

                }
                bin.Status = BinStatus.Free.ToString();
                Repository.ClientDb.Updateable(bin).AddQueue();
                await Repository.ClientDb.SaveQueuesAsync();
            }
        }
    }
}
