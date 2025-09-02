using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using Logic.LogicBase;
using Models.Model.Enum; 
using Models.Model;
using Models.Model.AutomationDevice;

namespace Logic.AutomationDevice
{
    public class WorkShopDeviceInfoMgr : DbOperationHandler
    {
        private readonly IMapper _mapper;

        public WorkShopDeviceInfoMgr(Repository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }
         
        public async Task<TableModel<AutoProdDeviceDto>> GetDeviceInfo(int pgSize, int pgIndex, string orderFiled, string orderType, string warehouseId, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "DeviceNo" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<AutoProdDevice>() 
                  .LeftJoin<InvWarehouse>((d,w)=>d.WarehouseId==w.WarehouseId)
                  .WhereIF(!string.IsNullOrEmpty(warehouseId), (d, w) => d.WarehouseId == warehouseId)
                  .Where((d, w) => d.DeviceNo.Contains(searchKey) || d.DeviceName.Contains(searchKey)||d.ConnectAddress.Contains(searchKey))
                  .Select<AutoProdDeviceDto>()
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(f =>
            {
                f.DeviceTypeDesc = EnumHelper.GetDescFromEnumVal<AutoProdDeviceType>(f.DeviceType);
            });
            var res = new TableModel<AutoProdDeviceDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<AutoProdDeviceDto> GetDeviceInfoDetail(int deviceId)
        {
            var data = await Repository.ClientDb.Queryable<AutoProdDevice>() 
                .Where(d => d.DeviceId == deviceId)
                .Select<AutoProdDeviceDto>().SingleAsync();
            data.DeviceTypeDesc = EnumHelper.GetDescFromEnumVal<AutoProdDeviceType>(data.DeviceType);
            return data;
        }

        public async Task<List<KeyValueModel>> GetDeviceInfoOptions(string deviceType="")
        {
            return await Repository.ClientDb.Queryable<AutoProdDevice>()
                .WhereIF(!string.IsNullOrEmpty(deviceType), w => w.DeviceType == deviceType)
                .Select(s => new KeyValueModel
                {
                    Key = s.DeviceId,
                    Value = s.DeviceName,
                }).ToListAsync();
        }

        public async Task AddDeviceInfo(AutoProdDeviceDto data)
        {
            if (string.IsNullOrEmpty(data.DeviceNo))
            {
                throw new BusinessException("保存失败,设备编号不能为空");
            }
            var exist = await Repository.Exist<AutoProdDevice>(x => x.DeviceNo == data.DeviceNo|| x.DeviceName == data.DeviceName);
            if (exist)
            {
                throw new BusinessException("保存失败,当前存在相同的设备编号或名称");
            }
            var model = _mapper.Map<AutoProdDevice>(data); 
            await Repository.AddAsync(model);
        }

        public async Task UpdateDeviceInfo(AutoProdDeviceDto data)
        {
            if (string.IsNullOrEmpty(data.DeviceNo))
            {
                throw new BusinessException("保存失败,设备编号不能为空");
            }
            var exist = await Repository.Exist<AutoProdDevice>(x =>x.DeviceId!=data.DeviceId && (x.DeviceNo == data.DeviceNo || x.DeviceName == data.DeviceName));
            if (exist)
            {
                throw new BusinessException("保存失败,当前存在相同的设备编号或名称");
            }
            var model = _mapper.Map<AutoProdDevice>(data);
            await Repository.UpdateAsync(model);
        }

        public async Task DelDeviceInfo(int[] devicesId)
        {
            var exist = await Repository.ClientDb.Queryable<AutoProdDeviceWarehouse>().Where(w => devicesId.Contains(w.DeviceId)).AnyAsync();
            if (exist)
            {
                throw new BusinessException("删除失败,设备中存在未删除的仓位信息");
            }
            await Repository.ClientDb.Deleteable<AutoProdDevice>(d => devicesId.Contains(d.DeviceId)).ExecuteCommandAsync();
        }

        public async Task<TableModel<AutoProdDeviceWarehouseDto>> GetDeviceBin(int pgSize, int pgIndex, string orderFiled, string orderType, string deviceType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "DeviceNo" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var data = Repository.ClientDb.Queryable<AutoProdDeviceWarehouse>()
                .InnerJoin<AutoProdDevice>((w,d)=>w.DeviceId == d.DeviceId)
                  .WhereIF(!string.IsNullOrEmpty(deviceType), (w,d)=>d.DeviceType==deviceType)
                  .Where((w, d) => d.DeviceNo.Contains(searchKey) || d.DeviceName.Contains(searchKey) || d.ConnectAddress.Contains(searchKey) || w.BinNo.Contains(searchKey) || w.AGVBinCode_Delivery.Contains(searchKey) || w.AGVBinCode_Receive.Contains(searchKey))
                  .Select<AutoProdDeviceWarehouseDto>()
                  .OrderBy($"{orderFiled} {orderType},Sort asc")
                  .ToPageList(pgIndex, pgSize, ref total);
            data.ForEach(f =>
            {
                f.DeviceTypeDesc= EnumHelper.GetDescFromEnumVal<AutoProdDeviceType>(f.DeviceType);
                f.BinStatusDesc = EnumHelper.GetDescFromEnumVal<BinStatus>(f.BinStatus);
            });
            var res = new TableModel<AutoProdDeviceWarehouseDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task<AutoProdDeviceWarehouseDto> GetDeviceBinDetail(int binId)
        {
            var data = await Repository.ClientDb.Queryable<AutoProdDeviceWarehouse>()
                .InnerJoin<AutoProdDevice>((w,d)=>w.DeviceId==d.DeviceId)
                .Where((w, d) => w.BinId == binId)
                .Select<AutoProdDeviceWarehouseDto>().SingleAsync();
            data.BinStatusDesc = EnumHelper.GetDescFromEnumVal<BinStatus>(data.BinStatus);
            return data;
        }

        public async Task AddDeviceBin(AutoProdDeviceWarehouseDto data)
        {
            if (string.IsNullOrEmpty(data.BinNo))
            {
                throw new BusinessException("保存失败,仓位编号不能为空");
            }
            var exist = await Repository.Exist<AutoProdDeviceWarehouse>(x => x.BinNo == data.BinNo);
            if (exist)
            {
                throw new BusinessException("保存失败,已存在相同的系统仓位编码");
            }
            if (!string.IsNullOrEmpty(data.AGVBinCode_Delivery))
            {
                exist = await Repository.Exist<AutoProdDeviceWarehouse>(x => x.AGVBinCode_Delivery == data.AGVBinCode_Delivery);
                if (exist)
                {
                    throw new BusinessException("保存失败,已存在相同的AGV仓位编码");
                }
            }
            if (!string.IsNullOrEmpty(data.AGVBinCode_Receive))
            {
                exist = await Repository.Exist<AutoProdDeviceWarehouse>(x => x.AGVBinCode_Receive == data.AGVBinCode_Receive);
                if (exist)
                {
                    throw new BusinessException("保存失败,已存在相同的AGV仓位编码");
                }
            }
            var model = _mapper.Map<AutoProdDeviceWarehouse>(data);
            model.BinStatus = BinStatus.Free.ToString();
            await Repository.AddAsync(model);
        }

        public async Task UpdateDeviceBin(AutoProdDeviceWarehouseDto data)
        {
            if (string.IsNullOrEmpty(data.BinNo))
            {
                throw new BusinessException("保存失败,设备编号不能为空");
            } 
            var exist = await Repository.Exist<AutoProdDeviceWarehouse>(x => x.BinId != data.BinId && x.BinNo == data.BinNo);
            if (exist)
            {
                throw new BusinessException("保存失败,已存在相同的系统仓位编码");
            }
            if (!string.IsNullOrEmpty(data.AGVBinCode_Delivery))
            {
                exist = await Repository.Exist<AutoProdDeviceWarehouse>(x => x.BinId != data.BinId && x.AGVBinCode_Delivery == data.AGVBinCode_Delivery);
                if (exist)
                {
                    throw new BusinessException("保存失败,已存在相同的AGV仓位编码");
                }
            }
            if (!string.IsNullOrEmpty(data.AGVBinCode_Receive))
            {
                exist = await Repository.Exist<AutoProdDeviceWarehouse>(x => x.BinId != data.BinId && x.AGVBinCode_Receive == data.AGVBinCode_Receive);
                if (exist)
                {
                    throw new BusinessException("保存失败,已存在相同的AGV仓位编码");
                }
            }
            var oldEntity = await Repository.GetSingeAsync<AutoProdDeviceWarehouse>(data.BinId);
            if (oldEntity == null)
            {
                throw new BusinessException("保存失败,系统未查询到仓位信息或已被删除");
            } 
            oldEntity.BinNo = data.BinNo;
            oldEntity.AGVBinCode_Delivery = data.AGVBinCode_Delivery;
            oldEntity.AGVBinCode_Receive = data.AGVBinCode_Receive;
            oldEntity.Sort = data.Sort;
            oldEntity.Tier = data.Tier;
            oldEntity.Column = data.Column;
            oldEntity.Row = data.Row;
            await Repository.UpdateAsync(oldEntity);
        }

        public async Task DelDeviceBin(int[] binsId)
        {
            var exist = await Repository.ClientDb.Queryable<AutoProdDeviceWarehouse>().Where(w => binsId.Contains(w.BinId) && w.BinStatus!=BinStatus.Free.ToString()).AnyAsync();
            if (exist)
            {
                throw new BusinessException("删除失败,存在不在闲置状态的仓库，暂时无法删除");
            }
            await Repository.ClientDb.Deleteable<AutoProdDeviceWarehouse>(d => binsId.Contains(d.BinId)).ExecuteCommandAsync();
        }
    }
}
