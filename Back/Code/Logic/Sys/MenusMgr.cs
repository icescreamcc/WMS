using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Logic.Sys
{
    /// <summary>
    /// 菜单管理
    /// </summary>
   public class MenusMgr: DbOperationHandler
    {
        private readonly BusinessCacheService _cacheService;

        private readonly BusinessCacheItem _businessCacheItem;
        public MenusMgr(Repository repository, BusinessCacheService cacheService, BusinessCacheItem businessCacheItem) : base(repository)
        {
            _cacheService = cacheService; 
            _businessCacheItem = businessCacheItem;
        }

        /// <summary>
        /// 获取菜单树形结构
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetMenusData()
        { 
            var menus = await Repository.ClientDb.Queryable<SysMenus>().Where(m=>m.IsValid).ToListAsync();
            var treeRoot = menus.Where(m => m.ParentId == "ROOT").Select(m => new TreeModel {Id=m.MenuId,Label=m.MenuName,Type=m.MenuType ,Remark=m.Icon,Rank=m.Rank,ParentId="ROOT"}).OrderBy(m=>m.Rank).ToList();
            treeRoot.ForEach(r =>
            {
                r.Children = menus.Where(m => m.ParentId == r.Id.ToString()).Select(m => new TreeModel { Id = m.MenuId, Label = m.MenuName, Type = m.MenuType, Remark = m.Icon, Rank = m.Rank,ParentId=m.ParentId }).OrderBy(m => m.Rank).ToList();
                if (r.Children.Count>0)
                {
                    r.Children.ForEach(c =>
                    {
                        c.Children=menus.Where(m=>m.ParentId==c.Id.ToString()).Select(m => new TreeModel { Id = m.MenuId, Label = m.MenuName, Type = m.MenuType, Remark = m.Icon, Rank = m.Rank ,ParentId = m.ParentId }).OrderBy(m => m.Rank).ToList();
                    });
                }
            });
            return treeRoot;
        }

        /// <summary>
        /// 获取菜单详细信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<Menu> GetMenuInfo(string menuId)
        {
            var res= await Repository.ClientDb.Queryable<SysMenus>().Where(m => m.MenuId == menuId)
                .Select(m => new Menu
                {
                    MenuId = menuId,
                    MenuName = m.MenuName,
                    CtrlName = m.CtrlName,
                    ActionName = m.ActionName,
                    ComponentPath = m.ComponentPath,
                    RoutePath = m.RoutePath,
                    Icon = m.Icon, 
                    ParentId = m.ParentId,
                    Rank = m.Rank,
                    Remark = m.Remark,
                    IsValid = m.IsValid,
                    IsVisible = m.IsVisible,
                    MenuType = m.MenuType
                })
                .SingleAsync();
            if (res.ParentId == "ROOT")
            {
                res.ParentName = "ROOT";
                res.ParentType = "ROOT";
            }
            else
            {
                var parent = await Repository.ClientDb.Queryable<SysMenus>().Where(m => m.MenuId == res.ParentId).SingleAsync();
                res.ParentName = parent.MenuName;
                res.ParentType = parent.MenuType;
            }
            return res;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddMenu(Menu data)
        {
            var existModel = await Repository.Exist<SysMenus>(d => d.MenuId == data.MenuId);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前菜单ID已存在");
            }
            var model = new SysMenus
            {
                MenuId = data.MenuId,
                MenuName = data.MenuName,
                CtrlName = data.CtrlName,
                ActionName = data.ActionName,
                ComponentPath = data.ComponentPath,
                RoutePath = data.RoutePath,
                Icon = data.Icon,
                ParentId = data.ParentId,
                Remark = data.Remark,
                IsVisible = data.IsVisible,
                MenuType = data.MenuType,
                IsValid = true,
                Rank = data.Rank
            };
            await Repository.AddAsync(model); 
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateMenu(Menu data)
        {
            var existModel = await Repository.Exist<SysMenus>(d => d.MenuId == data.MenuId);
            if (!existModel)
            {
                throw new BusinessException("保存失败,当前菜单ID不存在");
            }
            var model = new SysMenus
            {
                MenuId = data.MenuId,
                MenuName = data.MenuName,
                CtrlName = data.CtrlName,
                ActionName = data.ActionName,
                ComponentPath = data.ComponentPath,
                RoutePath = data.RoutePath,
                Icon = data.Icon,
                ParentId = data.ParentId,
                Remark = data.Remark,
                IsVisible = data.IsVisible,
                MenuType = data.MenuType,
                IsValid = true,
                Rank = data.Rank
            };
            await Repository.UpdateAsync(model);
            _cacheService.RemoveByKeys(_businessCacheItem.UserPermissionKey);
            _cacheService.RemoveByKeys(_businessCacheItem.UserMenusKey);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="parentId"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        public async Task DelMenu(string menuId,string parentId,string menuType)
        {
            var delMenus = new List<string>() { menuId };
            if (menuType == MenuType.Menu.ToString())
            {
                var menuChildren = await Repository.ClientDb.Queryable<SysMenus>().Where(m => m.ParentId == menuId).Select(m => m.MenuId).ToListAsync();
                delMenus.AddRange(menuChildren);
                if (parentId == "ROOT")
                {
                    var menuGrandChildren = await Repository.ClientDb.Queryable<SysMenus>().Where(m => menuChildren.Contains(m.ParentId)).Select(m => m.MenuId).ToListAsync();
                    delMenus.AddRange(menuGrandChildren);
                }
            }
            Repository.ClientDb.Deleteable<SysMenus>(m => delMenus.Contains(m.MenuId)).AddQueue();
            Repository.ClientDb.Deleteable<SysUserPermissions>(p => delMenus.Contains(p.MenuId)).AddQueue();
            Repository.ClientDb.SaveQueues();
            _cacheService.RemoveByKeys(_businessCacheItem.UserPermissionKey);
            _cacheService.RemoveByKeys(_businessCacheItem.UserMenusKey);
        }
    }
}
