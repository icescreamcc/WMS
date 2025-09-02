using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model.Enum;
using Models.Model.Prod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ProductOffLine
{
    public class PDAUnstackMgr : DbOperationHandler
    {
        private readonly MessageService _messageService;

        public PDAUnstackMgr(Repository repository, MessageService messageService) : base(repository)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// 根据小车唯一码查询已下架的生产交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
            carSoleCode = carSoleCode.Trim();
            var data = await Repository.ClientDb.Queryable<ProdOrders>()
                  .InnerJoin<ProdOrderDetails>((o, d) => o.DeliverNo == d.DeliverNo)
                  .LeftJoin<ProdCacheWarehouseFlow>((o, d, f) => f.SourceOrderNo == d.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString() && f.FlowType == FlowType.In.ToString())
                  .LeftJoin<ProdUnstackBinQueue>((o, d, f, q)=>q.CarSoleCode == f.SourceOrderNo)
                  .LeftJoin<InvBin>((o, d, f, q, b) => b.BinId == f.BinId)
                  .Where((o, d, f, q, b) => d.CarSoleCode == carSoleCode)
                  .Select((o, d, f, q, b) => new CarLoadDto
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
                      UnstackBinId=q.UnstackBinId,
                      UnstackBinNo=q.UnstackBinNo,
                      CreateDate = f.CreateDate,
                      UnitName = o.UnitName,
                      Status = d.Status
                  })
                 .ToListAsync();
            if (data != null && data.Count > 0)
            {
                var orderInfo = data.First();
                if (orderInfo.Status != ProdOrderDetailStatus.PutOut.ToString())
                { 
                    throw new BusinessException($"当前交接单还未下架，不允许上架到拆垛机");
                }
            }
            else
            {
                throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
            }
            return data.First();
        }

        /// <summary>
        /// 上架拆垛机检查
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <param name="binNo"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> CheckOnUnstack(string carSoleCode, string binNo)
        {
            carSoleCode = carSoleCode.Trim();
            binNo = binNo.Trim();
            if (string.IsNullOrEmpty(carSoleCode) || string.IsNullOrEmpty(binNo))
            {
                throw new BusinessException("请扫描或输入小车唯一码、库位码");
            }
            else
            {
                var orderDetail = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(e => e.CarSoleCode == carSoleCode);
                if (orderDetail == null)
                {
                    throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
                }
                if (orderDetail.Status != ProdOrderDetailStatus.PutOut.ToString())
                {
                    throw new BusinessException($"当前交接单还未下架，不允许上架到拆垛机");
                }
            }
            var exists = await Repository.ClientDb.Queryable<ProdUnstackBinQueue>() 
                .Where(q=>q.CarSoleCode==carSoleCode&&q.UnstackBinNo==binNo)
                .AnyAsync();
            if (!exists)
            {
                throw new BusinessException($"请检查：当前小车唯一码和库位码不匹配");
            }
            return true;
        }

        /// <summary>
        /// 上架拆垛机提交 
        /// 修改队列中对应小车的数据状态：Putaway
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SubmitOnUnStack(CarLoadDto data)
        {
            var isVaild = await CheckOnUnstack(data.CarSoleCode, data.ScanBinNo);
            if (isVaild)
            {
                var curUnstack = await Repository.ClientDb.Queryable<ProdUnstackBinQueue>().SingleAsync(s => s.CarSoleCode == data.CarSoleCode);
                curUnstack.Status=ProdUnstackBinQueueStatus.Putaway.ToString();
                await Repository.ClientDb.Updateable(curUnstack).ExecuteCommandAsync();
                await _messageService.CreateMessage(data.CreateUser, $"发货型号{data.ConsignNum}已上架拆垛机", $"拆垛库位:{data.ScanBinNo}", MessageType.Unstack);
            } 
        }
    }
}
