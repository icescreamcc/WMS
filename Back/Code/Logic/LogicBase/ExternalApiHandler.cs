using DbRepository.Repository;
using External.Common;
using Logic.LogicBase.CacheService;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase
{
    public class ExternalApiHandler : ApprovalHandler
    {
        private readonly BusinessCacheService _businessCacheService;

        private readonly BusinessCacheItem _businessCacheItem;

        public readonly HttpHelperAsync HttpHelper;
          
        public ExternalApiHandler(Repository repository, BusinessCacheService businessCacheService, BusinessCacheItem businessCacheItem, HttpHelperAsync httpHelper) : base(repository)
        {
            _businessCacheService = businessCacheService;
            _businessCacheItem = businessCacheItem;
            HttpHelper= httpHelper;
        }

        protected string GetAccessToken(string host, string providerName, string secret)
        {
            var tokenCache = _businessCacheService.GetString(_businessCacheItem.ExternalApiTokenKey, providerName+ secret);
            if (string.IsNullOrEmpty(tokenCache))
            {
                var url = host + "/api/Auth/GetAccessToken";
                var provider = new ExternalProviderAuthDto
                {
                    ProviderName = providerName,
                    ProviderSecret = secret
                };
                var res = HttpHelper.RequestPost<ExternalProviderAuthDto, ExternalResponseDto>(provider, url);
                var token = res == null ? null : res.data.ToString();
                _businessCacheService.SetString(_businessCacheItem.ExternalApiTokenKey, providerName + secret, token);
                return res == null ? null : res.data.ToString();
            }
            return tokenCache;
        }
    }
}
