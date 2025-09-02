using CommunityToolkit.HighPerformance.Helpers;
using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using External.Common;
using Logic.LogicBase;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Models.Model;
using Models.Model.AutomationDevice;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Inv;
using Models.Model.Purchase;
using Models.Model.Sys;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicCommon
{
    /// <summary>
    /// 用于单选、多选、模糊查询等列表选项数据查询
    /// </summary>
  public  class SelectOptionsService: DbOperationHandler
    {
        private readonly SysArgsService _sysArgsHelper;

        private readonly FileHelper _fileHelper;

        public SelectOptionsService(Repository repository, SysArgsService sysArgsHelper, FileHelper fileHelper) : base(repository)
        {
            _sysArgsHelper = sysArgsHelper;
            _fileHelper = fileHelper;
        }

        /// <summary>
        /// 获取所有部门 
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetDeptOptions()
        {
           var res=  Repository.ClientDb.Queryable<SysDepartments>().Select(d=>new KeyValueModel {Key=d.DeptId,Value=d.DeptName,Remark=d.DeptNo }).ToList(); 
            return await Task.FromResult(res);
        } 

        /// <summary>
        /// 获取所有省份 
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetProvinces()
        {
            var res =  Repository.ClientDb.Queryable<SysProvince>().Select(d => new KeyValueModel { Key = d.ProvinceId, Value = d.ProvinceName, Remark = d.ProvinceShotName }).ToList();
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 根据省份查询对应的城市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetCitysByProvince(int provinceId)
        {
            return await Repository.ClientDb.Queryable<SysCity>().Where(c => c.ProvinceId == provinceId).Select(c => new KeyValueModel { Key = c.CityId, Value = c.CityName }).ToListAsync();
        }

        /// <summary>
        /// 根据城市查询对应的区县
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetAreasByCity(int cityId)
        {
            return await Repository.ClientDb.Queryable<SysArea>().Where(c => c.CityId == cityId).Select(c => new KeyValueModel { Key = c.AreaId, Value = c.AreaName }).ToListAsync();
        }

        /// <summary>
        /// 根据单位类型获取单位
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetUnits( string unitType="")
        {
            var res= await Repository.ClientDb.Queryable<BaseUnits>().Where(c=>c.UnitType.Contains(unitType)&&c.IsValid).Select(c => new KeyValueModel { Key = c.UnitId, Value = c.UnitName,Remark=c.UnitNo,Type=c.UnitType }).ToListAsync();
            return res.OrderBy(x => x.Remark).ToList();
        }

        /// <summary>
        /// 获取物品分类
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetGoodsClassify(string group)
        {
            return await Repository.ClientDb.Queryable<BaseType>()
                .Where(w=>w.Group == group)
                .Select(s => new KeyValueModel { Key = s.TypeId, Value = s.TypeName }).ToListAsync();
        }

        /// <summary>
        /// 根据关键字查询用户信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUserByKey(string keyword, int limit = 20)
        {
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim();
            return await Repository.ClientDb.Queryable<SysUser>()
                .Where(u => u.UserCode.Contains(keyword) || u.UserName.Contains(keyword) || u.NickName.Contains(keyword) || u.AuthAccount.Contains(keyword) || u.DomainName.Contains(keyword))
                .Where(u => u.IsVaild == true)
                .OrderBy(u => u.UserName)
                .Take(limit)
                .Select<User>()
                .ToListAsync();
        }

        /// <summary>
        /// 根据关键字查询商品
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns> 
        public async Task<List<GoodsSimple>> GetGoodsByKey(string goodsClassify, string keyword, int limit = 20)
        {
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim();
            return await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)  
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs)=>wbs.SpecId==g.GoodsSpecificationId)
                .Where((g, t,wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword) || t.TypeName.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && g.IsValid==true && t.Group== goodsClassify)
                .OrderBy((g,t, wbs) =>new { g.GoodsClassifyId ,g.GoodsName})
                .Take(limit)
                .Select((g, t, wbs) => new GoodsSimple
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName=wbs.SpecName,
                    IsConstraintSpec=g.IsConstraintSpec,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName, 
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier =g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).ToListAsync();
        }

        /// <summary>
        /// 根据关键字查询商品(模糊查询、大类、小类、是否在SAP)
        /// </summary>
        /// <param name="goodsClassify"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="isSAP"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<GoodsSimple>> GetGoodsByKey(string goodsClassify, int goodsClassifyId,string isSAP, string keyword, int limit = 40)
        { 
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim();
            return await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && g.IsValid == true && t.Group == goodsClassify) 
                .WhereIF(!string.IsNullOrEmpty(isSAP), (g, t, wbs)=>g.IsInSAP==bool.Parse(isSAP))
                .WhereIF(goodsClassifyId > 0, (g, t, wbs) => g.GoodsClassifyId == goodsClassifyId) 
                .OrderBy((g, t, wbs) => new { g.GoodsClassifyId, g.GoodsName })
                .Take(limit)
                .Select((g, t, wbs) => new GoodsSimple
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    CustomerGoodsNo=g.CustomerGoodsNo,
                    CustomerIdentificationCode = g.CustomerIdentificationCode,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    IsConstraintSpec=g.IsConstraintSpec,
                    IsInSAP = g.IsInSAP,
                    IsTakeStockLock = g.IsTakeStockLock,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    CostPrice=g.CostPrice,
                    PriceUnitName=g.PriceUnitName,
                    CostPriceUnitName = g.CostPriceUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    GoodsField1=g.GoodsField1,
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier = g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).ToListAsync();
        }

        public async Task<List<GoodsSimple>> GetGoodsAutoSort(string goodsClassify, int goodsClassifyId, string area, string keyword, int limit = 30)
        {
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim();
            var data= await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && g.IsValid == true && t.Group == goodsClassify)
                .WhereIF(!string.IsNullOrEmpty(area), (g, t, wbs) => g.ForArea == area)
                .WhereIF(goodsClassifyId > 0, (g, t, wbs) => g.GoodsClassifyId == goodsClassifyId)
                .OrderBy((g, t, wbs) => new { g.GoodsClassifyId, g.GoodsName })
                .Take(limit)
                .Select((g, t, wbs) => new GoodsSimple
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    IsConstraintSpec = g.IsConstraintSpec,
                    IsInSAP = g.IsInSAP,
                    IsTakeStockLock = g.IsTakeStockLock,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    DeftStockBin= SqlFunc.Subqueryable<InvStorageWarehouseDetail>()
                    .InnerJoin<InvBin>((sud,sb)=>sud.BinId==sb.BinId)
                    .Where((sud, sb) => sud.GoodsId == g.GoodsId&&sud.Stock>0)
                    .SelectStringJoin((sud, sb)=>sb.BinNo,","),
                    DeftStockBinCell= SqlFunc.Subqueryable<InvStorageWarehouseDetail>()
                    .InnerJoin<InvWorkbinCell>((sud, sb) => sud.WorkbinCellId == sb.CellId)
                    .Where((sud, sb) => sud.GoodsId == g.GoodsId && sud.Stock > 0)
                    .SelectStringJoin((sud, sb) => sb.CellNo, ","),
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier = g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).ToListAsync();
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.GoodsPicture))
                    item.GoodsPicture = await _fileHelper.CompressImageFromUrlAsync(item.GoodsPicture, 30);
            } 
            return data;
        }

        /// <summary>
        /// 分页查询物料信息
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassify"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="area"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<TableModel<GoodsSimple>> GetGoodsByPage(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsClassify, int goodsClassifyId, string area, string keyword)
        {

            int total = 0;
            orderType = "desc";
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "StandardPackageStock" : orderFiled;
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim();
            var data = Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && g.IsValid == true && t.Group == goodsClassify)
                .WhereIF(!string.IsNullOrEmpty(area), (g, t, wbs) => g.ForArea == area)
                .WhereIF(goodsClassifyId > 0, (g, t, wbs) => g.GoodsClassifyId == goodsClassifyId)
               // .OrderBy((g, t, wbs) => new { g.GoodsClassifyId, g.GoodsName })
                .Select((g, t, wbs) => new GoodsSimple
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    IsConstraintSpec = g.IsConstraintSpec,
                    IsInSAP = g.IsInSAP,
                    IsTakeStockLock = g.IsTakeStockLock,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    DeftStockBin = SqlFunc.Subqueryable<InvStorageWarehouseDetail>()
                    .InnerJoin<InvBin>((sud, sb) => sud.BinId == sb.BinId)
                    .Where((sud, sb) => sud.GoodsId == g.GoodsId && sud.Stock > 0)
                    .SelectStringJoin((sud, sb) => sb.BinNo, ","),
                    DeftStockBinCell = SqlFunc.Subqueryable<InvStorageWarehouseDetail>()
                    .InnerJoin<InvWorkbinCell>((sud, sb) => sud.WorkbinCellId == sb.CellId)
                    .Where((sud, sb) => sud.GoodsId == g.GoodsId && sud.Stock > 0)
                    .SelectStringJoin((sud, sb) => sb.CellNo, ","),
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier = g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<GoodsSimple>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 分页查询物品
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="goodsClassify"></param>
        /// <param name="goodsClassifyId"></param>
        /// <param name="isSAP"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<TableModel<GoodsSimple>> GetGoodsByKey(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsClassify, int goodsClassifyId, string isSAP, string keyword)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim();
            var data = Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && g.IsValid == true && t.Group == goodsClassify)
                .WhereIF(!string.IsNullOrEmpty(isSAP), (g, t, wbs) => g.IsInSAP == bool.Parse(isSAP))
                .WhereIF(goodsClassifyId > 0, (g, t, wbs) => g.GoodsClassifyId == goodsClassifyId)
                .OrderBy((g, t, wbs) => new { g.GoodsClassifyId, g.GoodsName }) 
                .Select((g, t, wbs) => new GoodsSimple
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    IsConstraintSpec = g.IsConstraintSpec,
                    IsInSAP = g.IsInSAP,
                    IsTakeStockLock = g.IsTakeStockLock,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier = g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<GoodsSimple>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }


        //public async Task<TableModel<AutoProdTaskTrackingDto>> GetOrders(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd, int purchaseTypeId, string flowStatus, string goodsClassifyGroup, int goodsClassifyId)
        //{
        //    int total = 0;
        //    orderFiled = string.IsNullOrEmpty(orderFiled) ? "TaskCreateTime" : orderFiled;
        //    searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
        //    var data = Repository.ClientDb.Queryable<AutoProdTaskTracking>()
        //          .Select((p) => new AutoProdTaskTrackingDto
        //          {
        //              TaskId=p.TaskId,
        //              TaskCode=p.TaskCode,
        //              AGVReqCode=p.AGVReqCode,
        //              LineNo=p.LineNo,
        //              OrderNo=p.OrderNo,

        //              BusinessType=p.BusinessType,
        //              GoodsInfo = p.GoodsInfo,
        //              TaskType=p.TaskType,
        //              ActionType=p.ActionType,
        //              IsExecuting=p.IsExecuting,
        //              TaskModel=p.TaskModel,
        //              TaskStatus=p.TaskStatus,
        //              IsReturn=p.IsReturn,
        //              ReturnTaskStatus=p.ReturnTaskStatus,
        //              StartingDeviceNo=p.StartingDeviceNo,
        //          })
        //          .OrderBy($"{orderFiled} {orderType}")
        //          .ToPageList(pgIndex, pgSize, ref total);

        //    var res = new TableModel<AutoProdTaskTrackingDto>() { Total = total, Rows = data };
        //    return await Task.FromResult(res);
        //}



        /// <summary>
        /// 根据物料编码查询物料信息
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsClassify"></param>
        /// <returns></returns>
        public async Task<GoodsSimple> GetGoodsById(string goodsId, string goodsClassify)
        { 

            return await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId) 
                .Where((g, t, wbs) => g.GoodsId == goodsId && t.Group == goodsClassify) 
                .OrderBy((g, t, wbs) => new { g.GoodsClassifyId, g.GoodsName }) 
                .Select((g, t, wbs) => new GoodsSimple
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    IsConstraintSpec = g.IsConstraintSpec,
                    IsInSAP = g.IsInSAP,
                    IsTakeStockLock = g.IsTakeStockLock,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    CostPrice = g.CostPrice,
                    PriceUnitName = g.PriceUnitName,
                    CostPriceUnitName = g.CostPriceUnitName,
                    SafetyInventory = g.SafetyInventory,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    GoodsField1 = g.GoodsField1,
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier = g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).SingleAsync();
        }

        /// <summary>
        /// 根据关键字查询供应商
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<SupplierSimple>> GetSupplierByKey(string keyword, int limit = 20)
        {
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword.Trim();
            return await Repository.ClientDb.Queryable<BaseSuppliers>()
                .Where(g => g.SupplierId.Contains(keyword) || g.SupplierNo.Contains(keyword) || g.SupplierName.Contains(keyword) || g.SupplierPropertyName.Contains(keyword) || g.SupplierTypeName.Contains(keyword) || g.Province.Contains(keyword) || g.City.Contains(keyword))
                .Where(g => g.IsValid && g.IsDeleted==false)
                .Take(limit)
                .Select(g => new SupplierSimple
                {
                    SupplierId = g.SupplierId,
                    SupplierNo = g.SupplierNo,
                    SupplierName = g.SupplierName,
                    SupplierTypeName = g.SupplierTypeName,
                    SupplierPropertyName = g.SupplierPropertyName,
                    Province = g.Province,
                    City = g.City
                }).ToListAsync();
        }

        /// <summary>
        /// 获取所有仓库
        /// </summary>
        /// <returns></returns>
        public async Task<List<WarehouseSimple>> GetWarehouses()
        {
            return await Repository.ClientDb.Queryable<InvWarehouse>().Where(w => !w.IsAbandon).OrderBy(w => w.WarehouseName)
                .Select(w => new WarehouseSimple
                {
                    WarehouseId = w.WarehouseId,
                    WarehouseNo = w.WarehouseNo,
                    WarehouseName = w.WarehouseName,
                    ChargePerson = w.ChargePerson,
                    WarehouseType = w.WarehouseType
                }).ToListAsync();
        }

        /// <summary>
        /// 查询仓库中所有货架、货位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public async Task<List<WarehouseElement>> GetElementByWarehouse(string warehouseId)
        {
            var shelfQuery =  Repository.ClientDb.Queryable<InvShelf>().Where(w => w.WarehouseId == warehouseId)
                .OrderBy(o => o.Rank)
                .Select(s => new WarehouseElement
                {
                    Id = s.ShelfId,
                    No = s.ShelfNo,
                    Name = s.ShelfName,
                    Rank = s.Rank,
                    Status = "",
                    Props = s.Property,
                    IsAbandon = s.IsAbandon,
                    IsTakeStockLock=false,
                    Type = StorageUnitType.Shelf.ToString()
                });
            var binQuery =  Repository.ClientDb.Queryable<InvBin>().Where(w => w.WarehouseId == warehouseId && string.IsNullOrEmpty(w.ShelfId))
                .OrderBy(o => o.Rank)
                 .Select(s => new WarehouseElement
                 {
                     Id = s.BinId,
                     No = s.BinNo,
                     Name = s.BinName,
                     Rank = s.Rank,
                     Status = s.Status,
                     Props = s.Property,
                     IsAbandon = s.IsAbandon,
                     IsTakeStockLock= s.IsTakeStockLock,
                     Type = StorageUnitType.Bin.ToString()
                 });
            return await Repository.ClientDb.UnionAll(shelfQuery, binQuery).ToListAsync(); 
        }

        /// <summary>
        /// 根据仓库获取货架
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public async Task<List<WarehouseElement>> GetShelfByWarehouse(string warehouseId)
        {
            return await Repository.ClientDb.Queryable<InvShelf>().Where(w=>w.WarehouseId==warehouseId)
                .OrderBy(o=>o.Rank)
                .Select(s=>new WarehouseElement {
                    Id = s.ShelfId,
                    No = s.ShelfNo,
                    Name = s.ShelfName,
                    Rank = s.Rank,
                    Props = s.Property,
                    IsAbandon = s.IsAbandon,
                    Type = StorageUnitType.Shelf.ToString()
                })
                .ToListAsync();
        }

        /// <summary>
        /// 根据仓库获取库位
        /// </summary>
        /// <returns></returns>
        public async Task<List<WarehouseElement>> GetBinByWarehouse(string warehouseId)
        {
            return await Repository.ClientDb.Queryable<InvBin>()
                .Where(w=>w.WarehouseId==warehouseId)
                .OrderBy (o=>o.Rank)
                .Select(s => new WarehouseElement {
                    Id = s.BinId,
                    No = s.BinNo,
                    Name = s.BinName,
                    Rank = s.Rank,
                    Status = s.Status,
                    Props = s.Property,
                    IsAbandon = s.IsAbandon,
                    IsTakeStockLock = s.IsTakeStockLock,
                    Type = StorageUnitType.Bin.ToString()
                }).ToListAsync();
        }

        /// <summary>
        /// 根据货架获取货位
        /// </summary>
        /// <param name="shelfId"></param>
        /// <returns></returns>
        public async Task<List<WarehouseElement>> GetBinByShelf(string shelfId)
        {
            return await Repository.ClientDb.Queryable<InvBin>()
                .LeftJoin<InvShelf>((b,s)=>b.ShelfId==s.ShelfId)
                .LeftJoin<InvWorkbin>((b, s, wb) => wb.BinId == b.BinId)
                .LeftJoin<InvWorkbinSpecification>((b, s, wb, wbs) => wbs.SpecId == wb.SpecId)
                .Where((b, s, wb, wbs) => b.ShelfId == shelfId)
                .OrderBy((b, s, wb, wbs) => b.Rank)
                .Select((b, s, wb, wbs) => new WarehouseElement
                {
                    Id = b.BinId,
                    No = b.BinNo,
                    Name = b.BinName,
                    Rank = b.Rank,
                    Status = b.Status,
                    Props = b.Property,
                    IsAbandon = b.IsAbandon,
                    SpecificationId = wbs.SpecId,
                    SpecificationName = wbs.SpecName,
                    HasWorkbin =s.HasWorkbin,
                    IsTakeStockLock = b.IsTakeStockLock,
                    Type = StorageUnitType.Bin.ToString()
                }).ToListAsync();
        }

        /// <summary>
        /// 根据货位获取料箱
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        public async Task<List<WarehouseElement>> GetWorkbinCellsByBin(int binId)
        {
            return await Repository.ClientDb.Queryable<InvWorkbinCell>()
                .LeftJoin<InvWorkbin>((wbc, wb) => wbc.WorkbinId == wb.WorkbinId)
                .LeftJoin<InvWorkbinSpecification>((wbc, wb, wbs) => wbs.SpecId == wb.SpecId)  
                .Where((wbc, wb, wbs) => wb.BinId == binId )
                .Select((wbc, wb, wbs) => new WarehouseElement
                {
                    Id = wbc.CellId,
                    No = wbc.CellNo,
                    Name = wbc.CellNo,
                    ParentId=wb.WorkbinId,
                    ParentNo=wb.WorkbinNo,
                    Status = wbc.Status,
                    Props = wbs.Size,
                    Remark = wbs.LoadWeight,
                    SpecificationId=wbs.SpecId,
                    SpecificationName=wbs.SpecName, 
                    Type = StorageUnitType.WorkbinCell.ToString(), 
                    IsTakeStockLock = SqlFunc.Subqueryable<InvBin>().Where(b=>b.BinId==wb.BinId&&b.IsTakeStockLock).Any(), 
                }).ToListAsync();
        }

        /// <summary>
        /// 根据库位查询库存信息
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        public async Task<List<StorageWarehouseDetail>> GetStorageDetailsByBin(int binId)
        {
            return await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
                .LeftJoin<InvWarehouse>((d, w) => w.WarehouseId == d.WarehouseId)
                .LeftJoin<InvShelf>((d, w, s) => s.ShelfId == d.ShelfId)
                .LeftJoin<InvBin>((d, w, s, b) => b.BinId == d.BinId)
                .LeftJoin<InvWorkbin>((d, w, s, b, wb) => wb.WorkbinId == d.WorkbinId)
                .LeftJoin<InvWorkbinCell>((d, w, s, b, wb, wbc) => wbc.CellId == d.WorkbinCellId)
                .LeftJoin<BaseUnits>((d, w, s, b, wb, wbc, u) => u.UnitId == d.UnitId)
                .LeftJoin<BaseGoods>((d, w, s, b, wb, wbc, u,g)=>g.GoodsId==d.GoodsId)
                .Where((d, w, s, b, wb, wbc, u, g) => b.BinId==binId && d.Stock > 0)
                .Select<StorageWarehouseDetail>().ToListAsync();
        }

        /// <summary>
        /// 根据货架查询库存信息
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        public async Task<List<StorageWarehouseDetail>> GetStorageDetailsByShelf(string shelfId)
        {
            return await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
                .LeftJoin<InvWarehouse>((d, w) => w.WarehouseId == d.WarehouseId)
                .LeftJoin<InvShelf>((d, w, s) => s.ShelfId == d.ShelfId)
                .LeftJoin<InvBin>((d, w, s, b) => b.BinId == d.BinId)
                .LeftJoin<InvWorkbin>((d, w, s, b, wb) => wb.WorkbinId == d.WorkbinId)
                .LeftJoin<InvWorkbinCell>((d, w, s, b, wb, wbc) => wbc.CellId == d.WorkbinCellId)
                .LeftJoin<BaseUnits>((d, w, s, b, wb, wbc, u) => u.UnitId == d.UnitId)
                .LeftJoin<BaseGoods>((d, w, s, b, wb, wbc, u, g) => g.GoodsId == d.GoodsId)
                .Where((d, w, s, b, wb, wbc, u, g) => d.ShelfId== shelfId && d.Stock > 0)
                .Select<StorageWarehouseDetail>().ToListAsync();
        }

        /// <summary>
        /// 获取所有工厂区域
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetAreas(string plantNo = BusinessConst.PlantNo)
        {
            return await Repository.ClientDb.Queryable<ProdArea>().Where(w=>w.PlantNo== plantNo).Select(s => new KeyValueModel
            {
                Key = s.AreaNo,
                Value = s.AreaName
            }).ToListAsync();
        }

        /// <summary>
        /// 获取所有工厂产线
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetLines(string plantNo = BusinessConst.PlantNo)
        {
            return await Repository.ClientDb.Queryable<ProdLine>().Where(w => w.PlantNo == plantNo).Select(s => new KeyValueModel
            {
                Key = s.LineNo,
                Value = s.LineName
            }).ToListAsync();
        }

        /// <summary>
        /// 根据区域获取产线
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetLinesByArea(string areaNo)
        {
            return await Repository.ClientDb.Queryable<ProdLine>().Where(w=>w.AreaNo==areaNo).Select(s => new KeyValueModel
            {
                Key = s.LineNo,
                Value = s.LineName
            }).ToListAsync();
        }

        /// <summary>
        /// 获取料箱规格
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetWorkbinSpec()
        {
            return await Repository.ClientDb.Queryable<InvWorkbinSpecification>().OrderBy(o => o.Rank).Select(s => new KeyValueModel
            {
                Key = s.SpecId,
                Value = s.SpecName,
                Remark = s.CellCount
            }).ToListAsync();
        }

        /// <summary>
        /// 根据参数Key获取字典选项
        /// </summary>
        /// <param name="argskey"></param>
        /// <returns></returns>
        public async Task<Args> GetDictionaryOption(string argskey)
        {
            return await _sysArgsHelper.GetDictionaryOption(argskey);
        } 

        /// <summary>
        /// 字段类型选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetFieldTypes()
        {
            var data = EnumHelper.GetEnumValNames<ArgsType>(); 
            return await Task.FromResult(data);
        } 
    }
}
