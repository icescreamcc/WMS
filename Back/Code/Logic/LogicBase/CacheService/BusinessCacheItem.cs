using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase.CacheService
{
   public class BusinessCacheItem
    {  
        public  string UserMenusKey { get;set; }

        public  string UserPermissionKey { get; set; }

        public string ExternalApiTokenKey { get; set; }

        public BusinessCacheItem(IConfiguration configuration)
        {
            var appName = configuration.GetSection("AppConfig:AppName").Value;
            UserMenusKey = $"{appName}_UserMenus";
            UserPermissionKey=$"{appName}_UserPermission";
            ExternalApiTokenKey = $"{appName}_ExternalApiToken";
        } 
    }
}
