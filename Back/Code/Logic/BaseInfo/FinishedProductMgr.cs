using AutoMapper;
using DbRepository.Repository.DbModels;
using DbRepository.Repository;
using External.Common.Extension;
using External.Common;
using Logic.LogicBase;
using Logic.LogicCommon.FileStorage;
using Microsoft.Extensions.Configuration;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.BaseInfo
{
    public class FinishedProductMgr : DataPermissionHandler
    {
        private readonly IFileStorage _fileStorage;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public FinishedProductMgr(Repository repository, IFileStorage fileStorage, IMapper mapper, IConfiguration configuration) : base(repository)
        {
            _fileStorage = fileStorage;
            _mapper = mapper;
            _configuration = configuration;
        }

        #region 成品类型增删改查
        /// <summary>
        /// 获取成品类型树形结构
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetFinishedProductClassifyData()
        {
            var typeData = await Repository.ClientDb.Queryable<BaseType>().Where(w => w.Group == BaseTypeGroup.FinishedProduct.ToString()).ToListAsync();
            var rootChild = typeData.Where(m => m.ParentId == 0).Select(m => new TreeModel { Id = m.TypeId, Label = m.TypeName, Type = m.TypeNo, Remark = m.Remark, Rank = m.Rank, ParentId = "ROOT", ParentName = "ROOT" }).OrderBy(m => m.Rank).ToList();
            rootChild.ForEach(r =>
            {
                _getChildren(r, typeData);
            });
            var treeRoot = new List<TreeModel>
            {
                new TreeModel {Id="ROOT",Label="所有",Children=rootChild}
            };
            return treeRoot;
        }

        /// <summary>
        /// 无限迭代构建子级角色
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="data"></param>
        private void _getChildren(TreeModel parent, List<BaseType> data)
        {
            parent.Children = new List<TreeModel>();
            foreach (var r in data)
            {
                if (r.ParentId.ToString() == parent.Id.ToString())
                {
                    var child = new TreeModel { Id = r.TypeId, Label = r.TypeName, Type = r.TypeNo, Remark = r.Remark, Rank = r.Rank, ParentId = parent.Id, ParentName = parent.Label };
                    parent.Children.Add(child);
                    _getChildren(child, data);
                }
            }
        }

        /// <summary>
        /// 添加成品类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddFinishedProductClassify(BaseTypeDto data)
        {
            var existModel = await Repository.Exist<BaseType>(d => d.TypeNo == data.TypeNo);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前成品类型编码已存在");
            }
            var model = new BaseType
            {
                TypeNo = data.TypeNo,
                TypeName = data.TypeName,
                Rank = data.Rank,
                ParentId = data.ParentId,
                Remark = data.Remark,
                Group = BaseTypeGroup.FinishedProduct.ToString()
            };
            await Repository.AddAsync(model);
        }

        /// <summary>
        /// 修改成品类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateFinishedProductClassify(BaseTypeDto data)
        {
            var existModel = await Repository.Exist<BaseType>(d => d.TypeId == data.TypeId);
            if (!existModel)
            {
                throw new BusinessException("保存失败,当前成品类型ID不存在或已删除");
            }
            var model = new BaseType
            {
                TypeId = data.TypeId,
                TypeNo = data.TypeNo,
                TypeName = data.TypeName,
                Rank = data.Rank,
                ParentId = data.ParentId,
                Remark = data.Remark,
                Group = BaseTypeGroup.FinishedProduct.ToString()
            };
            await Repository.UpdateAsync(model);
        }

        /// <summary>
        /// 删除成品类型
        /// </summary>
        /// <param name="goodsTypeId"></param>
        /// <returns></returns>
        public async Task DelFinishedProductClassify(int goodsTypeId)
        {
            await Repository.DeleteAsync<BaseType>(x => x.TypeId == goodsTypeId);
        }

        #endregion

        #region 成品信息增删改查

        /// <summary>
        /// 分页查询成品列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="typeId"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<FinishedProductDto>> GetFinishedProductList(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, int typeId, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "GoodsName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<BaseGoods>()
                  .LeftJoin<BaseType>((g, a2) => a2.TypeId == g.GoodsClassifyId && a2.Group == BaseTypeGroup.FinishedProduct.ToString())
                  .WhereIF(typeId > 0, (g, a2) => g.GoodsClassifyId == typeId)
                  .WhereIF(typeId == 0, (g, a2) => a2.Group == BaseTypeGroup.FinishedProduct.ToString())
                  .Where((g, a2)=>g.IsDeleted==false)
                  .Where((g, a2) => g.GoodsName.Contains(searchKey) || g.GoodsNo.Contains(searchKey) || g.GoodsModel.Contains(searchKey) || g.GoodsProperty.Contains(searchKey))
                  .Select((g, a2) => new FinishedProductDto
                  {
                      GoodsId = g.GoodsId,
                      GoodsNo = g.GoodsNo,
                      GoodsName = g.GoodsName,
                      GoodsClassifyId = g.GoodsClassifyId,
                      GoodsClassifyName = a2.TypeName,
                      GoodsClassifyGroup=a2.Group,
                      GoodsModel = g.GoodsModel,
                      GoodsProperty = g.GoodsProperty,
                      GoodsLevel = g.GoodsLevel,
                      PackageCount = g.PackageCount,
                      PackageUnitId = g.PackageUnitId,
                      PackageUnitName = g.PackageUnitName,
                      MinPackageUnitId = g.MinPackageUnitId,
                      MinPackageUnitName = g.MinPackageUnitName,
                      MaxPackageUnitId = g.MaxPackageUnitId,
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
                      ForArea = g.ForArea,
                      Supplier = g.Supplier,
                      IsValid = g.IsValid,
                      CustomerGoodsNo = g.CustomerGoodsNo,
                      CustomerIdentificationCode = g.CustomerIdentificationCode,
                      GoodsField1 = g.GoodsField1,
                      GoodsField2 = g.GoodsField2,
                      GoodsField3 = g.GoodsField3,
                      GoodsField4 = g.GoodsField4,
                      GoodsField5 = g.GoodsField5
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<FinishedProductDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 获取成品明细
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<FinishedProductDto> GetFinishedProductDetail(string sampleId)
        {
            var data = await Repository.ClientDb.Queryable<BaseGoods>()
                .LeftJoin<SysArgsOptions>((g, a) => a.OptionId == g.GoodsTypeId && a.ArgsKey == BusinessConst.FinishedProductType)
                .LeftJoin<BaseType>((g, a, a2) => a2.TypeId == g.GoodsClassifyId && a2.Group == BaseTypeGroup.FinishedProduct.ToString())
                .LeftJoin<InvWorkbinSpecification>((g, a, a2, wbs) => wbs.SpecId == g.GoodsSpecificationId)
                .Where((g, a, a2, wbs) => g.GoodsId == sampleId)
                .Select((g, a, a2, wbs) => new FinishedProductDto
                {
                    GoodsId = g.GoodsId,
                    GoodsNo = g.GoodsNo,
                    GoodsName = g.GoodsName,
                    GoodsClassifyId = g.GoodsClassifyId,
                    GoodsClassifyName = a2.TypeName,
                    GoodsModel = g.GoodsModel,
                    GoodsProperty = g.GoodsProperty,
                    GoodsSpecificationId = g.GoodsSpecificationId,
                    GoodsSpecificationName = wbs.SpecName,
                    MaxStock = g.MaxStock,
                    IsConstraintSpec = g.IsConstraintSpec,
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
                    IsValid = g.IsValid,
                    CustomerGoodsNo = g.CustomerGoodsNo,
                    CustomerIdentificationCode = g.CustomerIdentificationCode,
                    GoodsField1 = g.GoodsField1,
                    GoodsField2 = g.GoodsField2,
                    GoodsField3 = g.GoodsField3,
                    GoodsField4 = g.GoodsField4,
                    GoodsField5 = g.GoodsField5
                }).SingleAsync();
            data.Photos = await Repository.ClientDb.Queryable<BaseFiles>()
                .Where(p => data.GoodsId == p.PrimaryId && p.FileInfoType == FileInfoType.GoodsPhoto.ToString())
                .Select(p => new Models.Model.Baseinfo.FileInfoDto { FileId = p.FileId, FileName = p.FileName, Url = p.Url, FileInfoType = p.FileInfoType, Path = p.Path, PrimaryId = p.PrimaryId, Remark = p.Remark })
                .ToListAsync();
            return data;
        }

        /// <summary>
        /// 添加成品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddFinishedProduct(FinishedProductDto data)
        {
            var lastData = await Repository.ClientDb.Queryable<BaseGoods>().MaxAsync(x => x.GoodsId);
            if (!string.IsNullOrEmpty(data.GoodsNo))
            {
                var exist = await Repository.Exist<BaseGoods>(x => x.GoodsModel == data.GoodsModel && x.GoodsName == data.GoodsName);
                if (exist)
                {
                    throw new BusinessException("保存失败,当前存在相同的成品名称和型号");
                }
                data.IsInSAP = true;
            }
            var model = _mapper.Map<BaseGoods>(data);
            model.GoodsId = GetPrimaryId("S", lastData);
            model.CreateDate = DateTime.Now;
            Repository.ClientDb.Insertable(model).AddQueue();
            if (data.Photos?.Count > 0)
            {
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                data.Photos.ForEach(p =>
                {
                    photos.Add(new BaseFiles { PrimaryId = model.GoodsId, FileName = p.FileName, FileInfoType = FileInfoType.GoodsPhoto.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                    isSetDeft = true;
                });
                Repository.ClientDb.Insertable(photos).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 修改成品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateFinishedProduct(FinishedProductDto data)
        {
            var existId = await Repository.Exist<BaseGoods>(u => u.GoodsId == data.GoodsId);
            if (!existId)
            {
                throw new BusinessException("保存失败,当前成品不存在或已删除");
            }
            var existNo = await Repository.Exist<BaseGoods>(x => x.GoodsModel == data.GoodsModel && x.GoodsName == data.GoodsName && x.GoodsId != data.GoodsId);
            if (existNo)
            {
                throw new BusinessException("保存失败,当前存在相同的成品名称和型号");
            }
            var model = _mapper.Map<BaseGoods>(data);
            model.ModifyDate = DateTime.Now;
            Repository.ClientDb.Updateable(model).AddQueue();
            if (data.Photos?.Count > 0)
            {
                var photos = new List<BaseFiles>();
                var isSetDeft = false;
                data.Photos.ForEach(p =>
                {
                    photos.Add(new BaseFiles { PrimaryId = model.GoodsId, FileName = p.FileName, FileInfoType = FileInfoType.GoodsPhoto.ToString(), Url = p.Url, IsDeft = !isSetDeft ? true : false });
                    isSetDeft = true;
                });
                Repository.ClientDb.Deleteable<BaseFiles>(x => x.PrimaryId == data.GoodsId && x.FileInfoType == FileInfoType.GoodsPhoto.ToString()).AddQueue();
                Repository.ClientDb.Insertable(photos).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 删除成品
        /// </summary>
        /// <param name="clientsId"></param>
        /// <returns></returns>
        public async Task DelFinishedProduct(string[] productId)
        {
            var models = await Repository.ClientDb.Queryable<BaseGoods>().Where(x => productId.Contains(x.GoodsId)).ToListAsync();
            models.ForEach(x => x.IsDeleted = true);
            Repository.ClientDb.Updateable(models).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public List<KeyValueModel> GetExportField()
        {
            return new List<KeyValueModel>
            {
                new KeyValueModel {Key="GoodsId",Value="系统编码",Remark=1},
                new KeyValueModel {Key="GoodsNo",Value="SAP编码",Remark=2},
                new KeyValueModel {Key="GoodsName",Value="成品名称",Remark=1},
                new KeyValueModel {Key="TypeName",Value="成品分类", Remark = 3},
                new KeyValueModel {Key="OptionName",Value="成品类型",Remark=4},
                new KeyValueModel {Key="GoodsModel",Value="成品型号", Remark = 5},
                new KeyValueModel {Key="CustomerGoodsNo",Value="客户料号", Remark = 5},
                new KeyValueModel {Key="CustomerIdentificationCode",Value="客户识别码", Remark = 5},
                new KeyValueModel {Key="SpecName",Value="储存规格", Remark = 6},
                new KeyValueModel {Key="MaxStock",Value="规格最大堆放量",Remark=1},
                new KeyValueModel {Key="PackageUnitName",Value="系统编码", Remark = 7},
                new KeyValueModel {Key="RefPurchPrice",Value="采购参考单价", Remark = 1},
                new KeyValueModel {Key="RefPurchPriceUnitName",Value="采购单位", Remark = 8},
                new KeyValueModel {Key="PriceUnitName",Value="价格单位", Remark = 9},
                new KeyValueModel {Key="SafetyInventory",Value="安全库存", Remark = 10},
                new KeyValueModel {Key="SafetyInventoryUnitName",Value="库存单位", Remark = 11},
                new KeyValueModel {Key="Direction",Value="用途", Remark = 12},
                new KeyValueModel {Key="ExpirationDate",Value="保质期（月）", Remark = 13},
                new KeyValueModel {Key="IsInSAP",Value="是否在SAP", Remark = 14},
                new KeyValueModel {Key="PurchaseCycle",Value="采购周期（天）", Remark = 15},
                new KeyValueModel {Key="PurchaseMinimum",Value="最小采购量", Remark = 16},
                new KeyValueModel {Key="PurchaseMinimumUnitName",Value="采购单位", Remark = 17},
                new KeyValueModel {Key="IsOldForNew",Value="是否以旧换新", Remark = 18},
                new KeyValueModel {Key="ForArea",Value="所属区域", Remark = 19},
                new KeyValueModel {Key="Supplier",Value="供应商", Remark = 20},
                new KeyValueModel {Key="Long",Value="长度", Remark = 21},
                new KeyValueModel {Key="Wide",Value="宽度", Remark = 22},
                new KeyValueModel {Key="Height",Value="高度", Remark = 23},
                new KeyValueModel {Key="SizeUnitName",Value="尺寸单位", Remark = 24},
                new KeyValueModel {Key="Weight",Value="重量", Remark = 25},
                new KeyValueModel {Key="WeightUnitName",Value="重量单位", Remark = 26},
                new KeyValueModel {Key="Remark",Value="备注", Remark = 27},
                new KeyValueModel {Key="Photo",Value="成品图片", Remark = 28}
            };
        }

        /// <summary>
        /// 查询成品BOM
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<List<BOMDto>> GetBOMList(string goodsId)
        {
           return await Repository.ClientDb.Queryable<BaseBOM>()
                .LeftJoin<BaseGoods>((b,g)=>g.GoodsId==b.MaterialId)
                .LeftJoin<BaseType>((b,g,t)=>t.TypeId==g.GoodsClassifyId)
                .Where((b, g, t) => b.ParentId == goodsId).Select((b, g, t) => new BOMDto
                {
                    MaterialId = b.MaterialId,
                    MaterialName=b.MaterialName,
                    MaterialModel=g.GoodsModel,
                    MaterialClassifyGroup =b.MaterialClassifyGroup,
                    MaterialClassifyName=t.TypeName,
                    PackageUnitId = g.PackageUnitId,
                    PackageUnitName=g.PackageUnitName,
                    MinPackageUnitId=g.MinPackageUnitId,
                    MinPackageUnitName=g.MinPackageUnitName,
                    MaxPackageUnitId=g.MaxPackageUnitId,
                    MaxPackageUnitName=g.MaxPackageUnitName,
                    ParentId=b.ParentId,
                    Quantity=b.Quantity,
                    Unit=b.Unit
                })
                .ToListAsync();
        }

        /// <summary>
        /// 修改成品BOM
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task UpdateBOM(List<BOMDto> data)
        {
            foreach (var item in data)
            {
                if (item.Quantity == 0)
                {
                    throw new BusinessException($"请指定{item.MaterialName}需求数量");
                }
                if (string.IsNullOrEmpty(item.Unit))
                {
                    throw new BusinessException($"请指定{item.MaterialName}数量单位");
                }
            }
            var bomEnity = _mapper.Map<List<BaseBOM>>(data);
            var parentId= data.First().ParentId;
            Repository.ClientDb.Deleteable<BaseBOM>(d => d.ParentId == parentId).AddQueue();
            Repository.ClientDb.Insertable(bomEnity).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 导出成品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<string> ExportFinishedProduct(string searchKey, string orderField, string orderType, int goodsClassifyId, List<KeyValueModel> fields)
        {
            if (fields.Count > 0)
            {
                string dbType = _configuration.GetSection("Sqlsugar:DbType").Value;
                orderField = string.IsNullOrEmpty(orderField) ? "GoodsName" : orderField;
                searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
                var param = new Dictionary<string, object>
                    {
                        { "@GoodsName", "%"+searchKey+"%" },
                        { "@GoodsModel", "%"+searchKey+"%" },
                        { "@GoodsNo", "%"+searchKey+"%" },
                        { "@Supplier", "%"+searchKey+"%" }
                    };
                StringBuilder sb = new StringBuilder();
                foreach (var field in fields)
                {
                    if (field.Key.ToString() == "Photo")
                    {
                        sb.Append($"(select Url from BaseFiles where PrimaryId=g.GoodsId and FileInfoType='GoodsPhoto' and IsDeft=1) as {field.Value},");
                    }
                    else if (field.Key.ToString() == "Remark")
                    {
                        if (dbType == "MySql")
                        {
                            sb.Append($"g.`{field.Key}` as {field.Value},");
                        }
                        else
                        {
                            sb.Append($"g.[{field.Key}] as {field.Value},");
                        }
                    }
                    else
                    {
                        if (dbType == "MySql")
                        {
                            sb.Append($"`{field.Key}` as {field.Value},");
                        }
                        else
                        {
                            sb.Append($"[{field.Key}] as {field.Value},");
                        }
                    }
                }
                var fieldStr = sb.ToString().TrimEnd(',');
                string sql = $@"select  {fieldStr}
                            from BaseGoods g
                            left join BaseType t on t.TypeId=g.GoodsClassifyId
                            left join SysArgsOptions a on a.OptionId=g.GoodsTypeId
                            left join InvWorkbinSpecification s on s.SpecId=g.GoodsSpecificationId
                            where g.IsDeleted=0 and ( case when {goodsClassifyId}=0 then t.Group='FinishedProduct' else g.GoodsClassifyId={goodsClassifyId} end ) 
                            and (g.GoodsName like @GoodsName or g.GoodsNo like @GoodsNo or g.GoodsModel like @GoodsModel or g.Supplier like @Supplier)
                            order by {orderField} {orderType}";
                var queryData = await Repository.QueryBySqlAsync(sql, param);
                var stream = ExcelHelper.ConvertDataTableToStream(queryData);
                var fileName = $"成品信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx";
                var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);
                return fileUrl;
            }
            return await Task.FromResult("");
        }
        #endregion
    }
}
