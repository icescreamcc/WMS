using DbRepository.Repository.DbModels;
using Logic.LogicCommon;
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
    public class UserController : AuthTokenController
    {
        private readonly UserMgr _userMgr;

        private readonly SelectOptionsService _selectOptionsServer;

        private const string _moduleName = "用户管理";

        public UserController(UserMgr userMgr, SelectOptionsService selectOptionsServer)
        {
            _userMgr = userMgr;
            _selectOptionsServer = selectOptionsServer; 
        }

        /// <summary>
        /// 获取用户详细信息,包括角色,权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<Menu>> GetUserMenus(string userId)
        { 
            return await _userMgr.GetUserMenus(userId); 
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<UserDetail> GetUserDetail(string userId)
        {
            return await _userMgr.GetUserDetail(userId);  
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<UserRole>> GetUserRoles(string userId)
        {
            return await _userMgr.GetUserRoles(userId); 
        }

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看用户信息", Models.Model.Enum.LogType.Read, _moduleName)]
        public async Task<TableModel<UserDetail>> GetUsers(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey,string userId)
        {
            return await _userMgr.GetUsers(userId, pgSize, pgIndex, orderFiled, ConvertOrderType(orderType), searchKey); 
        }

        /// <summary>
        /// 获取用户相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetAboutUserOptions()
        {
            var deptOptions = await _selectOptionsServer.GetDeptOptions();
            var provinceOptions = await _selectOptionsServer.GetProvinces();
            return new { DeptOptions =  deptOptions, ProvinceOptions =  provinceOptions }; 
        }
         
        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetCitysByProvince(int provinceId)
        {
            return await _selectOptionsServer.GetCitysByProvince(provinceId); 
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加用户", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task AddUser(UserDetail user)
        {
            await _userMgr.AddUser(user); 
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改用户信息", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateUser(UserDetail user)
        {
            await _userMgr.UpdateUser(user); 
        }

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("修改用户角色", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateUserRole(string userId, string[] roleId)
        {
             await _userMgr.UpdateUserRole(userId, roleId); 
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="passwordInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Skip]
        [BusinessLog("修改用户密码", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateUserPassword(UserPaasswordEditModel passwordInfo)
        {
            await _userMgr.UpdateUserPassword(passwordInfo); 
        }

        /// <summary>
        /// 初始化用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("初始化用户密码", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task InitUserPassword(string userId)
        {
             await _userMgr.InitUserPassword(userId); 
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isVaild"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("修改用户状态", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateUserStatus(string userId,bool isVaild)
        {
            await _userMgr.UpdateUserStatus(userId, isVaild); 
        }


        /// <summary>
        /// 删除用户(允许批量删除)
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除用户", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelUser(string [] userIds)
        {
             await _userMgr.DelUser(userIds); 
        }

        /// <summary>
        /// 导出用户
        /// </summary> 
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出用户", Models.Model.Enum.LogType.Export, _moduleName)]
        public async Task<string> ExportUsers( string searchKey, string orderField, string orderType, List<KeyValueModel> fields)
        {
            var fileUrl = await _userMgr.ExportUsers( searchKey, orderField, ConvertOrderType(orderType), fields); 
            return fileUrl;
        }

        /// <summary>
        /// 获取当前用户导出用户列表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldPermission>> GetAllowField(string userId)
        {
            return await _userMgr.GetAllowField<SysUser>(userId); 
        }
    }
}
