using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using External.Common;
using External.Common.Extension;
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks; 

namespace Logic.Sys
{
    /// <summary>
    /// 用户增删改查等业务操作类
    /// </summary>
   public class UserMgr : DataPermissionHandler
    {  
        private readonly SysArgsService _sysArgsHelper;

        private readonly IFileStorage _fileStorage;

        private readonly BusinessCacheService _cacheService;

        private readonly BusinessCacheItem _businessCacheItem;

        public UserMgr(Repository repository, SysArgsService sysArgsHelper, IFileStorage fileStorage, BusinessCacheService cacheService, BusinessCacheItem businessCacheItem) :base( repository)
        { 
            _sysArgsHelper = sysArgsHelper;
            _fileStorage = fileStorage;
            _cacheService = cacheService;  
            _businessCacheItem = businessCacheItem;
        }
         
        /// <summary>
        /// 查询用户详细信息,包括角色,权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Menu>> GetUserMenus(string userId)
        { 
            var adminAccount = await _sysArgsHelper.GetDeveloper();
            var data = _cacheService.GetList<Menu>(_businessCacheItem.UserMenusKey, userId);
            if (data==null)
            {
                if (userId == adminAccount.Value.ToString())
                {
                    data = await Repository.ClientDb.Queryable<SysMenus>()
                                    .Where(m => m.IsValid)
                                    .Select(x => new Menu { MenuId = x.MenuId, MenuName = x.MenuName, MenuType = x.MenuType, ParentId = x.ParentId, Icon = x.Icon, Rank = x.Rank, RoutePath = x.RoutePath, ComponentPath = x.ComponentPath,HideMenu=x.HideMenu,MenuDisplay=x.MenuDisplay,MenuLayout=x.MenuLayout,Url=x.Url })
                                    .OrderBy(x => x.Rank)
                                    .ToListAsync();
                }
                else
                {
                    //var d1 = Repository.ClientDb.Queryable<SysUser>()
                    //              .LeftJoin<SysUserRoles>((u, ur) => u.UserId == ur.UserId)
                    //              .LeftJoin<SysUserPermissions>((u, ur, p) => ur.RoleId == p.RoleId)
                    //              .LeftJoin<SysMenus>((u, ur, p, m) => p.MenuId == m.MenuId)
                    //              .Where((u, ur, p, m) => u.UserId == userId && m.IsValid && m.IsVisible)
                    //              .Select((u, ur, p, m) => new Menu { MenuId = m.MenuId, MenuName = m.MenuName, MenuType = m.MenuType, ParentId = m.ParentId, Icon = m.Icon, Rank = m.Rank, RoutePath = m.RoutePath, ComponentPath = m.ComponentPath, HideMenu = m.HideMenu, MenuDisplay = m.MenuDisplay, MenuLayout = m.MenuLayout, Url = m.Url });
                    //var d2 = Repository.ClientDb.Queryable<SysMenus>()
                    //         .Where(w => (w.MenuDisplay == MenuDisplay.DASHBOARD.ToString()|| w.MenuDisplay == MenuDisplay.OTHER.ToString()) && w.IsValid && w.IsVisible)
                    //         .Select(m => new Menu { MenuId = m.MenuId, MenuName = m.MenuName, MenuType = m.MenuType, ParentId = m.ParentId, Icon = m.Icon, Rank = m.Rank, RoutePath = m.RoutePath, ComponentPath = m.ComponentPath, HideMenu = m.HideMenu, MenuDisplay = m.MenuDisplay, MenuLayout = m.MenuLayout, Url = m.Url });
                    //data = await Repository.ClientDb.UnionAll(d1, d2).OrderBy(o => o.Rank).Distinct().ToListAsync();

                    string da = MenuDisplay.DASHBOARD.ToString();
                    string or = MenuDisplay.OTHER.ToString();
                    var d1 = Repository.ClientDb.Queryable<SysUser>()
                               .LeftJoin<SysUserRoles>((u, ur) => u.UserId == ur.UserId)
                               .LeftJoin<SysUserPermissions>((u, ur, p) => ur.RoleId == p.RoleId)
                               .LeftJoin<SysMenus>((u, ur, p, m) => p.MenuId == m.MenuId)
                               .Where((u, ur, p, m) => u.UserId == userId && m.IsValid && m.IsVisible)
                               .Select((u, ur, p, m) => new Menu { MenuId = m.MenuId, MenuName = m.MenuName, MenuType = m.MenuType, ParentId = m.ParentId, Icon = m.Icon, Rank = m.Rank, RoutePath = m.RoutePath, ComponentPath = m.ComponentPath, HideMenu = m.HideMenu, MenuDisplay = m.MenuDisplay, MenuLayout = m.MenuLayout, Url = m.Url });
                    var d2 = Repository.ClientDb.Queryable<SysMenus>()
                             .Where(w => (w.MenuDisplay == da || w.MenuDisplay == or) && w.IsValid && w.IsVisible)
                             .Select(m => new Menu { MenuId = m.MenuId, MenuName = m.MenuName, MenuType = m.MenuType, ParentId = m.ParentId, Icon = m.Icon, Rank = m.Rank, RoutePath = m.RoutePath, ComponentPath = m.ComponentPath, HideMenu = m.HideMenu, MenuDisplay = m.MenuDisplay, MenuLayout = m.MenuLayout, Url = m.Url });
                    data = await Repository.ClientDb.UnionAll(d1, d2).OrderBy(o => o.Rank).Distinct().ToListAsync();

                } 
                _cacheService.SetListAndKey(_businessCacheItem.UserMenusKey, userId, data);
            }
            return data;
        }

