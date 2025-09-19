using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Microsoft.VisualBasic;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Sys;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Inventory
{
    /// <summary>
    /// 出库业务处理类
    /// </summary>
   public class OutStorageMgr : ApprovalHandler
    {

        private readonly SysArgsService _sysArgsHelper;

        private readonly IFileStorage _fileStorage;

        private readonly StorageMgr _storageMgr;

        private readonly MessageService _messageService;

        private readonly SysArgsService _sysArgsService; 

        public OutStorageMgr(Repository repository, SysArgsService sysArgsHelper, IFileStorage fileStorage, StorageMgr storageMgr, MessageService messageService, SysArgsService sysArgsService) : base(repository)
        {
            _sysArgsHelper = sysArgsHelper;
            _fileStorage = fileStorage;
            _storageMgr = storageMgr;
            _messageService = messageService;
            _sysArgsService = sysArgsService;
        }

        /// <summary>
        /// 出库单分页查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public async Task<TableModel<OutStorage>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, string goodsGroup)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
            orderFiled = orderFiled == "remark" ? "i.remark" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim(); 
            var curApprover = await GetApprover(ApprovalDataType.OutStorage.ToString(), userId);
            var approvalModel = curApprover?.FirstOrDefault()?.ApprovalModel;
            var curApproverRank = curApprover?.Select(x => x.Rank).Distinct().ToList();
            var isAnyApproval = approvalModel == ApprovalModel.Any.ToString() && curApprover?.Count > 0;
            var data = Repository.ClientDb.Queryable<InvOutStorage>()
                  .LeftJoin<InvWarehouse>((i, w) => w.WarehouseId == i.WarehouseId)
                  .LeftJoin<InvOutStorageDetail>((i, w, d) => d.OrderNo == i.OrderNo)
                  .LeftJoin<BaseGoods>((i, w, d, g) => g.GoodsId == d.GoodsId)
                  .LeftJoin<BaseUnits>((i, w, d, g, b) => d.UnitId == b.UnitId)
                  .Where((i, w, d, g, b) =>
                    i.OrderNo.Contains(searchKey) ||
                    i.Remark.Contains(searchKey) ||
                    i.CreateUserName.Contains(searchKey) ||
                    g.GoodsName.Contains(searchKey) ||
                    g.GoodsNo.Contains(searchKey) ||
                    g.GoodsModel.Contains(searchKey))
                .Where((i, w, d, g, b) =>
                    i.GoodsClassify == goodsGroup &&
                    SqlFunc.ToDate(i.CreateDate) >= GetDateStart(dateStart) &&
                    SqlFunc.ToDate(i.CreateDate) <= GetDateEnd(dateEnd))
                  //.Where((i, w) => i.OrderNo.Contains(searchKey) || i.Remark.Contains(searchKey) || i.CreateUserName.Contains(searchKey) 
                  //|| SqlFunc.Subqueryable<InvOutStorageDetail>().InnerJoin<BaseGoods>((d,g)=>d.GoodsId==g.GoodsId).Where((d,g) => d.OrderNo == i.OrderNo && (d.GoodsName.Contains(searchKey)||g.GoodsNo.Contains(searchKey)||g.GoodsModel.Contains(searchKey))).Any())
                  //.Where((i, w) => i.GoodsClassify == goodsGroup && SqlFunc.ToDate(i.CreateDate) >= GetDateStart(dateStart) && SqlFunc.ToDate(i.CreateDate) <= GetDateEnd(dateEnd)) 
                  .Select((i, w, d, g, b) => new OutStorage
                  {
                      OrderNo = i.OrderNo,
                      OutStorageType = i.OutStorageType,
                      LineNo = i.LineNo,
                      GoodsClassify = i.GoodsClassify,
                      Status = i.Status,
                      CreateDate = i.CreateDate,
                      CreateUserId = i.CreateUserId,
                      CreateUserName = i.CreateUserName,
                      OutStorageDate = i.OutStorageDate,
                      WarehouseId = i.WarehouseId,
                      WarehouseName = w.WarehouseName,
                      Remark = i.Remark,
                      ApprovalStatus = i.ApprovalStatus,
                      ApprovalDate = i.ApprovalDate,
                      SourceOrderNo = i.SourceOrderNo,
                      GoodsName = g.GoodsName,
                      Quantity = d.Quantity,               // 计划数量
                      ActualQuantity = d.ActualQuantity,    // 实际数量
                      UnitName = b.UnitName,
                      ApprovalLastRank = SqlFunc.Subqueryable<ApprovalHis>().Where(h => h.PrimaryId == i.OrderNo && h.DataType == ApprovalDataType.OutStorage.ToString()).Max(h => h.ApprovalRank)
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(row =>
            {
                row.OutStorageTypeDesc = EnumHelper.GetDescFromEnumVal<OutStorageType>(row.OutStorageType);
                row.StatusDesc = EnumHelper.GetDescFromEnumVal<OutStorageStatus>(row.Status);
                row.GoodsClassifyDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(row.GoodsClassify);
                row.ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(row.ApprovalStatus);
                row.IsApproval = (isAnyApproval && row.Status == OutStorageStatus.Pending.ToString()) || (curApprover?.Count > 0 && curApproverRank.Contains(row.ApprovalLastRank + 1) && (row.Status == OutStorageStatus.Pending.ToString() || row.Status == OutStorageStatus.Approvaling.ToString()));
            });
            var res = new TableModel<OutStorage>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task<List<OutStorageDetail>> GetOrderDetail(string userId, string orderNo)
        {
            var data= await Repository.ClientDb.Queryable<InvOutStorageDetail>()
                .InnerJoin<BaseGoods>((i, g) => g.GoodsId == i.GoodsId)
                .LeftJoin<BaseUnits>((i, g, u) => u.UnitId == i.UnitId)
                 .LeftJoin<BaseType>((i, g, u, t) => t.TypeId == g.GoodsClassifyId)
                .LeftJoin<InvWarehouse>((i, g, u,t, w) => i.WarehouseId == w.WarehouseId)
                 .LeftJoin<InvShelf>((i, g, u, t, w, s) => s.ShelfId == i.ShelfId)
                .LeftJoin<InvBin>((i, g, u, t, w, s, b) => i.BinId == b.BinId)
                 .LeftJoin<InvWorkbin>((i, g, u, t, w, s, b, wb) => wb.WorkbinId == i.WorkbinId)
                .LeftJoin<InvWorkbinCell>((i, g, u, t, w, s, b, wb, wbc) => wbc.CellId == i.WorkbinCellId)
                .Where((i, g, u, t, w, s, b, wb, wbc) => i.OrderNo == orderNo)
                .Select((i, g, u, t, w, s, b, wb, wbc) => new OutStorageDetail
                {
                    WarehouseId = w.WarehouseId,
                    WarehouseNo = w.WarehouseNo,
                    WarehouseName = w.WarehouseName,
                    ShelfId = s.ShelfId,
                    ShelfNo = s.ShelfNo,
                    ShelfName = s.ShelfName,
                    BinId = b.BinId,
                    BinNo = b.BinNo,
                    BinName = b.BinName,
                    WorkbinId = wb.WorkbinId,
                    WorkbinNo = wb.WorkbinNo,
                    WorkbinCellId = wbc.CellId,
                    WorkbinCellNo = wbc.CellNo,
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    GoodsClassifyName = t.TypeName,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    Quantity = i.Quantity,
                    ActualQuantity = i.ActualQuantity,
                    UnitId = i.UnitId,
                    UnitName=u.UnitName,
                    OrderNo = i.OrderNo,
                    DetailId = i.DetailId,
                    Remark = i.Remark,
                    UnitPrice=g.CostPrice,
                    TotalPrice=SqlFunc.Round(g.CostPrice * i.Quantity,2),
                    PriceUnit=g.PriceUnitName,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).ToListAsync(); 
            return data;
        }

        /// <summary>
        /// 添加出库单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> AddOutStorage(OutStorage data)
        {
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("保存失败,请添加出库单明细");
            }
            var sameGoods = data.Details.GroupBy(d => new { d.GoodsId, d.WarehouseId, d.BinId, d.WorkbinCellId, d.UnitId }).Count();
            if (sameGoods != data.Details.Count)
            {
                throw new BusinessException("保存失败,同一物品在相同单位、货位下不允许多次添加"); 
            }
            //验证：指定的物品是否在盘点中
            var goodsArr = data.Details.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();
            foreach (var detail in data.Details)
            {
                var isTakeStockLock = goodsInfo.Exists(e => e.GoodsId == detail.GoodsId && e.IsTakeStockLock);
                if (isTakeStockLock)
                {
                    throw new BusinessException($"保存失败，物品{detail.GoodsName + detail.GoodsModel}正在盘点中，暂停出库");
                }
            }
            //验证：指定的货位是否在盘点中
            var binArr = data.Details.Select(s => s.BinId).Distinct().ToArray();
            var binInfo = await Repository.ClientDb.Queryable<InvBin>().Where(w => binArr.Contains(w.BinId)).ToListAsync();
            foreach (var detail in data.Details)
            {
                var isTakeStockLock = binInfo.Exists(e => e.BinId == detail.BinId && e.IsTakeStockLock);
                if (isTakeStockLock)
                {
                    throw new BusinessException($"保存失败，货位{detail.BinName}正在盘点中，暂停出库");
                }
            }
            //检查库存 
            await _storageMgr.StorageStatistics(data.CreateUserId, data.CreateUserName);
            var chkStorage = await _storageMgr.CheckStorage(data.Details);
            if (chkStorage)
            {
                //出库单 
                var lastData = await Repository.ClientDb.Queryable<InvOutStorage>().MaxAsync(x => x.OrderNo);
                var outStorageModel = new InvOutStorage
                {
                    OrderNo = GetPrimaryId("O", lastData),
                    SourceOrderNo = data.SourceOrderNo,
                    OutStorageType = data.OutStorageType,
                    LineNo = data.LineNo,
                    GoodsClassify = data.GoodsClassify,
                    CreateDate = DateTime.Now,
                    CreateUserId = data.CreateUserId,
                    CreateUserName = data.CreateUserName,
                    WarehouseId = data.WarehouseId,
                    Remark = data.Remark
                };
                //审批
                var isOutStorageApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsOutStorageApproval)).Value.ToString());
                if (isOutStorageApproval)
                {
                    //审批信息(如果当前创建人也是审批人,则需要修改和添加相应的审批信息) 
                    var process = await GetApprovalProcess(ApprovalDataType.OutStorage.ToString());
                    var curApprover = process.Where(a => a.ApproverId == data.CreateUserId).ToList();
                    if (curApprover?.Count > 0)
                    {
                        var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                        outStorageModel.ApprovalDate = DateTime.Now;
                        outStorageModel.ApproverId = data.CreateUserId;
                        outStorageModel.ApproverName = data.CreateUserName;
                        outStorageModel.ApproverRole = hightApprover.ApproverRole;
                        if (hightApprover.IsLastApproval)
                        {
                            outStorageModel.ApprovalStatus = ApprovalStatus.Approve.ToString();
                            outStorageModel.Status = OutStorageStatus.WaitOutStorage.ToString();
                        }
                        else
                        {
                            outStorageModel.Status = OutStorageStatus.Approvaling.ToString();
                            outStorageModel.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
                        }
                        var approvalHis = new ApprovalHis
                        {
                            ApprovalDate = DateTime.Now,
                            ApprovalModel = hightApprover.ApprovalModel,
                            ApprovalStatus = ApprovalStatus.Approve.ToString(),
                            ApproverId = data.CreateUserId,
                            ApproverName = data.CreateUserName,
                            ApproverRoleId = hightApprover.ApproverRole,
                            ApproverRoleName = hightApprover.ApproverRoleName,
                            DataType = ApprovalDataType.OutStorage.ToString(),
                            ApprovalRank = hightApprover.Rank,
                            PrimaryId = outStorageModel.OrderNo
                        };
                        Repository.ClientDb.Insertable(approvalHis).AddQueue();
                    }
                    else
                    {
                        outStorageModel.Status = OutStorageStatus.Pending.ToString();
                        outStorageModel.ApprovalStatus = ApprovalStatus.Pending.ToString();
                    }
                }
                else
                {
                    outStorageModel.Status = OutStorageStatus.WaitOutStorage.ToString();
                    outStorageModel.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
                }
                Repository.ClientDb.Insertable(outStorageModel).AddQueue();
                //出库单明细
                var outStorageDetail = data.Details
                    .Select(b => new InvOutStorageDetail
                    {
                        OrderNo = outStorageModel.OrderNo,
                        GoodsId = b.GoodsId,
                        GoodsName = b.GoodsName,
                        WarehouseId = b.WarehouseId,
                        ShelfId = b.ShelfId,
                        BinId = b.BinId,
                        WorkbinId = b.WorkbinId,
                        WorkbinCellId = b.WorkbinCellId,
                        Quantity = b.Quantity,
                        UnitId = b.UnitId,
                        Remark = b.Remark,
                        UnitPrice= goodsInfo.Single(s=>s.GoodsId== b.GoodsId).CostPrice,
                        TotalPrice= Math.Round(goodsInfo.Single(s => s.GoodsId == b.GoodsId).CostPrice * b.Quantity,2),
                        PriceUnit= goodsInfo.Single(s => s.GoodsId == b.GoodsId).PriceUnitName
                    })
                    .ToList();
                Repository.ClientDb.Insertable(outStorageDetail).AddQueue();
                await Repository.ClientDb.SaveQueuesAsync();
                var msgContent = $"{data.CreateUserName}提交了一份待确认的({EnumHelper.GetDescFromEnumVal<OutStorageType>(data.OutStorageType)})出库单";
                var msgRemark = $"{string.Join(',', data.Details.Select(s => s.GoodsName))}";
                await _messageService.CreateMessage(data.CreateUserName, msgContent, msgRemark, MessageType.OutStorage);
                return outStorageModel.OrderNo;
            }
            return "";
        }

        /// <summary>
        /// 修改出库单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateOutStorage(OutStorage data)
        {
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("保存失败,请添加出库单明细"); 
            }
            var oldOutStorage = await Repository.ClientDb.Queryable<InvOutStorage>().SingleAsync(u => u.OrderNo == data.OrderNo);
            if (string.IsNullOrEmpty(oldOutStorage?.OrderNo))
            {
                throw new BusinessException("保存失败,当前出库单号不存在或已删除"); 
            } 
            var isOutStorageApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsOutStorageApproval)).Value.ToString());
            if (oldOutStorage.Status== OutStorageStatus.OutStorage.ToString())
            {
                throw new BusinessException("保存失败,当前出库单出库，无法修改"); 
            }
            var sameGoods = data.Details.GroupBy(d => new { d.GoodsId, d.WarehouseId, d.BinId, d.WorkbinCellId, d.UnitId }).Count();
            if (sameGoods != data.Details.Count)
            {
                throw new BusinessException("保存失败,同一物品在相同单位、货位下不允许多次添加"); 
            }
            //验证：指定的物品是否在盘点中
            var goodsArr = data.Details.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();
            foreach (var detail in data.Details)
            {
                var isTakeStockLock = goodsInfo.Exists(e => e.GoodsId == detail.GoodsId && e.IsTakeStockLock);
                if (isTakeStockLock)
                {
                    throw new BusinessException($"保存失败，物品{detail.GoodsName + detail.GoodsModel}正在盘点中，暂停出库");
                }
            }
            //验证：指定的货位是否在盘点中
            var binArr = data.Details.Select(s => s.BinId).Distinct().ToArray();
            var binInfo = await Repository.ClientDb.Queryable<InvBin>().Where(w => binArr.Contains(w.BinId)).ToListAsync();
            foreach (var detail in data.Details)
            {
                var isTakeStockLock = binInfo.Exists(e => e.BinId == detail.BinId && e.IsTakeStockLock);
                if (isTakeStockLock)
                {
                    throw new BusinessException($"保存失败，货位{detail.BinName}正在盘点中，暂停出库");
                }
            }
            //计算并检查库存 
            await _storageMgr.StorageStatistics(data.CreateUserId, data.CreateUserName);
            var chkStorage = await _storageMgr.CheckStorage(data.Details);
            if (chkStorage)
            {
                //出库单  
                var outStorageModel = new InvOutStorage
                {
                    OrderNo = data.OrderNo,
                    SourceOrderNo = data.SourceOrderNo,
                    OutStorageType = data.OutStorageType,
                    LineNo = data.LineNo,
                    GoodsClassify = data.GoodsClassify,
                    CreateDate = oldOutStorage.CreateDate,
                    CreateUserId = oldOutStorage.CreateUserId,
                    CreateUserName = oldOutStorage.CreateUserName,
                    UpdateDate = DateTime.Now,
                    UpdateUserId = data.CreateUserId,
                    UpdateUserName = data.CreateUserName,
                    WarehouseId = data.WarehouseId,
                    Remark = data.Remark,
                };
                //审批
                if (isOutStorageApproval)
                {
                    //审批信息(如果当前修改人也是审批人,则需要修改和添加相应的审批信息)
                    Repository.ClientDb.Deleteable<ApprovalHis>(a => a.DataType == ApprovalDataType.OutStorage.ToString() && a.PrimaryId == data.OrderNo).AddQueue();
                    var process = await GetApprovalProcess(ApprovalDataType.OutStorage.ToString());
                    var curApprover = process.Where(a => a.ApproverId == data.CreateUserId).ToList();
                    if (curApprover?.Count > 0)
                    {
                        var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                        outStorageModel.ApprovalDate = DateTime.Now;
                        outStorageModel.ApproverId = data.CreateUserId;
                        outStorageModel.ApproverName = data.CreateUserName;
                        outStorageModel.ApproverRole = hightApprover.ApproverRole;
                        if (hightApprover.IsLastApproval)
                        {
                            outStorageModel.ApprovalStatus = ApprovalStatus.Approve.ToString();
                            outStorageModel.Status = OutStorageStatus.WaitOutStorage.ToString();
                        }
                        else
                        {
                            outStorageModel.Status = OutStorageStatus.Approvaling.ToString();
                            outStorageModel.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
                        }
                        var approvalHis = new ApprovalHis
                        {
                            ApprovalDate = DateTime.Now,
                            ApprovalModel = hightApprover.ApprovalModel,
                            ApprovalStatus = ApprovalStatus.Approve.ToString(),
                            ApproverId = data.CreateUserId,
                            ApproverName = data.CreateUserName,
                            ApproverRoleId = hightApprover.ApproverRole,
                            ApproverRoleName = hightApprover.ApproverRoleName,
                            DataType = ApprovalDataType.OutStorage.ToString(),
                            ApprovalRank = hightApprover.Rank,
                            PrimaryId = outStorageModel.OrderNo
                        };
                        Repository.ClientDb.Insertable(approvalHis).AddQueue();
                    }
                    else
                    {
                        outStorageModel.Status = OutStorageStatus.Pending.ToString();
                        outStorageModel.ApprovalStatus = ApprovalStatus.Pending.ToString();
                    }
                }
                else
                {
                    outStorageModel.Status = OutStorageStatus.WaitOutStorage.ToString();
                    outStorageModel.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
                }
                Repository.ClientDb.Updateable(outStorageModel).AddQueue();
                //出库单明细
                var outStorageDetail = data.Details.Select(b => new InvOutStorageDetail
                {
                    OrderNo = outStorageModel.OrderNo,
                    GoodsId = b.GoodsId,
                    GoodsName = b.GoodsName,
                    WarehouseId = b.WarehouseId,
                    ShelfId = b.ShelfId,
                    BinId = b.BinId,
                    WorkbinId = b.WorkbinId,
                    WorkbinCellId = b.WorkbinCellId,
                    Quantity = b.Quantity,
                    UnitId = b.UnitId,
                    Remark = b.Remark,
                    UnitPrice = goodsInfo.Single(s => s.GoodsId == b.GoodsId).CostPrice,
                    TotalPrice = Math.Round(goodsInfo.Single(s => s.GoodsId == b.GoodsId).CostPrice * b.Quantity, 2),
                    PriceUnit = goodsInfo.Single(s => s.GoodsId == b.GoodsId).PriceUnitName
                }).ToList();
                Repository.ClientDb.Deleteable<InvOutStorageDetail>(d => d.OrderNo == data.OrderNo).AddQueue();
                Repository.ClientDb.Insertable(outStorageDetail).AddQueue();
                await Repository.ClientDb.SaveQueuesAsync();
            }  
        }
          
        /// <summary>
        /// 删除出库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task DelOutStorage(string[] orderNo)
        {
            var check = await Repository.Exist<InvOutStorage>(e => orderNo.Contains(e.OrderNo) && e.Status == OutStorageStatus.OutStorage.ToString()); 
            if (check)
            {
                throw new BusinessException("删除失败,无法删除已出库的出库单"); 
            }
            //删除出库单
            Repository.ClientDb.Deleteable<InvOutStorage>(b => orderNo.Contains(b.OrderNo)).AddQueue();
            Repository.ClientDb.Deleteable<InvOutStorageDetail>(b => orderNo.Contains(b.OrderNo)).AddQueue();

            //删除领料单
            var requisitionOrders = await Repository.ClientDb.Queryable<InvOutStorage>()
                .Where(w => orderNo.Contains(w.OrderNo)&&w.OutStorageType== OutStorageType.ReceiveOut.ToString()&&!string.IsNullOrEmpty(w.SourceOrderNo))
                .Select(s => s.SourceOrderNo).ToListAsync();
            if(requisitionOrders?.Count>0)
            {
                Repository.ClientDb.Deleteable<InvRequisitionOrder>(d => requisitionOrders.Contains(d.OrderNo)).AddQueue();
                Repository.ClientDb.Deleteable<InvRequisitionOrderDetail>(d => requisitionOrders.Contains(d.OrderNo)).AddQueue();
            }
        
            //删除审批记录
            Repository.ClientDb.Deleteable<ApprovalHis>(a => a.DataType == ApprovalDataType.OutStorage.ToString() && orderNo.Contains(a.PrimaryId)).AddQueue(); 
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task UpdateActualQuantity(InvOutStorageDetail detail)
        {
            var isFullStorage = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(w => w.GoodsId == detail.GoodsId && w.BinId == detail.BinId && w.WorkbinCellId == detail.WorkbinCellId && w.Stock >= detail.ActualQuantity).AnyAsync();
            if (!isFullStorage)
            {
                throw new BusinessException("当前库存不满足实际出库数量");
            }
            var detailOutEntity = await Repository.ClientDb.Queryable<InvOutStorageDetail>()
                .SingleAsync(s => s.OrderNo == detail.OrderNo && s.GoodsId == detail.GoodsId && s.BinId == detail.BinId && s.WorkbinCellId == detail.WorkbinCellId);
            if (detailOutEntity != null)
            {
                detailOutEntity.Quantity = detail.ActualQuantity;
                detailOutEntity.ActualQuantity = detail.ActualQuantity;
                await Repository.ClientDb.Updateable(detailOutEntity).ExecuteCommandAsync();
                var outOrder = await Repository.GetSingeAsync<InvOutStorage>(detail.OrderNo);
                if (!string.IsNullOrEmpty(outOrder.SourceOrderNo)&&outOrder.OutStorageType==OutStorageType.ReceiveOut.ToString())
                {
                    var detilReqEnity = await Repository.ClientDb.Queryable<InvRequisitionOrderDetail>().Where(w => w.OrderNo == outOrder.SourceOrderNo && w.GoodsId == detail.GoodsId).SingleAsync();
                    if(detilReqEnity != null)
                    {
                        detilReqEnity.Quantity= detail.ActualQuantity;
                        detilReqEnity.ActualQuantity=detail.ActualQuantity;
                        await Repository.ClientDb.Updateable(detilReqEnity).ExecuteCommandAsync();
                    }
                } 
            } 
        }

        /// <summary>
        /// 出库确认
        /// </summary>
        /// <param name="ordersNo"></param>
        /// <returns></returns>
        public async Task ConfirmOutStorage(string ordersNo)
        {
            var order = await Repository.ClientDb.Queryable<InvOutStorage>().SingleAsync(u => ordersNo== u.OrderNo);
            var ordersDetail = await Repository.ClientDb.Queryable<InvOutStorageDetail>().Where(u => ordersNo == u.OrderNo).ToListAsync();
            foreach (var detail in ordersDetail)
            {
                if (detail.BinId == 0)
                {
                    throw new BusinessException("保存失败,请给出库明细指定出库货位");
                }
            }
            //验证：单据是否处于待出库状态
            if (order.Status != OutStorageStatus.WaitOutStorage.ToString())
            {
                throw new BusinessException("保存失败,当前单据不在待出库状态");
            }
            //验证：指定的物品是否在盘点中
            var goodsArr = ordersDetail.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();
            foreach (var detail in ordersDetail)
            {
                var takeStockLockGoods = goodsInfo.Single(e => e.GoodsId == detail.GoodsId);
                if (takeStockLockGoods.IsTakeStockLock)
                {
                    throw new BusinessException($"保存失败，物品{takeStockLockGoods.GoodsName + takeStockLockGoods.GoodsModel}正在盘点中，暂停出库");
                }
            }
            //验证：指定的货位是否在盘点中
            var binArr = ordersDetail.Select(s => s.BinId).Distinct().ToArray();
            var binInfo = await Repository.ClientDb.Queryable<InvBin>().Where(w => binArr.Contains(w.BinId)).ToListAsync();
            foreach (var detail in ordersDetail)
            {
                var takeStockLockBin = binInfo.Single(e => e.BinId == detail.BinId);
                if (takeStockLockBin.IsTakeStockLock)
                {
                    throw new BusinessException($"保存失败，货位{takeStockLockBin.BinName}正在盘点中，暂停出库");
                }
            }
            //计算并检查库存 
            await _storageMgr.StorageStatistics(order.CreateUserId, order.CreateUserName);
            await _storageMgr.CheckStorage(ordersDetail);
            //修改单据状态
            order.Status = OutStorageStatus.OutStorage.ToString();
            order.OutStorageDate = DateTime.Now;
            Repository.ClientDb.Updateable(order).AddQueue();
            //修改出库单明细实际出库数量 
            ordersDetail.ForEach(f =>
            {
                if (f.ActualQuantity == 0)
                {
                    f.ActualQuantity = f.Quantity;
                }
                f.TotalPrice=Math.Round(f.ActualQuantity*f.UnitPrice, 2);
            });
            Repository.ClientDb.Updateable(ordersDetail).AddQueue();
            //添加库存流水记录 
            var storageFlowDetailList = ordersDetail.Select(d => new InvStorageFlowDetail
            {
                FlowType = FlowType.Out.ToString(),
                GoodsId = d.GoodsId,
                OperateDate = DateTime.Now.ToStringExtension(),
                OperatorId = order.CreateUserId,
                OperatorName = order.CreateUserName,
                Quantity = -d.Quantity,
                SourceOrderNo = order.OrderNo,
                SourceStorageType = SourceStorageType.OutStorage.ToString(),
                SourceStorageSubType = order.OutStorageType,
                UnitId = d.UnitId,
                WarehouseId = d.WarehouseId,
                ShelfId = d.ShelfId,
                BinId = d.BinId,
                WorkbinId = d.WorkbinId,
                WorkbinCellId = d.WorkbinCellId,
                DateYear = int.Parse(DateTime.Now.ToString("yyyy")),
                DateMonth = int.Parse(DateTime.Now.ToString("yyyyMM")),
                DateDay = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                BatchNumber = long.Parse(DateTime.Now.ToStringNoSignExtension()),
                TotalPrice=d.TotalPrice,
                UnitPrice= d.UnitPrice,
                PriceUnit= d.PriceUnit,
                Remark=d.Remark,
                RemarkType=d.RemarkType,
            }).ToList();
            Repository.ClientDb.Insertable(storageFlowDetailList).AddQueue();
            //修改领料单状态 
            if (order.OutStorageType==OutStorageType.ReceiveOut.ToString() && !string.IsNullOrEmpty(order.SourceOrderNo))
            { 
                var requisitionOrder = await Repository.ClientDb.Queryable<InvRequisitionOrder>().SingleAsync(s => s.OrderNo == order.SourceOrderNo);
                if (requisitionOrder != null)
                {
                    var unitData = await Repository.ClientDb.Queryable<BaseUnits>().Where(w => w.UnitType == UnitType.Pack.ToString()).ToListAsync();
                    requisitionOrder.Status = RequisitionStatus.Delivered.ToString();
                    requisitionOrder.UpdateDate = DateTime.Now;
                    Repository.ClientDb.Updateable(requisitionOrder).AddQueue();
                    var requisitionOrderDetail = await Repository.ClientDb.Queryable<InvRequisitionOrderDetail>().Where(w => w.OrderNo == order.SourceOrderNo).ToListAsync();
                    foreach (var requisitionDetail in requisitionOrderDetail)
                    {
                        var outStorageDetail = ordersDetail.SingleOrDefault(s => s.GoodsId == requisitionDetail.GoodsId);
                        if (outStorageDetail != null)
                        {
                            requisitionDetail.ActualQuantity = outStorageDetail.ActualQuantity;
                            requisitionDetail.ActualUnitId = outStorageDetail.UnitId;
                            requisitionDetail.ActualUnitName = unitData.SingleOrDefault(s => s.UnitId == outStorageDetail.UnitId)?.UnitName;
                        }
                    }
                    Repository.ClientDb.Updateable(requisitionOrderDetail).AddQueue();
                }  
            }
            //如果存在AGV任务，则修改任务状态为已完成
            Repository.ClientDb.Updateable<AutoProdTaskTracking>().SetColumns(s => s.TaskStatus == AGVTaskStatus.End.ToString()).Where(s=>s.OrderNo== order.OrderNo&&s.TaskType == AutoTransTaskType.OutStorage.ToString()).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            //再次计算库存
            await _storageMgr.StorageStatistics(order.CreateUserId, order.CreateUserName);
        }
          
        /// <summary>
        /// 导出出库单
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        public async Task<string> ExportOutStorage(string searchKey, string orderField, string orderType, string dateStart, string dateEnd, string goodsGroup)
        {
            orderField = string.IsNullOrEmpty(orderField) ? "a.CreateDate" : orderField;
            orderField = orderField == "goodsId" ? "b.GoodsId" : orderField;
            orderField = orderField == "goodsName" ? "b.GoodsName" : orderField;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var ds = GetDateStart(dateStart);
            var dn = GetDateEnd(dateEnd);
            var param = new Dictionary<string, object>
            {
                { "@OrderNo", "%"+searchKey+"%" },
                { "@Remark", "%"+searchKey+"%" },
                { "@CreateUserName", "%"+searchKey+"%" },
                { "@GoodsName", "%"+searchKey+"%" },
                { "@GoodsClassify", goodsGroup},
                { "@ds", ds },
                { "@dn", dn },
            };
            string sql = $@"select 
                            a.OrderNo as 出库单号,
                            a.OutStorageType as 出库类型,
                            (select OrderType from invrequisitionorder sr join invrequisitionorderdetail srd on srd.OrderNo=sr.OrderNo where sr.OrderNo=a.SourceOrderNo and srd.GoodsId=b.GoodsId) as 领用类型,
                            a.LineNo as 适用产线, 
                            a.GoodsClassify as 大类,
                            d.TypeName as 小类,
                            c.GoodsName as 名称,
                            c.GoodsNo as SAP编码,
                            c.GoodsModel as 型号, 
                            c.Supplier as 供应商,
                            k.SupplierNo as 供应商编码,
                            b.Quantity as 计划出库数量,
                            b.ActualQuantity as 实际出库数量,
                            j.UnitName as 出库单位,
                            e.WarehouseId as 仓库ID,
                            e.WarehouseName as 仓库名称,
                            f.ShelfId as 货架ID,
                            f.ShelfNo as 货架编码,
                            g.BinId as 货位ID,
                            g.BinNo as 货位编码,
                            h.WorkbinId as 料箱ID,
                            h.WorkbinNo as 料箱编码,
                            i.CellId as 料箱单元格ID,
                            i.CellNo as 料箱单元格编码,
                            a.OutStorageDate as 出库时间,
                            a.CreateUserName as 创建人,
                            a.`Status` as 状态 ,
                            a.Remark as 备注,
                            ROUND(b.TotalPrice,2) as 出库总价,
                            ROUND(b.UnitPrice,2) as 出库单价,   
                            b.PriceUnit as 价格单位
                            from invoutstorage a
                            join invoutstoragedetail b on a.OrderNo=b.OrderNo
                            join basegoods c on c.GoodsId=b.GoodsId
                            join basetype d on d.TypeId=c.GoodsClassifyId
                            join invwarehouse e on e.WarehouseId=b.WarehouseId
                            join invshelf f on f.ShelfId=b.ShelfId
                            join invbin g on g.BinId=b.BinId
                            left join invworkbin h on h.WorkbinId=b.WorkbinId
                            left join invworkbincell i on i.CellId=b.WorkbinCellId
                            left join baseunits j on j.UnitId=b.UnitId
                            left join basesuppliers k on k.SupplierName=c.Supplier
                            where a.GoodsClassify=@GoodsClassify and DATE(a.CreateDate)>=@ds and DATE(a.CreateDate)<=@dn
                            and (a.OrderNo like @OrderNo or a.Remark like @Remark or a.CreateUserName like @CreateUserName or b.GoodsName like @GoodsName)
                            order by {orderField} {orderType}";
            var queryData = await Repository.QueryBySqlAsync(sql, param);
            if (queryData != null && queryData.Rows?.Count > 0)
            {
                foreach (DataRow row in queryData.Rows)
                {
                    if (row["出库类型"] != null)
                    {
                        var key = row["出库类型"].ToString();
                        row["出库类型"] = EnumHelper.GetDescFromEnumVal<OutStorageType>(key);
                    }
                }
            }
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"出库单信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
            return fileUrl;
        }

        /// <summary>
        /// 审批出库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isApprove"></param>
        /// <param name="opinion"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task ApprovalOutStorage(string[] orderNo,bool isApprove,string opinion, string userId,string userName)
        {
            var approvalStatus = isApprove ? ApprovalStatus.Approve.ToString() : ApprovalStatus.Reject.ToString();
            var orders = await Repository.ClientDb.Queryable<InvOutStorage>().Where(x => orderNo.Contains(x.OrderNo)).ToListAsync();
            foreach (var order in orders)
            {
                if (order.Status != OutStorageStatus.Pending.ToString() && order.Status != OutStorageStatus.Approvaling.ToString())
                {
                    throw new BusinessException("审批失败,当前存在未进入审批流程的出库单"); 
                }
            }
            //查询当前审批流程信息
            var process = await GetApprover(ApprovalDataType.OutStorage.ToString(),userId); 
            if (process?.Count == 0)
            {
                throw new BusinessException("审批失败,当前用户没有审批权限"); 
            }
            var apprivalModel = process.First().ApprovalModel; 
            //判断上一级是否已审批
            if (apprivalModel == ApprovalModel.Process.ToString())
            {
                var approvalHis = await Repository.ClientDb.Queryable<ApprovalHis>().Where(a => a.DataType == ApprovalDataType.OutStorage.ToString() && orderNo.Contains(a.PrimaryId)).ToListAsync();
                foreach (var curOrder in orderNo)
                {
                    var lastRank = approvalHis?.Count==0?0: approvalHis.Where(h => h.PrimaryId == curOrder).Max(h => h.ApprovalRank);
                    var approverRank = process.Select(x => x.Rank).Distinct().ToList();
                    if (!approverRank.Contains( lastRank +1))
                    {
                        throw new BusinessException("审批失败,单号{curOrder}需要等待下级审批"); 
                    }
                }
                if (!process.Exists(x=>x.IsLastApproval) && isApprove)
                {
                    approvalStatus = ApprovalStatus.Approvaling.ToString();
                }
            }
            //更新数据审批状态 
            var hightApprover = process.OrderByDescending(x => x.Rank).First(); 
            orders.ForEach(x =>
            {
                x.ApprovalDate = DateTime.Now;
                x.ApproverId = userId;
                x.ApproverName = userName;
                x.ApproverRole = hightApprover.ApproverRole;
                x.ApprovalStatus = approvalStatus; 
                x.Status = approvalStatus == ApprovalStatus.Approve.ToString() ? OutStorageStatus.WaitOutStorage.ToString() : approvalStatus;
            });
            Repository.ClientDb.Updateable(orders).AddQueue(); 
            //添加审批历史记录
            var approvalHisList = new List<ApprovalHis>();
            foreach(var id in orderNo)
            {
                approvalHisList.Add(new ApprovalHis
                {
                    ApprovalDate = DateTime.Now,
                    ApprovalModel = apprivalModel,
                    ApprovalStatus = isApprove ? ApprovalStatus.Approve.ToString() : ApprovalStatus.Reject.ToString(),
                    ApproverId = userId,
                    ApproverName = userName,
                    ApproverRoleId = hightApprover.ApproverRole,
                    ApproverRoleName = hightApprover.ApproverRoleName,
                    DataType = ApprovalDataType.OutStorage.ToString(),
                    Opinion= opinion,
                    ApprovalRank=  hightApprover.Rank,
                    PrimaryId = id
                }); 
            }
            Repository.ClientDb.Insertable(approvalHisList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
         
    }
}
