using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Prod;
using NPOI.OpenXml4Net.OPC;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ProductOffLine
{
    public class BufferWarehouseDashboardMgr : DbOperationHandler
    {
        private readonly IMapper _mapper;

        private readonly SysArgsService _sysArgsHelper;

        public BufferWarehouseDashboardMgr(Repository repository, IMapper mapper, SysArgsService sysArgsService) : base(repository)
        {
            _mapper = mapper;
            _sysArgsHelper = sysArgsService;
        }

        public async Task<TableModel<MatchingCarDto>> GetMatchingInfo(int pgSize,int pgIndex)
        {
            int total=0;
            var data=  Repository.ClientDb.Queryable<ProdMatchingCar>()
                 .InnerJoin<ProdOrders>((m, o) => m.DeliverNo == o.DeliverNo)
                 .LeftJoin<ProdOrderDetails>((m,o,d)=>d.CarSoleCode == m.CarSoleCode)
                 .LeftJoin<ProdUnstackBinQueue>((m,o,d,q)=>q.CarSoleCode==d.CarSoleCode)
                 .OrderBy((m, o, d, q) => new { m.ConsignNum, m.CarSoleCode }).OrderByDescending(m => m.MatchingCode)
                 .Where((m, o, d, q) => !m.IsPackage&&m.IsMatch)
                 .Select((m, o, d, q) =>new MatchingCarDto
                 {
                     MatchingId =m.MatchingId,
                     DeliverNo =m.DeliverNo,
                     ConsignNum =m.ConsignNum,
                     Prodct =m.Prodct,
                     CountByCar =m.CountByCar,
                     MatchingCount =m.MatchingCount,
                     Total =m.Total,
                     TotalPutout =o.TotalPutout,
                     UnitName =o.UnitName,
                     CarSoleCode =m.CarSoleCode,
                     MatchingNum =m.MatchingNum,
                     MatchingCode =m.MatchingCode,
                     PlanTotalByCar =m.PlanTotalByCar,
                     ActualTotalByCar =m.ActualTotalByCar,
                     BinId =m.BinId,
                     BinNo =m.BinNo,
                     UnstackBinId =q.UnstackBinId,
                     UnstackBinNo =q.UnstackBinNo,
                     IsPackage =m.IsPackage,
                     IsMatch = m.IsMatch,
                     MatchDate =m.MatchDate,
                     PackageDate =m.PackageDate,
                     Status =d.Status
                 }) 
                 .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<MatchingCarDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<List<WarehouseElement>> GetBufferWarehouseElement()
        {
            var args = await _sysArgsHelper.GetValueByKey(BusinessConst.ProductBufferWarehouse); 
            var bufferWarehouseType = args.Value.ToString();
            return await Repository.ClientDb.Queryable<InvBin>()
             .InnerJoin<InvWarehouse>((b, w) => b.WarehouseId == w.WarehouseId) 
             .LeftJoin<ProdCacheWarehouseFlow>((b,w,f)=>f.BinId==b.BinId && !f.IsStatistical && SqlFunc.Subqueryable<ProdOrderDetails>().Where(od=>od.CarSoleCode==f.SourceOrderNo&&f.SourceOrderType==SourceOrderType.Production.ToString()&&od.Status==ProdOrderDetailStatus.PutOut.ToString()).NotAny())
             .Where((b, w,f) => w.WarehouseType == bufferWarehouseType)
             .OrderBy((b, w,f) => b.Rank)
             .Select((b, w,f) => new WarehouseElement {
                 Id = b.BinId,
                 No = b.BinNo,
                 Name = b.BinName,
                 Rank = b.Rank,
                 Status = b.Status,
                 Props = b.Property,
                 IsAbandon = b.IsAbandon,
                 Product=f.Product,
                 Code=f.SourceOrderNo,
                 Total=f.ActualTotal,
                 Type = StorageUnitType.Bin.ToString()
             })
             .ToListAsync();
        }

        public async Task<BinStockInfo> GetStockInfoByBin(int binId)
        { 
            return await Repository.ClientDb.Queryable<InvBin>()
              .InnerJoin<ProdCacheWarehouseFlow>((b, f) => b.BinId == f.BinId && !f.IsStatistical)
              .InnerJoin<ProdOrderDetails>((b, f, d) => f.SourceOrderNo == d.CarSoleCode && f.SourceOrderType == SourceOrderType.Production.ToString())
              .InnerJoin<ProdOrders>((b, f, d, o) => o.DeliverNo == d.DeliverNo)
              .Where((b, f, d, o) => f.BinId == binId && (f.FlowType == FlowType.In.ToString() || f.FlowType == FlowType.Waiting.ToString()) && (b.Status == BinStatus.Full.ToString() || b.Status == BinStatus.Lock.ToString()))
              .Select<BinStockInfo>()
              .SingleAsync();
        }
    }
}
