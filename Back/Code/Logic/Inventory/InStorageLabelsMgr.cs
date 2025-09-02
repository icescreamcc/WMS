using DbRepository.Repository.DbModels;
using DbRepository.Repository;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.LogicBase;
using Models.Model.Inv;
using AutoMapper;

namespace Logic.Inventory
{
    public class InStorageLabelsMgr: DbOperationHandler
    {
        private readonly IMapper _mapper;
         

        public InStorageLabelsMgr(Repository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }


        /// <summary>
        /// 查询所有物品列表
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="goodsGroup"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<TableModel<GoodsSimple>> GetGoodsByKey(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId, string keyword)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            keyword = string.IsNullOrEmpty(keyword) ? "" : keyword.Trim();
            var data = Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword) || t.TypeName.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && t.Group == goodsGroup) 
                .WhereIF(goodsClassifyId > 0, (g, t, wbs) => g.GoodsClassifyId == goodsClassifyId)
                .Select((g, t, wbs) => new GoodsSimple
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitId=g.PackageUnitId,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitId=g.MinPackageUnitId,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitId=g.MaxPackageUnitId,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    IsTakeStockLock = g.IsTakeStockLock,
                    LastInventoryDate = g.LastInventoryDate,
                    LastInventoryOperator = g.LastInventoryOperator,
                    IsUnSubmitLabels=SqlFunc.Subqueryable<InvInStorageLabels>().Where(l=>l.GoodsId==g.GoodsId&&l.Status== InStorageLabelStatus.Scan.ToString()).Any(),
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier = g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).OrderBy($"{orderFiled} {orderType}").ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<GoodsSimple>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 根据物品查询推荐入库货位
        /// 只查询已存放该物品的货位
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<List<RecommendWorkbinCellDto>> GetWorkbinRecommend(string goodsId)
        {
            var query1 = Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
              .LeftJoin<InvWarehouse>((d, w) => w.WarehouseId == d.WarehouseId)
              .LeftJoin<InvShelf>((d, w, s) => s.ShelfId == d.ShelfId)
              .LeftJoin<InvBin>((d, w, s, b) => b.BinId == d.BinId)
              .LeftJoin<InvWorkbin>((d, w, s, b, wb) => wb.WorkbinId == d.WorkbinId)
              .LeftJoin<InvWorkbinCell>((d, w, s, b, wb, wbc) => wbc.CellId == d.WorkbinCellId)
              .LeftJoin<BaseUnits>((d, w, s, b, wb, wbc, u) => u.UnitId == d.UnitId)
              .LeftJoin<InvWorkbinSpecification>((d, w, s, b, wb, wbc, u, wbs) => wbs.SpecId == wb.SpecId)
              .LeftJoin<BaseGoods>((d, w, s, b, wb, wbc, u, wbs, g) => g.GoodsId == d.GoodsId)
              .Where((d, w, s, b, wb, wbc, u, wbs, g) => d.GoodsId == goodsId)
              .Select((d, w, s, b, wb, wbc, u, wbs, g) => new RecommendWorkbinCellDto
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
        /// 查询当前物品已扫码未提交的数据
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<List<InStorageLabelsDto>> GetUnSubmitCodes(string goodsId)
        {
            return await Repository.ClientDb.Queryable<InvInStorageLabels>()
                .LeftJoin<BaseGoods>((l,g)=>g.GoodsId==goodsId)
                .LeftJoin<BaseType>((l,g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvBin>((l,g,t,b)=>b.BinId==l.BinId)
                .LeftJoin<InvWorkbinCell>((l,g,t,b,wbc)=>wbc.CellId==l.WorkbinCellId)
                .Where((l, g, t, b, wbc) => l.Status == InStorageLabelStatus.Scan.ToString() && l.GoodsId== goodsId)
                .Select((l, g, t, b, wbc) => new InStorageLabelsDto
                {
                    GoodsId= g.GoodsId,
                    GoodsNo=g.GoodsNo,
                    GoodsName=g.GoodsName,
                    GoodsModel= g.GoodsModel,
                    GoodsClassifyGroup=t.Group,
                    GoodsClassifyName=t.TypeName,
                    CodeString=l.CodeString,
                    WarehouseId= l.WarehouseId,
                    ShelfId= l.ShelfId,
                    BinId= l.BinId,
                    BinNo=b.BinNo,
                    WorkbinId= l.WorkbinId,
                    WorkbinCellId= l.WorkbinCellId,
                    WorkbinCellNo=wbc.CellNo,
                    UnitId= l.UnitId,
                    UnitName= l.UnitName 
                }).ToListAsync();
        }

        /// <summary>
        /// 扫码货位码，查询当前货位是否可以存放指定的物品
        /// </summary>
        /// <param name="binNo"></param>
        /// <returns>返回货位信息，可能是料箱也可能是库位</returns>
        public async Task<StorageBin> ScanBinCheck(string scanNo,string goodsId)
        { 
            var storageBin = await Repository.ClientDb.Queryable<InvBin>()
                .LeftJoin<InvWorkbinCell>((b, wbc) => wbc.BinId == b.BinId)
                .Where((b, wbc) => b.BinNo == scanNo || wbc.CellNo == scanNo)
                .Select<StorageBin>()
                .SingleAsync();
            var goods = await Repository.GetSingeAsync<BaseGoods>(goodsId);
            //验证：指定的物品是否在盘点中
            if (goods == null)
            {
                throw new BusinessException($"保存失败，未查询到物料信息");
            }
            //验证：货位码是否有效
            if (storageBin == null)
            {
                throw new BusinessException($"保存失败，未查询到编号为{scanNo}的货位");
            }
            //验证：货位是否在盘点
            if (storageBin.IsTakeStockLock)
            {
                throw new BusinessException($"保存失败，货位{scanNo}正在盘点中，暂停入库");
            }
            if (goods.IsTakeStockLock)
            {
                throw new BusinessException($"保存失败，{goods.GoodsName + goods.GoodsModel}正在盘点中，暂停入库");
            }
            //验证：是否存在不同物品入库到相同货位或料箱
            if (!storageBin.IsVarietyStock)
            {
                if (storageBin.CellId > 0)
                {
                    var stockInfo = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
                          .Where(w => w.WorkbinCellId == storageBin.CellId && w.Stock > 0 && w.GoodsId!= goodsId)
                          .Select(s => s.GoodsId)
                          .Distinct()
                          .ToListAsync();
                    if (stockInfo?.Count > 0)
                    {
                        throw new BusinessException("保存失败,当前货位存在其他型号物料且不允许多样存放，请选择其他推荐货位");
                    }
                }
                else
                {
                    var stockInfo = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
                          .Where(w => w.BinId == storageBin.BinId && w.Stock > 0 && w.GoodsId != goodsId)
                          .Select(s => s.GoodsId)
                          .ToListAsync();
                    if (stockInfo?.Count > 0)
                    {
                        throw new BusinessException("保存失败,当前货位存在其他型号物料且不允许多样存放，请选择其他推荐货位");
                    }
                }
              
            }
            //验证：物品指定存放规格是否和指定的料箱匹配 
            if (storageBin.CellId > 0)
            {
                if (goods.IsConstraintSpec)
                {
                    var workbin = await Repository.GetSingeAsync<InvWorkbin>(storageBin.WorkbinId);
                    if(workbin!=null&& workbin.SpecId != goods.GoodsSpecificationId)
                    {
                        throw new BusinessException($"保存失败,当前货位规格与{goods.GoodsName}指定存放规格不匹配，请选择其他推荐货位");
                    } 
                }
            }
            return storageBin;
        }

        /// <summary>
        /// 扫码记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task SubmitScan(InStorageLabelsDto data)
        {
            if (string.IsNullOrEmpty(data.CodeString))
            {
                throw new BusinessException("请扫描标签码");
            }
            if (string.IsNullOrEmpty(data.GoodsId))
            {
                throw new BusinessException("请选择入库物品");
            }
            var exists = await Repository.ClientDb.Queryable<InvInStorageLabels>().AnyAsync(a => a.CodeString == data.CodeString);
            if (exists)
            {
                throw new BusinessException("该标签码已使用");
            }
            var entity = _mapper.Map<InvInStorageLabels>(data);
            if (!string.IsNullOrEmpty(data.ScanNo))
            {
                var storageBin = await ScanBinCheck(data.ScanNo, data.GoodsId);
                entity.WarehouseId = storageBin.WarehouseId;
                entity.ShelfId = storageBin.ShelfId;
                entity.BinId = storageBin.BinId;
                entity.WorkbinId = storageBin.WorkbinId;
                entity.WorkbinCellId = storageBin.CellId;
            }
            entity.Quantity = 1;
            entity.Status = InStorageLabelStatus.Scan.ToString();
            entity.CreateDate = DateTime.Now;
            await Repository.ClientDb.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 扫码提交
        /// </summary>
        /// <returns></returns>
        public async Task SubmitCode(InStorageLabelSubmitDto data)
        {
            var goods = await Repository.ClientDb.Queryable<BaseGoods>()
                   .InnerJoin<BaseType>((g, t) => t.TypeId == g.GoodsClassifyId)
                   .Where((g, t) => g.GoodsId == data.GoodsId && g.IsDeleted==false)
                   .Select((g, t) => new { g.GoodsName, t.Group })
                   .SingleAsync();
            if (goods == null)
            {
                throw new BusinessException("物品不存在或已被删除");
            }
            var curDate = DateTime.Now;
            //更新标签状态
            var unSubmitCodes = await Repository.ClientDb.Queryable<InvInStorageLabels>()
                .Where(w => w.Status == InStorageLabelStatus.Scan.ToString() && w.GoodsId == data.GoodsId)
                .Select<InStorageLabelsDto>()
                .ToListAsync();
            if(unSubmitCodes?.Count > 0)
            {
                unSubmitCodes.ForEach(f =>
                {
                    f.Status = InStorageLabelStatus.Submit.ToString();
                    f.UpdateDate = curDate;
                    f.UpdateUserId = data.UserId;
                    f.UpdateUserName = data.UserName;
                });
                //更新标签码状态
                Repository.ClientDb.Updateable(unSubmitCodes).AddQueue();
                //写入库单 
                var lastData = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo); 
                var inStorageModel = new InvInStorage
                {
                    OrderNo = GetPrimaryId("I", lastData),
                   // SourceOrderNo =,
                    InStorageType =string.IsNullOrEmpty(data.InStorageType)? InStorageType.InitialIn.ToString(): data.InStorageType, 
                    GoodsClassify = goods.Group,
                    CreateDate = curDate,
                    CreateUserId = data.UserId,
                    CreateUserName = data.UserId,
                    Remark = "扫码入库",
                    Status= InStorageStatus.WaitInStorage.ToString(),
                    ApprovalStatus = ApprovalStatus.NoApproval.ToString()
                };
                Repository.ClientDb.Insertable(inStorageModel).AddQueue();
                //写入新的入库单明细 
                var groupByUnitDetails = unSubmitCodes.GroupBy(g => new
                {
                    g.GoodsId, 
                    g.UnitId,
                    g.WarehouseId,
                    g.BinId,
                    g.WorkbinCellId
                }).Select(s => new
                {
                    s.Key.GoodsId,
                    s.Key.UnitId,
                    s.Key.WarehouseId,
                    s.Key.BinId,
                    s.Key.WorkbinCellId,
                    Quantity = s.Sum(sum => sum.Quantity)
                }) .ToList();
                var inStorageDetail = groupByUnitDetails.Select(b => new InvInStorageDetail
                {
                    OrderNo = inStorageModel.OrderNo,
                    GoodsId = b.GoodsId,
                    GoodsName = goods.GoodsName,
                    Quantity = b.Quantity,
                    UnitId = b.UnitId
                }).ToList();
                Repository.ClientDb.Insertable(inStorageDetail).AddQueue();
            } 
            await Repository.ClientDb.SaveQueuesAsync();
        } 
    }
}
