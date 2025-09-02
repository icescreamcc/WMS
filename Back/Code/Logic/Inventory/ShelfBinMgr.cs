using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicCommon;
using Models.Model.Inv;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Models.Model.Enum;

namespace Logic.Inventory
{
    public class ShelfBinMgr: DataPermissionHandler
    {
        private readonly SysArgsService _sysArgsHelper;

        private readonly IMapper _mapper; 

        public ShelfBinMgr(Repository repository, SysArgsService sysArgsHelper, IMapper mapper) : base(repository)
        {
            _sysArgsHelper = sysArgsHelper;
            _mapper = mapper;
        }

        /// <summary>
        /// 仓库、货架、货位树形结构数据
        /// 启用货架时必须启用货位
        /// 启用货位时不一定启用货架
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetWarehouseTree()
        { 
            var warehouseData = await Repository.ClientDb.Queryable<InvWarehouse>().Where(w=>!w.IsAbandon).ToListAsync();
            var treeWarehouse = new List<TreeModel>();
            if (warehouseData?.Count > 0)
            {
                warehouseData.ForEach(w =>
                {
                    treeWarehouse.Add(new TreeModel
                    {
                        Id = w.WarehouseId,
                        Label = w.WarehouseName,
                        Remark = w.WarehouseNo,
                        Type = StorageUnitType.Warehouse.ToString(),
                    });
                });
            }
            var shelfData = await Repository.ClientDb.Queryable<InvShelf>().OrderBy(o => o.Rank).ToListAsync();
            var binData = await Repository.ClientDb.Queryable<InvBin>().OrderBy(o => o.Rank).ToListAsync();
            treeWarehouse.ForEach(f =>
            {
                //仓库下的货架
                f.Children = shelfData.Where(w => w.WarehouseId == f.Id.ToString()).Select(s => new TreeModel { Id = s.ShelfId, Label = s.ShelfName, Remark = s.ShelfNo, Type = StorageUnitType.Shelf.ToString() }).ToList();
                if (binData?.Count > 0)
                {
                    //仓库下的库位
                    var childBin = binData.Where(w => w.WarehouseId == f.Id.ToString() && string.IsNullOrEmpty(w.ShelfId)).Select(s => new TreeModel { Id = s.BinId, Label = s.BinName, Remark = s.BinNo, Type = StorageUnitType.Bin.ToString() }).ToList();
                    f.Children.AddRange(childBin);
                    //货架下的库位
                    f.Children.ForEach(b =>
                    {
                        b.Children = binData.Where(w => w.ShelfId == b.Id.ToString()).Select(s => new TreeModel { Id = s.BinId, Label = s.BinName, Remark = s.BinNo, Type = StorageUnitType.Bin.ToString() }).ToList();
                    });
                }
            });
            return treeWarehouse;
        }

        public async Task<WarehouseDetail> GetWarehouseDetail(string warehouseId)
        {
            var res = await Repository.GetSingeAsync<InvWarehouse>(warehouseId);
            return _mapper.Map<WarehouseDetail>(res);
        }

        public async Task<Shelf> GetShelfDetail(string shelfId)
        {
           return await Repository.ClientDb.Queryable<InvShelf>()
                .LeftJoin<InvWarehouse>((s, w) => s.WarehouseId == w.WarehouseId) 
                .Where((s, w) => s.ShelfId == shelfId)
                .Select<Shelf>().SingleAsync(); 
        }

        public async Task<Bin> GetBinDetail(int binId)
        {
            return await Repository.ClientDb.Queryable<InvBin>()
              .LeftJoin<InvWarehouse>((b, w) => b.WarehouseId == w.WarehouseId)
              .LeftJoin<InvShelf>((b, w, s) => b.ShelfId == s.ShelfId)
              .Where((b, w, s) => b.BinId == binId)
              .Select<Bin>().SingleAsync(); 
        }

        #region 货架信息增删改

