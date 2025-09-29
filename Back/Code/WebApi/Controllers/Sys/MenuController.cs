using Logic.Sys; 
using Microsoft.AspNetCore.Mvc;
using Models.Model; 
using Models.Model.Sys; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Sys
{
    public class MenuController : AuthTokenController
    {
        private readonly MenusMgr _menuMgr; 

        public MenuController(MenusMgr menuMgr)
        {
            _menuMgr = menuMgr; 
        }

        /// <summary>
        /// 获取菜单树形结构
        /// </summary> 
        /// <returns></returns>
        [HttpGet] 
        public async Task<List<TreeModel>> GetMenusData()
        { 
            return await _menuMgr.GetMenusData(); 
        }

        /// <summary>
        /// 获取菜单详细信息
        /// </summary>
        /// <param name="menuId"></param> 
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<Menu> GetMenuInfo(string menuId)
        {
            return await _menuMgr.GetMenuInfo(menuId);  
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加菜单", Models.Model.Enum.LogType.Add)]
        public async Task AddMenu(Menu data)
        {
             await _menuMgr.AddMenu(data);  
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改菜单", Models.Model.Enum.LogType.Update)]
        public async Task UpdateMenu(Menu data)
        {
            await _menuMgr.UpdateMenu(data); 
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="parentId"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除菜单", Models.Model.Enum.LogType.Del)]
        public async Task DelMenu(string menuId, string parentId, string menuType)
        {
             await _menuMgr.DelMenu(menuId,  parentId,  menuType); 
        }
         
    }
}
