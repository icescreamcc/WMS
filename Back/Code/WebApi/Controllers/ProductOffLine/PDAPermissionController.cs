 using Logic.ProductOffLine;
using Logic.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.ProductOffLine
{
    public class PDAPermissionController : AuthTokenController
    {
        private readonly PDAPermissionMgr _mobilePermissionMgr;

        public PDAPermissionController(PDAPermissionMgr mobilePermissionMgr)
        {
            _mobilePermissionMgr = mobilePermissionMgr;
        }

        [HttpGet]
        [Skip]
        public async Task<List<Menu>> GetUserMenus(string userId)
        {
            return await _mobilePermissionMgr.GetUserMenus(userId);
        }
    }
}