        public async Task<string> AddShelf(Shelf data)
        {
            var exist = await Repository.Exist<InvShelf>(u => u.ShelfNo == data.ShelfNo);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货架编号已存在");
            }
            exist =await Repository.Exist<InvShelf>(e=>e.ShelfName == data.ShelfName);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货架名称已存在");
            }
            var model = _mapper.Map<InvShelf>(data);
            var lastData = await Repository.ClientDb.Queryable<InvShelf>().MaxAsync(x => x.ShelfId);
            model.ShelfId = GetPrimaryId("S", lastData);
            await Repository.ClientDb.Insertable(model).ExecuteCommandAsync(); 
            return model.ShelfId;
        }

        public async Task UpdateShelf(Shelf data)
        {
            var exist = await Repository.Exist<InvShelf>(u => u.ShelfNo == data.ShelfNo && u.ShelfId != data.ShelfId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货架编号已存在");
            }
            exist = await Repository.Exist<InvShelf>(e => e.ShelfName == data.ShelfName&& e.ShelfId != data.ShelfId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货架名称已存在");
            } 
            if (!data.HasWorkbin)
            {
                var workbins = await Repository.ClientDb.Queryable<InvWorkbin>().Where(w => w.ShelfId == data.ShelfId).ToListAsync();
                if (workbins.Count>0)
                {
                    var hasWorkbinsNo = workbins.Any(a => !string.IsNullOrEmpty(a.WorkbinNo));
                    if (hasWorkbinsNo)
                    {
                        var existStock = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().AnyAsync(a => a.ShelfId == data.ShelfId && a.WorkbinCellId > 0 && a.Stock > 0);
                        var existFlow= await Repository.ClientDb.Queryable<InvStorageFlowDetail>().AnyAsync(a=>a.ShelfId==data.ShelfId&&a.WorkbinCellId > 0 &&a.IsStatistics==false);
                        if (existStock|| existFlow)
                        {
                            throw new BusinessException("保存失败,当前货架料箱存在库存数据，不能取消料箱");
                        } 
                    }
                    Repository.ClientDb.Deleteable(workbins).AddQueue();
                    var workbinId = workbins.Select(s => s.WorkbinId).ToList();
                    var workbinCell = await Repository.ClientDb.Queryable<InvWorkbinCell>().Where(w => workbinId.Contains(w.WorkbinId)).ToListAsync();
                    if (workbinCell?.Count > 0)
                    {
                        Repository.ClientDb.Deleteable(workbinCell).AddQueue();
                    }
                }
            } 
            var model = _mapper.Map<InvShelf>(data);
            Repository.ClientDb.Updateable(model).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task DelShelf(string shelfId)
        {
           
            var exist = await Repository.ClientDb.Queryable<InvBin>().AnyAsync(a => (a.Status == BinStatus.Lock.ToString() || a.Status == BinStatus.Full.ToString()) && shelfId== a.ShelfId);
            if (exist)
            {
                throw new BusinessException("删除失败,当前货架中存在正在使用的库位");
            }
            var data = await Repository.ClientDb.Queryable<InvShelf>().SingleAsync(s => s.ShelfId == shelfId);
            if (data.HasWorkbin)
            {
                var workbins = await Repository.ClientDb.Queryable<InvWorkbin>().Where(w => w.ShelfId == data.ShelfId).ToListAsync();
                if (workbins.Count > 0)
                {
                    var hasWorkbinsNo = workbins.Any(a => !string.IsNullOrEmpty(a.WorkbinNo));
                    if (hasWorkbinsNo)
                    {
                        var existStock = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().AnyAsync(a => a.ShelfId == data.ShelfId && a.WorkbinCellId > 0 && a.Stock > 0);
                        var existFlow = await Repository.ClientDb.Queryable<InvStorageFlowDetail>().AnyAsync(a => a.ShelfId == data.ShelfId && a.WorkbinCellId > 0 && a.IsStatistics == false);
                        if (existStock || existFlow)
                        {
                            throw new BusinessException("删除失败,当前货架存库存数据");
                        }
                    }
                    Repository.ClientDb.Deleteable(workbins).AddQueue();
                    var workbinId = workbins.Select(s => s.WorkbinId).ToList();
                    var workbinCell = await Repository.ClientDb.Queryable<InvWorkbinCell>().Where(w => workbinId.Contains(w.WorkbinId)).ToListAsync();
                    if (workbinCell?.Count > 0)
                    {
                        Repository.ClientDb.Deleteable(workbinCell).AddQueue();
                    }
                }
            }
            Repository.ClientDb.Deleteable<InvShelf>(b => shelfId.Contains(b.ShelfId)).AddQueue();
            Repository.ClientDb.Deleteable<InvBin>(b => shelfId.Contains(b.ShelfId)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        #endregion

        #region 货位信息增删改

        public async Task<int> AddBin(Bin data)
        {
            var exist = await Repository.Exist<InvBin>(u => u.BinNo == data.BinNo);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货位编号已存在");
            }
            exist = await Repository.Exist<InvBin>(e => e.BinName == data.BinName);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货位名称已存在");
            }
            var model = _mapper.Map<InvBin>(data);
            model.Status= BinStatus.Free.ToString();
            var binId = await Repository.ClientDb.Insertable(model).ExecuteReturnIdentityAsync();

            var hasWorkbin=await Repository.ClientDb.Queryable<InvShelf>().AnyAsync(a=>a.ShelfId==data.ShelfId&&a.HasWorkbin);
            if (hasWorkbin)
            {
                var workbin = new InvWorkbin
                {
                    WarehouseId = model.WarehouseId,
                    ShelfId = model.ShelfId,
                    BinId = binId
                };
                var sameBinNoArgsVal = (await _sysArgsHelper.GetValueByKey(BusinessConst.IsWorkbinNoSameBinNo)).Value.ToString();
                var isSameBinNo=bool.Parse(sameBinNoArgsVal);
                if (isSameBinNo)
                {
                    workbin.WorkbinNo = data.BinNo;
                }
               await Repository.ClientDb.Insertable(workbin).ExecuteCommandAsync();
            } 
            return binId;
        }

        public async Task UpdateBin(Bin data)
        {
            var exist = await Repository.Exist<InvBin>(u => u.BinNo == data.BinNo && u.BinId != data.BinId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货位编号已存在");
            }
            exist = await Repository.Exist<InvBin>(e => e.BinName == data.BinName && e.BinId != data.BinId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前货位名称已存在");
            }
            var model = _mapper.Map<InvBin>(data);
            Repository.ClientDb.Updateable(model).AddQueue();
            var workbin = await Repository.ClientDb.Queryable<InvWorkbin>().Where(w => w.BinId == data.BinId).SingleAsync();
            if(workbin != null)
            { 
                var sameBinNoArgsVal = (await _sysArgsHelper.GetValueByKey(BusinessConst.IsWorkbinNoSameBinNo)).Value.ToString();
                var isSameBinNo = bool.Parse(sameBinNoArgsVal);
                if (isSameBinNo)
                {
                    workbin.WorkbinNo = data.BinNo;
                    Repository.ClientDb.Updateable(workbin).AddQueue();
                }
            }
           await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task DelBin(int binId)
        {
            var bin = await Repository.GetSingeAsync<InvBin>(binId); 
            if (bin.Status== BinStatus.Lock.ToString() || bin.Status == BinStatus.Full.ToString())
            {
                throw new BusinessException("删除失败,当前库位正在使用中");
            }
            if (!string.IsNullOrEmpty(bin.ShelfId))
            {
                var shelf = await Repository.ClientDb.Queryable<InvShelf>().SingleAsync(s => s.ShelfId == bin.ShelfId);
                if (shelf.HasWorkbin)
                {
                    var workbin = await Repository.ClientDb.Queryable<InvWorkbin>().Where(w => w.BinId == binId).SingleAsync();
                    if (workbin != null)
                    {
                        if (!string.IsNullOrEmpty(workbin.WorkbinNo))
                        {
                            var existStock = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().AnyAsync(a => a.BinId == binId && a.WorkbinCellId > 0 && a.Stock > 0);
                            var existFlow = await Repository.ClientDb.Queryable<InvStorageFlowDetail>().AnyAsync(a => a.BinId == binId && a.WorkbinCellId > 0 && a.IsStatistics == false);
                            if (existStock || existFlow)
                            {
                                throw new BusinessException("删除失败,当前货位存在库存数据");
                            }
                        }
                        Repository.ClientDb.Deleteable(workbin).AddQueue();
                        var workbinCell = await Repository.ClientDb.Queryable<InvWorkbinCell>().Where(w => w.WorkbinId == workbin.WorkbinId).ToListAsync();
                        if (workbinCell?.Count > 0)
                        {
                            Repository.ClientDb.Deleteable(workbinCell).AddQueue();
                        }
                    }
                }
            } 
            Repository.ClientDb.Deleteable(bin).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        #endregion
    }
}
