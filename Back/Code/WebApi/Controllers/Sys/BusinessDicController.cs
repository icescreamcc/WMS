using Logic.Sys; 
using Microsoft.AspNetCore.Mvc;
using Models.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Sys
{ 
    public class BusinessDicController : AuthTokenController
    {
        private readonly BusinessDicMgr _businessDicMgr;

        private const string _moduleName = "字典管理";

        public BusinessDicController(BusinessDicMgr businessDicMgr)
        {
            _businessDicMgr = businessDicMgr; 
        }

        /// <summary>
        /// 获取所有字典列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看字典信息", Models.Model.Enum.LogType.Read, _moduleName)]
        public async Task<List<Args>> GetArgs(string searchKey)
        {
            return await _businessDicMgr.GetArgs(searchKey); 
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加字典", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task AddArgs(Args data)
        {
            await _businessDicMgr.AddArgs(data); 
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改字典", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateArgs(Args data)
        {
             await _businessDicMgr.UpdateArgs(data); 
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="argsKey"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除字典", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelArgs(string argsKey)
        {
            await _businessDicMgr.DelArgs(argsKey); 
        }
    }
}
