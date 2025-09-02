using Logic.Sys; 
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
    public class FieldsPermissionController : AuthTokenController
    {
        private readonly FieldPermissionMgr _fieldPermissionMgr; 

        public FieldsPermissionController(FieldPermissionMgr fieldPermissionMgr)
        {
            _fieldPermissionMgr = fieldPermissionMgr; 
        }

        /// <summary>
        /// 获取所有表及字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<TreeModel>> GetPermissionTbFields()
        {
            return await _fieldPermissionMgr.GetPermissionTbFields(); 
        }

        /// <summary>
        /// 获取指定表字段信息
        /// </summary> 
        /// <param name="fieldsManageId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<FieldPermissions> GetPermissioField(string fieldsManageId)
        {
            return await _fieldPermissionMgr.GetPermissioField(fieldsManageId); 
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<Role>> GetRoles()
        {
            return await _fieldPermissionMgr.GetRoles(); 
        }

        /// <summary>
        /// 修改表字段权限
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改字段权限", Models.Model.Enum.LogType.Update)]
        public async Task SetPermissioFieldRole(FieldPermissions data)
        {
             await _fieldPermissionMgr.SetPermissioFieldRole(data); 
        }
    }
}
