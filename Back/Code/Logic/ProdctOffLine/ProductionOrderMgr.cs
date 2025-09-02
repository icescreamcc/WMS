using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Inv;
using Models.Model;
using Models.Model.Prod;
using Logic.LogicCommon;
using Models.Model.Enum;
using External.Common;
using Org.BouncyCastle.Asn1.X9;
using StackExchange.Redis;

namespace Logic.ProductOffLine
{
    public class ProductionOrderMgr: DbOperationHandler
    {
        private readonly IMapper _mapper;

        private readonly SysArgsService _sysArgsHelper;

        public ProductionOrderMgr(Repository repository, IMapper mapper, SysArgsService sysArgsHelper) : base(repository)
        {
            _mapper = mapper;
            _sysArgsHelper= sysArgsHelper;
        }

        public async Task<TableModel<ProdOrdersDto>> GetOrders(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var query = Repository.ClientDb.Queryable<ProdOrders>()
                .Where(w => w.DeliverNo.Contains(searchKey) || w.ProdctionTypeNo.Contains(searchKey) || w.Line.Contains(searchKey) || w.ProductName.Contains(searchKey))
                .OrderBy($"{orderFiled} {orderType}")
                .ToPageList(pgIndex, pgSize, ref total);
            var data = _mapper.Map<List<ProdOrdersDto>>(query);
            data.ForEach(x => x.StatusDesc = EnumHelper.GetDescFromEnumVal<ProdOrderStatus>(x.Status));
            var res = new TableModel<ProdOrdersDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<ProdOrdersDto> GetOrderDetail(string deliverNo)
        {
            var orderEnity = await Repository.ClientDb.Queryable<ProdOrders>().Where(o => o.DeliverNo == deliverNo).SingleAsync();
            var order=_mapper.Map<ProdOrdersDto>(orderEnity);
            var detailList= await Repository.ClientDb.Queryable<ProdOrderDetails>().Where(o => o.DeliverNo == deliverNo).ToListAsync();
            if(detailList?.Count > 0)
            { 
                order.Details =_mapper.Map<List<ProdOrderDetailsDto>>(detailList);
                order.Details.ForEach(x => x.StatusDesc = EnumHelper.GetDescFromEnumVal<ProdOrderDetailStatus>(x.Status));
            }
            order.StatusDesc = EnumHelper.GetDescFromEnumVal<ProdOrderStatus>(order.Status);
            return order; 
        }
          

        public async Task<List<KeyValueModel>> GetOrderStatus()
        {
            var data = EnumHelper.GetEnumValNames<ProdOrderStatus>();
            return await Task.FromResult(data);
        }

        public async Task AddOrder(ProdOrdersDto data)
        {
            var exist = await Repository.Exist<ProdOrders>(u => u.DeliverNo == data.DeliverNo);
            if (exist)
            {
                throw new BusinessException("保存失败,当前生产订单号已存在");
            }
            if (data.CountByCar % data.MatchingCount != 0)
            {
                throw new BusinessException("保存失败,小车配对数与需求车次不满足配对规则，请重新调整计划生产数量");
            }
            var entity = _mapper.Map<ProdOrders>(data); 
            entity.CreateDate = DateTime.Now;
            entity.Status=ProdOrderStatus.Create.ToString();
            await Repository.AddAsync(entity);
        }

        public async Task UpdateOrder(ProdOrdersDto data)
        {
            if(data.Status!= ProdOrderStatus.Create.ToString() && data.Status != ProdOrderStatus.Allot.ToString())
            {
                var msg = EnumHelper.GetDescFromEnumVal<ProdOrderStatus>(data.Status);
                throw new BusinessException($"保存失败,当前生产订单{msg},不允许修改");
            }
            var exist = await Repository.Exist<ProdOrders>(u => u.OrderId == data.OrderId);
            if (!exist)
            {
                throw new BusinessException("保存失败,当前生产订单号不存在或已删除");
            }
            exist = await Repository.Exist<ProdOrders>(u => u.DeliverNo == data.DeliverNo&&u.OrderId != data.OrderId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前生产订单号与其他订单号存在重复");
            }
            if (data.CountByCar % data.MatchingCount != 0)
            {
                throw new BusinessException("保存失败,小车配对数与需求车次不满足配对规则，请重新调整计划生产数量");
            }
            var entity = _mapper.Map<ProdOrders>(data);
            entity.ModifyDate = DateTime.Now;
            Repository.ClientDb.Updateable(entity).AddQueue();

            var oldDeliverNo = await Repository.ClientDb.Queryable<ProdOrders>().Where(w=>w.OrderId==data.OrderId).Select(w=>w.DeliverNo).SingleAsync();
             Repository.ClientDb.Updateable<ProdOrderDetails>().SetColumns(s => s.DeliverNo == data.DeliverNo).Where(w => w.DeliverNo == oldDeliverNo).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task DelOrder(string [] deliverNoArr)
        {
            var orders=await Repository.ClientDb.Queryable<ProdOrders>().Where(o => deliverNoArr.Contains(o.DeliverNo)).ToListAsync();
            foreach (var order in orders)
            {
                if(order.Status!=ProdOrderStatus.Create.ToString()&& order.Status != ProdOrderStatus.Allot.ToString())
                {
                    var msg = EnumHelper.GetDescFromEnumVal<ProdOrderStatus>(order.Status);
                    throw new BusinessException($"当前订单{msg}，不允许删除"); 
                }
            }
            Repository.ClientDb.Deleteable<ProdOrders>(d=> deliverNoArr.Contains(d.DeliverNo)).AddQueue();
            Repository.ClientDb.Deleteable<ProdOrderDetails>(d => deliverNoArr.Contains(d.DeliverNo)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
         
        public async Task<List<ProdOrderDetailsDto>> CreateCarCode(int orderId)
        {
            var entity = await Repository.GetSingeAsync<ProdOrders>(orderId);
            if (entity.Status != ProdOrderStatus.Create.ToString())
            {
                throw new BusinessException("创建失败,该生产订单已开始执行成品下线，无法再次创建");
            }
            var args = await _sysArgsHelper.GetValueByKey(BusinessConst.CarCodePrefix);
            var prefix=args.Value.ToString(); 
            if (entity.CountByCar == 0)
            {
                if(entity.Total == 0 || entity.TotalByCar == 0)
                {
                    throw new BusinessException("创建失败,生产订单的生产数量和装车数量必须大于0");
                }
                var val =(decimal)(entity.Total / entity.TotalByCar);
                entity.CountByCar = (int)Math.Ceiling(val);
            }
            var ts = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1).ToUniversalTime()).TotalMicroseconds;
            var details = new List<ProdOrderDetails>();
            var residueTotal=entity.Total; 
            for (int i=1;i<=entity.CountByCar;i++)
            { 
                var suffix = Math.Ceiling(ts)+ orderId+i;
                var detail = new ProdOrderDetails
                {
                    DeliverNo = entity.DeliverNo,
                    DetailNo = "TRANSIT" + suffix,
                    CarSoleCode=prefix+ suffix,
                    CarRank=i,
                    PlanTotalByCar= residueTotal>entity.TotalByCar? entity.TotalByCar: residueTotal,
                    Status= ProdOrderDetailStatus.Allot.ToString(),
                }; 
                details.Add(detail);
                residueTotal -= entity.TotalByCar;
            }
            entity.Status= ProdOrderStatus.Allot.ToString();
            entity.ModifyDate = DateTime.Now;
            Repository.ClientDb.Updateable(entity).AddQueue();
            Repository.ClientDb.Insertable(details).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            return _mapper.Map<List<ProdOrderDetailsDto>>(details);
        }
         

        public async Task UpdatePrintDate(int orderId)
        {
            var order = await Repository.GetSingeAsync<ProdOrders>(orderId);
            order.PrintDate = DateTime.Now;
            await Repository.ClientDb.Updateable(order).ExecuteCommandAsync();
        }

        /// <summary>
        /// 手动关闭订单（手动完成）
        /// 前提：1.缓存仓小车全部出库，2.配对全部完成包装
        /// </summary>
        /// <param name="deliverNo"></param>
        /// <returns></returns>
        public async Task UpdateOrderClosed(string deliverNo,string userName)
        {
            var existNotFlow = await Repository.ClientDb.Queryable<ProdOrderDetails>() 
                .InnerJoin<ProdCacheWarehouseFlow>((d, f) => f.SourceOrderNo == d.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString())
                .Where(( d, f) =>d.DeliverNo== deliverNo && f.IsStatistical == false).AnyAsync();
            if (existNotFlow)
            {
                throw new BusinessException("当前订单还有未下架的小车，无法关闭");
            }
            var isMatchNotPackage = await Repository.ClientDb.Queryable<ProdMatchingCar>().Where(w => w.DeliverNo == deliverNo && !w.IsPackage).AnyAsync();
            if (isMatchNotPackage)
            {
                throw new BusinessException("当前订单还有配对未完成的包装，无法关闭");
            }
            var order = await Repository.ClientDb.Queryable<ProdOrders>().SingleAsync(o => o.DeliverNo == deliverNo);
            order.Status = ProdOrderStatus.Closed.ToString();
            order.ModifyDate = DateTime.Now;
            order.ModifyUser = userName;
            Repository.ClientDb.Updateable(order).AddQueue(); 
            Repository.ClientDb.Updateable<ProdOrderDetails>().SetColumns(s => s.Status == ProdOrderDetailStatus.Closed.ToString()).Where(s =>s.DeliverNo== deliverNo&&s.Status != ProdOrderDetailStatus.Packed.ToString()).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
    }
}