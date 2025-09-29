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
    public class UserPermissionController : AuthTokenController
    {
        private readonly UserPermissionMgr _userPermissionMgr; 

        private readonly OrganizationMgr _organizetionMgr;

        private const string _moduleName = "用户权限管理";

        public UserPermissionController(UserPermissionMgr userPermissionMgr, OrganizationMgr organizetionMgr)
        {
            _userPermissionMgr = userPermissionMgr;
            _organizetionMgr = organizetionMgr; 
        }

        /// <summary>
        /// 查询公司组织架构数据并组建成树形结构模型
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        public async Task<TreeModel> GetOrganizationData()
        {
            return await _organizetionMgr.GetOrganizationData(); 
        }

        /// <summary>
        /// 所有菜单,并标识当前角色的权限菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<PermissionMenus>> GetPermissionMenus(string roleId)
        {
            return await _userPermissionMgr.GetPermissionMenus(roleId); 
        }

        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionMenus"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("修改角色权限", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdatePermissions(string roleId, List<PermissionMenus> permissionMenus)
        {
            await _userPermissionMgr.UpdatePermissions(roleId, permissionMenus); 
        }
    }
}
