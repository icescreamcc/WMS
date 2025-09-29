using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Prod;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ProductOffLine
{
    /// <summary>
    /// PDA扫码装车服务类
    /// </summary>
    public class PDACarLoadMgr : DbOperationHandler
    {
        private readonly SysArgsService _sysArgsHelper;

        private readonly MessageService _messageService;

        public PDACarLoadMgr(Repository repository, SysArgsService sysArgsHelper, MessageService messageService) : base(repository)
        {
            _sysArgsHelper = sysArgsHelper;
            _messageService = messageService;
        }

        /// <summary>
        /// 根据小车唯一码查询已分配小车的交接单信息
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        public async Task<CarLoadDto> GetProductionOrderInfo(string carSoleCode)
        {
            carSoleCode = carSoleCode.Trim();
           var data= await Repository.ClientDb.Queryable<ProdOrders>()
                .InnerJoin<ProdOrderDetails>((o, d) => o.DeliverNo == d.DeliverNo)
                .Where((o, d) => d.CarSoleCode == carSoleCode)
                .Select((o, d) => new CarLoadDto
                {
                    DetailNo= d.DetailNo,
                    ConsignNum=o.ConsignNum,
                    CarSoleCode= d.CarSoleCode,
                    DeliverNo= d.DeliverNo,
                    ProdctionTypeNo= o.ProdctionTypeNo,
                    CountByCar= o.CountByCar,
                    MatchingCount= o.MatchingCount,
                    Total= o.Total,
                    PlanTotalByCar= d.PlanTotalByCar,
                    UnitName= o.UnitName,
                    Status= d.Status 
                }).SingleAsync();
            if(data != null)
            {
                if(data.Status != ProdOrderDetailStatus.Allot.ToString())
                {
                    var msg = EnumHelper.GetDescFromEnumVal<ProdOrderDetailStatus>(data.Status);
                    throw new BusinessException($"当前交接单{msg}，请更换其他交接单");
                }
            }
            else
            {
                throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
            }
            return data;
        }

        /// <summary>
        /// 根据小车唯一码查询推荐存放的缓存库位
        /// 小车码->查询订单信息->查询库位状态（是否存在空余库位）->是：查询该订单的库存流水记录的最后一车存放库位->荐相邻库位
        /// </summary>
        /// <param name="carSoleCode"></param>
        /// <returns></returns>
        public async Task<BinSimple?> GetFreeBin(string carSoleCode)
        {
            carSoleCode = carSoleCode.Trim();
            if (string.IsNullOrEmpty(carSoleCode))
            {
                throw new BusinessException("请扫描或输入小车唯一码");
            } 
            var orderDetail = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(e => e.CarSoleCode == carSoleCode);
            if (orderDetail == null)
            {
                throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
            } 
            if (orderDetail.Status!=ProdOrderDetailStatus.Allot.ToString())
            {
                var msg = EnumHelper.GetDescFromEnumVal<ProdOrderDetailStatus>(orderDetail.Status);
                throw new BusinessException($"该交接单{msg},请更换交接单");
            }
            var args = await _sysArgsHelper.GetValueByKey(BusinessConst.ProductBufferWarehouse);
            if(args.Value==null)
            {
                throw new BusinessException("请先在系统参数中指定成品缓存仓库");
            }
            var bufferWarehouseType = args.Value.ToString();
            //查询缓存仓闲置的库位
            var binInfo = await Repository.ClientDb.Queryable<InvBin>()
                .InnerJoin<InvWarehouse>((b, w) => b.WarehouseId == w.WarehouseId)
                .Where((b, w) => w.WarehouseType == bufferWarehouseType && b.Status == BinStatus.Free.ToString()&&!b.IsAbandon)
                .OrderBy((b, w) => b.Rank)
                .Select((b, w) => new BinSimple  {BinId=  b.BinId, BinNo= b.BinNo, Rank= b.Rank }) 
                .ToListAsync();
            if(binInfo?.Count==0)
            {
                throw new BusinessException("当前没有空余的缓存仓库位");
            }
            else if(binInfo?.Count==1)
            {
                return binInfo[0];
            }
            //查询当前订单所有的入库信息
            var lastFlowBin = await Repository.ClientDb.Queryable<ProdCacheWarehouseFlow>()
                 .InnerJoin<InvWarehouse>((f, w) => f.WarehouseId == w.WarehouseId)
                 .InnerJoin<ProdOrderDetails>((f, w, d) => d.CarSoleCode == f.SourceOrderNo)
                 .Where((f, w, d) => f.SourceOrderNo == carSoleCode && f.FlowType == FlowType.In.ToString() && f.SourceOrderType == SourceOrderType.Production.ToString() && w.WarehouseType == bufferWarehouseType)
                 .OrderByDescending((f, w, d) => f.FlowId)
                 .Select((f, w, d) => f.BinId)
                 .FirstAsync();
            if (lastFlowBin == 0)
            {
                //第一次入库
                return binInfo?.First();
            }
            else
            {
                //后续入库推荐与上一次入库相邻的库位
                return binInfo?.Where(w=>w.BinId!= lastFlowBin).FirstOrDefault();
            }
        }

        /// <summary>
        /// 锁定库位
        /// 修改库位状态：Lock
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task LockBin(CarLoadDto data)
        { 
            if (data.BinId == 0)
            {
                throw new BusinessException("请先查询推荐存放的库位");
            }
            var binEntity = await Repository.GetSingeAsync<InvBin>(data.BinId);
            if(binEntity.Status==BinStatus.Full.ToString())
            {
                throw new BusinessException($"库位{binEntity.BinNo}处于非空余状态，无法锁定");
            }
            binEntity.Status = BinStatus.Lock.ToString();
            await Repository.ClientDb.Updateable(binEntity).ExecuteCommandAsync(); 
        }

        /// <summary>
        /// 确认装车
        /// 锁定库位
        /// 修改生产订单的状态：CarLoading（已开始装车）
        /// 修改生产订单明细的状态：CarLoading（已装车）
        /// 插等待入库的流水记录：Waiting
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SubmitCarLoad(CarLoadDto data)
        {
            data.CarSoleCode = data.CarSoleCode.Trim();
            if (string.IsNullOrEmpty(data.CarSoleCode))
            {
                throw new BusinessException("请扫描或输入小车唯一码");
            }
            else
            {
                var orderDetail = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(e => e.CarSoleCode == data.CarSoleCode);
                if (orderDetail == null)
                {
                    throw new BusinessException("系统未查询到该小车唯一码，请检查是否正确");
                }
                if (orderDetail.Status != ProdOrderDetailStatus.Allot.ToString())
                {
                    var msg = EnumHelper.GetDescFromEnumVal<ProdOrderDetailStatus>(orderDetail.Status);
                    throw new BusinessException($"该交接单{msg},请更换交接单");
                }
            }
            if (data.ActualTotal == 0)
            {
                throw new BusinessException("请输入实际装车数量");
            }
            else
            {
                if (data.ActualTotal > data.Total)
                {
                    throw new BusinessException("输入有误，实际装车数量已超过了生产数量");
                }
            }
            if (data.BinId == 0)
            {
                throw new BusinessException("请先查询推荐存放的库位");
            }
            var binEntity = await Repository.GetSingeAsync<InvBin>(data.BinId);
            if (binEntity.Status != BinStatus.Free.ToString())
            {
                throw new BusinessException($"库位{binEntity.BinNo}处于非空闲状态，无法锁定上架");
            }
            binEntity.Status = BinStatus.Lock.ToString();
            Repository.ClientDb.Updateable(binEntity).AddQueue();

            var orderEntity=await Repository.ClientDb.Queryable<ProdOrders>().SingleAsync(s=>s.DeliverNo==data.DeliverNo);
            orderEntity.Status=ProdOrderStatus.CarLoading.ToString();
            orderEntity.ModifyDate=DateTime.Now;
            orderEntity.ModifyUser = data.CreateUser;
            Repository.ClientDb.Updateable(orderEntity).AddQueue();

            var detailEntity = await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(s => s.CarSoleCode == data.CarSoleCode);
            detailEntity.Status= ProdOrderDetailStatus.CarLoading.ToString();
            Repository.ClientDb.Updateable(detailEntity).AddQueue();

            
            var flowEntity = new ProdCacheWarehouseFlow
            {
                FlowType = FlowType.Waiting.ToString(),
                ActualTotal = data.ActualTotal,
                SourceOrderNo = data.CarSoleCode,
                SourceOrderType = SourceOrderType.Production.ToString(),
                WarehouseId = binEntity.WarehouseId,
                ShelfId = binEntity.ShelfId,
                BinId = binEntity.BinId,
                Product = data.ProdctionTypeNo,
                CreateDate = DateTime.Now,
                CreateUser = data.CreateUser
            };  
            Repository.ClientDb.Insertable(flowEntity).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            await _messageService.CreateMessage(data.CreateUser, $"发货型号{data.ConsignNum}已装车", $"小车唯一码：{data.CarSoleCode}", MessageType.CarLoading);
        }
    }
}
