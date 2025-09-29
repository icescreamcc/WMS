using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using External.Log;

namespace Logic.Inventory
{
    public class StorageMgr: ApprovalHandler
    {
        private readonly SysArgsService _sysArgsHelper;

        private readonly MessageService _messageService;

        private readonly IFileStorage _fileStorage;

        private readonly LogHelper _logHelper;
        public StorageMgr(Repository repository, SysArgsService sysArgsHelper, MessageService messageService, LogHelper logHelper, IFileStorage fileStorage) : base(repository)
        {
            _sysArgsHelper = sysArgsHelper;
            _messageService = messageService;
            _fileStorage = fileStorage;
            _logHelper = logHelper;
            
        }

        /// <summary>
        /// 库存汇总分页查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public async Task<TableModel<Storage>> GetStorageList(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string warehouseId, string goodsGroup)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsClassifyName" : orderFiled; 
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<InvStorage>()
                  .InnerJoin<BaseGoods>((s, g) => s.GoodsId == g.GoodsId)
                  .LeftJoin<BaseType>((s, g, t) => t.TypeId == g.GoodsClassifyId)
                  .LeftJoin<BaseSuppliers>((s,g,t,p)=>p.SupplierName==g.Supplier)
                  .Where((s, g,t, p) => s.GoodsId.Contains(searchKey) || g.GoodsName.Contains(searchKey) || g.GoodsModel.Contains(searchKey) || g.GoodsNo.Contains(searchKey)  || g.GoodsProperty.Contains(searchKey) || g.Supplier.Contains(searchKey))
                  .Where((s, g, t, p) =>t.Group==goodsGroup)
                  .WhereIF(!string.IsNullOrEmpty(warehouseId), (s, g, t, p) => SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(d => d.GoodsId == s.GoodsId && d.WarehouseId == warehouseId).Any())
                  .Select((s, g, t, p) => new Storage
                  { 
                      StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud=>sud.GoodsId==g.GoodsId&&sud.UnitId==g.PackageUnitId && sud.Stock>0).Sum(sud=>sud.Stock),2),
                      MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId && sud.Stock > 0).Sum(sud => sud.Stock), 2),
                      MaxPackageStock= SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId && sud.Stock > 0).Sum(sud => sud.Stock),2) ,
                      LastStatisticsDate = s.LastStatisticsDate,
                      LastOperatorId = s.LastOperatorId,
                      LastOperatorName = s.LastOperatorName, 
                      GoodsId = s.GoodsId,
                      GoodsNo = g.GoodsNo,
                      GoodsName = g.GoodsName,
                      GoodsLevel = g.GoodsLevel,
                      GoodsModel = g.GoodsModel,
                      GoodsProperty = g.GoodsProperty,
                      GoodsClassifyName = t.TypeName,
                      SupplierNo=p.SupplierNo,
                      SupplierName=p.SupplierName,
                      PackageCount = g.PackageCount,
                      MaxPackageCount = g.MaxPackageCount,
                      PackageUnitName = g.PackageUnitName,
                      MinPackageUnitName = g.MinPackageUnitName,
                      MaxPackageUnitName = g.MaxPackageUnitName,
                      CostPrice = g.CostPrice, 
                      RefPurchPrice = g.RefPurchPrice,  
                      SafetyInventory = g.SafetyInventory, 
                      SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                      GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total); 
            var res = new TableModel<Storage>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 库存明细分页查询
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        public async Task<TableModel<StorageDetailDto>> GetStorageDetailList(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string warehouseId, string goodsGroup)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsClassifyName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<InvStorage>()
                  .InnerJoin<BaseGoods>((s, g) => s.GoodsId == g.GoodsId)
                  .InnerJoin<BaseType>((s, g, t) => t.TypeId == g.GoodsClassifyId)
                  .InnerJoin<InvStorageWarehouseDetail>((s, g, t, d) => d.GoodsId == s.GoodsId)
                  .LeftJoin<BaseUnits>((s,g,t,d,u)=> u.UnitId==d.UnitId)
                  .LeftJoin<InvWarehouse>((s, g, t, d, u,w)=>w.WarehouseId==d.WarehouseId)
                  .LeftJoin<InvShelf>((s, g, t, d, u,w,sh)=>sh.ShelfId==d.ShelfId)
                  .LeftJoin<InvBin>((s, g, t, d, u, w, sh,b)=>b.BinId==d.BinId)
                  .LeftJoin<InvWorkbinCell>((s, g, t, d, u, w, sh, b,c)=>c.CellId==d.WorkbinCellId)
                  .LeftJoin<BaseSuppliers>((s, g, t, d, u, w, sh, b, c,p)=>p.SupplierName==g.Supplier)
                  .Where((s, g, t, d, u, w, sh, b, c, p) => s.GoodsId.Contains(searchKey) || g.GoodsName.Contains(searchKey) || g.GoodsModel.Contains(searchKey) || g.GoodsNo.Contains(searchKey) || g.GoodsProperty.Contains(searchKey) || g.Supplier.Contains(searchKey))
                  .Where((s, g, t, d, u, w, sh, b, c,p) => t.Group == goodsGroup && d.Stock>0)  
                  .WhereIF(!string.IsNullOrEmpty(warehouseId), (s, g, t, d, u, w, sh, b, c,p)=>d.WarehouseId==warehouseId)
                  .Select((s, g, t, d, u, w, sh, b, c, p) => new StorageDetailDto
                  {
                      Stock=d.Stock, 
                      LastStatisticsDate = s.LastStatisticsDate,
                      LastOperatorId = s.LastOperatorId,
                      LastOperatorName = s.LastOperatorName,
                      GoodsId = s.GoodsId,
                      GoodsNo = g.GoodsNo,
                      GoodsName = g.GoodsName,
                      GoodsLevel = g.GoodsLevel,
                      GoodsModel = g.GoodsModel,
                      GoodsProperty = g.GoodsProperty,
                      SupplierName=p.SupplierName,
                      SupplierNo=p.SupplierNo,
                      GoodsClassifyName = t.TypeName,
                      PackageCount = g.PackageCount,
                      MaxPackageCount = g.MaxPackageCount,
                      PackageUnitName = g.PackageUnitName,
                      MinPackageUnitName = g.MinPackageUnitName,
                      MaxPackageUnitName = g.MaxPackageUnitName,
                      CostPrice = g.CostPrice,
                      RefPurchPrice = g.RefPurchPrice,
                      SafetyInventory = g.SafetyInventory,
                      SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                      WarehouseId=d.WarehouseId,
                      WarehouseName=w.WarehouseName,
                      ShelfId=d.ShelfId,
                      ShelfName=sh.ShelfName,
                      BinId=d.BinId,
                      BinName=b.BinName,
                      CellId=d.WorkbinCellId,
                      CellNo=c.CellNo,
                      UnitId=d.UnitId,
                      UnitName=u.UnitName
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<StorageDetailDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 库存流水分页查询
        /// </summary> 
        /// <param name="goodsId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="flowType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public async Task<TableModel<StorageFlowDetail>> GetStorageFlowList(string goodsId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey,string flowType, string dateStart, string dateEnd)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "OperateDate" : orderFiled;
            orderFiled = orderFiled == "goodsId" ? "g.GoodsId" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            flowType = flowType == "All" ? "" : flowType;
            var data = Repository.ClientDb.Queryable<InvStorageFlowDetail>()
                  .InnerJoin<BaseGoods>((f, g) => f.GoodsId == g.GoodsId)
                  .LeftJoin<BaseUnits>((f, g, u) => u.UnitId == f.UnitId)
                  .LeftJoin<InvWarehouse>((f, g, u,w)=>w.WarehouseId==f.WarehouseId)
                  .LeftJoin<InvShelf>((f, g, u, w,s)=>s.ShelfId==f.ShelfId)
                  .LeftJoin<InvBin>((f, g, u, w, s,b)=>b.BinId==f.BinId)
                  .LeftJoin<InvWorkbin>((f, g, u, w, s,b,wb)=>wb.WorkbinId==f.WorkbinId)
                  .LeftJoin<InvWorkbinCell>((f, g, u, w, s, b, wb,wbc)=>wbc.CellId==f.WorkbinCellId)
                  .Where((f, g, u, w, s, b, wb, wbc) => f.GoodsId==goodsId)
                  .Where((f, g, u, w, s, b, wb, wbc) => f.SourceOrderNo.Contains(searchKey)||f.OperatorName.Contains(searchKey) || w.WarehouseName.Contains(searchKey) || s.ShelfName.Contains(searchKey) || b.BinName.Contains(searchKey) || g.GoodsName.Contains(searchKey))
                  .Where((f, g, u, w, s, b, wb, wbc) => SqlFunc.ToDate(f.OperateDate) >= GetDateStart(dateStart) && SqlFunc.ToDate(f.OperateDate) <= GetDateEnd(dateEnd))
                  .WhereIF(!string.IsNullOrEmpty(flowType), (f, g, u, w, s, b, wb, wbc) => f.FlowType== flowType)
                  .Select<StorageFlowDetail>()
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(row =>
            {
                row.SourceStorageTypeDesc = EnumHelper.GetDescFromEnumVal<SourceStorageType>(row.SourceStorageType);
                row.FlowTypeDesc = EnumHelper.GetDescFromEnumVal<FlowType>(row.FlowType);
                row.SourceStorageSubTypeDesc = EnumHelper.GetDescFromEnumVal<SourceStorageSubType>(row.SourceStorageSubType);
            });
            var res = new TableModel<StorageFlowDetail>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 获取库存明细
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<List<StorageWarehouseDetail>> GetStorageDetails(string goodsId)
        {
            return await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
                .LeftJoin<InvWarehouse>((d,w)=>w.WarehouseId==d.WarehouseId)
                .LeftJoin<InvShelf>((d,w,s)=>s.ShelfId==d.ShelfId)
                .LeftJoin<InvBin>((d,w,s,b)=>b.BinId==d.BinId)
                .LeftJoin<InvWorkbin>((d, w, s, b, wb) => wb.WorkbinId == d.WorkbinId)
                .LeftJoin<InvWorkbinCell>((d, w, s, b, wb, wbc) => wbc.CellId == d.WorkbinCellId)
                .LeftJoin<BaseUnits>((d, w, s, b, wb, wbc,u) => u.UnitId == d.UnitId)
                .LeftJoin<BaseGoods>((d, w, s, b, wb, wbc, u,g)=>g.GoodsId==d.GoodsId)
                .Where((d, w, s, b, wb, wbc, u,g) => d.GoodsId == goodsId && d.Stock > 0)
                .Select<StorageWarehouseDetail>().ToListAsync();
        }
 

        /// <summary>
        /// 库存汇总计算
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task StorageStatistics(string userId, string userName)
        {
            //获取未统计的流水记录
            var flowData = await Repository.ClientDb.Queryable<InvStorageFlowDetail>() .Where(f => f.IsStatistics==false).ToListAsync();
            if (flowData?.Count > 0)
            {
                //获取已统计的库存信息
                var goodsId = flowData.Select(f => f.GoodsId).Distinct().ToList();
                var storageData = await Repository.ClientDb.Queryable<InvStorage>().Where(s=> goodsId.Contains(s.GoodsId)).ToListAsync(); 
                var storageDetailData = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(s => goodsId.Contains(s.GoodsId)).ToListAsync();
                //计算并修改库存信息 
                _setInvData(userId, userName, flowData, storageData, storageDetailData);
                //根据库存明细计算并修改物品当前库存单位的库存总量
                var goodsList= storageData.Select(s=>s.GoodsId).ToList();
                var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsList.Contains(w.GoodsId)).ToListAsync();
                foreach(var storage in storageData)
                {
                    var curGoods= goodsInfo.Single(s=>s.GoodsId==storage.GoodsId);
                    var totalStock = storageDetailData.Where(w => w.GoodsId == storage.GoodsId && w.UnitId == curGoods.SafetyInventoryUnitId).Sum(s => s.Stock);
                    storage.Stock = totalStock;
                } 
                var updateStorageData = storageData.Where(x => string.IsNullOrEmpty(x.Remark)).ToList();
                var addStorageData = storageData.Where(x => x.Remark=="1").ToList();
                var updateStorageDetailData = storageDetailData.Where(x => string.IsNullOrEmpty(x.Remark)).ToList();
                var addStorageDetailData = storageDetailData.Where(x => x.Remark == "1").ToList();
                if (updateStorageData?.Count > 0)
                {
                    Repository.ClientDb.Updateable(updateStorageData).AddQueue();
                }
                if (addStorageData?.Count > 0)
                {
                    addStorageData.ForEach(x => x.Remark = null);
                    Repository.ClientDb.Insertable(addStorageData).AddQueue();
                }
                if (updateStorageDetailData?.Count > 0)
                {
                    Repository.ClientDb.Updateable(updateStorageDetailData).AddQueue();
                }
                if (addStorageDetailData?.Count > 0)
                {
                    addStorageDetailData.ForEach(x => x.Remark = null);
                    Repository.ClientDb.Insertable(addStorageDetailData).AddQueue();
                } 
                Repository.ClientDb.Updateable(flowData).AddQueue();
                 await Repository.ClientDb.SaveQueuesAsync();
            } 
        }

        /// <summary>
        /// 物品单价计算
        /// 取所有的入库单，按物品ID分组，汇总入库数量和入库总价，单价=入库总价/入库数量
        /// </summary>
        /// <returns></returns>
        public async Task UnitPriceStatistics()
        {
            var data = await Repository.ClientDb.Queryable<InvStorageFlowDetail>().Where(w => w.FlowType == FlowType.In.ToString() && w.TotalPrice > 0)
                 .GroupBy(w => new { w.GoodsId }).Select(w => new
                 {
                     w.GoodsId,
                     TotalPrice = SqlFunc.AggregateSum(w.TotalPrice),
                     TotalQty = SqlFunc.AggregateSum(w.Quantity)
                 }).ToListAsync();
            if(data!=null&& data.Count > 0)
            {
                var goodsArr = data.Select(s => s.GoodsId).ToList();
                var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();
                foreach (var goods in goodsInfo)
                {
                    var curData = data.SingleOrDefault(s => s.GoodsId == goods.GoodsId);
                    if (curData != null)
                    {
                        goods.CostPrice = (float)(Math.Round(( curData.TotalPrice/ curData.TotalQty)));
                    }
                }
                _logHelper.LogInfo("StorageMgr.UnitPriceStatistics", "修改价格", null);
                await Repository.ClientDb.Updateable(goodsInfo).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 根据已存物品库存信息重新计算并赋值库存量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="flowData"></param>
        /// <param name="storageData"></param>
        /// <param name="storageDetailData"></param>
        private void _setInvData(string userId, string userName,List<InvStorageFlowDetail> flowData, List<InvStorage> storageData, List<InvStorageWarehouseDetail> storageDetailData )
        { 
            foreach (var flow in flowData)
            {
                var curStorageData = storageData.SingleOrDefault(g => g.GoodsId == flow.GoodsId);
                if (curStorageData == null)
                {
                    storageData.Add(new InvStorage
                    {
                        GoodsId = flow.GoodsId,
                        LastOperatorId = userId,
                        LastOperatorName = userName,
                        LastStatisticsDate = DateTime.Now.ToStringExtension(),
                        Remark = "1"
                    }); 
                }
                else
                {
                    curStorageData.LastStatisticsDate = DateTime.Now.ToStringExtension();
                    curStorageData.LastOperatorId = userId;
                    curStorageData.LastOperatorName = userName;
                } 
                var curStorageDetail = storageDetailData.Where(g => g.GoodsId == flow.GoodsId && g.UnitId == flow.UnitId && g.WarehouseId == flow.WarehouseId && g.BinId==flow.BinId && g.WorkbinCellId==flow.WorkbinCellId).SingleOrDefault();
                if (curStorageDetail!=null)
                { 
                    curStorageDetail.Stock += flow.Quantity;
                }
                else
                {
                    storageDetailData.Add(new InvStorageWarehouseDetail
                    {
                        GoodsId = flow.GoodsId,
                        Stock = flow.Quantity, 
                        UnitId = flow.UnitId,
                        WarehouseId = flow.WarehouseId,
                        ShelfId=flow.ShelfId,
                        BinId = flow.BinId,
                        WorkbinId = flow.WorkbinId,
                        WorkbinCellId = flow.WorkbinCellId,
                        Remark = "1"
                    });
                }
                flow.IsStatistics = true;
            }
        }

        /// <summary>
        /// 检查库存
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public async Task<bool> CheckStorage(List<OutStorageDetail> details)
        {
            var goodsId = details.Select(x => x.GoodsId).Distinct().ToList();
            var storageData = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(s => goodsId.Contains(s.GoodsId)).ToListAsync();
            foreach (var detail in details)
            {
                if (!storageData.Exists(s => s.GoodsId == detail.GoodsId && detail.WarehouseId == s.WarehouseId && detail.Quantity <= s.Stock && detail.UnitId == s.UnitId && detail.BinId == s.BinId && detail.WorkbinCellId==s.WorkbinCellId))
                {
                    throw new BusinessException($"{detail.GoodsName}库存不够,请检查出库数量、单位及货位是否选择正确");
                }
            }
            return true;
        }

        /// <summary>
        /// 检查库存
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public async Task<bool> CheckStorage(List<InvOutStorageDetail> details)
        {
            var goodsId = details.Select(x => x.GoodsId).Distinct().ToList();
            var storageData = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(s => goodsId.Contains(s.GoodsId)).ToListAsync();
            foreach (var detail in details)
            {
                if (!storageData.Exists(s => s.GoodsId == detail.GoodsId && detail.WarehouseId == s.WarehouseId && detail.Quantity <= s.Stock && detail.UnitId == s.UnitId && detail.BinId == s.BinId && detail.WorkbinCellId == s.WorkbinCellId))
                {
                    throw new BusinessException($"{detail.GoodsName}库存不够,请检查出库数量、单位及货位是否选择正确");
                }
            }
            return true;
        }

        /// <summary>
        /// 添加库存调拨,生成入库单和出库单
        /// </summary>
        /// <param name="allocationOrder"></param>
        /// <returns></returns>
        public async Task AddAllocationStorage(AllocationOrder data)
        {
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("保存失败,请添加调拨单明细"); 
            }
            foreach(var detail in data.Details)
            {
                if (detail.InBinId == detail.OutBinId && detail.InWorkbinCellId == detail.OutWorkbinCellId)
                {
                    throw new BusinessException($"保存失败,{detail.GoodsName}调拨出库与入库货位重复，无效的调拨");
                }
            }
            var inDetail = data.Details.Select(b => new InvInStorageDetail
            { 
                GoodsId = b.GoodsId,
                GoodsName = b.GoodsName,
                WarehouseId = data.InWarehouseId,
                ShelfId = b.InShelfId,
                BinId = b.InBinId,
                WorkbinId = b.InWorkbinId,
                WorkbinCellId = b.InWorkbinCellId,
                Quantity = b.Quantity,
                UnitId = b.UnitId
            }).ToList();
            var sameGoods = inDetail.GroupBy(d => new { d.GoodsId, d.WarehouseId, d.BinId, d.WorkbinCellId, d.UnitId }).Count();
            if (sameGoods != inDetail.Count)
            {
                throw new BusinessException("保存失败,同一物品在相同单位、货位下不允许多次添加");
            }
            //验证：指定的物品是否在盘点中
            var goodsArr = inDetail.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();
            foreach (var detail in inDetail)
            {
                var takeStockLockGoods = goodsInfo.Single(e => e.GoodsId == detail.GoodsId);
                if (takeStockLockGoods.IsTakeStockLock)
                {
                    throw new BusinessException($"保存失败，物品{takeStockLockGoods.GoodsName + takeStockLockGoods.GoodsModel}正在盘点中，暂停入库");
                }
            }
            //验证：指定的入库货位是否在盘点中
            var binArr = inDetail.Select(s => s.BinId).Distinct().ToArray();
            var binInfo = await Repository.ClientDb.Queryable<InvBin>().Where(w => binArr.Contains(w.BinId)).ToListAsync();
            foreach (var detail in inDetail)
            {
                var takeStockLockBin = binInfo.Single(e => e.BinId == detail.BinId);
                if (takeStockLockBin.IsTakeStockLock)
                {
                    throw new BusinessException($"保存失败，货位{takeStockLockBin.BinName}正在盘点中，暂停入库");
                }
            }
            //验证：本次入库明细中是否存在不同物品入库到相同货位或料箱
            var invBinIdArr = inDetail.Where(w => w.WorkbinCellId == 0).Select(s => s.BinId).Distinct().ToArray();
            if (invBinIdArr?.Length > 0)
            {
                foreach (var binId in invBinIdArr)
                {
                    var isVarietyStock = binInfo.Any(a => a.BinId == binId && a.IsVarietyStock == true);
                    if (!isVarietyStock)
                    {
                        var sameBinGoodsCount = inDetail.Where(d => d.BinId == binId).GroupBy(d => d.GoodsId).Count();
                        if (sameBinGoodsCount > 1)
                        {
                            throw new BusinessException("保存失败,调拨明细中存在不同物品选择了相同的入库货位，请选择其他推荐货位");
                        }
                    } 
                }
            }
            var invCellIdArr = inDetail.Where(w => w.WorkbinCellId > 0).Select(s => s.WorkbinCellId).Distinct().ToArray();
            if (invCellIdArr?.Length > 0)
            {
                foreach (var cellId in invCellIdArr)
                {
                    var parentBinId = inDetail.First(f => f.WorkbinCellId == cellId).BinId;
                    var isVarietyStock = binInfo.Any(a => a.BinId == parentBinId && a.IsVarietyStock == true);
                    if (!isVarietyStock)
                    {
                        var sameCellGoodsCount = inDetail.Where(d => d.WorkbinCellId == cellId).GroupBy(d => d.GoodsId).Count();
                        if (sameCellGoodsCount > 1)
                        {
                            throw new BusinessException("保存失败,调拨明细中存在不同物品选择了相同的入库料箱，请选择其他推荐料箱");
                        }
                    } 
                }
            }
            //验证：指定的料箱单元格是否已存在其他待入库的物品
            var waitInOrders = await Repository.ClientDb.Queryable<InvInStorageDetail>()
                .LeftJoin<InvInStorage>((d, s) => s.OrderNo == d.OrderNo)
                .Where((d, s) => s.Status == InStorageStatus.WaitInStorage.ToString())
                .Select((d, s) => new { d.GoodsId, d.BinId, d.WorkbinCellId }).Distinct().ToListAsync();
            if (waitInOrders.Count > 0)
            {
                if (invBinIdArr?.Length > 0)
                {
                    var waitInOrdersByBin = waitInOrders.Where(w => w.WorkbinCellId == 0).Distinct().ToList();
                    if (waitInOrdersByBin.Count > 0)
                    {
                        foreach (var detail in inDetail)
                        {
                            var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                            if (!isVarietyStock)
                            {
                                var exists = waitInOrdersByBin.Exists(e => e.GoodsId != detail.GoodsId && e.BinId == detail.BinId);
                                if (exists)
                                {
                                    var existBin = await Repository.GetSingeAsync<InvBin>(detail.BinId);
                                    throw new BusinessException($"保存失败,检测到货位{existBin.BinName}存在其他待入库的物品，请选择其他推荐货位");
                                }
                            } 
                        }
                    }
                }
                if (invCellIdArr?.Length > 0)
                {
                    var waitInOrdersByCell = waitInOrders.Where(w => w.WorkbinCellId > 0).Distinct().ToList();
                    if (waitInOrdersByCell.Count > 0)
                    {
                        foreach (var detail in inDetail)
                        {
                            var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                            if (!isVarietyStock)
                            {
                                var exists = waitInOrdersByCell.Exists(e => e.GoodsId != detail.GoodsId && e.WorkbinCellId == detail.WorkbinCellId);
                                if (exists)
                                {
                                    var existCell = await Repository.GetSingeAsync<InvWorkbinCell>(detail.BinId);
                                    throw new BusinessException($"保存失败,检测到料箱{existCell.CellNo}存在其他待入库的物品，请选择其他推荐料箱");
                                }
                            } 
                        }
                    }
                }
            }
            //验证：指定的料箱单元格是否已存放其他物品  
            if (invBinIdArr?.Length > 0)
            {
                var invInfo = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(w => invBinIdArr.Contains(w.BinId)).ToListAsync();
                if (invInfo?.Count > 0)
                {
                    foreach (var detail in inDetail)
                    {
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId == 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.Stock > 0);
                                if (exist)
                                {
                                    var existBin = await Repository.GetSingeAsync<InvBin>(detail.BinId);
                                    throw new BusinessException($"保存失败，货位{existBin.BinName}已存放其他物品");
                                }
                            }
                        } 
                    }
                }
            } 
            if (invCellIdArr?.Length > 0)
            {
                var invInfo = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(w => invCellIdArr.Contains(w.WorkbinCellId)).ToListAsync();
                if (invInfo?.Count > 0)
                {
                    foreach (var detail in inDetail)
                    {
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId > 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.WorkbinCellId == detail.WorkbinCellId && e.Stock > 0);
                                if (exist)
                                {
                                    var existCell = await Repository.GetSingeAsync<InvWorkbinCell>(detail.BinId);
                                    throw new BusinessException($"保存失败，料箱单元格{existCell.CellNo}已存放其他物品");
                                }
                            }
                        } 
                    }
                }
            }
            //验证：物品指定存放规格是否和指定的料箱匹配
            if (invCellIdArr?.Length > 0)
            {
                var invWorkbinArr = inDetail.Where(w => w.WorkbinId > 0).Select(s => s.WorkbinId).Distinct().ToArray();
                var workbinInfo = await Repository.ClientDb.Queryable<InvWorkbin>().Where(w => invWorkbinArr.Contains(w.WorkbinId)).ToListAsync();
                foreach (var detail in inDetail)
                {
                    var curGoods = goodsInfo.Single(s => s.GoodsId == detail.GoodsId);
                    if (curGoods.IsConstraintSpec)
                    {
                        var curWorkbin = workbinInfo.Single(s => s.WorkbinId == detail.WorkbinId);
                        if (curGoods.GoodsSpecificationId != curWorkbin.SpecId)
                        {
                            throw new BusinessException($"保存失败，物品{curGoods.GoodsName}指定存放规格与料箱{curWorkbin.WorkbinNo}规格不一致");
                        }
                    } 
                } 
            }
            //验证：指定的出库货位是否在盘点中
            var outDetail = data.Details.Select(b => new OutStorageDetail
            {
                GoodsId = b.GoodsId,
                GoodsName = b.GoodsName,
                WarehouseId = data.OutWarehouseId,
                ShelfId=b.OutShelfId, 
                BinId = b.OutBinId,
                WorkbinId = b.OutWorkbinId,
                WorkbinCellId = b.OutWorkbinCellId,
                Quantity = b.Quantity,
                UnitId = b.UnitId
            }).ToList(); 
            var outBinArr = outDetail.Select(s => s.BinId).Distinct().ToArray();
            var outBinInfo = await Repository.ClientDb.Queryable<InvBin>().Where(w => outBinArr.Contains(w.BinId)).ToListAsync();
            foreach (var detail in outDetail)
            {
                var takeStockLockBin = outBinInfo.Single(e => e.BinId == detail.BinId);
                if (takeStockLockBin.IsTakeStockLock)
                {
                    throw new BusinessException($"保存失败，货位{takeStockLockBin.BinName}正在盘点中，暂停出库");
                }
            }
            //计算并检查库存
            await StorageStatistics(data.CreateUserId, data.CreateUserName);
            var chkStorage = await CheckStorage(outDetail);
            if (chkStorage)
            {
                //设置入库、出库相关数据 
                await _setInStorage(data);
                await _setOutStorage(data);
                await Repository.ClientDb.SaveQueuesAsync();
            }
            var msgContent = $"{data.CreateUserName}提交了一份待确认的库存调拨单";
            var msgRemark = $"{string.Join(',', data.Details.Select(s => s.GoodsName))}";
            await _messageService.CreateMessage(data.CreateUserName, msgContent, msgRemark, MessageType.AllocationStorage);
        } 

        /// <summary>
        /// 入库单设置 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        private async Task _setInStorage(AllocationOrder data)
        {
            var curDate = DateTime.Now;
            var lastData = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo);
            var inStorageModel = new InvInStorage
            {
                OrderNo = GetPrimaryId("I", lastData),
                InStorageType = InStorageType.AllocationIn.ToString(),
                GoodsClassify = data.GoodsClassify,
                CreateDate = curDate,
                CreateUserId = data.CreateUserId,
                CreateUserName = data.CreateUserName,
                WarehouseId = data.InWarehouseId,
                Remark = data.Remark,
                Status = InStorageStatus.WaitInStorage.ToString(),
                ApprovalStatus = ApprovalStatus.NoApproval.ToString()
            };  
            Repository.ClientDb.Insertable(inStorageModel).AddQueue();
            //入库单明细
            var inStorageDetail = data.Details.Select(b => new InvInStorageDetail
            {
                OrderNo = inStorageModel.OrderNo,
                GoodsId = b.GoodsId,
                GoodsName = b.GoodsName,
                WarehouseId = data.InWarehouseId,
                ShelfId = b.InShelfId,
                BinId = b.InBinId,
                WorkbinId = b.InWorkbinId,
                WorkbinCellId = b.InWorkbinCellId,
                Quantity = b.Quantity,
                UnitId = b.UnitId
            }).ToList();
            Repository.ClientDb.Insertable(inStorageDetail).AddQueue(); 
        }
          
        /// <summary>
        /// 出库单设置(不走审批流)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        private async Task _setOutStorage(AllocationOrder data)
        {
            var curDate = DateTime.Now;
            //出库单 
            var lastData = await Repository.ClientDb.Queryable<InvOutStorage>().MaxAsync(x => x.OrderNo);
            var outStorageModel = new InvOutStorage
            {
                OrderNo = GetPrimaryId("O", lastData),
                OutStorageType = OutStorageType.AllocationOut.ToString(),
                GoodsClassify=data.GoodsClassify,
                CreateDate = curDate,
                CreateUserId = data.CreateUserId,
                CreateUserName = data.CreateUserName,
                WarehouseId = data.OutWarehouseId,
                Remark = data.Remark,
                Status = OutStorageStatus.WaitOutStorage.ToString(),
                ApprovalStatus = ApprovalStatus.NoApproval.ToString()
            }; 
            Repository.ClientDb.Insertable(outStorageModel).AddQueue();
            //出库单明细
            var outStorageDetail = data.Details.Select(b => new InvOutStorageDetail
            {
                OrderNo = outStorageModel.OrderNo,
                GoodsId = b.GoodsId,
                GoodsName = b.GoodsName,
                WarehouseId = data.OutWarehouseId,
                ShelfId = b.OutShelfId,
                BinId = b.OutBinId,
                WorkbinId = b.OutWorkbinId,
                WorkbinCellId = b.OutWorkbinCellId,
                Quantity = b.Quantity,
                UnitId = b.UnitId
            }).ToList();
            Repository.ClientDb.Insertable(outStorageDetail).AddQueue();
        }

        /// <summary>
        /// 库存汇总导出
        /// </summary>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public async Task<string> ExportStorage(string orderFiled, string orderType, string searchKey, string warehouseId, string goodsGroup)
        {
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            warehouseId = string.IsNullOrEmpty(warehouseId) ? "" : warehouseId;
            var param = new Dictionary<string, object>
            {
                { "@GoodsId", "%"+searchKey+"%" },
                { "@GoodsName", "%"+searchKey+"%" },
                { "@GoodsModel", "%"+searchKey+"%" },
                { "@GoodsNo", "%"+searchKey+"%" },
                { "@Supplier", "%"+searchKey+"%" }
            }; 
            string sql = $@"select *,ROUND(Query.成本单价*Query.标准包装单位库存,2) as 成本总价 from (
                                select 
                                s.GoodsId as 系统编码
                                ,g.GoodsNo as SAP编码
                                ,g.GoodsName  as 物品名称
                                ,g.GoodsModel  as 物品型号
                                ,t.TypeName  as 物品分类
                                ,g.Supplier as 供应商
                                ,p.SupplierNo as 供应商编码
                                ,g.SafetyInventory   as 安全库存
                                ,g.SafetyInventoryUnitName  as 安全库存单位
                                ,(select SUM(Stock) from InvStorageWarehouseDetail where GoodsId=s.GoodsId and UnitId=g.PackageUnitId GROUP BY GoodsId,UnitId) as 标准包装单位库存
                                ,g.PackageUnitName as 标准包装单位
                                ,(select SUM(Stock) from InvStorageWarehouseDetail where GoodsId=s.GoodsId and UnitId=g.MinPackageUnitId GROUP BY GoodsId,UnitId) as 最小包装单位库存
                                ,g.MinPackageUnitName as 最小包装单位
                                ,(select SUM(Stock) from InvStorageWarehouseDetail where GoodsId=s.GoodsId and UnitId=g.MaxPackageUnitId GROUP BY GoodsId,UnitId) as 最大包装单位库存 
                                ,g.MaxPackageUnitName as 最大包装单位
                                ,s.LastStatisticsDate as 最后汇总时间 
                                ,g.PriceUnitName as 价格单位
                                ,ROUND(g.CostPrice,2) as 成本单价      
                                from InvStorage s 
                                join BaseGoods g on g.GoodsId=s.GoodsId
                                left join BaseType t on t.TypeId=g.GoodsClassifyId
                                left join BaseSuppliers p on p.SupplierName=g.Supplier
                                where (g.GoodsId like @GoodsId or g.GoodsName like @GoodsName or g.GoodsModel like @GoodsModel or g.GoodsNo like @GoodsNo or g.Supplier like @Supplier) and 
                                t.Group ='{goodsGroup}' and
                                ( case when '{warehouseId}'='' then true else exists(select 1 from InvStorageWarehouseDetail swd where swd.GoodsId=s.GoodsId and swd.WarehouseId='{warehouseId}') end )  
                                ORDER BY {orderFiled} {orderType}
                                )Query";
            var queryData = await Repository.QueryBySqlAsync(sql, param);
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"库存信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx"; 
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel); 
            return fileUrl;
        }

        /// <summary>
        /// 库存明细导出
        /// </summary>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public async Task<string> ExportStorageDetail(string orderFiled, string orderType, string searchKey, string warehouseId, string goodsGroup)
        {
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            warehouseId = string.IsNullOrEmpty(warehouseId) ? "" : warehouseId;
            var param = new Dictionary<string, object>
            {
                { "@GoodsId", "%"+searchKey+"%" },
                { "@GoodsName", "%"+searchKey+"%" },
                { "@GoodsModel", "%"+searchKey+"%" },
                { "@GoodsNo", "%"+searchKey+"%" },
                { "@Supplier", "%"+searchKey+"%" }
            };
            string sql = $@"select   
                             s.GoodsId as 系统编码
                            ,g.GoodsNo as SAP编码
                            ,g.GoodsName  as 物品名称
                            ,g.GoodsModel  as 物品型号
                            ,t.TypeName  as 物品分类
                            ,g.SafetyInventory   as 安全库存
                            ,g.SafetyInventoryUnitName  as 安全库存单位
                            ,w.WarehouseName as 仓库 
                            ,sh.ShelfName as 货架 
                            ,b.BinName as 货位 
                            ,c.CellNo as 料箱编号
                            ,d.Stock as 库存量
                            ,u.UnitName as 库存单位 
                            ,s.LastStatisticsDate as 最后汇总时间       
                            from InvStorage s 
                            join BaseGoods g on g.GoodsId=s.GoodsId
                            join BaseType t on t.TypeId=g.GoodsClassifyId
                            join InvStorageWarehouseDetail d on d.GoodsId=s.GoodsId
                            left join BaseUnits u on u.UnitId=d.UnitId
                            left join InvWarehouse w on w.WarehouseId=d.WarehouseId
                            left join InvShelf sh on sh.ShelfId=d.ShelfId
                            left join InvBin b on b.BinId=d.BinId
                            left join InvWorkbinCell c on c.CellId=d.WorkbinCellId
                            where (g.GoodsId like @GoodsId or g.GoodsName like @GoodsName or g.GoodsModel like @GoodsModel or g.GoodsNo like @GoodsNo or g.Supplier like @Supplier) and 
                            t.Group ='{goodsGroup}' and ( case when '{warehouseId}'='' then true else d.WarehouseId='{warehouseId}' end ) 
                            ORDER BY {orderFiled} {orderType}";
            var queryData = await Repository.QueryBySqlAsync(sql, param);
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"库存信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
            return fileUrl;
        }

        /// <summary>
        /// 库存流水记录导出
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="orderFieled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="flowType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public async Task<string> ExportStorageFlow(string goodsId, string orderFieled, string orderType, string searchKey, string flowType, string dateStart, string dateEnd)
        {
            orderFieled = string.IsNullOrEmpty(orderFieled) ? "OperateDate" : orderFieled;
            orderFieled = orderFieled == "goodsId" ? "g.GoodsId" : orderFieled; 
            int ds =string.IsNullOrEmpty(dateStart)?0: int.Parse(DateTime.Parse(dateStart).ToString("yyyyMMdd"));
            int dn= string.IsNullOrEmpty(dateEnd) ? 99999999 : int.Parse(DateTime.Parse(dateEnd).ToString("yyyyMMdd"));
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var param = new Dictionary<string, object>
            {
                { "@GoodsId", goodsId },
                { "@ds", ds },
                { "@dn", dn },
                { "@SourceOrderNo", "%"+searchKey+"%" },
                { "@OperatorName", "%"+searchKey+"%" },
                { "@WarehouseName", "%"+searchKey+"%" },
                { "@ShelfName", "%"+searchKey+"%" },
                { "@BinName", "%"+searchKey+"%" } 
            };
            string sql = $@"select 
                            g.GoodsName as 名称
                            ,g.GoodsModel as 型号
                            ,(case f.FlowType when 'In' then '入库' else '出库' end ) as 类别
                            ,(case f.SourceStorageSubType 
                                when 'InitialIn' then '初期库存'
                                when 'PurchaseIn' then '采购入库'
                                when 'ProductIn' then '生产入库'
                                when 'LeaseIn' then '借用退还'
                                when 'ReturnIn' then '退货入库'
                                when 'AllocationIn' then '调拨入库'
                                when 'OtherIn' then '其他入库'
                                when 'SalesOut' then '销售出库'
                                when 'ReceiveOut' then '生产领用'
                                when 'LeaseOut' then '借用出库'
                                when 'AllocationOut' then '调拨出库'
                                when 'TakeStockIn' then '盘点入库'
                                when 'TakeStockOut' then '盘点出库'
                                when 'OtherOut' then '其他出库'
                                else f.SourceStorageSubType  end ) as 类型
                            ,f.Quantity as 数量
                            ,u.UnitName as 单位
                            ,f.OperatorName as 操作人
                            ,f.OperateDate as 操作时间
                            ,f.SourceOrderNo as 单据号
                            ,(case f.SourceStorageType 
                                when 'InStorage' then '入库单' 
                                when 'OutStorage' then '出库单' 
                                when 'Allocation' then '库存调拨' 
                                else f.SourceStorageType end) as 单据来源
                            from InvStorageFlowDetail f
                            join BaseGoods g on g.GoodsId=f.GoodsId
                            left join BaseUnits u on u.UnitId=f.UnitId
                            left join InvWarehouse w on w.WarehouseId=f.WarehouseId
                            left join InvShelf s on s.ShelfId=f.ShelfId
                            left join InvBin b on b.BinId=f.BinId
                            left join InvWorkbin wb on wb.WorkbinId=f.WorkbinId
                            left join InvWorkbinCell wbc on wbc.CellId=f.WorkbinId
                            where g.GoodsId=@GoodsId  and  f.DateDay>=@ds and f.DateDay<=@dn and (case when '{flowType}'='' then true else f.FlowType='{flowType}' end)
                            and (f.SourceOrderNo like @SourceOrderNo or f.OperatorName like @OperatorName or w.WarehouseName like @WarehouseName or s.ShelfName like @ShelfName or b.BinName like @BinName)
                            order by {orderFieled} {orderType}";
            var queryData = await Repository.QueryBySqlAsync(sql, param);
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"库存流水记录导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel); 
            return fileUrl;
        }
    }
}
