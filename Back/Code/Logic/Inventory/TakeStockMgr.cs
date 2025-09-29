using DbRepository.Repository;
using Logic.LogicBase;
using Models.Model.Baseinfo;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbRepository.Repository.DbModels;
using Models.Model.Enum;
using Models.Model.Sys;
using SqlSugar;
using Models.Model.Inv;
using NPOI.SS.Formula.Functions;
using Microsoft.VisualBasic;
using StackExchange.Redis;
using External.Common.Extension;
using static RabbitMQ.Client.Logging.RabbitMqClientEventSource;
using Models.Model.EchartsModel;
using NPOI.Util;
using MathNet.Numerics.LinearAlgebra.Factorization;
using External.Common;
using Logic.LogicCommon.FileStorage;
using System.Data;

namespace Logic.Inventory
{
    public class TakeStockMgr : DbOperationHandler
    {
        private readonly StorageMgr _storageMgr;

        private readonly IFileStorage _fileStorage;

        public TakeStockMgr(Repository repository, StorageMgr storageMgr, IFileStorage fileStorage) : base(repository)
        {
            _storageMgr = storageMgr;
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// 查询备件消耗趋势
        /// </summary>
        /// <returns></returns>
        public async Task<RadarModel> GetExpendTrend(string classifyGroup)
        {
            var curYear=DateTime.Now.Year;
            var queryData=await Repository.ClientDb.Queryable<InvStorageFlowDetail>()
                .LeftJoin<BaseGoods>((sfd,g)=>g.GoodsId==sfd.GoodsId)
                .LeftJoin<BaseType>((sfd,g,t)=>t.TypeId==g.GoodsClassifyId)
                .LeftJoin<BaseUnits>((sfd, g, t,u)=>u.UnitId==sfd.UnitId)
                .Where((sfd, g, t,u)=>sfd.FlowType== FlowType.Out.ToString()&& sfd.DateYear== curYear && t.Group== classifyGroup) 
                .GroupBy((sfd, g, t, u)=>new { g.GoodsClassifyId,sfd.DateMonth })
                .Select((sfd, g, t, u) => new GoodsExpendTrendDto
                {
                    GoodsClassify=t.TypeName,
                    DateMonth=sfd.DateMonth,
                    Count= SqlFunc.Abs(SqlFunc.AggregateSum(sfd.Quantity))
                }).ToListAsync();
            var classifyData=await Repository.ClientDb.Queryable<BaseType>().Where(w=>w.Group== classifyGroup).ToListAsync();
            var data = new RadarModel();
            data.YearMonthArray = _getcurYearMonths();
            data.IndicatorArray = new List<RadarIndicator>();
            data.DataArray = new List<RadarData>();
            if (classifyData?.Count > 0)
            {
                data.MaxValue = queryData?.Count>0? queryData.Max(m => m.Count):0;
                foreach (var classify in classifyData)
                {
                    data.IndicatorArray.Add(new RadarIndicator
                    {
                        Text = classify.TypeName,
                        Max= data.MaxValue
                    }); 
                }
                foreach (var month in data.YearMonthArray)
                {
                    var radarData = new RadarData();
                    radarData.Name = month;
                    radarData.Value = new List<float>();
                    foreach (var classify in classifyData)
                    {
                        var dataValue = queryData.SingleOrDefault(w => w.GoodsClassify == classify.TypeName && w.DateMonth == int.Parse(month));
                        if (dataValue != null)
                        {
                            radarData.Value.Add(dataValue.Count);
                        }
                        else
                        {
                            radarData.Value.Add(0);
                        }
                    }
                    data.DataArray.Add(radarData);
                }
            } 
            return data;
        }

        private List<string> _getcurYearMonths()
        {
            var curYear=DateTime.Now.Year;
            var curMonth=DateTime.Now.Month;
            var monthList=new List<string>();
            for(int i=1;i<=curMonth;i++)
            {
                if (i < 10)
                {
                    monthList.Add(curYear + "0" + i);
                }
                else
                {
                    monthList.Add(curYear + "" + i);
                }
            }
            return monthList;
        }

        #region 按物品盘点

        /// <summary>
        /// 查询当前盘点被锁定的物品
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="typeId"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<GoodsTakeStockDto>> GetTakeStockLockGoods(int pgSize, int pgIndex, string orderFiled, string orderType, int typeId, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<BaseGoods>()
                  .LeftJoin<SysArgsOptions>((g, a) => a.OptionId == g.GoodsTypeId && a.ArgsKey == BusinessConst.SparePartType)
                  .LeftJoin<BaseType>((g, a, a2) => a2.TypeId == g.GoodsClassifyId && a2.Group == BaseTypeGroup.SparePart.ToString())
                  .WhereIF(typeId > 0, (g, a, a2) => g.GoodsClassifyId == typeId)
                  .Where((g, a, a2) => g.IsTakeStockLock&& g.IsDeleted == false)
                  .Where((g, a, a2) => g.GoodsName.Contains(searchKey) || g.GoodsNo.Contains(searchKey) || g.GoodsModel.Contains(searchKey) || g.Supplier.Contains(searchKey))
                  .Select((g, a, a2) => new GoodsTakeStockDto
                  {
                      GoodsId = g.GoodsId,
                      GoodsNo = g.GoodsNo,
                      GoodsName = g.GoodsName,
                      GoodsClassifyId = g.GoodsClassifyId,
                      GoodsClassifyName = a2.TypeName,
                      GoodsTypeId = g.GoodsTypeId,
                      GoodsTypeName = a.OptionName,
                      GoodsModel = g.GoodsModel,
                      GoodsProperty = g.GoodsProperty,
                      SafetyInventory = g.SafetyInventory,
                      SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                      Remark = g.Remark,
                      PurchaseCycle = g.PurchaseCycle,
                      PurchaseMinimum = g.PurchaseMinimum,
                      PurchaseMinimumUnitName = g.PurchaseMinimumUnitName,
                      ForArea = g.ForArea,
                      Supplier = g.Supplier,
                      LastInventoryDate=g.LastInventoryDate,
                      LastInventoryOperator=g.LastInventoryOperator,
                      Photo = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<GoodsTakeStockDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 查询历史盘点记录
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="month"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<TakeStockReportDto>> GetTakeStockHis(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId, int month, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "OperateDate" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<InvStorageFlowDetail>()
                .InnerJoin<BaseGoods>((sfd, g) => g.GoodsId == sfd.GoodsId)
                .InnerJoin<BaseType>((sfd, g, t) => t.TypeId == g.GoodsClassifyId)
                .InnerJoin<InvWarehouse>((sfd, g, t, w) => w.WarehouseId == sfd.WarehouseId)
                .LeftJoin<InvShelf>((sfd, g, t, w, s) => s.ShelfId == sfd.ShelfId)
                .LeftJoin<InvBin>((sfd, g, t, w, s, b) => b.BinId == sfd.BinId)
                .LeftJoin<InvWorkbin>((sfd, g, t, w, s, b, wb) => wb.WorkbinId == sfd.WorkbinId)
                .LeftJoin<InvWorkbinCell>((sfd, g, t, w, s, b, wb, wbc) => wbc.CellId == sfd.WorkbinCellId)
                .LeftJoin<BaseUnits>((sfd, g, t, w, s, b, wb, wbc, u) => u.UnitId == sfd.UnitId) 
                .Where((sfd, g, t, w, s, b, wb, wbc, u)=> sfd.SourceStorageSubType == SourceStorageSubType.TakeStockIn.ToString()|| sfd.SourceStorageSubType == SourceStorageSubType.TakeStockOut.ToString() || sfd.SourceStorageSubType == SourceStorageSubType.TakeStockNoProfitOrLoss.ToString())
                .Where((sfd, g, t, w, s, b, wb, wbc, u) => sfd.DateMonth == month && t.Group==goodsGroup)
                .Where((sfd, g, t, w, s, b, wb, wbc, u) => g.GoodsName.Contains(searchKey) || g.GoodsModel.Contains(searchKey) || g.Supplier.Contains(searchKey) || s.ShelfName.Contains(searchKey) || b.BinName.Contains(searchKey))
                .WhereIF(goodsClassifyId > 0, (sfd, g, t, w, s, b, wb, wbc, u) => g.GoodsClassifyId == goodsClassifyId)
                .Select<TakeStockReportDto>()
                .OrderBy($"{orderFiled} {orderType}")
                .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<TakeStockReportDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<TakeStockReasonDto> GetTaskStockDetil(int flowId)
        {
            return await Repository.ClientDb.Queryable<InvStorageFlowDetail>().Where(w => w.FlowId == flowId).Select<TakeStockReasonDto>().SingleAsync();
        }

        public async Task UpdateReason(TakeStockReasonDto data)
        {
            var model = await Repository.GetSingeAsync<InvStorageFlowDetail>(data.FlowId);
            model.RemarkType = data.RemarkType;
            model.Remark = data.Remark;
            await Repository.ClientDb.Updateable(model).ExecuteCommandAsync();
        }

        public async Task<string> ExportTakeStockHis(string searchKey, string orderField, string orderType, string goodsGroup, int goodsClassifyId, int month)
        {
            orderField = "A.OperateDate";
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey; 
            var param = new Dictionary<string, object>
            {
                { "@GoodsName", "%"+searchKey+"%" },
                { "@GoodsModel", "%"+searchKey+"%" },
                { "@Supplier", "%"+searchKey+"%" },
                { "@ShelfName", "%"+searchKey+"%" },
                { "@BinName", "%"+searchKey+"%" },
                { "@Group", goodsGroup}, 
                { "@DateMonth", month },
            };
            string sql = $@"SELECT
                            B.GoodsName AS 名称,
                            B.GoodsModel AS 型号,
                            B.Supplier AS 供应商,
                            J.SupplierNo AS 供应商编码,
                            C.TypeName AS 分类,
                            D.WarehouseName AS 仓库,
                            E.ShelfNo AS 货架,
                            F.BinNo AS 货位,
                            H.CellNo AS 料箱,
                            A.Quantity AS 数量,
                            (CASE A.FlowType WHEN 'In' THEN '盘盈' ELSE '盘亏' END) AS 盈亏,
                            A.OperateDate AS 盘点时间,
                            A.OperatorName AS 盘点人员
                            FROM InvStorageFlowDetail A
                            INNER JOIN BaseGoods B ON A.GoodsId=B.GoodsId
                            INNER JOIN BaseType C ON C.TypeId=B.GoodsClassifyId
                            INNER JOIN InvWarehouse D ON D.WarehouseId=A.WarehouseId
                            LEFT JOIN InvShelf E ON E.ShelfId=A.ShelfId
                            LEFT JOIN InvBin F ON F.BinId=A.BinId
                            LEFT JOIN InvWorkbin G ON G.WorkbinId=A.WorkbinId
                            LEFT JOIN InvWorkbinCell H ON H.CellId=A.WorkbinCellId
                            LEFT JOIN BaseUnits I ON I.UnitId=A.UnitId
                            LEFT JOIN BaseSuppliers J ON J.SupplierName=B.Supplier
                            WHERE C.Group=@Group AND A.DateMonth=@DateMonth AND (A.SourceStorageSubType='{SourceStorageSubType.TakeStockIn}' || A.SourceStorageSubType='{SourceStorageSubType.TakeStockOut}')
                            AND (B.GoodsName LIKE @GoodsName || B.GoodsModel LIKE @GoodsModel || B.Supplier LIKE @Supplier ||E.ShelfName LIKE @ShelfName || F.BinName LIKE @BinName)
                            AND (CASE WHEN {goodsClassifyId}=0 then 1=1 else B.GoodsClassifyId={goodsClassifyId} END)
                            ORDER BY {orderField} {orderType}";
            var queryData = await Repository.QueryBySqlAsync(sql, param); 
            var stream = ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"{month}{EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(goodsGroup)}盘点信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
            return fileUrl;
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
        public async Task<TableModel<GoodsSimple>> GetGoodsByKey(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsGroup, int goodsClassifyId,string warehouseId, string keyword, bool isTakeStockCurDate)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            keyword = string.IsNullOrEmpty(keyword) ? "" : keyword.Trim();
            var curYear = DateTime.Now.Year;
            var curMonth = DateTime.Now.Month;
            var data = Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId)  
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId.Contains(keyword) || g.GoodsNo.Contains(keyword) || g.GoodsName.Contains(keyword) || g.GoodsModel.Contains(keyword) || g.Supplier.Contains(keyword) || g.GoodsProperty.Contains(keyword) || t.TypeName.Contains(keyword))
                .Where((g, t, wbs) => g.IsDeleted == false && t.Group == goodsGroup )
                .WhereIF(!string.IsNullOrEmpty(warehouseId), (g, t,  wbs)=>SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(swd=>swd.WarehouseId==warehouseId&&swd.GoodsId==g.GoodsId).Any())
                .WhereIF(string.IsNullOrEmpty(warehouseId), (g, t, wbs) => SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(swd =>swd.GoodsId == g.GoodsId).Any())
                .WhereIF(goodsClassifyId > 0, (g, t, wbs) => g.GoodsClassifyId == goodsClassifyId)
                .WhereIF(isTakeStockCurDate, (g, t, wbs) => SqlFunc.ToDate(g.LastInventoryDate).Year == curYear && SqlFunc.ToDate(g.LastInventoryDate).Month == curMonth)
                .WhereIF(!isTakeStockCurDate, (g, t, wbs) => string.IsNullOrEmpty(g.LastInventoryDate) || (SqlFunc.ToDate(g.LastInventoryDate).Year != curYear || SqlFunc.ToDate(g.LastInventoryDate).Month != curMonth))
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
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventoryUnitName =g.SafetyInventoryUnitName, 
                    IsTakeStockLock = g.IsTakeStockLock,
                    LastInventoryDate = g.LastInventoryDate,
                    LastInventoryOperator = g.LastInventoryOperator,
                    StandardPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.PackageUnitId).Sum(sud => sud.Stock), 2),
                    MinPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MinPackageUnitId).Sum(sud => sud.Stock), 2),
                    MaxPackageStock = SqlFunc.Round(SqlFunc.Subqueryable<InvStorageWarehouseDetail>().Where(sud => sud.GoodsId == g.GoodsId && sud.UnitId == g.MaxPackageUnitId).Sum(sud => sud.Stock), 2),
                    Supplier = g.Supplier,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url)
                }).OrderBy($"{orderFiled} {orderType}").ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<GoodsSimple>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<bool> GetGoodsTakeStockStatus(string goodsId)
        {
            return await Repository.ClientDb.Queryable<BaseGoods>().AnyAsync(a => a.GoodsId == goodsId && a.IsTakeStockLock);
        }

        /// <summary>
        /// 盘点锁定物品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task SetTakeStockLockByGoods(List<string> goodsId)
        {
            await Repository.ClientDb.Updateable<BaseGoods>().SetColumns(s => s.IsTakeStockLock == true).Where(s => goodsId.Contains(s.GoodsId)).ExecuteCommandAsync();
        }

        /// <summary>
        /// 查询指定物品的存储明细
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<GoodsDetailDto> GetGoodsInventoryDetail(string goodsId)
        {
            var data = await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<BaseType>((g, t) => g.GoodsClassifyId == t.TypeId) 
                .LeftJoin<InvWorkbinSpecification>((g, t, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, t, wbs) => g.GoodsId == goodsId)
                .Select((g, t, wbs) => new GoodsDetailDto
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyGroup = t.Group,
                    GoodsClassifyName = t.TypeName,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    GoodsLevel = g.GoodsLevel,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    PackageCount = g.PackageCount,
                    MaxPackageCount = g.MaxPackageCount,
                    PackageUnitName = g.PackageUnitName,
                    MinPackageUnitName = g.MinPackageUnitName,
                    MaxPackageUnitName = g.MaxPackageUnitName,
                    SafetyInventoryUnitName = g.SafetyInventoryUnitName,
                    CostPrice = g.CostPrice,
                    PriceUnitName = g.PriceUnitName,
                    Supplier = g.Supplier
                }).SingleAsync();
            data.Photos = await Repository.ClientDb.Queryable<BaseFiles>()
               .Where(p => data.GoodsId == p.PrimaryId && p.FileInfoType == FileInfoType.GoodsPhoto.ToString())
               .Select(p => new FileInfoDto { FileId = p.FileId, FileName = p.FileName, Url = p.Url, FileInfoType = p.FileInfoType, Path = p.Path, PrimaryId = p.PrimaryId, Remark = p.Remark })
               .ToListAsync();
            data.InventoryDetails = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
                .LeftJoin<InvWarehouse>((d, w) => w.WarehouseId == d.WarehouseId)
                .LeftJoin<InvShelf>((d, w, s) => s.ShelfId == d.ShelfId)
                .LeftJoin<InvBin>((d, w, s, b) => b.BinId == d.BinId)
                .LeftJoin<InvWorkbin>((d, w, s, b, wb) => wb.WorkbinId == d.WorkbinId)
                .LeftJoin<InvWorkbinCell>((d, w, s, b, wb, wbc) => wbc.CellId == d.WorkbinCellId)
                .LeftJoin<BaseUnits>((d, w, s, b, wb, wbc, u) => u.UnitId == d.UnitId)
                .Where((d, w, s, b, wb, wbc, u) => d.GoodsId == goodsId)
                .Select<StorageWarehouseDetail>().ToListAsync();
            return data;
        }

        /// <summary>
        /// 盘点保存（按物品盘点）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddTaskStockOrderByGoods(TakeStockGoodsDto data)
        { 
            foreach(var detail in data.InventoryDetails)
            {
                if (detail.StockActual < 0)
                {
                    throw new BusinessException("实际库存数量不能小于0");
                }
            }
            var curDate = DateTime.Now;
            var warehouse = data.InventoryDetails.Select(s => s.WarehouseId).Distinct().Single();
            var lastInOrderNo = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo);
            var inStorageModel = new InvInStorage
            {
                OrderNo = GetPrimaryId("I", lastInOrderNo),
                InStorageType = InStorageType.TakeStockIn.ToString(),
                GoodsClassify = data.GoodsClassifyGroup,
                CreateDate = curDate,
                CreateUserId = data.CreateUserId,
                CreateUserName = data.CreateUserName,
                WarehouseId = warehouse, 
                InstorageDate = curDate,
                Remark=data.Remark,
                Status = InStorageStatus.WaitInStorage.ToString(),
                ApprovalStatus = ApprovalStatus.NoApproval.ToString()
            };
            var inStorageDetails = new List<InvInStorageDetail>(); 
            var lastOutData = await Repository.ClientDb.Queryable<InvOutStorage>().MaxAsync(x => x.OrderNo);
            var outStorageModel = new InvOutStorage
            {
                OrderNo = GetPrimaryId("O", lastOutData),
                OutStorageType = OutStorageType.TakeStockOut.ToString(),
                GoodsClassify = data.GoodsClassifyGroup,
                CreateDate = curDate,
                CreateUserId = data.CreateUserId,
                CreateUserName = data.CreateUserName,
                WarehouseId = warehouse, 
                OutStorageDate = curDate,
                Status = OutStorageStatus.WaitOutStorage.ToString(),
                ApprovalStatus = ApprovalStatus.NoApproval.ToString()
            }; 
            var outStorageDetails = new List<InvOutStorageDetail>();
            var flowDetails = new List<InvStorageFlowDetail>();
            foreach (var detail in data.InventoryDetails)
            {
                if (detail.Stock != detail.StockActual)
                {
                    var increment=detail.StockActual- detail.Stock ;
                    if(increment > 0)
                    {
                        //新增入库单补充实际增量
                        var inStorageDetail = new InvInStorageDetail
                        {
                            OrderNo = inStorageModel.OrderNo,
                            GoodsId = data.GoodsId,
                            GoodsName = data.GoodsName,
                            WarehouseId = detail.WarehouseId,
                            ShelfId = detail.ShelfId,
                            BinId = detail.BinId,
                            WorkbinId = detail.WorkbinId,
                            WorkbinCellId = detail.CellId,
                            Quantity = increment,
                            ActualQuantity = increment,
                            UnitId = detail.UnitId,
                            UnitPrice=data.UnitPrice,
                            TotalPrice=data.UnitPrice* increment,
                            PriceUnit=data.PriceUnit,
                            Remark = detail.Remark,
                            RemarkType=detail.RemarkType
                        };
                        inStorageDetails.Add(inStorageDetail);
                        //var storageFlowDetailList = new InvStorageFlowDetail
                        //{
                        //    FlowType = FlowType.In.ToString(),
                        //    GoodsId = data.GoodsId,
                        //    OperateDate = curDate.ToStringExtension(),
                        //    OperatorId = data.CreateUserId,
                        //    OperatorName = data.CreateUserName,
                        //    Quantity = increment,
                        //    SourceOrderNo = inStorageModel.OrderNo,
                        //    SourceStorageType = SourceStorageType.InStorage.ToString(),
                        //    SourceStorageSubType = SourceStorageSubType.TakeStockIn.ToString(),
                        //    UnitId = detail.UnitId,
                        //    WarehouseId = detail.WarehouseId,
                        //    ShelfId = detail.ShelfId,
                        //    BinId = detail.BinId,
                        //    WorkbinId = detail.WorkbinId,
                        //    WorkbinCellId = detail.CellId,
                        //    DateYear= int.Parse(curDate.ToString("yyyy")),
                        //    DateMonth = int.Parse(curDate.ToString("yyyyMM")),
                        //    DateDay = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                        //    BatchNumber = long.Parse(DateTime.Now.ToStringNoSignExtension())
                        //};
                        //flowDetails.Add(storageFlowDetailList);
                    }
                    else
                    {
                        //新增出库单补充实际减量
                        var outStorageDetail = new InvOutStorageDetail
                        {
                            OrderNo = outStorageModel.OrderNo,
                            GoodsId = data.GoodsId,
                            GoodsName = data.GoodsName,
                            WarehouseId = detail.WarehouseId,
                            ShelfId = detail.ShelfId,
                            BinId = detail.BinId,
                            WorkbinId = detail.WorkbinId,
                            WorkbinCellId = detail.CellId,
                            Quantity = Math.Abs(increment),
                            ActualQuantity = Math.Abs(increment),
                            UnitId = detail.UnitId,
                            UnitPrice = data.UnitPrice,
                            TotalPrice = data.UnitPrice * increment,
                            PriceUnit = data.PriceUnit,
                            Remark = detail.Remark,
                            RemarkType=detail.RemarkType
                        };
                        outStorageDetails.Add(outStorageDetail);
                        //var storageFlowDetailList = new InvStorageFlowDetail
                        //{
                        //    FlowType = FlowType.Out.ToString(),
                        //    GoodsId = data.GoodsId,
                        //    OperateDate = curDate.ToStringExtension(),
                        //    OperatorId = data.CreateUserId,
                        //    OperatorName = data.CreateUserName,
                        //    Quantity = increment,
                        //    SourceOrderNo = outStorageModel.OrderNo,
                        //    SourceStorageType = SourceStorageType.OutStorage.ToString(),
                        //    SourceStorageSubType = SourceStorageSubType.TakeStockOut.ToString(),
                        //    UnitId = detail.UnitId,
                        //    WarehouseId = detail.WarehouseId,
                        //    ShelfId = detail.ShelfId,
                        //    BinId = detail.BinId,
                        //    WorkbinId = detail.WorkbinId,
                        //    WorkbinCellId = detail.CellId,
                        //    DateYear = int.Parse(curDate.ToString("yyyy")),
                        //    DateMonth = int.Parse(curDate.ToString("yyyyMM")),
                        //    DateDay = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                        //    BatchNumber = long.Parse(DateTime.Now.ToStringNoSignExtension())
                        //};
                        //flowDetails.Add(storageFlowDetailList);
                    }
                }
                else
                {
                    var storageFlowDetailList = new InvStorageFlowDetail
                    {
                        FlowType = FlowType.In.ToString(),
                        GoodsId = data.GoodsId,
                        OperateDate = curDate.ToStringExtension(),
                        OperatorId = data.CreateUserId,
                        OperatorName = data.CreateUserName,
                        Quantity = 0,
                        SourceOrderNo = "",
                        SourceStorageType = "",
                        SourceStorageSubType = SourceStorageSubType.TakeStockNoProfitOrLoss.ToString(),
                        UnitId = detail.UnitId,
                        WarehouseId = detail.WarehouseId,
                        ShelfId = detail.ShelfId,
                        BinId = detail.BinId,
                        WorkbinId = detail.WorkbinId,
                        WorkbinCellId = detail.CellId,
                        DateYear = int.Parse(curDate.ToString("yyyy")),
                        DateMonth = int.Parse(curDate.ToString("yyyyMM")),
                        DateDay = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                        BatchNumber = long.Parse(DateTime.Now.ToStringNoSignExtension()),
                        Remark=detail.Remark,
                        RemarkType = detail.RemarkType,
                        UnitPrice=data.UnitPrice,
                        TotalPrice=0,
                        PriceUnit=data.PriceUnit
                    };
                    flowDetails.Add(storageFlowDetailList);
                }
            }
            if (inStorageDetails.Count > 0)
            {
                Repository.ClientDb.Insertable(inStorageModel).AddQueue();
                Repository.ClientDb.Insertable(inStorageDetails).AddQueue();
            }
            if (outStorageDetails.Count > 0)
            {
                Repository.ClientDb.Insertable(outStorageModel).AddQueue();
                Repository.ClientDb.Insertable(outStorageDetails).AddQueue();
            }
            if (flowDetails.Count > 0)
            {
                Repository.ClientDb.Insertable(flowDetails).AddQueue();
            }
            var goods =await Repository.GetSingeAsync<BaseGoods>(data.GoodsId);
            goods.IsTakeStockLock = false;
            goods.LastInventoryDate = curDate.ToStringExtension();
            goods.LastInventoryOperator = data.CreateUserName;
            Repository.ClientDb.Updateable(goods).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            await _storageMgr.StorageStatistics(data.CreateUserId, data.CreateUserName);
        }

        #endregion

        #region 按货位盘点
         
        /// <summary>
        /// 获取当前盘点被锁定的库位
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="typeId"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<BinTakeStockDto>> GetTakeStockLockInvBin(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "BinNo" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<InvBin>()
                .LeftJoin<InvWarehouse>((b, w) => b.WarehouseId == w.WarehouseId)
                .LeftJoin<InvShelf>((b, w, s) => s.ShelfId == b.ShelfId)
                .Where((b, w, s) => b.IsTakeStockLock)
                .Where((b, w, s) => w.WarehouseName.Contains(searchKey) || s.ShelfName.Contains(searchKey) || b.BinName.Contains(searchKey))
                .Select<BinTakeStockDto>()
                .OrderBy($"{orderFiled} {orderType}")
                .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<BinTakeStockDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);

        }

        /// <summary>
        /// 分页查询货位信息
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="warehouseId"></param>
        /// <param name="shelfId"></param>
        /// <param name="workbinSpecId"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<TableModel<BinTakeStockDto>> GetBins(int pgSize, int pgIndex, string warehouseId, string shelfId,int workbinSpecId, string goodsGroup, string orderFiled, string orderType, string keyword,bool isTakeStockCurDate)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "b.BinNo" : "b."+orderFiled;
            keyword = string.IsNullOrEmpty(keyword) ? "" : keyword.Trim();
            var curYear = DateTime.Now.Year;
            var curMonth = DateTime.Now.Month;
            var data = Repository.ClientDb.Queryable<InvBin>()
                  .InnerJoin<InvWarehouse>((b, w) => b.WarehouseId == w.WarehouseId)
                  .LeftJoin<InvShelf>((b, w, s) => s.ShelfId == b.ShelfId)
                  .LeftJoin<InvWorkbin>((b, w, s, wb) => wb.BinId == b.BinId)
                  .LeftJoin<InvWorkbinSpecification>((b, w, s, wb, wbs) => wbs.SpecId == wb.SpecId)
                  .WhereIF(!string.IsNullOrEmpty(warehouseId), (b, w, s, wb, wbs) => w.WarehouseId == warehouseId)
                  .WhereIF(!string.IsNullOrEmpty(shelfId), (b, w, s, wb, wbs) => b.ShelfId == shelfId)
                  .WhereIF(workbinSpecId>0, (b, w, s, wb, wbs)=>wbs.SpecId==workbinSpecId)
                  .WhereIF(isTakeStockCurDate, (b, w, s, wb, wbs) => SqlFunc.ToDate(b.LastInventoryDate).Year == curYear && SqlFunc.ToDate(b.LastInventoryDate).Month==curMonth)
                  .WhereIF(!isTakeStockCurDate, (b, w, s, wb, wbs) =>string.IsNullOrEmpty(b.LastInventoryDate) ||(SqlFunc.ToDate(b.LastInventoryDate).Year != curYear || SqlFunc.ToDate(b.LastInventoryDate).Month != curMonth))
                  .Where((b, w, s, wb, wbs) => b.BinName.Contains(keyword)||b.Remark.Contains(keyword)) 
                  .Where((b, w, s, wb, wbs)=>SqlFunc.Subqueryable<InvStorageWarehouseDetail>()
                                            .LeftJoin<BaseGoods>((swd,g)=>g.GoodsId==swd.GoodsId)
                                            .LeftJoin<BaseType>((swd,g,gt)=>gt.TypeId==g.GoodsClassifyId)
                                            .Where((swd, g, gt)=>gt.Group==goodsGroup&&swd.BinId==b.BinId&&swd.Stock>0).Any())
                  .Select((b, w, s, wb, wbs) => new BinTakeStockDto
                  {
                      WarehouseId = w.WarehouseId,
                      WarehouseName = w.WarehouseName,
                      ShelfId = b.ShelfId,
                      ShelfName=s.ShelfName,
                      BinId = b.BinId,
                      BinNo = b.BinNo,
                      BinName = b.BinName,
                      Specification=b.Specification,
                      Property=b.Property,
                      Rank=b.Rank,
                      Remark=b.Remark,
                      WorkbinId=wb.WorkbinId,
                      WorkbinNo=wb.WorkbinNo,
                      SpecId=wbs.SpecId,
                      SpecName=wbs.SpecName,
                      HasWorkbin=s.HasWorkbin,
                      IsTakeStockLock=b.IsTakeStockLock,
                      LastInventoryDate=b.LastInventoryDate,
                      LastInventoryOperator=b.LastInventoryOperator  
                  }).OrderBy($"{orderFiled} {orderType}").ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<BinTakeStockDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<bool> GetBinTakeStockStatus(int binId)
        {
            return await Repository.ClientDb.Queryable<InvBin>().AnyAsync(a => a.BinId == binId && a.IsTakeStockLock);
        }

        /// <summary>
        /// 盘点锁定库位
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task SetTakeStockLockByBin(List<int> binId)
        {
            await Repository.ClientDb.Updateable<InvBin>().SetColumns(s => s.IsTakeStockLock == true).Where(s => binId.Contains(s.BinId)).ExecuteCommandAsync();
        } 

        /// <summary>
        /// 查询指定库位存储明细
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        public async Task<BinTakeStockDetailDto> GetBinInventoryDetail(int binId)
        { 
            var data = await Repository.ClientDb.Queryable<InvBin>()
                .InnerJoin<InvWarehouse>((b, w) => b.WarehouseId == w.WarehouseId)
                .LeftJoin<InvShelf>((b, w, s) => s.ShelfId == b.ShelfId)
                .LeftJoin<InvWorkbin>((b, w, s, wb) => wb.BinId == b.BinId)
                .LeftJoin<InvWorkbinSpecification>((b, w, s, wb, wbs) => wbs.SpecId == wb.SpecId)
                .Where((b, w, s, wb, wbs) =>b.BinId== binId)   
                .Select((b, w, s, wb, wbs) => new BinTakeStockDetailDto
                {
                    WarehouseId = w.WarehouseId,
                    WarehouseName = w.WarehouseName,
                    ShelfId = b.ShelfId,
                    ShelfName = s.ShelfName,
                    BinId = b.BinId,
                    BinNo = b.BinNo,
                    BinName = b.BinName,
                    Specification = b.Specification,
                    Property = b.Property,
                    Rank = b.Rank,
                    Remark = b.Remark,
                    WorkbinId = wb.WorkbinId,
                    WorkbinNo = wb.WorkbinNo,
                    SpecId = wbs.SpecId,
                    SpecName = wbs.SpecName,
                    HasWorkbin = s.HasWorkbin,
                    IsTakeStockLock = b.IsTakeStockLock,
                    LastInventoryDate = b.LastInventoryDate,
                    LastInventoryOperator = b.LastInventoryOperator, 
                }).SingleAsync();
            data.InventoryDetails=await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>()
                .InnerJoin<BaseGoods>((sw,g)=>sw.GoodsId==g.GoodsId)
                .InnerJoin<BaseType>((sw,g,gt)=>g.GoodsClassifyId==gt.TypeId)
                .LeftJoin<BaseUnits>((sw, g, gt,u)=>u.UnitId==sw.UnitId)
                .LeftJoin<InvWorkbin>((sw, g, gt, u,wb)=>wb.WorkbinId==sw.WorkbinId)
                .LeftJoin<InvWorkbinCell>((sw, g, gt, u, wb,wbc) =>wbc.CellId==sw.WorkbinCellId)
                .Where((sw, g, gt, u, wb, wbc) =>sw.BinId==binId)
                .OrderBy((sw, g, gt, u, wb, wbc)=>wbc.CellId)
                .Select((sw, g, gt, u, wb, wbc) =>new GoodsInventory
                {
                    GoodsId=g.GoodsId,
                    GoodsNo=g.GoodsNo,
                    GoodsName=g.GoodsName,
                    GoodsClassifyGroup=gt.Group,
                    GoodsModel=g.GoodsModel,
                    GoodsClassifyName=gt.TypeName,
                    GoodsPicture = SqlFunc.Subqueryable<BaseFiles>().Where(p => p.FileInfoType == FileInfoType.GoodsPhoto.ToString() && p.PrimaryId == g.GoodsId && p.IsDeft).Select(p => p.Url),
                    WorkbinId =sw.WorkbinId,
                    WorkbinNo=wb.WorkbinNo,
                    CellId=wbc.CellId,
                    CellNo=wbc.CellNo,
                    Stock=sw.Stock,
                    UnitId=sw.UnitId,
                    UnitName=u.UnitName,
                    UnitPrice=g.CostPrice,
                    TotalPrice=SqlFunc.ToDouble(g.CostPrice * sw.Stock),
                    PriceUnit=g.PriceUnitName
                }).ToListAsync();
            return data;
        }

        /// <summary>
        /// 盘点保存（按货位盘点）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddTaskStockOrderByBin(BinTakeStockDetailDto data)
        {
            foreach (var detail in data.InventoryDetails)
            {
                if (detail.StockActual < 0)
                {
                    throw new BusinessException("实际库存数量不能小于0");
                }
            }
            var curDate = DateTime.Now;
            var goodsClassifyGroup = data.InventoryDetails.Select(s => s.GoodsClassifyGroup).Distinct().Single();
            var lastInOrderNo = await Repository.ClientDb.Queryable<InvInStorage>().MaxAsync(x => x.OrderNo);
            var inStorageModel = new InvInStorage
            {
                OrderNo = GetPrimaryId("I", lastInOrderNo),
                InStorageType = InStorageType.TakeStockIn.ToString(),
                GoodsClassify = goodsClassifyGroup,
                CreateDate = curDate,
                CreateUserId = data.CreateUserId,
                CreateUserName = data.CreateUserName,
                WarehouseId = data.WarehouseId, 
                InstorageDate = curDate,
                Status = InStorageStatus.WaitInStorage.ToString(),
                ApprovalStatus = ApprovalStatus.NoApproval.ToString()
            };
            var inStorageDetails = new List<InvInStorageDetail>();
            var lastOutData = await Repository.ClientDb.Queryable<InvOutStorage>().MaxAsync(x => x.OrderNo);
            var outStorageModel = new InvOutStorage
            {
                OrderNo = GetPrimaryId("O", lastOutData),
                OutStorageType = OutStorageType.TakeStockOut.ToString(),
                GoodsClassify = goodsClassifyGroup,
                CreateDate = curDate,
                CreateUserId = data.CreateUserId,
                CreateUserName = data.CreateUserName,
                WarehouseId = data.WarehouseId, 
                OutStorageDate = curDate,
                Status = OutStorageStatus.WaitOutStorage.ToString(),
                ApprovalStatus = ApprovalStatus.NoApproval.ToString()
            };
            var outStorageDetails = new List<InvOutStorageDetail>();
            var flowDetails = new List<InvStorageFlowDetail>();
            foreach (var detail in data.InventoryDetails)
            {
                if (detail.Stock != detail.StockActual)
                {
                    var increment = detail.StockActual - detail.Stock;
                    if (increment > 0)
                    {
                        //新增入库单补充实际增量
                        var inStorageDetail = new InvInStorageDetail
                        {
                            OrderNo = inStorageModel.OrderNo,
                            GoodsId = detail.GoodsId,
                            GoodsName = detail.GoodsName,
                            WarehouseId = data.WarehouseId,
                            ShelfId = data.ShelfId,
                            BinId = data.BinId,
                            WorkbinId = detail.WorkbinId,
                            WorkbinCellId = detail.CellId,
                            Quantity = increment,
                            ActualQuantity = increment,
                            UnitId = detail.UnitId,
                            UnitPrice= detail.UnitPrice,
                            TotalPrice=detail.UnitPrice*increment,
                            PriceUnit=detail.PriceUnit,
                            Remark = detail.Remark,
                            RemarkType =detail.RemarkType
                        };
                        inStorageDetails.Add(inStorageDetail);
                        //var storageFlowDetailList = new InvStorageFlowDetail
                        //{
                        //    FlowType = FlowType.In.ToString(),
                        //    GoodsId = detail.GoodsId,
                        //    OperateDate = curDate.ToStringExtension(),
                        //    OperatorId = data.CreateUserId,
                        //    OperatorName = data.CreateUserName,
                        //    Quantity = increment,
                        //    SourceOrderNo = inStorageModel.OrderNo,
                        //    SourceStorageType = SourceStorageType.InStorage.ToString(),
                        //    SourceStorageSubType = SourceStorageSubType.TakeStockIn.ToString(),
                        //    UnitId = detail.UnitId,
                        //    WarehouseId = data.WarehouseId,
                        //    ShelfId = data.ShelfId,
                        //    BinId = data.BinId,
                        //    WorkbinId = detail.WorkbinId,
                        //    WorkbinCellId = detail.CellId,
                        //    DateYear = int.Parse(curDate.ToString("yyyy")),
                        //    DateMonth = int.Parse(curDate.ToString("yyyyMM")),
                        //    DateDay = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                        //    BatchNumber = long.Parse(DateTime.Now.ToStringNoSignExtension())
                        //};
                        //flowDetails.Add(storageFlowDetailList); 
                    }
                    else
                    {
                        //新增出库单补充实际减量
                        var outStorageDetail = new InvOutStorageDetail
                        {
                            OrderNo = outStorageModel.OrderNo,
                            GoodsId = detail.GoodsId,
                            GoodsName = detail.GoodsName,
                            WarehouseId = data.WarehouseId,
                            ShelfId = data.ShelfId,
                            BinId = data.BinId,
                            WorkbinId = detail.WorkbinId,
                            WorkbinCellId = detail.CellId,
                            Quantity = Math.Abs(increment),
                            ActualQuantity = Math.Abs(increment),
                            UnitId = detail.UnitId,
                            UnitPrice = detail.UnitPrice,
                            TotalPrice = detail.UnitPrice * increment,
                            PriceUnit = detail.PriceUnit,
                            Remark = detail.Remark,
                            RemarkType = detail.RemarkType
                        };
                        outStorageDetails.Add(outStorageDetail);
                        //var storageFlowDetailList = new InvStorageFlowDetail
                        //{
                        //    FlowType = FlowType.Out.ToString(),
                        //    GoodsId = detail.GoodsId,
                        //    OperateDate = curDate.ToStringExtension(),
                        //    OperatorId = data.CreateUserId,
                        //    OperatorName = data.CreateUserName,
                        //    Quantity = increment,
                        //    SourceOrderNo = outStorageModel.OrderNo,
                        //    SourceStorageType = SourceStorageType.OutStorage.ToString(),
                        //    SourceStorageSubType = SourceStorageSubType.TakeStockOut.ToString(),
                        //    UnitId = detail.UnitId,
                        //    WarehouseId = data.WarehouseId,
                        //    ShelfId = data.ShelfId,
                        //    BinId = data.BinId,
                        //    WorkbinId = detail.WorkbinId,
                        //    WorkbinCellId = detail.CellId,
                        //    DateYear = int.Parse(curDate.ToString("yyyy")),
                        //    DateMonth = int.Parse(curDate.ToString("yyyyMM")),
                        //    DateDay = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                        //    BatchNumber = long.Parse(DateTime.Now.ToStringNoSignExtension())
                        //};
                        //flowDetails.Add(storageFlowDetailList);
                    }
                }
                else
                {
                    var storageFlowDetailList = new InvStorageFlowDetail
                    {
                        FlowType = FlowType.In.ToString(),
                        GoodsId = detail.GoodsId,
                        OperateDate = curDate.ToStringExtension(),
                        OperatorId = data.CreateUserId,
                        OperatorName = data.CreateUserName,
                        Quantity = 0,
                        SourceOrderNo = "",
                        SourceStorageType = "",
                        SourceStorageSubType = SourceStorageSubType.TakeStockNoProfitOrLoss.ToString(),
                        UnitId = detail.UnitId,
                        WarehouseId = data.WarehouseId,
                        ShelfId = data.ShelfId,
                        BinId = data.BinId,
                        WorkbinId = detail.WorkbinId,
                        WorkbinCellId = detail.CellId,
                        DateYear = int.Parse(curDate.ToString("yyyy")),
                        DateMonth = int.Parse(curDate.ToString("yyyyMM")),
                        DateDay = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                        BatchNumber = long.Parse(DateTime.Now.ToStringNoSignExtension()),
                        Remark=detail.Remark,
                        RemarkType = detail.RemarkType,
                        UnitPrice = detail.UnitPrice,
                        TotalPrice=0,
                        PriceUnit=detail.PriceUnit
                    };
                    flowDetails.Add(storageFlowDetailList);
                }
            }
            if (inStorageDetails.Count > 0)
            {
                Repository.ClientDb.Insertable(inStorageModel).AddQueue();
                Repository.ClientDb.Insertable(inStorageDetails).AddQueue();
            }
            if (outStorageDetails.Count > 0)
            {
                Repository.ClientDb.Insertable(outStorageModel).AddQueue();
                Repository.ClientDb.Insertable(outStorageDetails).AddQueue();
            }
            if (flowDetails.Count > 0)
            {
                Repository.ClientDb.Insertable(flowDetails).AddQueue();
            }
            var goods = await Repository.GetSingeAsync<InvBin>(data.BinId);
            goods.IsTakeStockLock = false;
            goods.LastInventoryDate = curDate.ToStringExtension();
            goods.LastInventoryOperator = data.CreateUserName;
            Repository.ClientDb.Updateable(goods).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            await _storageMgr.StorageStatistics(data.CreateUserId, data.CreateUserName);
        }
        #endregion
    }
}
