using External.Cache;
using External.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Token
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;

        private readonly ICacheProvider _cacheProvider;

        public TokenHelper(IConfiguration configuration, ICacheProvider cacheProvider)
        {
            _configuration = configuration;
            _cacheProvider = cacheProvider;
        } 

        /// <summary>
        /// 生成加密token字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="signature"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public string BulidToken(string userId,string userName, string signature,string device)
        {
            var expiration = _configuration.GetSection("Token")["Expiration"];
            var secretKey = _configuration.GetSection("Token")["Secret"];
            var obj = new TokenModel
            {
                UserId = userId,
                UserName= userName,
                Signature = signature,
                Expiration=DateTime.Now.AddSeconds(int.Parse(expiration)),
                Device= device
            };
            var jsonStr = JsonSerializer.Serialize(obj);
           return  EncryptionHelper.DesEncrypt(jsonStr, secretKey);
        }

        /// <summary>
        /// 解密token
        /// </summary>
        /// <param name="str"></param> 
        /// <returns></returns>
        public TokenModel GetTokenInfo(string str)
        {
            var secretKey = _configuration.GetSection("Token")["Secret"];
            var jsonStr =  EncryptionHelper.DesDecrypt(str, secretKey);
            var model = JsonSerializer.Deserialize<TokenModel>(jsonStr);
            return model;
        }

        /// <summary>
        /// 生成验签凭证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string BulidSignature(string userId)
        {
            var str = userId +","+ DateTime.Now.ToString();
            return  EncryptionHelper.SHA256Encrypt(str);
        } 

        public void SetTokenCache(string userID, string token)
        {
            string key = _getTokenCacheKey(userID);
            _= _cacheProvider.SetString(key, token);
        }

        public string GetTokenCache(string userId)
        {
            string key = _getTokenCacheKey(userId);
            return _cacheProvider.GetString(key);
        }

        public void RemoveTokenCache(string userId)
        {
            string key = _getTokenCacheKey(userId);
            _= _cacheProvider.Remove(key);
        }

        private string _getTokenCacheKey(string userId)
        {
            var appName = _configuration.GetSection("AppConfig")["AppName"];
            return $"Token_{appName}_{userId}";
        }
    }
}
