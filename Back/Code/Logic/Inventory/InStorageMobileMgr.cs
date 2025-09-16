using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Common.Extension;
using Logic.AutomationDevice;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Sys;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Inventory
{
    /// <summary>
    /// 入库业务处理类
    /// </summary>
   public class InStorageMobileMgr : ApprovalHandler
    {

        private readonly SysArgsService _sysArgsHelper;

        private readonly IFileStorage _fileStorage;

        private readonly StorageMgr _storageMgr;

        private readonly MessageService _messageService;

        private readonly AutoTransportHandler _autoTransportHandler;

        public InStorageMobileMgr(Repository repository, SysArgsService sysArgsHelper, IFileStorage fileStorage, StorageMgr storageMgr, MessageService messageService, AutoTransportHandler autoTransportHandler) : base(repository)
        {
            _sysArgsHelper = sysArgsHelper;
            _fileStorage = fileStorage;
            _storageMgr = storageMgr;
            _messageService = messageService;
            _autoTransportHandler = autoTransportHandler;
        }

        /// <summary>
        /// 入库单分页查询
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
        public async Task<TableModel<InStorage>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd,string goodsGroup)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "CreateDate" : orderFiled;
            orderFiled = orderFiled == "remark" ? "i.remark" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            //var underlingUsers = await GetUnderlingUsers(userId);
            var curApprover = await GetApprover(ApprovalDataType.InStorage.ToString(), userId);
            var approvalModel = curApprover?.FirstOrDefault()?.ApprovalModel;
            var curApproverRank = curApprover?.Select(x => x.Rank).Distinct().ToList();
            var isAnyApproval = approvalModel == ApprovalModel.Any.ToString() && curApprover?.Count > 0;
            var data = Repository.ClientDb.Queryable<InvInStorage>()
                  .LeftJoin<InvWarehouse>((i, w) => w.WarehouseId == i.WarehouseId)
                  .Where((i, w) => i.OrderNo.Contains(searchKey) || i.Remark.Contains(searchKey) || i.CreateUserName.Contains(searchKey)
                 || SqlFunc.Subqueryable<InvInStorageDetail>().InnerJoin<BaseGoods>((d, g) => d.GoodsId == g.GoodsId).Where((d, g) => d.OrderNo == i.OrderNo && (d.GoodsName.Contains(searchKey) || g.GoodsNo.Contains(searchKey) || g.GoodsModel.Contains(searchKey))).Any())
                  .Where((i, w) => i.GoodsClassify == goodsGroup && SqlFunc.ToDate(i.CreateDate) >= GetDateStart(dateStart) && SqlFunc.ToDate(i.CreateDate) <= GetDateEnd(dateEnd)) 
                  .Select((i, w) => new InStorage
                  {
                      OrderNo = i.OrderNo,
                      InStorageType = i.InStorageType,
                      GoodsClassify = i.GoodsClassify,
                      Status = i.Status,
                      Responsible=i.Responsible,
                      ResponsibleId=i.ResponsibleId,
                      CreateDate = i.CreateDate,
                      CreateUserId = i.CreateUserId,
                      CreateUserName = i.CreateUserName,
                      InstorageDate = i.InstorageDate,
                      WarehouseId = i.WarehouseId,
                      WarehouseName = w.WarehouseName,
                      Remark = i.Remark,
                      ApprovalStatus = i.ApprovalStatus,
                      ApprovalDate = i.ApprovalDate,
                      SourceOrderNo = i.SourceOrderNo,
                      UpdateUserId = i.UpdateUserId,
                      UpdateUserName = i.UpdateUserName,
                      UpdateDate = i.UpdateDate,
                      ApprovalLastRank = SqlFunc.Subqueryable<ApprovalHis>().Where(h => h.PrimaryId == i.OrderNo && h.DataType == ApprovalDataType.InStorage.ToString()).Max(h => h.ApprovalRank)
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(row =>
            {
                row.InStorageTypeDesc = EnumHelper.GetDescFromEnumVal<InStorageType>(row.InStorageType);
                row.StatusDesc = EnumHelper.GetDescFromEnumVal<InStorageStatus>(row.Status);
                row.GoodsClassifyDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(row.GoodsClassify);
                row.ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(row.ApprovalStatus);
                row.IsApproval = (isAnyApproval && row.Status == InStorageStatus.Pending.ToString()) || (curApprover?.Count > 0 && curApproverRank.Contains(row.ApprovalLastRank + 1) && (row.Status == InStorageStatus.Pending.ToString() || row.Status == InStorageStatus.Approvaling.ToString()));
            });

            var orderNos = data.Select(d => d.OrderNo).ToList();

            var photos = await Repository.ClientDb.Queryable<BaseFiles>()
             .Where(p => orderNos.Contains(p.PrimaryId) && p.FileInfoType == FileInfoType.InStoragePhoto.ToString())
             .ToListAsync();

            // 逐个挂载到单据上
            data.ForEach(row =>
            {
                row.GoodsPicture = photos
                    .Where(p => p.PrimaryId == row.OrderNo)
                    .Select(p => new FileInfoDto
                    {
                        FileId = p.FileId,
                        FileName = p.FileName,
                        Url = p.Url,
                        FileInfoType = p.FileInfoType,
                        Path = p.Path,
                        PrimaryId = p.PrimaryId,
                        Remark = p.Remark
                    })
                    .ToList();
            });
            var res = new TableModel<InStorage>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 获取入库单明细
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task<List<InStorageDetail>> GetOrderDetail( string orderNo)
        {
            var data= await Repository.ClientDb.Queryable<InvInStorageDetail>()
                .InnerJoin<BaseGoods>((i, g) => g.GoodsId == i.GoodsId)
                .LeftJoin<BaseUnits>((i,g,u)=>u.UnitId==i.UnitId)
                .LeftJoin<BaseType>((i,g,u,t)=>t.TypeId==g.GoodsClassifyId)
                .LeftJoin<InvWarehouse>((i, g, u, t,w) => i.WarehouseId == w.WarehouseId)
                .LeftJoin<InvShelf>((i, g, u, t, w,s)=>s.ShelfId==i.ShelfId)
                .LeftJoin<InvBin>((i, g, u, t, w, s, b) => i.BinId == b.BinId)
                .LeftJoin<InvWorkbin>((i, g, u, t, w, s, b,wb)=>wb.WorkbinId==i.WorkbinId)
                .LeftJoin<InvWorkbinCell>((i, g, u, t, w, s, b, wb,wbc) => wbc.CellId == i.WorkbinCellId)
                .Where((i, g, u, t, w, s, b, wb, wbc) => i.OrderNo == orderNo)
                .Select((i, g, u, t, w, s, b, wb, wbc) => new InStorageDetail
                {
                    WarehouseId = w.WarehouseId,
                    WarehouseNo = w.WarehouseNo,
                    WarehouseName = w.WarehouseName,
                    ShelfId=s.ShelfId,
                    ShelfNo=s.ShelfNo,
                    ShelfName=s.ShelfName,
                    BinId = b.BinId,
                    BinNo = b.BinNo,
                    BinName = b.BinName,
                    WorkbinId=wb.WorkbinId,
                    WorkbinNo=wb.WorkbinNo,
                    WorkbinCellId=wbc.CellId,
                    WorkbinCellNo=wbc.CellNo,
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    GoodsSpecificationId= g.GoodsSpecificationId,
                    GoodsClassifyName=t.TypeName,
                    PackageCount=g.PackageCount,
                    MaxPackageCount=g.MaxPackageCount, 
                    PackageUnitName=g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName =g.MaxPackageUnitName,
                    Quantity = i.Quantity,
                    ActualQuantity=i.ActualQuantity,
                    UnitId=i.UnitId,
                    UnitName = u.UnitName,
                    OrderNo = i.OrderNo,
                    DetailId = i.DetailId,
                    Remark = i.Remark,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    PriceUnit = i.PriceUnit,
                    //Photos = SqlFunc.Subqueryable<BaseFiles>().Where(p=>p.FileInfoType ==FileInfoType.InStoragePhoto.ToString() && p.PrimaryId == i.OrderNo && p.IsDeft).Select(p=>p.Url),
                }).ToListAsync();

            // 获取所有订单附件
            var orderPhotos = await Repository.ClientDb.Queryable<BaseFiles>()
                .Where(p => p.FileInfoType == FileInfoType.InStoragePhoto.ToString()
                         && p.PrimaryId == orderNo)
                .Select(p => p.Url)
                .ToListAsync();

            // 赋值给每条明细
            data.ForEach(d => d.Photos = orderPhotos);

            return data;
        }

        /// <summary>
        /// 1.根据物品查询推荐入库料箱
        /// 2.根据入库隔离规则，判断是否存放到上一次入库的料箱，还是重新启用新的料箱
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<List<RecommendWorkbinCellDto>> GetWorkbinRecommend(string goodsId)
        {
            int isolateDays = 0;
            var isolateArgsObj = await _sysArgsHelper.GetValueByKey(BusinessConst.InStorageIsolateDays);
            if(isolateArgsObj != null)
            {
                int.TryParse(isolateArgsObj.Value.ToString(), out isolateDays);
            }
            var query1 =  Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
              .LeftJoin<InvWarehouse>((d, w) => w.WarehouseId == d.WarehouseId)
              .LeftJoin<InvShelf>((d, w, s) => s.ShelfId == d.ShelfId)
              .LeftJoin<InvBin>((d, w, s, b) => b.BinId == d.BinId)
              .LeftJoin<InvWorkbin>((d, w, s, b, wb) => wb.WorkbinId == d.WorkbinId)
              .LeftJoin<InvWorkbinCell>((d, w, s, b, wb, wbc) => wbc.CellId == d.WorkbinCellId)
              .LeftJoin<BaseUnits>((d, w, s, b, wb, wbc, u) => u.UnitId == d.UnitId)
              .LeftJoin<InvWorkbinSpecification>((d, w, s, b, wb, wbc, u,wbs)=>wbs.SpecId==wb.SpecId)
               .LeftJoin<BaseGoods>((d, w, s, b, wb, wbc, u, wbs,g) => g.GoodsId==d.GoodsId)
              .Where((d, w, s, b, wb, wbc, u, wbs,g) => d.GoodsId == goodsId && d.Stock>0)
              .Select((d, w, s, b, wb, wbc, u, wbs,g) =>new RecommendWorkbinCellDto
              {
                  GoodsId = d.GoodsId,
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
                  CellId = wbc.CellId,
                  CellNo = wbc.CellNo,
                  SpecId = wbs.SpecId,
                  SpecName = wbs.SpecName,
                  MaxStock = g.MaxStock,
                  MaxStockUnitId = g.SafetyInventoryUnitId,
                  MaxStockUnitName = g.SafetyInventoryUnitName,
                  Stock = d.Stock,
                  UnitId = d.UnitId,
                  UnitName = u.UnitName
              });
 
            return await query1.ToListAsync();
        }

        /// <summary>
        /// 查询指定仓库和规格的货架、货位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="specId"></param>
        /// <returns></returns>
        public async Task<List<WorkbinDto>> GetBinRecommend(string warehouseId, int specId)
        {
            return await Repository.ClientDb.Queryable<InvWorkbin>()
                .Where(wb => wb.SpecId == specId && wb.WarehouseId == warehouseId)
                .Select<WorkbinDto>()
                .ToListAsync(); 
        }

        /// <summary>
        /// 添加入库单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddInStorage(InStorage data)
        {
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("保存失败,请添加入库单明细");
            }
            foreach(var detail in data.Details)
            {
                if(detail.BinId== 0)
                {
                    throw new BusinessException("保存失败,请给入库明细指定入库货位");
                }
            }
            var sameGoods = data.Details.GroupBy(d => new { d.GoodsId, d.WarehouseId, d.BinId,d.WorkbinCellId, d.UnitId }).Count();
            if (sameGoods != data.Details.Count)
            {
                throw new BusinessException("保存失败,同一物品在相同单位、仓库库位下不允许多次添加"); 
            } 
            //验证：指定的物品是否在盘点中
            var goodsArr = data.Details.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();
            foreach (var detail in data.Details)
            {
                var isTakeStockLock = goodsInfo.Exists(e => e.GoodsId == detail.GoodsId && e.IsTakeStockLock);
                if (isTakeStockLock)
                {
                    throw new BusinessException($"保存失败，物品{detail.GoodsName+detail.GoodsModel}正在盘点中，暂停入库");
                }
            }
            //验证：指定的货位是否在盘点中
            var binArr = data.Details.Select(s => s.BinId).Distinct().ToArray();
            var binInfo = await Repository.ClientDb.Queryable<InvBin>().Where(w => binArr.Contains(w.BinId)).ToListAsync();
            foreach (var detail in data.Details)
            {
                var isTakeStockLock= binInfo.Exists(e=>e.BinId==detail.BinId&&e.IsTakeStockLock);
                if (isTakeStockLock)
                {
                    throw new BusinessException($"保存失败，货位{detail.BinName}正在盘点中，暂停入库");
                }
            }
            //验证：本次入库明细中是否存在不同物品入库到相同货位或料箱
            var invBinIdArr = data.Details.Where(w => w.WorkbinCellId == 0).Select(s => s.BinId).Distinct().ToArray(); 
            if (invBinIdArr?.Length > 0)
            {
                foreach (var binId in invBinIdArr)
                {
                    var isVarietyStock = binInfo.Any(a => a.BinId == binId && a.IsVarietyStock == true);
                    if (!isVarietyStock)
                    {
                        var sameBinGoodsCount = data.Details.Where(d => d.BinId == binId).GroupBy(d => d.GoodsId).Count();
                        if (sameBinGoodsCount > 1)
                        {
                            throw new BusinessException("保存失败,入库明细中存在不同物品选择了相同货位，请选择其他推荐货位");
                        }
                    }
                      
                }
            }
            var invCellIdArr = data.Details.Where(w => w.WorkbinCellId > 0).Select(s => s.WorkbinCellId).Distinct().ToArray();
            if (invCellIdArr?.Length > 0)
            {
                foreach (var cellId in invCellIdArr)
                {
                    var parentBinId = data.Details.First(f => f.WorkbinCellId == cellId).BinId;
                    var isVarietyStock = binInfo.Any(a => a.BinId == parentBinId && a.IsVarietyStock == true);
                    if (!isVarietyStock)
                    {
                        var sameCellGoodsCount = data.Details.Where(d => d.WorkbinCellId == cellId).GroupBy(d => d.GoodsId).Count();
                        if (sameCellGoodsCount > 1)
                        {
                            throw new BusinessException("保存失败,入库明细中存在不同物品选择了相同料箱，请选择其他推荐料箱");
                        }
                    } 
                }
            }
            //验证：指定的料箱单元格是否已存在其他待入库的物品
            var waitInOrders = await Repository.ClientDb.Queryable<InvInStorageDetail>()
                .LeftJoin<InvInStorage>((d, s) => s.OrderNo == d.OrderNo)
                .Where((d, s) => s.Status == InStorageStatus.WaitInStorage.ToString())
                .Select((d, s)=>new {d.GoodsId,d.BinId,d.WorkbinCellId}).Distinct().ToListAsync();
            if(waitInOrders.Count > 0)
            {
                if (invBinIdArr?.Length > 0)
                {
                    var waitInOrdersByBin = waitInOrders.Where(w => w.WorkbinCellId == 0).Distinct().ToList();
                    if (waitInOrdersByBin.Count > 0)
                    {
                        foreach(var detail in data.Details)
                        {
                            var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                            if (!isVarietyStock)
                            {
                                var exists = waitInOrdersByBin.Exists(e => e.GoodsId != detail.GoodsId && e.BinId == detail.BinId);
                                if (exists)
                                {
                                    throw new BusinessException($"保存失败,检测到货位{detail.BinName}存在其他待入库的物品，请选择其他推荐货位");
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
                        foreach (var detail in data.Details)
                        { 
                            var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                            if (!isVarietyStock)
                            {
                                var exists = waitInOrdersByCell.Exists(e => e.GoodsId != detail.GoodsId && e.WorkbinCellId == detail.WorkbinCellId);
                                if (exists)
                                {
                                    throw new BusinessException($"保存失败,检测到料箱{detail.WorkbinCellNo}存在其他待入库的物品，请选择其他推荐料箱");
                                }
                            } 
                        }
                    }
                }
            } 
            //验证：指定的料箱单元格是否已存放其他已入库的物品  
            if (invBinIdArr?.Length > 0)
            {
                var invInfo = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(w => invBinIdArr.Contains(w.BinId)).ToListAsync();
                if (invInfo?.Count > 0)
                {
                    foreach (var detail in data.Details)
                    {
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId == 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.Stock > 0);
                                if (exist)
                                {
                                    throw new BusinessException($"保存失败，货位{detail.BinName}已存放其他物品，请选择其他推荐货位");
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
                    foreach (var detail in data.Details)
                    { 
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId > 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.WorkbinCellId == detail.WorkbinCellId && e.Stock > 0);
                                if (exist)
                                {
                                    throw new BusinessException($"保存失败，料箱单元格{detail.WorkbinCellNo}已存放其他物品，请选择其他推荐料箱");
                                }
                            }
                        } 
                    }
                }
            }
            //验证：物品指定存放规格是否和指定的料箱匹配 
            if (invCellIdArr?.Length > 0)
            {
                var invWorkbinArr = data.Details.Where(w=>w.WorkbinId>0).Select(s => s.WorkbinId).Distinct().ToArray();
                var workbinInfo = await Repository.ClientDb.Queryable<InvWorkbin>().Where(w => invWorkbinArr.Contains(w.WorkbinId)).ToListAsync();
                foreach (var detail in data.Details)
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
            //入库单 
            var lastData = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo);
            var inStorageModel = new InvInStorage
            {
                OrderNo = GetPrimaryId("I", lastData),
                SourceOrderNo=data.SourceOrderNo,
                InStorageType =data.InStorageType, 
                GoodsClassify = data.GoodsClassify,
                CreateDate = DateTime.Now,
                CreateUserId=data.CreateUserId,
                CreateUserName=data.CreateUserName, 
                ResponsibleId=data.ResponsibleId,
                Responsible=data.Responsible,
                WarehouseId=data.WarehouseId, 
                Remark = data.Remark
            }; 
            var isInStorageApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsInStorageApproval)).Value.ToString());
            if (isInStorageApproval)
            {
                //审批信息(如果当前创建人也是审批人,则需要修改和添加相应的审批信息) 
                var process = await GetApprovalProcess(ApprovalDataType.InStorage.ToString());
                var curApprover = process.Where(a => a.ApproverId == data.CreateUserId).ToList();
                if (curApprover?.Count > 0)
                {
                    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                    inStorageModel.ApprovalDate = DateTime.Now;
                    inStorageModel.ApproverId = data.CreateUserId;
                    inStorageModel.ApproverName = data.CreateUserName;
                    inStorageModel.ApproverRole = hightApprover.ApproverRole;
                    if (hightApprover.IsLastApproval)
                    {
                        inStorageModel.ApprovalStatus = ApprovalStatus.Approve.ToString();
                        inStorageModel.Status = InStorageStatus.WaitInStorage.ToString();
                    }
                    else
                    {
                        inStorageModel.Status = InStorageStatus.Approvaling.ToString();
                        inStorageModel.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
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
                        DataType = ApprovalDataType.InStorage.ToString(),
                        ApprovalRank = hightApprover.Rank,
                        PrimaryId = inStorageModel.OrderNo
                    };
                    Repository.ClientDb.Insertable(approvalHis).AddQueue();
                }
                else
                {
                    inStorageModel.Status = InStorageStatus.Pending.ToString();
                    inStorageModel.ApprovalStatus = ApprovalStatus.Pending.ToString();
                }
            }
            else
            {
                inStorageModel.Status = InStorageStatus.WaitInStorage.ToString();
                inStorageModel.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
            }
            Repository.ClientDb.Insertable(inStorageModel).AddQueue();
            //入库单明细
            var inStorageDetail = data.Details.Select(b => new InvInStorageDetail
            {
                OrderNo = inStorageModel.OrderNo,
                GoodsId = b.GoodsId,
                GoodsName = b.GoodsName,
                WarehouseId = b.WarehouseId,
                ShelfId = b.ShelfId,
                BinId = b.BinId,
                WorkbinId=b.WorkbinId,
                WorkbinCellId = b.WorkbinCellId,
                Quantity = b.Quantity, 
                UnitId = b.UnitId,
                Remark = b.Remark,
                TotalPrice = b.TotalPrice,
                UnitPrice = b.UnitPrice,
                PriceUnit = b.PriceUnit
            }).ToList();
            Repository.ClientDb.Insertable(inStorageDetail).AddQueue(); 
            await Repository.ClientDb.SaveQueuesAsync();
            var msgContent = $"{data.CreateUserName}提交了一份待确认的({EnumHelper.GetDescFromEnumVal<InStorageType>(data.InStorageType)})入库单";
            var msgRemark = $"{string.Join(',', data.Details.Select(s => s.GoodsName))}";
            await _messageService.CreateMessage(data.CreateUserName, msgContent, msgRemark, MessageType.InStorage);

            if (data.GoodsPicture?.Count > 0)
            {   
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                foreach (var p in data.GoodsPicture)
                {
                    photos.Add(new BaseFiles
                    {
                        PrimaryId = inStorageModel.OrderNo,   // ✅ 绑定到整张单据
                        FileName = p.FileName,
                        FileInfoType = FileInfoType.InStoragePhoto.ToString(),
                        Url = p.Url,
                        IsDeft = !isSetDeft
                    });
                    isSetDeft = true;
                }
                Repository.ClientDb.Insertable(photos).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();

        }

        /// <summary>
        /// 修改入库单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateInStorage(InStorage data)
        {
            if (data.Details?.Count == 0)
            {
                throw new BusinessException("保存失败,请添加入库单明细"); 
            }
            foreach (var detail in data.Details)
            {
                if (detail.BinId == 0)
                {
                    throw new BusinessException("保存失败,请给入库明细指定入库货位");
                }
            }
            var sameGoods = data.Details.GroupBy(d => new { d.GoodsId, d.WarehouseId, d.BinId, d.WorkbinCellId, d.UnitId }).Count();
            if (sameGoods != data.Details.Count)
            {
                throw new BusinessException("保存失败,同一物品在相同单位、仓库库位下不允许多次添加"); 
            }
            var oldInStorage = await Repository.ClientDb.Queryable<InvInStorage>().SingleAsync(u => u.OrderNo == data.OrderNo);
            if (string.IsNullOrEmpty(oldInStorage?.OrderNo))
            {
                throw new BusinessException("保存失败,当前入库单号不存在或已删除"); 
            } 
            var isInStorageApproval = bool.Parse((await _sysArgsHelper.GetValueByKey(BusinessConst.IsInStorageApproval)).Value.ToString());
            if (oldInStorage.Status== InStorageStatus.InStorage.ToString())
            {
                throw new BusinessException("保存失败,当前入库单已入库，无法修改"); 
            }
            //验证：指定的物品是否在盘点中
            var goodsArr = data.Details.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfo = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToListAsync();
            foreach (var detail in data.Details)
            {
                var isTakeStockLock = goodsInfo.Exists(e => e.GoodsId == detail.GoodsId && e.IsTakeStockLock);
                if (isTakeStockLock)
                {
                    throw new BusinessException($"保存失败，物品{detail.GoodsName + detail.GoodsModel}正在盘点中，暂停入库");
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
                    throw new BusinessException($"保存失败，货位{detail.BinName}正在盘点中，暂停入库");
                }
            }
            //验证：本次入库明细中是否存在不同物品入库到相同货位或料箱
            var invBinIdArr = data.Details.Where(w => w.WorkbinCellId == 0).Select(s => s.BinId).Distinct().ToArray();
            if (invBinIdArr?.Length > 0)
            {
                foreach (var binId in invBinIdArr)
                {
                    var isVarietyStock = binInfo.Any(a => a.BinId == binId && a.IsVarietyStock == true);
                    if (!isVarietyStock)
                    {
                        var sameBinGoodsCount = data.Details.Where(d => d.BinId == binId).GroupBy(d => d.GoodsId).Count();
                        if (sameBinGoodsCount > 1)
                        {
                            throw new BusinessException("保存失败,入库明细中存在不同物品选择了相同货位，请选择其他推荐货位");
                        }
                    } 
                }
            }
            var invCellIdArr = data.Details.Where(w => w.WorkbinCellId > 0).Select(s => s.WorkbinCellId).Distinct().ToArray();
            if (invCellIdArr?.Length > 0)
            {
                foreach (var cellId in invCellIdArr)
                {
                    var parentBinId = data.Details.First(f => f.WorkbinCellId == cellId).BinId;
                    var isVarietyStock = binInfo.Any(a => a.BinId == parentBinId && a.IsVarietyStock == true);
                    if (!isVarietyStock)
                    {
                        var sameCellGoodsCount = data.Details.Where(d => d.WorkbinCellId == cellId).GroupBy(d => d.GoodsId).Count();
                        if (sameCellGoodsCount > 1)
                        {
                            throw new BusinessException("保存失败,入库明细中存在不同物品选择了相同料箱，请选择其他推荐料箱");
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
                        foreach (var detail in data.Details)
                        {
                            var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                            if (!isVarietyStock)
                            {
                                var exists = waitInOrdersByBin.Exists(e => e.GoodsId != detail.GoodsId && e.BinId == detail.BinId);
                                if (exists)
                                {
                                    throw new BusinessException($"保存失败,检测到货位{detail.BinName}存在其他待入库的物品，请选择其他推荐货位");
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
                        foreach (var detail in data.Details)
                        {
                            var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                            if (!isVarietyStock)
                            {
                                var exists = waitInOrdersByCell.Exists(e => e.GoodsId != detail.GoodsId && e.WorkbinCellId == detail.WorkbinCellId);
                                if (exists)
                                {
                                    throw new BusinessException($"保存失败,检测到料箱{detail.WorkbinCellNo}存在其他待入库的物品，请选择其他推荐料箱");
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
                    foreach (var detail in data.Details)
                    {
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId == 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.Stock > 0);
                                if (exist)
                                {
                                    throw new BusinessException($"保存失败，货位{detail.BinName}已存放其他物品，请选择其他推荐货位");
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
                    foreach (var detail in data.Details)
                    {
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId > 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.WorkbinCellId == detail.WorkbinCellId && e.Stock > 0);
                                if (exist)
                                {

                                    throw new BusinessException($"保存失败，料箱单元格{detail.WorkbinCellNo}已存放其他物品，请选择其他推荐料箱");
                                }
                            }
                        }
                    }
                }
            }
            //验证：物品指定存放规格是否和指定的料箱匹配
            if (invCellIdArr?.Length > 0)
            {
                var invWorkbinArr = data.Details.Where(w => w.WorkbinId > 0).Select(s => s.WorkbinId).Distinct().ToArray();
                var workbinInfo = await Repository.ClientDb.Queryable<InvWorkbin>().Where(w => invWorkbinArr.Contains(w.WorkbinId)).ToListAsync();
                foreach (var detail in data.Details)
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
            //入库单  
            var inStorageModel = new InvInStorage
            {
                OrderNo = data.OrderNo,
                SourceOrderNo = data.SourceOrderNo,
                InStorageType = data.InStorageType, 
                GoodsClassify = data.GoodsClassify,
                CreateDate = oldInStorage.CreateDate,
                CreateUserId = oldInStorage.CreateUserId,
                CreateUserName = oldInStorage.CreateUserName,
                ResponsibleId = data.ResponsibleId,
                Responsible = data.Responsible,
                UpdateDate = DateTime.Now,
                UpdateUserId=data.CreateUserId,
                UpdateUserName=data.CreateUserName,
                WarehouseId = data.WarehouseId,
                Remark = data.Remark,
            };
            if (isInStorageApproval)
            {
                //审批信息(如果当前修改人也是审批人,则需要修改和添加相应的审批信息)
                Repository.ClientDb.Deleteable<ApprovalHis>(a => a.DataType == ApprovalDataType.InStorage.ToString() && a.PrimaryId == data.OrderNo).AddQueue();
                var process = await GetApprovalProcess(ApprovalDataType.InStorage.ToString());
                var curApprover = process.Where(a => a.ApproverId == data.CreateUserId).ToList();
                if (curApprover?.Count > 0)
                {
                    var hightApprover = curApprover.OrderByDescending(x => x.Rank).First();
                    inStorageModel.ApprovalDate = DateTime.Now;
                    inStorageModel.ApproverId = data.CreateUserId;
                    inStorageModel.ApproverName = data.CreateUserName;
                    inStorageModel.ApproverRole = hightApprover.ApproverRole;
                    if (hightApprover.IsLastApproval)
                    {
                        inStorageModel.ApprovalStatus = ApprovalStatus.Approve.ToString();
                        inStorageModel.Status = InStorageStatus.WaitInStorage.ToString();
                    }
                    else
                    {
                        inStorageModel.Status = InStorageStatus.Approvaling.ToString();
                        inStorageModel.ApprovalStatus = ApprovalStatus.Approvaling.ToString();
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
                        DataType = ApprovalDataType.InStorage.ToString(),
                        ApprovalRank = hightApprover.Rank,
                        PrimaryId = inStorageModel.OrderNo
                    };
                    Repository.ClientDb.Insertable(approvalHis).AddQueue();
                }
                else
                {
                    inStorageModel.Status = InStorageStatus.Pending.ToString();
                    inStorageModel.ApprovalStatus = ApprovalStatus.Pending.ToString();
                }
            }
            else
            {
                inStorageModel.Status = InStorageStatus.WaitInStorage.ToString();
                inStorageModel.ApprovalStatus = ApprovalStatus.NoApproval.ToString();
            }
            Repository.ClientDb.Updateable(inStorageModel).AddQueue();
            //入库单明细
            var inStorageDetail = data.Details.Select(b => new InvInStorageDetail
            {
                OrderNo = inStorageModel.OrderNo,
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
                TotalPrice = b.TotalPrice,
                UnitPrice = b.UnitPrice,
                PriceUnit = b.PriceUnit
            }).ToList();
            Repository.ClientDb.Deleteable<InvInStorageDetail>(d => d.OrderNo == data.OrderNo).AddQueue();
            Repository.ClientDb.Insertable(inStorageDetail).AddQueue();
           
            data.GoodsPicture.ForEach(p =>
            {
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                data.GoodsPicture.ForEach(p =>
                {
                    photos.Add(new BaseFiles { PrimaryId = data.OrderNo, FileName = p.FileName, FileInfoType = FileInfoType.InStoragePhoto.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                    isSetDeft = true;
                });
                Repository.ClientDb.Deleteable<BaseFiles>(x => x.PrimaryId == data.OrderNo && x.FileInfoType == FileInfoType.InStoragePhoto.ToString()).AddQueue();
                Repository.ClientDb.Insertable(photos).AddQueue();
            });
            await Repository.ClientDb.SaveQueuesAsync();
        } 
         

        /// <summary>
        /// 入库确认
        /// </summary>
        /// <param name="ordersNo"></param>
        /// <returns></returns>
        public async Task ConfirmInStorage(string  ordersNo)
        {
            var order = await Repository.ClientDb.Queryable<InvInStorage>().SingleAsync(u => ordersNo==u.OrderNo);
            var ordersDetail = await Repository.ClientDb.Queryable<InvInStorageDetail>().Where(u => ordersNo == u.OrderNo).ToListAsync();
            _validInStorage(order, ordersDetail);
            order.Status = InStorageStatus.InStorage.ToString();
            order.InstorageDate = DateTime.Now;
            Repository.ClientDb.Updateable(order).AddQueue();
         
            //修改入库单明细实际入库数量 
            ordersDetail.ForEach(f=>f.ActualQuantity = f.Quantity);
            Repository.ClientDb.Updateable(ordersDetail).AddQueue();
            //添加库存流水记录  
            var storageFlowDetailList = ordersDetail.Select(d => new InvStorageFlowDetail
            {
                FlowType = FlowType.In.ToString(),
                GoodsId = d.GoodsId,
                OperateDate = DateTime.Now.ToStringExtension(),
                OperatorId = order.CreateUserId,
                OperatorName = order.CreateUserName,
                Quantity = d.ActualQuantity,
                SourceOrderNo = order.OrderNo,
                SourceStorageType = SourceStorageType.InStorage.ToString(),
                SourceStorageSubType = order.InStorageType,
                UnitId = d.UnitId,
                WarehouseId = d.WarehouseId,
                ShelfId = d.ShelfId,
                BinId = d.BinId,
                WorkbinId = d.WorkbinId,
                WorkbinCellId = d.WorkbinCellId,
                DateYear = int.Parse(DateTime.Now.ToString("yyyy")),
                DateMonth = int.Parse(DateTime.Now.ToString("yyyyMM")),
                DateDay= int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                BatchNumber=long.Parse(DateTime.Now.ToStringNoSignExtension()),
                TotalPrice=d.TotalPrice,
                UnitPrice=d.UnitPrice,
                PriceUnit=d.PriceUnit,
                Remark=d.Remark,
                RemarkType=d.RemarkType,
            }).ToList(); 
            Repository.ClientDb.Insertable(storageFlowDetailList).AddQueue();
            //如果存在AGV任务，则修改任务状态为已完成
            var agvTask = Repository.ClientDb.Queryable<AutoProdTaskTracking>().Where(s => s.OrderNo == order.OrderNo && s.TaskType == AutoTransTaskType.InStorage.ToString()).ToList();
            if(agvTask != null && agvTask.Count > 0)
            {
                agvTask.ForEach(f =>
                {
                    f.TaskStatus = AGVTaskStatus.End.ToString();
                    f.ReturnTaskStatus = AGVTaskStatus.End.ToString();
                });
                Repository.ClientDb.Updateable(agvTask).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync(); 
            //重新计算库存
            await _storageMgr.StorageStatistics(order.CreateUserId, order.CreateUserName);
            //重新计算物品单价 
            _ = _storageMgr.UnitPriceStatistics();
        }

        private  void _validInStorage(InvInStorage order , List<InvInStorageDetail> ordersDetail)
        {
            foreach (var detail in ordersDetail)
            {
                if (detail.BinId == 0)
                {
                    throw new BusinessException("保存失败,请给入库明细指定入库货位");
                }
            }
            //验证：如果是采购入库，则相关收货计划的状态必须是已收货状态
            if (order.InStorageType == InStorageType.PurchaseIn.ToString() && !string.IsNullOrEmpty(order.SourceOrderNo))
            {
                var goodsIdArr = ordersDetail.Select(s => s.GoodsId).ToList();
                var receivingDetails =  Repository.ClientDb.Queryable<ReceivingOrderDetail>().Where(w => w.OrderNo == order.SourceOrderNo && goodsIdArr.Contains(w.GoodsId)).ToList();
                if (receivingDetails.Count > 0)
                {
                    foreach (var detail in receivingDetails)
                    {
                        if (detail.DetailStatus != ReceivingOrderDetailStatus.Received.ToString())
                        {
                            throw new BusinessException($"保存失败,请先确认收货计划订单{detail.OrderNo}中的物料{detail.GoodsNo}已收货");
                        }
                        detail.DetailStatus = ReceivingOrderDetailStatus.InStorage.ToString();
                    }
                    Repository.ClientDb.Updateable(receivingDetails).AddQueue();
                }
            }
            //验证：单据是否处于待入库状态
            if (order.Status != InStorageStatus.WaitInStorage.ToString())
            {
                throw new BusinessException("保存失败,当前单据不在待入库状态");
            }
            //验证：指定的物品是否在盘点中
            var goodsArr = ordersDetail.Select(s => s.GoodsId).Distinct().ToArray();
            var goodsInfo =  Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsArr.Contains(w.GoodsId)).ToList();
            foreach (var detail in ordersDetail)
            {
                var takeStockLockGoods = goodsInfo.Single(e => e.GoodsId == detail.GoodsId);
                if (takeStockLockGoods.IsTakeStockLock)
                {
                    throw new BusinessException($"保存失败，物品{takeStockLockGoods.GoodsName + takeStockLockGoods.GoodsModel}正在盘点中，暂停入库");
                }
            }
            //验证：指定的货位是否在盘点中
            var binArr = ordersDetail.Select(s => s.BinId).Distinct().ToArray();
            var binInfo =  Repository.ClientDb.Queryable<InvBin>().Where(w => binArr.Contains(w.BinId)).ToList();
            foreach (var detail in ordersDetail)
            {
                var takeStockLockBin = binInfo.Single(e => e.BinId == detail.BinId);
                if (takeStockLockBin.IsTakeStockLock)
                {
                    throw new BusinessException($"保存失败，货位{takeStockLockBin.BinName}正在盘点中，暂停入库");
                }
            }

            //验证：指定的料箱单元格是否已存放其他物品  
            var invBinIdArr = ordersDetail.Where(w => w.WorkbinCellId == 0).Select(s => s.BinId).Distinct().ToArray();
            if (invBinIdArr?.Length > 0)
            {
                var invInfo = Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(w => invBinIdArr.Contains(w.BinId)).ToList();
                if (invInfo?.Count > 0)
                {
                    foreach (var detail in ordersDetail)
                    {
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId == 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.Stock > 0);
                                if (exist)
                                {
                                    var existBin =  Repository.GetSinge<InvBin>(detail.BinId);
                                    throw new BusinessException($"保存失败，货位{existBin.BinName}已存放其他物品");
                                }
                            }
                        }
                    }
                }

            }
            var invCellIdArr = ordersDetail.Where(w => w.WorkbinCellId > 0).Select(s => s.WorkbinCellId).Distinct().ToArray();
            if (invCellIdArr?.Length > 0)
            {
                var invInfo =  Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().Where(w => invCellIdArr.Contains(w.WorkbinCellId)).ToList();
                if (invInfo?.Count > 0)
                {
                    foreach (var detail in ordersDetail)
                    {
                        var isVarietyStock = binInfo.Any(a => a.BinId == detail.BinId && a.IsVarietyStock == true);
                        if (!isVarietyStock)
                        {
                            if (detail.WorkbinCellId > 0)
                            {
                                var exist = invInfo.Exists(e => e.GoodsId != detail.GoodsId && e.WarehouseId == detail.WarehouseId && e.BinId == detail.BinId && e.WorkbinCellId == detail.WorkbinCellId && e.Stock > 0);
                                if (exist)
                                {
                                    var existCell =  Repository.GetSinge<InvWorkbinCell>(detail.WorkbinCellId);
                                    throw new BusinessException($"保存失败，料箱{existCell.CellNo}已存放其他物品");
                                }
                            }
                        }
                    }
                }
            }
            if (invCellIdArr?.Length > 0)
            {
                var invWorkbinArr = ordersDetail.Where(w => w.WorkbinId > 0).Select(s => s.WorkbinId).Distinct().ToArray();
                var workbinInfo =  Repository.ClientDb.Queryable<InvWorkbin>().Where(w => invWorkbinArr.Contains(w.WorkbinId)).ToList();
                foreach (var detail in ordersDetail)
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
        }

        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task DelInStorage(string[] orderNo)
        {
            var check = await Repository.Exist<InvInStorage>(e => (orderNo.Contains(e.OrderNo) && e.Status == InStorageStatus.InStorage.ToString())); 
            if (check)
            {
                throw new BusinessException("删除失败,无法删除已入库的入库单"); 
            } 
            //删除入库单
            Repository.ClientDb.Deleteable<InvInStorage>(b => orderNo.Contains(b.OrderNo)).AddQueue();
            Repository.ClientDb.Deleteable<InvInStorageDetail>(b => orderNo.Contains(b.OrderNo)).AddQueue(); 
            //删除审批记录
            Repository.ClientDb.Deleteable<ApprovalHis>(a => a.DataType == ApprovalDataType.InStorage.ToString() && orderNo.Contains(a.PrimaryId)).AddQueue(); 
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task UpdateActualQuantity(InvInStorageDetail detail)
        {  
            var detailInEntity = await Repository.ClientDb.Queryable<InvInStorageDetail>()
                .SingleAsync(s => s.OrderNo == detail.OrderNo && s.GoodsId == detail.GoodsId && s.BinId == detail.BinId && s.WorkbinCellId == detail.WorkbinCellId);
            if (detailInEntity != null)
            {
                detailInEntity.Quantity = detail.ActualQuantity;
                detailInEntity.ActualQuantity = detail.ActualQuantity;
                await Repository.ClientDb.Updateable(detailInEntity).ExecuteCommandAsync(); 
            }
        }

        /// <summary>
        /// 调度AGV入库
        /// </summary>
        /// <returns></returns>
        public async Task AGVScheduling(string orderNo)
        {
            var order = await Repository.GetSingeAsync<InvInStorage>(orderNo);
            var ordersDetail = await Repository.ClientDb.Queryable<InvInStorageDetail>().Where(u => orderNo == u.OrderNo).ToListAsync();
           //检查明细是否允许入库
            _validInStorage(order, ordersDetail); 
            _autoTransportHandler.CreateTransTask(new Models.Model.AutomationDevice.AutoTransTaskInput
            {
                OrderNo = orderNo,
                TaskType = AutoTransTaskType.InStorage.ToString(),
                BusinessType = order.InStorageType,
                TaskModel = AGVTaskModel.TransportSingle.ToString(), 
                GoodsClassifyGroup = order.GoodsClassify,
                OperatorId = order.UpdateUserId,
                OperatorName = order.UpdateUserName
            }); 
        }

        /// <summary>
        /// 导出入库单
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<string> ExportInStorage(string searchKey, string orderField, string orderType, string dateStart, string dateEnd,string goodsGroup)
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
                            a.OrderNo as 入库单号,
                            a.InStorageType as 入库类型, 
                            a.GoodsClassify as 大类,
                            d.TypeName as 小类,
                            c.GoodsName as 名称,
                            c.GoodsNo as SAP编码,
                            c.GoodsModel as 型号, 
                            c.Supplier as 供应商,
                            k.SupplierNo as 供应商编码,
                            b.Quantity as 计划入库数量,
                            b.ActualQuantity as 实际入库数量,
                            j.UnitName as 入库单位,
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
                            a.CreateDate as 创建时间,
                            a.CreateUserName as 创建人,
                            a.`Status` as 状态 ,
                            a.Remark as 备注,
                            ROUND(b.TotalPrice,2) as 入库总价,
                            ROUND(b.UnitPrice,2) as 入库单价,
                            b.PriceUnit as 价格单位
                            from invinstorage a
                            join invinstoragedetail b on a.OrderNo=b.OrderNo
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
            if(queryData!= null&& queryData.Rows?.Count>0)
            {
                foreach(DataRow row in queryData.Rows)
                {
                    if (row["入库类型"] != null)
                    {
                        var key = row["入库类型"].ToString();
                        row["入库类型"]=EnumHelper.GetDescFromEnumVal<InStorageType>(key);
                    }
                }
            }
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"入库单信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
            return fileUrl;
        }

        /// <summary>
        /// 审批入库单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isApprove"></param>
        /// <param name="opinion"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task ApprovalInStorage(string[] orderNo,bool isApprove,string opinion, string userId,string userName)
        {
            var approvalStatus = isApprove ? ApprovalStatus.Approve.ToString() : ApprovalStatus.Reject.ToString();
            var orders = await Repository.ClientDb.Queryable<InvInStorage>().Where(x => orderNo.Contains(x.OrderNo)).ToListAsync();
            foreach(var order in orders)
            {
                if(order.Status!=InStorageStatus.Pending.ToString()&& order.Status != InStorageStatus.Approvaling.ToString())
                {
                    throw new BusinessException("审批失败,当前存在未进入审批流程的入库单"); 
                }
            }
            //查询当前审批流程信息
            var process = await GetApprover(ApprovalDataType.InStorage.ToString(),userId); 
            if (process?.Count == 0)
            {
                throw new BusinessException("审批失败,当前用户没有审批权限"); 
            }
            var apprivalModel = process.First().ApprovalModel; 
            //判断上一级是否已审批
            if (apprivalModel == ApprovalModel.Process.ToString())
            {
                var approvalHis = await Repository.ClientDb.Queryable<ApprovalHis>().Where(a => a.DataType == ApprovalDataType.InStorage.ToString() && orderNo.Contains(a.PrimaryId)).ToListAsync();
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
            //更新入库单主表审批状态 
            var hightApprover = process.OrderByDescending(x => x.Rank).First(); 
            orders.ForEach(x =>
            {
                x.ApprovalDate = DateTime.Now;
                x.ApproverId = userId;
                x.ApproverName = userName;
                x.ApproverRole = hightApprover.ApproverRole;
                x.ApprovalStatus = approvalStatus; 
                x.Status = approvalStatus == ApprovalStatus.Approve.ToString() ? InStorageStatus.WaitInStorage.ToString() : approvalStatus;
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
                    DataType = ApprovalDataType.InStorage.ToString(),
                    Opinion= opinion,
                    ApprovalRank=  hightApprover.Rank,
                    PrimaryId = id
                }); 
            }
            Repository.ClientDb.Insertable(approvalHisList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 备件导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task SparePartExport(string fileName, string fileType, Stream stream)
        {
            DataTable dt = new DataTable();
            var fileUrl = string.Empty;
            MemoryStream fs = new MemoryStream();
            stream.CopyTo(fs);
            stream.Position = 0;
            fs.Seek(0, SeekOrigin.Begin);  
            dt = ExcelHelper.ConvertStreamToDataTable(stream, fileType, "", true, 0);
            if (dt == null || dt.Rows.Count == 0)
            { 
                throw new BusinessException("导入文件行数为空");
            }
            if (!dt.Columns.Contains("备件名称(CN)") || !dt.Columns.Contains("型号") || !dt.Columns.Contains("新库位号") || !dt.Columns.Contains("入库数量") || !dt.Columns.Contains("单位"))
            {
                throw new BusinessException("表头必须包含以下字段：备件名称(CN)、型号、新库位号、入库数量、单位");
            }
            var dataList = new List<StorageExportModel>();
            foreach (DataRow row in dt.Rows)
            {
                var model = new StorageExportModel();
                model.GoodsName = row["备件名称(CN)"].ToString().Trim();
                model.GoodsModel = row["型号"].ToString().Trim();
                model.BinNo = row["新库位号"].ToString().Trim();
                model.UnitName = row["单位"].ToString().Trim();
                float q;
                float.TryParse(row["入库数量"].ToString().Trim(), out q);
                if (string.IsNullOrEmpty(model.GoodsName) || string.IsNullOrEmpty(model.GoodsModel) || string.IsNullOrEmpty(model.BinNo) || string.IsNullOrEmpty(model.UnitName) || q <= 0)
                {
                    continue;
                }
                model.Quantity = q;
                dataList.Add(model);
            }
            if (dataList.Count > 0)
            {
                var goodsNameArr = dataList.Select(s => s.GoodsName).ToList();
                var unitArr = dataList.Select(s => s.UnitName).Distinct().ToList();
                var workbinArr = dataList.Select(s => s.BinNo).Distinct().ToList();
                var goodsEnity = await Repository.ClientDb.Queryable<BaseGoods>().Where(w => goodsNameArr.Contains(w.GoodsName)).ToListAsync();
                var unitEntity = await Repository.ClientDb.Queryable<BaseUnits>().Where(w => unitArr.Contains(w.UnitName)).ToListAsync();
                var workbinEntity = await Repository.ClientDb.Queryable<InvWorkbinCell>().Where(w => workbinArr.Contains(w.CellNo)).ToListAsync();
                if (goodsEnity.Count == 0 || unitEntity.Count == 0 || workbinEntity.Count == 0)
                {
                    throw new BusinessException("本次导入的数据与系统基础数据不匹配，请对照系统基础数据进行修改");
                }
                var warehouseArr = workbinEntity.Select(s => s.WarehouseId).Distinct().ToList();
                double detailCapacity = 40;
                var orderCount = Math.Ceiling((dataList.Count / detailCapacity)); 
                var orderList = new List<InvInStorage>();
                var lastData = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo);
                var curOrderNo = GetPrimaryId("I", lastData);
                foreach (var w in warehouseArr)
                {
                    for(var i=0;i< orderCount; i++)
                    {
                        orderList.Add(new InvInStorage
                        {
                            OrderNo = curOrderNo,
                            InStorageType = InStorageType.InitialIn.ToString(),
                            GoodsClassify = BaseTypeGroup.SparePart.ToString(),
                            CreateDate = DateTime.Now,
                            WarehouseId = w,
                            Status = InStorageStatus.WaitInStorage.ToString(),
                            ApprovalStatus = ApprovalStatus.NoApproval.ToString(),
                            CreateUserId=BusinessConst.UserAdmin,
                            CreateUserName="管理员",
                            
                        });
                        curOrderNo = GetPrimaryId("I", curOrderNo);
                    }
                   
                }
                var orderDetailList = new List<InvInStorageDetail>();
                int index = 1;
                foreach (var data in dataList)
                {
                    if(goodsEnity.Count(s => s.GoodsName == data.GoodsName && s.GoodsModel == data.GoodsModel) > 1)
                    { 
                        throw new BusinessException($"系统检测到备件：[{data.GoodsName}{data.GoodsModel}]存在多个记录");
                    }
                    var curGoods = goodsEnity.SingleOrDefault(s => s.GoodsName == data.GoodsName && s.GoodsModel == data.GoodsModel);
                    if (curGoods == null)
                    {
                        throw new BusinessException($"系统未检测到备件：[{data.GoodsName}{data.GoodsModel}]");
                    }
                    var curUnit = unitEntity.SingleOrDefault(s => s.UnitName == data.UnitName);
                    if (curUnit == null)
                    {
                        throw new BusinessException($"系统未检测到单位名称：{data.UnitName}");
                    }
                    var curWorkbin = workbinEntity.SingleOrDefault(s => s.CellNo == data.BinNo);
                    if (curWorkbin == null)
                    {
                        throw new BusinessException($"系统未检测到料箱编码：{data.BinNo}");
                    }

                    var curOrder = orderList.Where(s => s.WarehouseId == curWorkbin.WarehouseId && s.Remark!= detailCapacity.ToString()).OrderByDescending(s => s.Remark).First();
                    curOrder.Remark = index.ToString();
                    orderDetailList.Add(new InvInStorageDetail
                    {
                        OrderNo = curOrder.OrderNo,
                        GoodsId = curGoods.GoodsId,
                        GoodsName = curGoods.GoodsName,
                        Quantity = data.Quantity,
                        UnitId = curUnit.UnitId,
                        WarehouseId = curWorkbin.WarehouseId,
                        ShelfId = curWorkbin.ShelfId,
                        BinId = curWorkbin.BinId,
                        WorkbinId = curWorkbin.WorkbinId,
                        WorkbinCellId = curWorkbin.CellId
                    });
                    index++;
                    if (index > detailCapacity)
                        index = 1;
                }
                Repository.ClientDb.Insertable(orderList).AddQueue();
                Repository.ClientDb.Insertable(orderDetailList).AddQueue();
                await Repository.ClientDb.SaveQueuesAsync();
            }
        } 
    }
}
