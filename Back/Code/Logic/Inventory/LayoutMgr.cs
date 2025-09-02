using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Prod;
using NPOI.Util;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Inventory
{
    public class LayoutMgr: DataPermissionHandler
    { 

        private readonly IMapper _mapper;

        public LayoutMgr(Repository repository, IMapper mapper) : base(repository)
        { 
            _mapper = mapper;
        }

        public async Task<List<WarehouseElement>> GetElementByWarehouse(string warehouseId)
        {
            var shelfList = await Repository.ClientDb.Queryable<InvShelf>().Where(w => w.WarehouseId == warehouseId)
                .OrderBy(o=>o.Rank)
                .Select(s=>new WarehouseElement
                {
                    Id=s.ShelfId,
                    No=s.ShelfNo,
                    Name=s.ShelfName,
                    Rank=s.Rank,
                    Props=s.Property,
                    IsAbandon=s.IsAbandon,
                    Type= StorageUnitType.Shelf.ToString()
                }).ToListAsync();
            var binList = await Repository.ClientDb.Queryable<InvBin>().Where(w=>w.WarehouseId==warehouseId&&string.IsNullOrEmpty(w.ShelfId))
                .OrderBy(o=>o.Rank)
                 .Select(s => new WarehouseElement
                 {
                     Id = s.BinId,
                     No = s.BinNo,
                     Name = s.BinName,
                     Rank = s.Rank,
                     Status=s.Status,
                     Props = s.Property,
                     IsAbandon = s.IsAbandon,
                     Type = StorageUnitType.Bin.ToString()
                 }).ToListAsync();
           return shelfList.Concat(binList).ToList();
        }

        public async Task<List<WarehouseElement>> GetElementByShelf(string shelfId)
        {
            return await Repository.ClientDb.Queryable<InvBin>().Where(w => w.ShelfId == shelfId)
                .LeftJoin<InvShelf>((b,s)=>b.ShelfId==s.ShelfId)
                .OrderBy((b, s) => b.Rank)
                 .Select((b, s) => new WarehouseElement
                 {
                     Id = b.BinId,
                     No = b.BinNo,
                     Name = b.BinName,
                     Rank = b.Rank,
                     Status = b.Status,
                     Props=s.Property,
                     IsAbandon = s.IsAbandon,
                     Type = StorageUnitType.Bin.ToString()
                 }).ToListAsync();
        }

        public async Task<List<WarehouseElement>> GetWorkbinCells(int binId)
        {
            return await Repository.ClientDb.Queryable<InvWorkbinCell>()
                .LeftJoin<InvWorkbin>((wbc,wb)=>wbc.WorkbinId==wb.WorkbinId)
                .LeftJoin<InvWorkbinSpecification>((wbc, wb, wbs)=>wbs.SpecId==wb.SpecId)
                .Where((wbc, wb, wbs) => wb.BinId == binId)
                .Select((wbc, wb, wbs) => new WarehouseElement
                {
                    Id=wbc.CellId,
                    No= wbc.CellNo,
                    Name= wbc.CellNo,
                    Status = wbc.Status,
                    Props=wbs.Size,
                    Remark=wbs.LoadWeight,
                    Type = StorageUnitType.WorkbinCell.ToString()
                }).ToListAsync();
        }

        public async Task<BinStockInfo> GetStockInfoByBin(int binId)
        { 
          return await Repository.ClientDb.Queryable<InvBin>()  
                .InnerJoin<ProdCacheWarehouseFlow>((b, f) => b.BinId == f.BinId&&!f.IsStatistical) 
                .InnerJoin<ProdOrderDetails>((b, f, d) => f.SourceOrderNo == d.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString())
                .InnerJoin<ProdOrders>((b, f, d,o)=>o.DeliverNo==d.DeliverNo)
                .Where((b, f, d,o) => f.BinId == binId && (f.FlowType == FlowType.In.ToString() || f.FlowType == FlowType.Waiting.ToString()) && (b.Status == BinStatus.Full.ToString() || b.Status == BinStatus.Lock.ToString()))
                .Select<BinStockInfo>()
                .SingleAsync(); 
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
        public async Task UnlockBin(int binId,string userName)
        {
            var bin=await Repository.GetSingeAsync<InvBin>(binId);
            if(bin != null)
            {
                if(bin.Status != BinStatus.Lock.ToString())
                {
                    throw new BusinessException("当前库位已不在锁定状态");
                }
                var flow=await Repository.ClientDb.Queryable<ProdCacheWarehouseFlow>().SingleAsync(w=>w.BinId==binId&&w.FlowType==FlowType.Waiting.ToString());
                if(flow != null)
                {
                    Repository.ClientDb.Deleteable(flow).AddQueue();
                     
                    var detail=await Repository.ClientDb.Queryable<ProdOrderDetails>().SingleAsync(w=>w.CarSoleCode==flow.SourceOrderNo&&flow.SourceOrderType== SourceOrderType.Production.ToString());
                    detail.Status = ProdOrderDetailStatus.Allot.ToString();
                    Repository.ClientDb.Updateable(detail).AddQueue();

                    var orderDetails = await Repository.ClientDb.Queryable<ProdOrderDetails>().CountAsync(w => w.DeliverNo == detail.DeliverNo && w.CarSoleCode != detail.CarSoleCode&&w.Status== ProdOrderDetailStatus.Allot.ToString());
                    if (orderDetails == 0)
                    {
                        var order = await Repository.ClientDb.Queryable<ProdOrders>().SingleAsync(w => w.DeliverNo == detail.DeliverNo);
                        order.Status=ProdOrderStatus.Allot.ToString();
                        order.ModifyDate = DateTime.Now;
                        order.ModifyUser = userName;
                        Repository.ClientDb.Updateable(order).AddQueue();
                    }
                   
                }
                bin.Status=BinStatus.Free.ToString();
                Repository.ClientDb.Updateable(bin).AddQueue();
                await Repository.ClientDb.SaveQueuesAsync();
            }
        }
    }
}
