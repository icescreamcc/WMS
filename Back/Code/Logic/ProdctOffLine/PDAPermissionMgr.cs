using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Logic.LogicCommon;
using Models.Model.Enum;
using Models.Model.Sys;
using System.Drawing;

namespace Logic.ProductOffLine
{
    public class PDAPermissionMgr: DbOperationHandler
    {
        private readonly SysArgsService _sysArgsHelper;

        private readonly BusinessCacheService _cacheService;

        public PDAPermissionMgr(Repository repository, SysArgsService sysArgsHelper, BusinessCacheService cacheService) : base(repository)
        {
            _sysArgsHelper = sysArgsHelper;
            _cacheService = cacheService;
        }

        public async Task<List<Menu>> GetUserMenus(string userId)
        {
            var adminAccount = await _sysArgsHelper.GetDeveloper();
            if (userId == adminAccount.Value.ToString())
            {
                return await Repository.ClientDb.Queryable<SysMenus>()
                                .Where(m => m.IsValid && m.MenuDisplay == "MOBILE" && m.MenuType == MenuType.Menu.ToString())
                                .Select(x => new Menu { MenuId = x.MenuId, MenuName = x.MenuName,Icon=x.Icon, Rank = x.Rank, RoutePath = x.RoutePath, ComponentPath = x.ComponentPath ,MenuLayout=x.MenuLayout})
                                .OrderBy(x => x.Rank)
                                .ToListAsync();
            }
            else
            {
                return await Repository.ClientDb.Queryable<SysUser>()
                              .LeftJoin<SysUserRoles>((u, ur) => u.UserId == ur.UserId)
                              .LeftJoin<SysUserPermissions>((u, ur, p) => ur.RoleId == p.RoleId)
                              .LeftJoin<SysMenus>((u, ur, p, m) => p.MenuId == m.MenuId)
                              .Where((u, ur, p, m) => u.UserId == userId && m.IsValid &&  m.MenuDisplay == "MOBILE" && m.MenuType == MenuType.Menu.ToString())
                              .Select((u, ur, p, m) => new Menu { MenuId = m.MenuId, MenuName = m.MenuName, Icon=m.Icon, Rank = m.Rank, RoutePath = m.RoutePath, ComponentPath = m.ComponentPath, MenuLayout = m.MenuLayout })
                              .MergeTable().OrderBy(m => m.Rank).Distinct()
                              .ToListAsync();
            } 
        }
    }
}