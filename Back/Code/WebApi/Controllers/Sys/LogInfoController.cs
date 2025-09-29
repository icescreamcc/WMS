using Logic.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter;
using WebApi.Response;

namespace WebApi.Controllers.Sys
{ 
    public class LogInfoController : AuthTokenController
    {
        private readonly SysLogMgr _sysLogMgr;

        private const string _moduleName = "日志管理";

        public LogInfoController(SysLogMgr sysLogMgr)
        {
            _sysLogMgr = sysLogMgr;
        }

        /// <summary>
        /// 分页查询操作日志
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看系统操作日志", Models.Model.Enum.LogType.Read, _moduleName)]
        public async Task<TableModel<BusinessLog>> GetLogs(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd)
        {
           return await _sysLogMgr.GetLogs(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), searchKey, dateStart, dateEnd);
        }
    }
}
