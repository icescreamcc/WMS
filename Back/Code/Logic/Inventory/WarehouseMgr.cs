using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Inv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace Logic.Inventory
{
    /// <summary>
    /// 仓库业务处理类
    /// </summary>
   public class WarehouseMgr: DataPermissionHandler
    {  
        private readonly SysArgsService _sysArgsHelper;

        private readonly IMapper _mapper;

        public WarehouseMgr(Repository repository, SysArgsService sysArgsHelper, IMapper mapper ) : base(repository)
        { 
            _sysArgsHelper = sysArgsHelper;
            _mapper= mapper;
        } 

        /// <summary>
        /// 分页查询所有仓库信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<WarehouseDetail>> GetWarehouses(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "WarehouseNo" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<InvWarehouse>()
                  .Where(c => c.WarehouseNo.Contains(searchKey) || c.WarehouseName.Contains(searchKey) || c.Address.Contains(searchKey) || c.ChargePerson.Contains(searchKey) || c.Remark.Contains(searchKey))
                  .Select(c => new WarehouseDetail
                  {
                      WarehouseId = c.WarehouseId,
                      WarehouseNo = c.WarehouseNo,
                      WarehouseName = c.WarehouseName,
                      WarehouseType = c.WarehouseType,
                      Province=c.Province,
                      City=c.City,
                      Address = c.Address,
                      ChargePerson = c.ChargePerson,
                      ChargePersonPhone = c.ChargePersonPhone,
                      InventoryDateOfLast = c.InventoryDateOfLast,
                      InventoryOperatorIdOfLast = c.InventoryOperatorIdOfLast,
                      InventoryOperatorNameOfLast = c.InventoryOperatorNameOfLast,
                      Remark = c.Remark,
                      IsAbandon = c.IsAbandon,
                      SpareField1 = c.SpareField1,
                      SpareField2 = c.SpareField2,
                      SpareField3 = c.SpareField3,
                      SpareField4 = c.SpareField4,
                      SpareField5 = c.SpareField5
                  })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total); 
            var res = new TableModel<WarehouseDetail>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }
         

        /// <summary>
        /// 根据仓库查询库位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public async Task<List<WarehouseBin>> GetWarehouseBins(string warehouseId)
        {
            return await Repository.ClientDb.Queryable<InvBin>()
                .Where(b => b.WarehouseId == warehouseId)
                .OrderBy(b => b.BinId)
                .Select(b => new WarehouseBin { WarehouseId=b.WarehouseId, BinId=b.BinId, BinNo=b.BinNo,BinName=b.BinName, Remark=b.Remark }).ToListAsync();
        }

        #region 仓库信息增删改 

        /// <summary>
        /// 添加仓库、库位信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddWarehouseBin(WarehouseDetail data)
        {
            var exist = await Repository.Exist<InvWarehouse>(u =>u.WarehouseNo == data.WarehouseNo);
            if (exist)
            {
                throw new BusinessException("保存失败,当前仓库编码已存在");
            }
            var lastData = await Repository.ClientDb.Queryable<InvWarehouse>().MaxAsync(x => x.WarehouseId); 
            var model = new InvWarehouse
            {
                WarehouseId = GetPrimaryId("W", lastData),
                WarehouseNo = data.WarehouseNo,
                WarehouseName = data.WarehouseName,
                WarehouseType = data.WarehouseType,
                Province = data.Province,
                City = data.City,
                Address = data.Address,
                ChargePerson = data.ChargePerson,
                ChargePersonPhone = data.ChargePersonPhone,
                InventoryDateOfLast = data.InventoryDateOfLast,
                InventoryOperatorIdOfLast = data.InventoryOperatorIdOfLast,
                InventoryOperatorNameOfLast = data.InventoryOperatorNameOfLast,
                IsAbandon = data.IsAbandon,
                Remark = data.Remark,
                SpareField1 = data.SpareField1,
                SpareField2 = data.SpareField2,
                SpareField3 = data.SpareField3,
                SpareField4 = data.SpareField4,
                SpareField5 = data.SpareField5
            };
            await Repository.ClientDb.Insertable(model).ExecuteCommandAsync(); 
        }

        /// <summary>
        /// 修改仓库、库位信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateWarehouseBin(WarehouseDetail data)
        {
            var exist = await Repository.Exist<InvWarehouse>(u => u.WarehouseId == data.WarehouseId);
            if (!exist)
            {
                throw new BusinessException("保存失败,当前仓库ID不存在或已删除"); 
            }
            exist = await Repository.Exist<InvWarehouse>( u => u.WarehouseId != data.WarehouseId&&u.WarehouseNo == data.WarehouseNo);
            if (exist)
            {
                throw new BusinessException("保存失败,当前仓库编码已存在"); 
            }
            var model = new InvWarehouse
            {
                WarehouseId = data.WarehouseId,
                WarehouseNo = data.WarehouseNo,
                WarehouseName = data.WarehouseName,
                WarehouseType = data.WarehouseType,
                Province = data.Province,
                City = data.City,
                Address = data.Address,
                ChargePerson = data.ChargePerson,
                ChargePersonPhone = data.ChargePersonPhone,
                InventoryDateOfLast = data.InventoryDateOfLast,
                InventoryOperatorIdOfLast = data.InventoryOperatorIdOfLast,
                InventoryOperatorNameOfLast = data.InventoryOperatorNameOfLast,
                IsAbandon = data.IsAbandon,
                Remark = data.Remark,
                SpareField1 = data.SpareField1,
                SpareField2 = data.SpareField2,
                SpareField3 = data.SpareField3,
                SpareField4 = data.SpareField4,
                SpareField5 = data.SpareField5
            };
            await Repository.ClientDb.Updateable(model).ExecuteCommandAsync();  
        }

        /// <summary>
        /// 删除仓库、库位信息
        /// </summary>
        /// <param name="warehousesId"></param>
        /// <returns></returns>
        public async Task DelWarehouseBin(string[] warehousesId)
        {
            var exist=await Repository.ClientDb.Queryable<InvBin>().AnyAsync(a=>(a.Status == BinStatus.Lock.ToString() || a.Status == BinStatus.Full.ToString())&& warehousesId.Contains(a.WarehouseId));
            if (exist)
            {
                throw new BusinessException("删除失败,当前仓库存在正在使用的库位");
            }
            var existStock = await Repository.ClientDb.Queryable<InvStorageWarehouseDetail>().AnyAsync(a => warehousesId.Contains(a.WarehouseId) && a.Stock > 0);
            var existFlow = await Repository.ClientDb.Queryable<InvStorageFlowDetail>().AnyAsync(a => warehousesId.Contains(a.WarehouseId) && a.IsStatistics == false);
            if (existStock || existFlow)
            {
                throw new BusinessException("删除失败,当前仓库存在库存数据，不能取消胶箱");
            }
            Repository.ClientDb.Deleteable<InvWarehouse>(b => warehousesId.Contains(b.WarehouseId)).AddQueue();
            Repository.ClientDb.Deleteable<InvShelf>(b => warehousesId.Contains(b.WarehouseId)).AddQueue();
            Repository.ClientDb.Deleteable<InvBin>(b => warehousesId.Contains(b.WarehouseId)).AddQueue();
            Repository.ClientDb.Deleteable<InvWorkbin>(b => warehousesId.Contains(b.WarehouseId)).AddQueue();
            Repository.ClientDb.Deleteable<InvWorkbinCell>(b => warehousesId.Contains(b.WarehouseId)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        #endregion
         
    }
}
