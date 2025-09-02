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
using System.Text;
using System.Threading.Tasks;

namespace Logic.ProductOffLine
{
    /// <summary>
    /// PDA扫码下架服务类
    /// </summary>
    public class PDAPutoutMgr : DbOperationHandler
    {
        private readonly MessageService _messageService;

        private readonly SysArgsService _sysArgsHelper;

        public PDAPutoutMgr(Repository repository, MessageService messageService, SysArgsService sysArgsHelper) : base(repository)
        {
            _messageService = messageService;
            _sysArgsHelper = sysArgsHelper;
        }

        /// <summary>
        /// 根据小车唯一码查询已配对的生产交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
            carSoleCode = carSoleCode.Trim();
            var data= await Repository.ClientDb.Queryable<ProdOrders>()
                  .InnerJoin<ProdOrderDetails>((o, d) => o.DeliverNo == d.DeliverNo)
                  .LeftJoin<ProdCacheWarehouseFlow>((o, d, f) => f.SourceOrderNo == d.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString() && f.FlowType == FlowType.In.ToString())
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
                      ActualTotal = f.ActualTotal,
                      BinId = f.BinId,
                      BinNo = b.BinNo,
                      CreateDate = f.CreateDate,
                      UnitName = o.UnitName,
                      Status = d.Status
                  })
                 .ToListAsync();
            if (data != null && data.Count > 0)
            {
                var orderInfo = data.First();
                if (orderInfo.Status != ProdOrderDetailStatus.Matching.ToString())
                { 
                    throw new BusinessException($"当前交接单不在配对状态，不允许下架");
                }
            }
            else
            {
                throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
            }
            return data.First(); 
        }

        /// <summary>
        /// 扫码下架
        /// 校验小车唯一码和库位码 
        /// 先进先出校验：根据小车唯一码查询配对表中MatchingNum的顺序
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <param name="binNo"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> CheckPutout(string carSoleCode, string binNo)
        {
            carSoleCode = carSoleCode.Trim();
            binNo = binNo.Trim();

            if (string.IsNullOrEmpty(carSoleCode) || string.IsNullOrEmpty(binNo))
            {
                throw new BusinessException("请扫描或输入小车唯一码、库位码");
            }
            var orderDetail = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(e => e.CarSoleCode == carSoleCode);
            if (orderDetail == null)
            {
                throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
            }
            if (orderDetail.Status != ProdOrderDetailStatus.Matching.ToString())
            {
                throw new BusinessException($"该车成品不在配对状态，不能执行下架操作");
            }
            var existsFlow = await Repository.ClientDb.Queryable<ProdCacheWarehouseFlow>()
                .InnerJoin<InvBin>((f, b) => f.BinId == b.BinId)
                .Where((f, b) => f.SourceOrderNo == carSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString() && b.BinNo == binNo && f.FlowType == FlowType.In.ToString())
                .AnyAsync();
            if (!existsFlow)
            {
                throw new BusinessException($"请检查：当前小车唯一码和库位码不匹配");
            }
            var args = await _sysArgsHelper.GetValueByKey(BusinessConst.IsFIFO); 
            var isFIFO = args == null ? false : bool.Parse(args.Value.ToString());
            if (isFIFO)
            {
                var details = await Repository.ClientDb.Queryable<ProdOrderDetails>().Where(w => w.DeliverNo == orderDetail.DeliverNo).OrderBy(o => o.CarSoleCode).ToListAsync();
                for (int i = 0; i < details.Count; i++)
                {
                    var curCar = details[i];
                    if (curCar.CarSoleCode == orderDetail.CarSoleCode)
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            var perCar = details[i - 1];
                            if (perCar.Status != ProdOrderDetailStatus.PutOut.ToString())
                            {
                                throw new BusinessException($"系统启用了先进先出，需要将前面的车次先下架");
                            }
                        }
                    } 
                }
            } 
            return true;
        }

        /// <summary>
        /// 提交下架
        /// 修改生产订单状态：PutOut（已开始下架）
        /// 修改生产订单已下架数量
        /// 修改生产订单明细状态：PutOut（已下架）
        /// 插入出库流水记录 :Out
        /// 修改当前小车码所有的库存流水记录状态IsStatistical=true
        /// 修改库位状态：Free
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SubmitPutout(CarLoadDto data)
        {
            data.CarSoleCode=data.CarSoleCode.Trim();
            var binEntity = await Repository.GetSingeAsync<InvBin>(data.BinId);
            var isVaild = await CheckPutout(data.CarSoleCode, binEntity.BinNo);
            if (isVaild)
            {
                var orderEntity = await Repository.ClientDb.Queryable<ProdOrders>().SingleAsync(s => s.DeliverNo == data.DeliverNo);
                orderEntity.Status = ProdOrderStatus.PutOut.ToString();
                orderEntity.ModifyDate = DateTime.Now;
                orderEntity.ModifyUser = data.CreateUser;
                orderEntity.TotalPutout += data.ActualTotal;
                Repository.ClientDb.Updateable(orderEntity).AddQueue(); 

                var detailEntity = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(s => s.CarSoleCode == data.CarSoleCode);
                detailEntity.Status = ProdOrderDetailStatus.PutOut.ToString();
                Repository.ClientDb.Updateable(detailEntity).AddQueue();

                var flowOut = new ProdCacheWarehouseFlow
                {
                    FlowType = FlowType.Out.ToString(),
                    ActualTotal = -data.ActualTotal,
                    SourceOrderNo = data.CarSoleCode,
                    SourceOrderType = SourceOrderType.Production.ToString(),
                    WarehouseId = binEntity.WarehouseId,
                    ShelfId = binEntity.ShelfId,
                    BinId = binEntity.BinId,
                    Product = data.ProdctionTypeNo,
                    CreateDate = DateTime.Now,
                    CreateUser = data.CreateUser,
                    IsStatistical=true 
                }; 
                Repository.ClientDb.Insertable(flowOut).AddQueue();

                var curCarFlowIn = await Repository.ClientDb.Queryable<ProdCacheWarehouseFlow>().SingleAsync(f => f.SourceOrderNo == data.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString() && f.BinId == data.BinId && f.FlowType == FlowType.In.ToString());
                curCarFlowIn.IsStatistical = true;
                Repository.ClientDb.Updateable(curCarFlowIn).AddQueue();

                binEntity.Status = BinStatus.Free.ToString();
                Repository.ClientDb.Updateable(binEntity).AddQueue();
                await Repository.ClientDb.SaveQueuesAsync();
                await _messageService.CreateMessage(data.CreateUser, $"发货型号{data.ConsignNum}已下架", $"小车唯一码：{data.CarSoleCode}", MessageType.PutOut);
            }
        }
    }
}
