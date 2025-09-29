 using Microsoft.AspNetCore.Mvc; 
using Models.Model;
using System.Threading.Tasks;
using WebApi.Filter;
using Models.Model.Enum;
using External.Common;
using Logic.AutomationDevice;
using Models.Model.AutomationDevice;

namespace WebApi.Controllers.AutomationDevice
{ 
    public class AGVSettingController : AuthTokenController
    {
        private readonly AGVSettingMgr _agvSettingMgr;

        private const string _moduleName = "AGV配置";

        public AGVSettingController(AGVSettingMgr aGVSettingMgr)
        {
            _agvSettingMgr = aGVSettingMgr;
        }

        [HttpGet]
        [BusinessLog("查看AGV配置列表", LogType.Read, _moduleName)]
        public async Task<TableModel<AutoProdDeviceAGVDto>> GetAGVInfo(int pgSize, int pgIndex, string orderFiled, string orderType, string agvType, string searchKey)
        {
            return await _agvSettingMgr.GetAGVInfo(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), agvType, searchKey);
        }

        [HttpGet]
        [Skip]
        public async Task<AutoProdDeviceAGVDto> GetAGVInfoDetail(int agvId)
        {
            return await _agvSettingMgr.GetAGVInfoDetail(agvId);
        }

        [HttpGet]
        [Skip]
        public object GetArgs()
        { 
            var agvTypeOptions = EnumHelper.GetEnumValNames<AGVType>();
            var agvStatusOptions = EnumHelper.GetEnumValNames<AGVStatus>();
            return new { agvTypeOptions, agvStatusOptions };
        }

        [HttpPost]
        [BusinessLog("添加AGV配置", LogType.Add, _moduleName)]
        public async Task AddAGVInfo(AutoProdDeviceAGVDto data)
        {
             await _agvSettingMgr.AddAGVInfo(data);
        }

        [HttpPost]
        [BusinessLog("修改AGV配置", LogType.Update, _moduleName)]
        public async Task UpdateAGVInfo(AutoProdDeviceAGVDto data)
        {
            await _agvSettingMgr.UpdateAGVInfo(data);
        }

        [HttpGet]
        [BusinessLog("删除AGV配置", LogType.Del, _moduleName)]
        public async Task DelAGVInfo(int agvId)
        {
            await _agvSettingMgr.DelAGVInfo(agvId);
        }
    }
}
