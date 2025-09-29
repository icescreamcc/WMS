using Logic.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Sys;
using Models.Model;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Sys
{
    public class ExternalProviderController : AuthTokenController
    {
        private readonly ExternalProviderMgr _externalProviderMgr;

        private const string _moduleName = "第三方账号管理";

        public ExternalProviderController(ExternalProviderMgr externalProviderMgr)
        {
            _externalProviderMgr = externalProviderMgr;
        }

        [HttpGet]
        [BusinessLog("查看第三方账号信息", Models.Model.Enum.LogType.Read, _moduleName)]
        public async Task<TableModel<ExternalProviderDto>> GetProviders(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _externalProviderMgr.GetProviders(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey);
        }

        [HttpPost]
        [BusinessLog("添加第三方账号", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task AddProvider(ExternalProviderDto input)
        {
            await _externalProviderMgr.AddProvider(input);
        }

        [HttpPost]
        [BusinessLog("修改第三方账号", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateProvider(ExternalProviderDto input)
        {
            await _externalProviderMgr.UpdateProvider(input);
        }

        [HttpPost]
        [BusinessLog("删除第三方账号", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelProvider(string[] providerName)
        {
            await _externalProviderMgr.DelProvider(providerName);
        }
    }
}
