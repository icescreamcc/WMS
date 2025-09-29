using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Microsoft.Extensions.Configuration;
using Models.Model.AutomationDevice;
using Models.Model.Enum;
using Models.Model.Inv; 

namespace Logic.AutomationDevice
{
    public class AutoTransportInfrastructureMgr : DbOperationHandler
    {
        private readonly IConfiguration _configuration;
        public AutoTransportInfrastructureMgr(Repository repository, IConfiguration configuration) : base(repository)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取自动运输相关的基础数据
        /// </summary>
        /// <returns></returns>
        public object GetAutoTransportInfrastructureInfo(string goodsClassifyGroup)
        { 
            //仓库、货架
            var warehouseBinQuery = Repository.ClientDb.Queryable<InvWarehouse>()
                .InnerJoin<InvShelf>((w, s) => s.WarehouseId == w.WarehouseId)
                .Where((w, s) => w.WarehouseType == goodsClassifyGroup && !w.IsAbandon && !s.IsAbandon)
                .Select((w, s) => new
                {
                    w.WarehouseId,
                    w.WarehouseName,
                    s.ShelfId,
                    s.ShelfNo,
                    ShelfProperty = s.Property,
                    ShelfRank = s.Rank
                }).ToList();
            var warehouseInfo = warehouseBinQuery.GroupBy(g => new { g.WarehouseId, g.WarehouseName }).Select(g => new WarehouseDto { WarehouseId = g.Key.WarehouseId, WarehouseName = g.Key.WarehouseName }).ToList();
            foreach (var warehouse in warehouseInfo)
            {
                warehouse.Areas = new List<WarehouseAreaDto>();
                var areas = warehouseBinQuery.Where(w => w.WarehouseId == warehouse.WarehouseId).OrderBy(w => w.ShelfRank).Select(w => w.ShelfNo.Split("-")[1][0]).Distinct().ToList();
                foreach (var area in areas)
                {
                    var shelfs = warehouseBinQuery.Where(w => w.WarehouseId == warehouse.WarehouseId && w.ShelfNo.Split("-")[1][0] == area).ToList();
                    var shelfProperty = shelfs.Where(w => !string.IsNullOrEmpty(w.ShelfProperty)).Select(w => w.ShelfProperty).First();
                    var shelfCol = int.Parse(shelfProperty[0].ToString());
                    warehouse.Areas.Add(new WarehouseAreaDto
                    {
                        AreaNo = area.ToString(),
                        Shelfs = shelfs.Select(w => new WarehouseShelfDto
                        {
                            ShelfId = w.ShelfId,
                            ShelfNo = w.ShelfNo,
                            Rank = w.ShelfRank,
                            Col = shelfCol
                        }).ToList(),
                    });
                }
            }
            //传送设备及仓位 DeviceResponseDto
            var warehouseIdArr = warehouseInfo.Select(w => w.WarehouseId).ToList();
            var deviceInfo = Repository.ClientDb.Queryable<AutoProdDevice>().Where(w => warehouseIdArr.Contains(w.WarehouseId) && w.IsActive).Select<DeviceDto>().ToList();
            var deviceBin = Repository.ClientDb.Queryable<AutoProdDeviceWarehouse>().Select<DeviceBinDetailDto>().ToList();
            deviceInfo.ForEach(d =>
            {
                d.BinDetails = deviceBin.Where(w => w.DeviceId == d.DeviceId).ToList();
            });
            warehouseInfo.ForEach(f =>
            {
                f.TransportDevice = deviceInfo.Where(w => w.WarehouseId == f.WarehouseId).ToList();
            });
            //AGV小车
            var agvInfo = Repository.ClientDb.Queryable<AutoProdDeviceAGV>().Select<AutoProdDeviceAGVDto>().ToList();
            return new {warehouseInfo, agvInfo };
        }
         
        /// <summary>
        /// 检查当前操作的客户端是否为领料窗口的客户端电脑
        /// </summary>
        /// <param name="clientIP"></param>
        /// <returns></returns>
        public bool IsAllowAutoTransport(string clientIP)
        {
            var allowIP= _configuration.GetSection("AutoTransport:AllowIP").Value;
            return clientIP == allowIP;
        }

        /// <summary>
        /// 根据领料的历史记录查询需要送货的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        public async Task<List<RequisitionGoodsDetail>> GetHisTask(string orderNo)
        {
            return await Repository.ClientDb.Queryable<InvRequisitionOrder>()
              .InnerJoin<InvRequisitionOrderDetail>((o, d) => d.OrderNo == o.OrderNo)
              .LeftJoin<BaseGoods>((o, d, g) => g.GoodsId == d.GoodsId)
              .LeftJoin<BaseType>((o, d, g, t) => t.TypeId == g.GoodsClassifyId)
              .LeftJoin<BaseFiles>((o, d, g, t, f) => f.PrimaryId == g.GoodsId && f.IsDeft && f.FileInfoType == FileInfoType.GoodsPhoto.ToString())
              .Where((o, d, g, t, f) => o.OrderNo == orderNo)
              .OrderByDescending((o, d, g, t, f) => o.CreateDate)
              .Select<RequisitionGoodsDetail>()
              .ToListAsync();
        } 
    } 
}
