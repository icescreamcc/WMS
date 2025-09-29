using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Prod;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ProductOffLine
{
    /// <summary>
    /// PDA扫码上架服务类
    /// </summary>
    public class PDAPutawayMgr : DbOperationHandler
    {
        private readonly MessageService _messageService;

        private readonly SysArgsService _sysArgsHelper;

        public PDAPutawayMgr(Repository repository, MessageService messageService, SysArgsService sysArgsService): base(repository)
        {
            _messageService = messageService;
            _sysArgsHelper= sysArgsService;
        }

        /// <summary>
        /// 根据小车唯一码查询已开始装车的生产交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
            carSoleCode = carSoleCode.Trim();
            var data = await Repository.ClientDb.Queryable<ProdOrders>()
                  .InnerJoin<ProdOrderDetails>((o, d) => o.DeliverNo == d.DeliverNo)
                  .LeftJoin<ProdCacheWarehouseFlow>((o, d, f) => f.SourceOrderNo == d.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString() && f.FlowType == FlowType.Waiting.ToString())
                  .LeftJoin<InvBin>((o, d, f, b) => b.BinId == f.BinId)
                  .Where((o, d, f, b) => d.CarSoleCode == carSoleCode)
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
                       ActualTotal=f.ActualTotal,
                       BinId = f.BinId,
                       BinNo=b.BinNo,
                       CreateDate = f.CreateDate,
                       UnitName = o.UnitName,
                       Status = d.Status
                   })
                  .ToListAsync();
            if (data != null&& data.Count>0)
            {
                var orderInfo = data.First();
                if (orderInfo.Status != ProdOrderDetailStatus.CarLoading.ToString())
                { 
                    if (orderInfo.Status == ProdOrderDetailStatus.Allot.ToString())
                    {
                        throw new BusinessException($"当前交接单还未装车，请更换其他交接单");
                    }
                    var msg = EnumHelper.GetDescFromEnumVal<ProdOrderDetailStatus>(orderInfo.Status);
                    throw new BusinessException($"当前交接单{msg}，请更换其他交接单");
                }
            }
            else
            {
                throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
            }
            return data.First();
        }

        /// <summary>
        /// 扫码上架
        /// 校验小车唯一码和库位码
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <param name="binNo"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> CheckPutaway(string carSoleCode,string binNo)
        {
            carSoleCode = carSoleCode.Trim();
            binNo= binNo.Trim();
            if (string.IsNullOrEmpty(carSoleCode)|| string.IsNullOrEmpty(binNo))
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
                if (orderDetail.Status != ProdOrderDetailStatus.CarLoading.ToString())
                {
                    var msg = EnumHelper.GetDescFromEnumVal<ProdOrderDetailStatus>(orderDetail.Status);
                    if (orderDetail.Status== ProdOrderDetailStatus.Allot.ToString())
                        throw new BusinessException($"该交接单还未装车,请更换交接单");
                    else
                        throw new BusinessException($"该交接单{msg},请更换交接单");
                }
            } 
            var existsFlow= await Repository.ClientDb.Queryable<ProdCacheWarehouseFlow>()
                .InnerJoin<InvBin>((f,b)=>f.BinId==b.BinId)
                .Where((f,b)=>f.SourceOrderNo==carSoleCode&&f.SourceOrderType== SourceOrderType.Production.ToString() && b.BinNo==binNo&&f.FlowType==FlowType.Waiting.ToString())
                .AnyAsync();
            if (!existsFlow)
            {
                throw new BusinessException($"当前库位码不是指定的入库库位");
            }
            return true;
        }

        /// <summary>
        /// 提交上架
        /// 修改生产订单状态：Putaway（开始上架）
        /// 修改生产订单明细状态：Putaway（已上架）
        /// 修改入库流水记录状态 ：In
        /// 修改库位状态：Full
        /// 小车配对
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SubmitPutaway(CarLoadDto data)
        {
            data.CarSoleCode = data.CarSoleCode.Trim();
            var isVaild = await CheckPutaway(data.CarSoleCode, data.ScanBinNo);
            if (isVaild)
            {
                var orderEntity = await Repository.ClientDb.Queryable<ProdOrders>().SingleAsync(s => s.DeliverNo == data.DeliverNo);
                orderEntity.Status = ProdOrderStatus.Putaway.ToString();
                orderEntity.ModifyDate = DateTime.Now;
                orderEntity.ModifyUser = data.CreateUser;
                Repository.ClientDb.Updateable(orderEntity).AddQueue();


                var detailEntity = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(s => s.CarSoleCode == data.CarSoleCode);
                detailEntity.Status = ProdOrderDetailStatus.Putaway.ToString();
                Repository.ClientDb.Updateable(detailEntity).AddQueue();

                var flowEntity = await Repository.ClientDb.Queryable<ProdCacheWarehouseFlow>().SingleAsync(f => f.SourceOrderNo == data.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString() && f.BinId == data.BinId && f.FlowType == FlowType.Waiting.ToString());
                flowEntity.FlowType = FlowType.In.ToString();
                flowEntity.CreateDate = DateTime.Now;
                flowEntity.CreateUser = data.CreateUser;
                Repository.ClientDb.Updateable(flowEntity).AddQueue();

                var binEntity = await Repository.ClientDb.Queryable<InvBin>().SingleAsync(s => s.BinNo == data.ScanBinNo);
                binEntity.Status = BinStatus.Full.ToString();
                Repository.ClientDb.Updateable(binEntity).AddQueue();
                var matchBins = await SetMatchCar(data);
                await Repository.ClientDb.SaveQueuesAsync();
                await _messageService.CreateMessage(data.CreateUser, $"发货型号{data.ConsignNum}已上架", $"小车唯一码：{data.CarSoleCode}", MessageType.Putaway);
                if (matchBins.Count > 0)
                { 
                    var binStr = string.Join(",", matchBins);
                    await _messageService.CreateMessage(data.CreateUser, $"发货型号{data.ConsignNum}配对成功，请前往缓存仓下架", $"库位:{binStr}", MessageType.Matching);
                } 
            }
        }


        /// <summary>
        /// 成品小车配对
        /// 配对逻辑规则：每个生产订单的第一车可以随机指定一个配对码，后续车次则根据订单指定的配对车数计配对计算，赋予与第一车相同的配对码，满足配对数后，后续车次开始重新指定一个配对码，以此类推
        /// 配对成功后修改已配对的订单明细状态：Matching（已配对）
        /// 配对成功后修改已配对信息IsMatch=true 
        /// 配对成功后查询并推荐拆垛机库位
        /// 拆垛机器排队逻辑规则：每个配对的第一车可以随机指定一个拆垛机器库位，则配对的后续车次都只能指定该库位且保持连续性，中间不能穿插其他车次
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> SetMatchCar(CarLoadDto data)
        {
            data.CarSoleCode = data.CarSoleCode.Trim();
            //查询当前生产订单未配对完成的数据
            var matchData = await Repository.ClientDb.Queryable<ProdMatchingCar>().Where(w => w.DeliverNo == data.DeliverNo&&!w.IsMatch &&!w.IsPackage).ToListAsync();
            var newCarMatch = new ProdMatchingCar
            {
                DeliverNo = data.DeliverNo,
                ConsignNum = data.ConsignNum,
                Prodct = data.ProdctionTypeNo,
                CountByCar = data.CountByCar,
                MatchingCount = data.MatchingCount,
                Total = data.Total,
                CarSoleCode = data.CarSoleCode,
                PlanTotalByCar = data.PlanTotalByCar,
                ActualTotalByCar = data.ActualTotal,
                BinId= data.BinId,
                BinNo = data.BinNo,
                MatchDate = DateTime.Now,
                MatchingNum = matchData.Count + 1
            };
           //写入拆垛机队列
            var unstackQueue = new ProdUnstackBinQueue
            {
                CarSoleCode=data.CarSoleCode,
                OrderNo=data.DeliverNo,
                CreateDate= DateTime.Now,
                Status=ProdUnstackBinQueueStatus.Booking.ToString()
            };
            //获取未完成配对的配对码，并赋值给当前车次的配对码，同时赋值与配对相同的拆垛机库位（只会存在一个未配对完成的配对码，这里仍然当一个List处理）
            var matchCarSoleCode = new List<string>();
            var matchCodeList = matchData.Select(s => s.MatchingCode).Distinct().ToList();  
            foreach (var code in matchCodeList)
            {
                var curMatch = matchData.Where(w => w.MatchingCode == code).ToList();
                if (curMatch.Count < data.MatchingCount)
                {
                    var firstMatch = curMatch.First();
                    newCarMatch.MatchingCode = firstMatch.MatchingCode;
                    var curQueue=await Repository.ClientDb.Queryable<ProdUnstackBinQueue>().Where(w => w.MatchingCode==firstMatch.MatchingCode).FirstAsync();
                    unstackQueue.UnstackBinId = curQueue.UnstackBinId;
                    unstackQueue.UnstackBinNo = curQueue.UnstackBinNo;
                    unstackQueue.MatchingCode = newCarMatch.MatchingCode;
                    matchCarSoleCode.AddRange(curMatch.Select(s => s.CarSoleCode));
                    matchCarSoleCode.Add(data.CarSoleCode);
                    break;
                }
            }
            //如果不存在未完成的配对，则指定一个新的配对码和新的拆垛机库位
            if (string.IsNullOrEmpty(newCarMatch.MatchingCode))
            {
                newCarMatch.MatchingCode = Guid.NewGuid().ToString("N").ToUpper();
                var unstackBin = await _getUnstackBin(data.DeliverNo);
                if(unstackBin != null)
                {
                    unstackQueue.UnstackBinId = unstackBin.BinId;
                    unstackQueue.UnstackBinNo = unstackBin.BinNo;
                    unstackQueue.MatchingCode = newCarMatch.MatchingCode;
                }
                else
                {
                    throw new BusinessException($"请在仓库管库位管理中创建拆垛机库位");
                }
                matchCarSoleCode.Add(data.CarSoleCode);
            }
            //插入拆垛机库位队列
            Repository.ClientDb.Insertable(unstackQueue).AddQueue();
            //如果加上当前车次后满足了配对小车数，则更新生产订单状态和配对数据的状态
            if (matchCarSoleCode.Count == data.MatchingCount)
            { 
                Repository.ClientDb.Updateable<ProdOrderDetails>().SetColumns(s => s.Status == ProdOrderDetailStatus.Matching.ToString()).Where(s => matchCarSoleCode.Contains(s.CarSoleCode)).AddQueue();
                Repository.ClientDb.Updateable<ProdMatchingCar>().SetColumns(s => s.IsMatch ==true).Where(s => matchCarSoleCode.Contains(s.CarSoleCode)).AddQueue();
                newCarMatch.IsMatch = true;
            }  
            //插入当前车次的配对信息
            Repository.ClientDb.Insertable(newCarMatch).AddQueue();  
            var binArr= new List<string>();
            if(matchCarSoleCode.Count==data.MatchingCount)
            {
                binArr = matchData.Select(s => s.BinNo).ToList();
                binArr.Add(data.BinNo);
            }
            return binArr;
        }

        /// <summary>
        /// 查询并推荐拆垛机库位 ProdUnstackBinQueue
        /// 1.优先指定未预约或已下架的库位 
        /// 2.当队列中所有库位都被预定时，寻找该订单号相同的库位排队，如果没有该订单的排队记录，则默认指定拆垛机第一个库位  
        /// 成品配对-预定拆垛库位-上架-下架（无法确定什么时候下架）
        /// </summary>
        /// <returns></returns>
        private async Task<BinSimple> _getUnstackBin(string deliverNo)
        {
            var unstackWarehouseType = (await _sysArgsHelper.GetValueByKey(BusinessConst.UnstackWarehouse)).Value.ToString();
             var binData= await Repository.ClientDb.Queryable<InvBin>()
                .InnerJoin<InvWarehouse>((b,w)=>b.WarehouseId==w.WarehouseId) 
                .Where((b,w)=>w.WarehouseType==unstackWarehouseType 
                && SqlFunc.Subqueryable<ProdUnstackBinQueue>().Where(sw => sw.UnstackBinId == b.BinId&&(sw.Status == ProdUnstackBinQueueStatus.Booking.ToString()|| sw.Status == ProdUnstackBinQueueStatus.Putaway.ToString())).NotAny() 
                ) 
                .OrderBy((b,w)=>b.Rank)
                .Select((b, w) => new BinSimple {BinId= b.BinId,BinNo= b.BinNo })
                .FirstAsync();
            if (binData==null)
            {
                binData=await Repository.ClientDb.Queryable<ProdUnstackBinQueue>()
                    .Where(w=>w.OrderNo==deliverNo)
                    .OrderBy(w=>w.CreateDate)
                    .Select(b => new BinSimple { BinId = b.UnstackBinId, BinNo = b.UnstackBinNo })
                    .FirstAsync();
                if (binData == null)
                {
                     binData = await Repository.ClientDb.Queryable<InvBin>()
                       .InnerJoin<InvWarehouse>((b, w) => b.WarehouseId == w.WarehouseId)
                       .Where((b, w) => w.WarehouseType == unstackWarehouseType )
                       .OrderBy((b, w) => b.Rank)
                       .Select((b, w) => new BinSimple { BinId = b.BinId, BinNo = b.BinNo })
                       .FirstAsync();
                }                
            }
            return binData;
        }
    }
}