        /// <summary>
        /// 获取用户的菜权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Menu>> GetPermissionMenus(string userId)
        {
            var data = _cacheService.GetList<Menu>(_businessCacheItem.UserPermissionKey, userId);
            if (data == null)
            {
                data = await Repository.ClientDb.Queryable<SysUserRoles, SysUserPermissions, SysMenus>((ur, up, m) => ur.RoleId == up.RoleId && up.MenuId == m.MenuId)
                          .Where((ur, up, m) => ur.UserId == userId)
                          .Select((ur, up, m) => new Menu { MenuId = m.MenuId, CtrlName = m.CtrlName, ActionName = m.ActionName })
                          .Distinct()
                          .ToListAsync();
                _cacheService.SetListAndKey(_businessCacheItem.UserPermissionKey, userId, data);
            } 
            return data;
        }


        /// <summary>
        /// 查询用户明细
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDetail> GetUserDetail(string userId)
        { 
            var res = await Repository.ClientDb.Queryable<SysUser>()
             .LeftJoin<SysDepartments>((u, d) => u.DeptId == d.DeptId)
             .LeftJoin<SysProvince>((u, d, p) => u.ProvinceName == p.ProvinceName)
             .LeftJoin<SysCity>((u, d, p, c) => u.CityName == c.CityName)
             .Select((u, d,p,c) => new UserDetail
             {
                 UserId = u.UserId,
                 UserName = u.UserName,
                 NickName = u.NickName,
                 Address = u.Address,
                 AuthAccount = u.AuthAccount, 
                 CardId=u.CardId,
                 DomainName=u.DomainName,
                 UserCode=u.UserCode,
                 CityId =c.CityId,
                 CityName=u.CityName,
                 CreateDate = u.CreateDate,
                 DepartureDate = u.DepartureDate,
                 DeptId = d.DeptId,
                 DeptName=d.DeptName,
                 Email = u.Email,
                 IsVaild = u.IsVaild,
                 MobilePhone = u.MobilePhone,
                 Phone = u.Phone, 
                 ProvinceId=p.ProvinceId,
                 ProvinceName=u.ProvinceName,
                 Remark = u.Remark,
                 Wechat = u.Wechat,
                 EditDate=u.EditDate
             }).MergeTable().SingleAsync(x => x.UserId == userId); 
            return res;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserRole>> GetUserRoles(string userId)
        { 
            var res = await Repository.ClientDb.Queryable<SysRoles>().Select(r => new UserRole
            {
                UserId=userId,
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                IsSelected = SqlFunc.Subqueryable<SysUserRoles>().Where(ur => ur.UserId == userId && ur.RoleId == r.RoleId).Any()
            }).ToListAsync();
            return res;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles()
        {
            return await Repository.ClientDb.Queryable<SysRoles>().Select(r=>new Role { RoleId=r.RoleId,RoleName=r.RoleName}).ToListAsync();
        }

        /// <summary>
        /// 用户列表分页查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<UserDetail>> GetUsers(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            RefAsync<int> total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "UserId" : orderFiled;
            orderFiled = orderFiled.ToLower() == "isvaild" ? "u.isvaild" : orderFiled; 
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey.Trim();
            var res = await Repository.ClientDb.Queryable<SysUser>()
                .LeftJoin<SysDepartments>((u, d) => u.DeptId == d.DeptId)
                .Where((u, d) => u.UserName.Contains(searchKey) || u.AuthAccount.Contains(searchKey) || u.Email.Contains(searchKey) || u.UserCode.Contains(searchKey) || u.MobilePhone.Contains(searchKey) || u.NickName.Contains(searchKey) || u.DomainName.Contains(searchKey) || d.DeptName.Contains(searchKey))
                .Select((u, d) => new UserDetail
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    NickName = u.NickName,
                    Address = u.Address,
                    AuthAccount = u.AuthAccount,
                    CardId = u.CardId,
                    DomainName = u.DomainName,
                    UserCode = u.UserCode,
                    CityName =u.CityName,
                    CreateDate = u.CreateDate,  
                    EditDate = u.EditDate,  
                    DepartureDate = u.DepartureDate,
                    DeptId=d.DeptId,
                    DeptName=d.DeptName,
                    Email = u.Email,
                    IsVaild = u.IsVaild,
                    MobilePhone = u.MobilePhone,
                    Phone = u.Phone,  
                    ProvinceName=u.ProvinceName,
                    Remark = u.Remark,
                    Wechat = u.Wechat
                })
                .OrderBy($"{orderFiled} {orderType}")
                .ToPageListAsync(pgIndex, pgSize, total);
            await SetDeniedFieldValue<SysUser, UserDetail>(userId, res); 
            return new TableModel<UserDetail> (total,res);
        }

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUser(UserDetail user)
        {
            if (string.IsNullOrEmpty(user.UserCode) || string.IsNullOrEmpty(user.AuthAccount) || string.IsNullOrEmpty(user.DomainName))
            {
                throw new BusinessException("保存失败,工号、UI编号、域用户名不能为空");
            }
            if (string.IsNullOrEmpty(user.DeptId))
            {
                throw new BusinessException("保存失败,请指定用户所属部门");
            }
            var existModel = await Repository.ClientDb.Queryable<SysUser>().AnyAsync(u => u.UserCode == user.UserCode);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前工号已存在");
            }
            existModel = await Repository.ClientDb.Queryable<SysUser>().AnyAsync(u => u.AuthAccount == user.AuthAccount);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前ui编号已存在");
            }
            existModel = await Repository.ClientDb.Queryable<SysUser>().AnyAsync(u => u.DomainName == user.DomainName);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前域用户名已存在");
            }
            var model = new SysUser
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = EncryptionHelper.MD5Encrypt((await _sysArgsHelper.GetValueByKey(BusinessConst.InitialPassword)).Value.ToString()),
                Address = user.Address,
                AuthAccount = user.UserId,
                CardId = user.CardId,
                UserCode = user.UserCode,
                DomainName = user.DomainName,
                CityName = user.CityName,
                CreateDate = DateTime.Now.ToStringExtension(), 
                DeptId = user.DeptId, 
                Email = user.Email,
                IsVaild = true,
                MobilePhone = user.MobilePhone,
                NickName = user.NickName,
                Phone = user.Phone,
                ProvinceName = user.ProvinceName,
                Remark = user.Remark,
                Wechat = user.Wechat,
                DepartureDate= user.DepartureDate
            };
            await Repository.AddAsync(model); 
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUser(UserDetail user)
        {
            var existModel = await  Repository.GetSingeAsync<SysUser>(user.UserId);
            if (existModel == null)
            {
                throw new BusinessException("保存失败,当前用户不存在或已被删除"); 
            }
            if (string.IsNullOrEmpty(user.UserCode) || string.IsNullOrEmpty(user.AuthAccount) || string.IsNullOrEmpty(user.DomainName))
            {
                throw new BusinessException("保存失败,工号、ui编号、域用户名不能为空");
            }
            if (string.IsNullOrEmpty(user.DeptId))
            {
                throw new BusinessException("保存失败,请指定用户所属部门");
            }
            var exist = await Repository.ClientDb.Queryable<SysUser>().AnyAsync(u => u.UserCode == user.UserCode && u.UserId != user.UserId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前工号已存在");
            }
            exist = await Repository.ClientDb.Queryable<SysUser>().AnyAsync(u => u.AuthAccount == user.AuthAccount && u.UserId != user.UserId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前ui编号已存在");
            }
            exist = await Repository.ClientDb.Queryable<SysUser>().AnyAsync(u => u.DomainName == user.DomainName && u.UserId != user.UserId);
            if (exist)
            {
                throw new BusinessException("保存失败,当前域用户名已存在");
            }
            existModel.UserName = user.UserName;
            existModel.Address = user.Address;
            existModel.CityName = user.CityName;
            existModel.EditDate = DateTime.Now.ToStringExtension();
            existModel.DepartureDate = DateTimeHelper.ConvertToString(user.DepartureDate);
            existModel.DeptId = user.DeptId;
            existModel.CardId = user.CardId;
            existModel.DomainName = user.DomainName;
            existModel.UserCode = user.UserCode;
            existModel.Email = user.Email;
            existModel.MobilePhone = user.MobilePhone;
            existModel.NickName = user.NickName;
            existModel.Phone = user.Phone;
            existModel.ProvinceName = user.ProvinceName;
            existModel.Remark = user.Remark;
            existModel.Wechat = user.Wechat;
            await Repository.UpdateAsync(existModel); 
        }

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task  UpdateUserRole(string userId,string [] roleId)
        {
            var userRoles = new List<SysUserRoles>();
            foreach(string rid in roleId)
            {
                userRoles.Add(new SysUserRoles { RoleId = rid, UserId = userId });
            }
            if (userId == BusinessConst.UserAdmin && !userRoles.Exists(x=>x.RoleId== BusinessConst.RoleAdmin))
            {
                throw new BusinessException("保存失败,超级管理员用户需要管理员角色"); 
            }
            Repository.ClientDb.Deleteable<SysUserRoles>(x => x.UserId == userId).AddQueue();
            Repository.ClientDb.Insertable(userRoles).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            _cacheService.RemoveByKeys(_businessCacheItem.UserPermissionKey);
            _cacheService.RemoveByKeys(_businessCacheItem.UserMenusKey);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUserPassword(UserPaasswordEditModel passwordInfo)
        {
            var existModel = await Repository.GetSingeAsync<SysUser>(passwordInfo.UserId);
            if (existModel == null)
            {
                throw new BusinessException("保存失败,当前用户信息不存在或已被删除"); 
            }
            passwordInfo.NewPassword = EncryptionHelper.MD5Encrypt(passwordInfo.NewPassword);
            passwordInfo.OldPassword = EncryptionHelper.MD5Encrypt(passwordInfo.OldPassword);
            if (passwordInfo.OldPassword != existModel.Password)
            {
                throw new BusinessException("保存失败,您输入的原始密码有误,请重新输入"); 
            } 
            existModel.Password = passwordInfo.NewPassword;
            existModel.EditDate = DateTime.Now.ToStringExtension();
            await Repository.UpdateAsync(existModel); 
        }

        /// <summary>
        /// 初始化用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task InitUserPassword(string userId)
        {
            var initPwd= (await _sysArgsHelper.GetValueByKey(BusinessConst.InitialPassword)).Value.ToString();
            var existModel = await Repository.GetSingeAsync<SysUser>(userId);
            if (existModel == null)
            {
                throw new BusinessException("密码初始化失败,当前用户信息不存在或已被删除"); 
            } 
            existModel.Password = EncryptionHelper.MD5Encrypt(initPwd);
            existModel.EditDate = DateTime.Now.ToStringExtension();
            await Repository.UpdateAsync(existModel); 
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isVaild"></param>
        /// <returns></returns>
        public async Task UpdateUserStatus(string userId,bool isVaild)
        {
            var existModel = await  Repository.GetSingeAsync<SysUser>(userId);
            if (existModel == null)
            {
                throw new BusinessException("用户状态修改失败,当前用户信息不存在或已被删除"); 
            }
            if (userId == BusinessConst.UserAdmin)
            {
                throw new BusinessException("用户状态修改失败,超级管理员用户不允许被冻结"); 
            }
            existModel.IsVaild = isVaild;
            await Repository.UpdateAsync(existModel); 
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public async Task DelUser(string [] usersId)
        {
            foreach (var userId in usersId)
            {
                if (userId == BusinessConst.UserAdmin)
                {
                    throw new BusinessException("删除失败,超级管理员用户不允许被删除"); 
                }
            }
            Repository.ClientDb.Deleteable<SysUser>(u=> usersId.Contains(u.UserId)).AddQueue();
            Repository.ClientDb.Deleteable<SysUserRoles>(u => usersId.Contains(u.UserId)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }


        /// <summary>
        /// 文件导出
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<string> ExportUsers( string searchKey, string orderField, string orderType, List<KeyValueModel> fields)
        {
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey; 
            var typeList =new List<Type> { typeof(SysUser), typeof(SysDepartments) };
            StringBuilder sb = new StringBuilder();
            fields.ForEach(f =>
            {
                if(f.Key.ToString()== "DeptId")
                {
                    f.Key = "DeptName";
                }
                string fieldStr = SqlJoint.GetSelectFieldFormat(f, typeList); 
                sb.Append(fieldStr); 
            });
            string selField = sb.ToString().TrimEnd(',');
            typeList.ToList().Reverse();
            string orderbyStr = SqlJoint.GetOrderFieldFormat(orderField, orderType, typeList); 
            var param = new Dictionary<string, object>();
            param.Add("UserName", searchKey);
            param.Add("ProvinceName", searchKey);
            param.Add("CityName", searchKey);
            param.Add("MobilePhone", searchKey);
            param.Add("Address", searchKey);
            param.Add("DeptName", searchKey); 
            string sql = $@"select {selField} from SysUser 
                            left join SysDepartments  on SysDepartments.DeptId=SysUser.DeptId 
                            where [UserName] like '%'+@UserName+'%' or SysUser.ProvinceName like '%'+@ProvinceName+'%' 
                            or SysUser.CityName like '%'+@CityName+'%' or SysUser.MobilePhone like '%'+@MobilePhone+'%' or SysUser.[Address] like '%'+@Address+'%'
                            or SysDepartments.DeptName like '%'+@DeptName+'%'
                            {orderbyStr}"; 
            var queryData = await Repository.QueryBySqlAsync(sql, param);
            var stream= ExcelHelper.ConvertDataTableToStream(queryData);
            var fileName = $"用户信息导出{DateTime.Now.ToStringNoSignExtension()}.xlsx"; 
            var savePath= @$"{Directory.GetCurrentDirectory()}\Files\Export\{fileName}";
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Excel);  
            return fileUrl;
        } 
         
    }
}
