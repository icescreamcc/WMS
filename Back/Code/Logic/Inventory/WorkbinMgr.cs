using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Inv;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SqlSugar;

namespace Logic.Inventory
{
    public class WorkbinMgr : DataPermissionHandler
    {
        private readonly IMapper _mapper;

        public WorkbinMgr(Repository repository,IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<TableModel<WorkbinDto>> GetWorkbins( int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "ShelfNo" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<InvWorkbin>()
                  .LeftJoin<InvWarehouse>((wb,w)=>w.WarehouseId==wb.WarehouseId)
                  .LeftJoin<InvShelf>((wb,w,s)=>s.ShelfId==wb.ShelfId)
                  .LeftJoin<InvBin>((wb,w,s,b)=>b.BinId==wb.BinId)
                  .LeftJoin<InvWorkbinSpecification>((wb, w, s, b,wbs)=>wbs.SpecId==wb.SpecId)
                  .Where((wb, w, s,b, wbs) => w.WarehouseNo.Contains(searchKey) || w.WarehouseName.Contains(searchKey) || s.ShelfNo.Contains(searchKey) || s.ShelfName.Contains(searchKey) || wb.WorkbinNo.Contains(searchKey)||wb.Remark.Contains(searchKey))
                  .Select<WorkbinDto>()
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total); 
            var mapData=_mapper.Map<List<WorkbinDto>>(data);
            var res = new TableModel<WorkbinDto>() { Total = total, Rows = mapData };
            return await Task.FromResult(res);
        }

        public async Task<List<WorkbinCellDto>> GetWorkbinCells(int workbinId)
        {
            return await Repository.ClientDb.Queryable<InvWorkbinCell>().Where(w=>w.WorkbinId==workbinId).Select<WorkbinCellDto>().ToListAsync();
        }

        public async Task<List<WorkbinCellDto>> GetWorkbinCells(string cellNo)
        {
            return await Repository.ClientDb.Queryable<InvWorkbinCell>()
                .Where(w => w.WorkbinId == SqlFunc.Subqueryable<InvWorkbinCell>().Where(c=>c.CellNo==cellNo).Select(c=>c.WorkbinId)).Select<WorkbinCellDto>().ToListAsync();
        }

        public async Task<List<WorkbinSpecificationDto>> GetWorkbinSpec()
        {
            return await Repository.ClientDb.Queryable<InvWorkbinSpecification>().Select<WorkbinSpecificationDto>().ToListAsync();
        }

        public async Task UpdateWorkbin(WorkbinDto data)
        {
            if (string.IsNullOrEmpty(data.WorkbinNo))
            {
                throw new BusinessException("保存失败,请指定料箱编号");
            }
            var exist = await Repository.Exist<InvWorkbin>(e => e.WorkbinNo == data.WorkbinNo&& e.WorkbinId!=data.WorkbinId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前料箱编号已存在");
            }
            var existStock = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().AnyAsync(a => a.WorkbinId == data.WorkbinId  && a.Stock > 0);
            var existFlow = await Repository.ClientDb.Queryable<InvStorageFlowDetail>().AnyAsync(a => a.WorkbinId == data.WorkbinId  && a.IsStatistics == false);
            if (existStock || existFlow)
            {
                throw new BusinessException("保存失败,当前料箱存在库存数据，不允许更改料箱规格");
            }
            var entity = _mapper.Map<InvWorkbin>(data);
            Repository.ClientDb.Updateable(entity).AddQueue();

            var spec= await Repository.ClientDb.Queryable<InvWorkbinSpecification>().SingleAsync(s=>s.SpecId==data.SpecId);
            var cells = new List<InvWorkbinCell>();
            for (var i=1;i<=spec.CellCount;i++)
            {
                cells.Add(new InvWorkbinCell
                {
                    WarehouseId = data.WarehouseId,
                    ShelfId = data.ShelfId,
                    BinId = data.BinId,
                    WorkbinId = data.WorkbinId,
                    CellNo = data.WorkbinNo+"-" + i  
                }); 
            }
            Repository.ClientDb.Deleteable<InvWorkbinCell>(d=>d.WorkbinId==data.WorkbinId).AddQueue();
            Repository.ClientDb.Insertable(cells).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public  async Task AddWorkbinSpeci(WorkbinSpecificationDto data)
        {
            if ( string.IsNullOrEmpty(data.SpecName))
            {
                throw new BusinessException("请指定规格名称");
            }
            var exist = await Repository.Exist<InvWorkbinSpecification>(e =>e.SpecName==data.SpecName);
            if (exist)
            {
                throw new BusinessException("规格名称已存在");
            }
            var entiy = _mapper.Map<InvWorkbinSpecification>(data);
            await Repository.AddAsync(entiy);
        }

        public async Task UpdateWorkbinSpeci(WorkbinSpecificationDto data)
        {
            if ( string.IsNullOrEmpty(data.SpecName))
            {
                throw new BusinessException("保存失败,请指定规格名称");
            }
            var exist = await Repository.Exist<InvWorkbinSpecification>(e => (e.SpecName == data.SpecName&&e.SpecId!=data.SpecId));
            if (exist)
            {
                throw new BusinessException("保存失败,规格名称已存在");
            }
            exist=await Repository.Exist<InvWorkbin>(e=>e.SpecId==data.SpecId);
            if (exist)
            {
                throw new BusinessException("该规格已有料箱使用，请先修改该料箱的规格");
            }
            var entiy = _mapper.Map<InvWorkbinSpecification>(data);
            await Repository.UpdateAsync(entiy);
        }

        public async Task DelWorkbinSpeci(int specId)
        {
            await Repository.DeleteAsync<InvWorkbinSpecification>(d=>d.SpecId==specId);
        }
    }
}
