using Logic.LogicCommon;
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

namespace WebApi.Controllers.Sys
{ 
    public class ApprovalController : AuthTokenController
    { 

        private readonly ApprovalMgr _approvalMgr;

        private const string _moduleName = "审批流管理";

        public ApprovalController(ApprovalMgr approvalMgr)
        { 
            _approvalMgr = approvalMgr;
        }

        /// <summary>
        /// 获取审批项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet] 
        public async Task<List<TreeModel>> GetApprovalSubject()
        {
            return await _approvalMgr.GetApprovalSubject(); 
        }

        /// <summary>
        /// 根据审批数据类型获取审批流程
        /// </summary>
        /// <param name="approvalDataType"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<ApprovalProcessModel>> GetProcess(string approvalDataType)
        {
            return await _approvalMgr.GetProcess(approvalDataType); 
        }

        /// <summary>
        /// 获取所有角色信息
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<Role>> GetRoles()
        {
            return await _approvalMgr.GetRoles(); 
        }

        /// <summary>
        /// 获取审批模式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetApprovalModels()
        {
            return await _approvalMgr.GetApprovalModels(); 
        }

        /// <summary>
        /// 保存审批设置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改审批流", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateProcess(List<ApprovalProcessModel> data)
        {
           await _approvalMgr.UpdateProcess(data); 
        }
    }
}
