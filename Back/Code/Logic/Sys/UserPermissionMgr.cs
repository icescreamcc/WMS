using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{

    /// <summary>
    /// 角色权限业务处理
    /// </summary>
   public class UserPermissionMgr: DbOperationHandler
    {
        private readonly BusinessCacheService _cacheService;


        private readonly BusinessCacheItem _businessCacheItem;

        public UserPermissionMgr(Repository repository, BusinessCacheService cacheService, BusinessCacheItem businessCacheItem) : base(repository)
        {
            _cacheService = cacheService;
            _businessCacheItem = businessCacheItem;
        }

        /// <summary>
        /// 所有菜单,并标识当前角色的权限菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<PermissionMenus>> GetPermissionMenus(string roleId)
        {
            var perm = new List<SysUserPermissions>();
            if (!string.IsNullOrEmpty(roleId))
            {
                perm = await Repository.ClientDb.Queryable<SysUserPermissions>().Where(p => p.RoleId == roleId).ToListAsync();
            }
            var menus = await Repository.ClientDb.Queryable<SysMenus>().Where(m => m.IsValid&&m.IsVisible).ToListAsync();
            var treeRoot = menus.Where(m => m.ParentId == "ROOT").Select(m => new PermissionMenus { MenuId=m.MenuId,MenuName=m.MenuName,MenuType=m.MenuType,Rank=m.Rank }).OrderBy(m => m.Rank).ToList();
            treeRoot.ForEach(r =>
            {
                //r.IsAuth = perm.Exists(p => p.MenuId == r.MenuId);
                r.MenuChildren = menus.Where(m => m.ParentId == r.MenuId).Select(m => new PermissionMenus { ParentId=r.MenuId, MenuId = m.MenuId, MenuName = m.MenuName, MenuType = m.MenuType, Rank = m.Rank }).OrderBy(m => m.Rank).ToList();
                if (r.MenuChildren?.Count > 0)
                {
                    r.MenuChildren.ForEach(c =>
                    {
                        c.IsAuth = perm.Exists(p => p.MenuId == c.MenuId);
                        c.MenuChildren = menus.Where(m => m.ParentId == c.MenuId).Select(m => new PermissionMenus { ParentId=c.MenuId, MenuId = m.MenuId, MenuName = m.MenuName, MenuType = m.MenuType, Rank = m.Rank }).OrderBy(m => m.Rank).ToList();
                        if (c.MenuChildren?.Count > 0)
                        {
                            c.MenuChildren.ForEach(c2 =>
                            {
                                c2.IsAuth = perm.Exists(p => p.MenuId == c2.MenuId);
                            });
                            if (c.MenuChildren.All(c2 => c2.IsAuth))
                            {
                                c.IsAuth = true;
                            } 
                        }
                    });

                }
            });
            return treeRoot;
        }

        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menusId"></param>
        /// <returns></returns>
        public async Task UpdatePermissions(string roleId, List<PermissionMenus> permissionMenus)
        {
            var modelList = new List<SysUserPermissions>();
            foreach (var menu in permissionMenus)
            {
                var selectedList = menu.MenuChildren?.Where(m => m.IsAuth)?.ToList();
                if (selectedList?.Count() > 0)
                {
                    modelList.Add(new SysUserPermissions { RoleId = roleId, MenuId = menu.MenuId });
                    if (!string.IsNullOrEmpty(menu.ParentId))
                    {
                        modelList.Add(new SysUserPermissions { RoleId = roleId, MenuId = menu.ParentId });
                    }
                    selectedList.ForEach(m =>
                    {
                        modelList.Add(new SysUserPermissions { RoleId = roleId, MenuId = m.MenuId });
                    });
                }
            }
            modelList = modelList.GroupBy(x => new { x.RoleId, x.MenuId }).Select(x => new SysUserPermissions { RoleId = x.Key.RoleId, MenuId = x.Key.MenuId }).ToList();
            Repository.ClientDb.Deleteable<SysUserPermissions>(m => m.RoleId == roleId).AddQueue();
            Repository.ClientDb.Insertable(modelList).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            _cacheService.RemoveByKeys(_businessCacheItem.UserPermissionKey);
            _cacheService.RemoveByKeys(_businessCacheItem.UserMenusKey);
        }
    }
}
