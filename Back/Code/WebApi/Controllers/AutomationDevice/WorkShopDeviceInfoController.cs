using Microsoft.AspNetCore.Mvc; 
using Models.Model;
using System.Threading.Tasks;
using Models.Model.Enum;
using WebApi.Filter;
using External.Common;
using Logic.LogicCommon;
using System.Collections.Generic;
using Logic.AutomationDevice;
using Models.Model.AutomationDevice;

namespace WebApi.Controllers.AutomationDevice
{ 
    public class WorkShopDeviceInfoController : AuthTokenController
    {
        private readonly WorkShopDeviceInfoMgr _workShopDeviceInfoMgr;

        private readonly SelectOptionsService _selectOptionsService;

        private const string _moduleName = "车间设备基础数据";

        public WorkShopDeviceInfoController(WorkShopDeviceInfoMgr workShopDeviceInfoMgr, SelectOptionsService selectOptionsService)
        {
            _workShopDeviceInfoMgr = workShopDeviceInfoMgr;
            _selectOptionsService = selectOptionsService;
        }

        [HttpGet]
        [BusinessLog("查看车间设备列表", LogType.Read, _moduleName)]
        public async Task<TableModel<AutoProdDeviceDto>> GetDeviceInfo(int pgSize, int pgIndex, string orderFiled, string orderType, string warehouseId, string searchKey)
        {
            return await _workShopDeviceInfoMgr.GetDeviceInfo(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), warehouseId, searchKey);
        }

        [HttpGet]
        [Skip]
        public async Task<AutoProdDeviceDto> GetDeviceInfoDetail(int deviceId)
        {
            return await _workShopDeviceInfoMgr.GetDeviceInfoDetail(deviceId);
        }

        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetDeviceInfoOptions(string deviceType)
        {
            return await _workShopDeviceInfoMgr.GetDeviceInfoOptions(deviceType);
        }

        [HttpGet]
        [Skip]
        public async Task<object> GetArgs()
        {
            var warehouseOptions = await _selectOptionsService.GetWarehouses(); 
            var deviceTypeOptions = EnumHelper.GetEnumValNames<AutoProdDeviceType>();
            return new { warehouseOptions, deviceTypeOptions };
        }

        [HttpPost]
        [BusinessLog("添加设备信息", LogType.Add, _moduleName)]
        public async Task AddDeviceInfo(AutoProdDeviceDto data)
        {
            await _workShopDeviceInfoMgr.AddDeviceInfo(data);
        }

        [HttpPost]
        [BusinessLog("修改设备信息", LogType.Update, _moduleName)]
        public async Task UpdateDeviceInfo(AutoProdDeviceDto data)
        {
            await _workShopDeviceInfoMgr.UpdateDeviceInfo(data);
        }

        [HttpPost]
        [BusinessLog("删除设备信息", LogType.Del, _moduleName)]
        public async Task DelDeviceInfo(int[] devicesId)
        {
            await _workShopDeviceInfoMgr.DelDeviceInfo(devicesId);
        }

        [HttpGet]
        [Skip]
        public async Task<TableModel<AutoProdDeviceWarehouseDto>> GetDeviceBin(int pgSize, int pgIndex, string orderFiled, string orderType, string deviceType, string searchKey)
        {
            return await _workShopDeviceInfoMgr.GetDeviceBin(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), deviceType, searchKey);
        }

        [HttpGet]
        [Skip]
        public async Task<AutoProdDeviceWarehouseDto> GetDeviceBinDetail(int binId)
        {
            return await _workShopDeviceInfoMgr.GetDeviceBinDetail(binId);
        }

        [HttpPost]
        [BusinessLog("添加设备仓位信息", LogType.Add, _moduleName)]
        public async Task AddDeviceBin(AutoProdDeviceWarehouseDto data)
        {
            await _workShopDeviceInfoMgr.AddDeviceBin(data);
        }

        [HttpPost]
        [BusinessLog("修改设备仓位信息", LogType.Update, _moduleName)]
        public async Task UpdateDeviceBin(AutoProdDeviceWarehouseDto data)
        {
            await _workShopDeviceInfoMgr.UpdateDeviceBin(data);
        }

        [HttpPost]
        [BusinessLog("删除设备仓位信息", LogType.Del, _moduleName)]
        public async Task DelDeviceBin(int[] binsId)
        {
            await _workShopDeviceInfoMgr.DelDeviceBin(binsId);
        }
    }
}
