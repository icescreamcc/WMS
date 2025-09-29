using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Model.EchartsModel;
using External.Common;
using NPOI.SS.Formula.Functions;
using System.Security.Policy;
using System.IO;
using NPOI.SS.UserModel;

namespace Logic.BaseInfo
{
    public class GoodsMgr : DataPermissionHandler
    {
        public GoodsMgr(Repository repository) : base(repository)
        {
        }

        /// <summary>
        /// 分页查询物品列表
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsGroup"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<TableModel<GoodsSimple>> GetGoodsList(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId, string shelfId, int binId, string keyword)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            keyword = string.IsNullOrEmpty(keyword) ? "" : keyword.Trim();
            var data = Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword) || t.TypeName.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && t.Group == goodsGroup)
                .WhereIF(!string.IsNullOrEmpty(shelfId), (g, t, wbs) => SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sw => sw.GoodsId == g.GoodsId && sw.ShelfId == shelfId && sw.Stock > 0).Any())
                .WhereIF(binId > 0, (g, t, wbs) => SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sw => sw.GoodsId == g.GoodsId && sw.BinId == binId).Any())
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
                    IsInSAP = g.IsInSAP,
                    ProductDate = g.ProductDate,
                    ExpirationDate = g.ExpirationDate,
                    PurchaseCycle = g.PurchaseCycle,
                    PurchaseMinimum = g.PurchaseMinimum,
                    PurchaseMinimumUnitName = g.PurchaseMinimumUnitName,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    IsTakeStockLock = g.IsTakeStockLock,
                    LastInventoryDate = g.LastInventoryDate,
                    LastInventoryOperator = g.LastInventoryOperator,
                    Supplier = g.Supplier,
                    CustomerGoodsNo=g.CustomerGoodsNo,
                    CustomerIdentificationCode=g.CustomerIdentificationCode,
                    PackageUnitId =g.PackageUnitId,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).OrderBy($"{orderFiled} {orderType}").ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<GoodsSimple>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 获取物品明细
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<GoodsDetailDto> GetGoodsDetail(string goodsId)
        {
            var data = await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<SysArgsOptions>((g, a) => a.OptionId == g.GoodsTypeId)
                .LeftJoin<BaseType>((g, a, a2) => a2.TypeId == g.GoodsClassifyId)
                .LeftJoin<InvWorkbinSpecification>((g, a, a2, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, a, a2, wbs) => g.GoodsId == goodsId)
                .Select((g, a, a2, wbs) => new GoodsDetailDto
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsTypeId = g.GoodsTypeId,
                    GoodsTypeName = a.OptionName,
                    GoodsClassifyId = g.GoodsClassifyId,
                    GoodsClassifyName = a2.TypeName,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    MaxStock = g.MaxStock,
                    PackageCount = g.PackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    MaxPackageCount = g.MaxPackageCount,
                    Long = g.Long,
                    Wide = g.Wide,
                    Height = g.Height,
                    SizeUnitName = g.SizeUnitName,
                    Weight = g.Weight,
                    WeightUnitName = g.WeightUnitName,
                    Color = g.Color,
                    Source = g.Source,
                    CostPrice = g.CostPrice,
                    CostPriceUnitName = g.CostPriceUnitName,
                    RefPurchPrice = g.RefPurchPrice,
                    RefPurchPriceUnitName = g.RefPurchPriceUnitName,
                    PriceUnitName = g.PriceUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    Direction = g.Direction,
                    ProductDate = g.ProductDate,
                    ExpirationDate = g.ExpirationDate,
                    Remark = g.Remark,
                    IsInSAP = g.IsInSAP,
                    PurchaseCycle = g.PurchaseCycle,
                    PurchaseMinimum = g.PurchaseMinimum,
                    PurchaseMinimumUnitName = g.PurchaseMinimumUnitName,
                    IsOldForNew = g.IsOldForNew,
                    CreateDate = g.CreateDate,
                    CreateUser = g.CreateUser,
                    ModifyUser = g.ModifyUser,
                    ModifyDate = g.ModifyDate,
                    ForArea = g.ForArea,
                    Supplier = g.Supplier,
                    CustomerGoodsNo = g.CustomerGoodsNo,
                    CustomerIdentificationCode = g.CustomerIdentificationCode,
                    IsValid = g.IsValid,
                    GoodsField1 = g.GoodsField1,
                    GoodsField2 = g.GoodsField2,
                    GoodsField3 = g.GoodsField3,
                    GoodsField4 = g.GoodsField4,
                    GoodsField5 = g.GoodsField5
                }).SingleAsync();
            data.Photos = await Repository.ClientDb.Queryable<BaseFiles>()
                .Where(p => data.GoodsId == p.PrimaryId && p.FileInfoType == FileInfoType.GoodsPhoto.ToString())
                .Select(p => new FileInfoDto { FileId = p.FileId, FileName = p.FileName, Url = p.Url, FileInfoType = p.FileInfoType, Path = p.Path, PrimaryId = p.PrimaryId, Remark = p.Remark })
                .ToListAsync();
            return data;
        }

        /// <summary>
        /// 更新物品图片
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="Photos"></param>
        /// <returns></returns>
        public async Task UpdateGoodsPhoto(string goodsId, List<FileInfoDto> Photos)
        {
            Repository.ClientDb.Deleteable<BaseFiles>(x => x.PrimaryId == goodsId && x.FileInfoType == FileInfoType.GoodsPhoto.ToString()).AddQueue();
            if (Photos?.Count > 0)
            {
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                Photos.ForEach(p =>
                {
                    photos.Add(new BaseFiles { PrimaryId = goodsId, FileName = p.FileName, FileInfoType = FileInfoType.GoodsPhoto.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                    isSetDeft = true;
                });
                Repository.ClientDb.Insertable(photos).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 获取所有指定分类的物品（用于外部系统数据对接）
        /// </summary>
        /// <param name="goodsGroup"></param>
        /// <returns></returns>
        public async Task<List<GoodsExternalDto>> GetAllGoods(string goodsGroup)
        {
            return await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => t.TypeId == g.GoodsClassifyId)
                .Where((g, t) => t.Group == goodsGroup)
                .Select((g, t) => new GoodsExternalDto
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsModel = g.GoodsModel,
                    GoodsClassifyId = g.GoodsClassifyId,
                    GoodsClassifyName = t.TypeName,
                    GoodsProperty = g.GoodsProperty,
                    GoodsTypeId = g.GoodsTypeId,
                    ModifyDate = g.ModifyDate,
                    Supplier = g.Supplier
                }).ToListAsync();
        }

        /// <summary>
        /// 物品消耗雷达图数据
        /// </summary>
        /// <param name="classifyGroup"></param>
        /// <returns></returns>
        public async Task<List<TreeMapModel>> GetGoodsTreeMapData(string classifyGroup)
        {
            var parents = new List<TreeMapModel>();
            var queryData = await Repository.ClientDb.Queryable<BaseGoods>()
                .InnerJoin<BaseType>((g, t) => t.TypeId == g.GoodsClassifyId)
                .InnerJoin<InvStorageWarehouseDetail>((g, t, s) => s.GoodsId == g.GoodsId && s.Stock > 0)
                .LeftJoin<BaseUnits>((g, t, s, u) => u.UnitId == s.UnitId)
                .Where((g, t, s, u) => g.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(classifyGroup), (g, t, s, u) => t.Group == classifyGroup)
                .GroupBy((g, t, s, u) => new { t.Group, g.GoodsId, g.GoodsName, g.GoodsModel, t.TypeName, u.UnitName })
                .Select((g, t, s, u) => new
                {
                    g.GoodsId,
                    g.GoodsName,
                    g.GoodsModel,
                    t.Group,
                    t.TypeName,
                    Stock = SqlFunc.AggregateSum(SqlFunc.IsNullOrEmpty(s.Stock) ? 0 : s.Stock),
                    u.UnitName
                })
                .ToListAsync();
            if (queryData?.Count > 0)
            {
                parents = queryData.GroupBy(g => new { g.Group }).Select(p => new TreeMapModel { Remark = p.Key.Group }).ToList();
                foreach (var parent in parents)
                {
                    parent.Name = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(parent.Remark);
                    parent.Path = parent.Name;
                    parent.Value = queryData.Where(c => c.Group == parent.Remark).Sum(s => s.Stock);
                    parent.Children = queryData.Where(w => w.Group == parent.Remark).GroupBy(g => new { g.TypeName }).Select(s => new TreeMapModel { Name = s.Key.TypeName, Path = s.Key.TypeName }).ToList();
                    foreach (var child in parent.Children)
                    {
                        child.Value = queryData.Where(c => c.Group == parent.Remark && c.TypeName == child.Name).Sum(s => s.Stock);
                        child.Children = queryData.Where(w => w.Group == parent.Remark && w.TypeName == child.Name).Select(w => new TreeMapModel { Id = w.GoodsId, Name = w.GoodsName, Path = w.GoodsName, Remark = w.UnitName }).ToList();
                        foreach (var child2 in child.Children)
                        {
                            var goods = queryData.Where(w => w.Group == parent.Remark && w.TypeName == child.Name && w.GoodsId == child2.Id && w.UnitName == child2.Remark).Single();
                            child2.Value = goods.Stock;
                            child2.Remark = goods.UnitName;
                            child2.Props = goods.GoodsModel;
                            if (!string.IsNullOrEmpty(child2.Props))
                            {
                                child2.Name += $"({child2.Props})";
                            }
                        }
                    }
                }
            }
            return parents;
        }

        public async Task<TableModel<BaseFilesDto>> GetBaseFiles(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey,string orderNo, string fileInfoType)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "FileId" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<BaseFiles>()
                  .Where(p => p.FileName.Contains(searchKey) || p.Url.Contains(searchKey))
                  .Where(p => p.PrimaryId==orderNo && p.FileInfoType == fileInfoType)
                  .Select(p => new BaseFilesDto
                  {
                      FileId = p.FileId,
                      FileName = p.FileName,
                      FileInfoType = p.FileInfoType,
                      PrimaryId = p.PrimaryId,
                      Url = p.Url,
                      Path = p.Path,
                      IsDeft = p.IsDeft,
                      Remark = p.Remark,
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<BaseFilesDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task DelBaseFiles(int[] fileIds)
        {
            var details = await Repository.ClientDb.Queryable<BaseFiles>().Where(w => fileIds.Contains(w.FileId)).ToListAsync();
            Repository.ClientDb.Deleteable(details).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

    }
}
