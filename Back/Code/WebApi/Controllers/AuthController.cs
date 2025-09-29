 using Logic.Authentication; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System.Threading.Tasks;
using WebApi.Token;
using WebApi.Response;
using System.Security.Claims;  
using Models.Model; 
using Logic.LogicBase;
using Models.Model.Sys;
using Logic.Inventory;
using Logic.Sys;

namespace WebApi.Controllers
{
    public class AuthController : AnonymousController
    {

        private readonly AccountPasswordAuth _accountPasswordAuth; 

        private readonly TokenHelper _tokenHelper;

        private readonly External.Log.LogHelper _logHelper;

        private readonly WindowsUserAuth _windowsUserAuth;

        private readonly RequisitionMgr _requisitionMgr;
        private readonly SysArgsMgr _sysArgsMgr;

        public AuthController(AccountPasswordAuth accountPasswordAuth, TokenHelper tokenHelper, External.Log.LogHelper logHelper, WindowsUserAuth windowsUserAuth, RequisitionMgr requisitionMgr, SysArgsMgr sysArgsMgr)
        {
            _accountPasswordAuth = accountPasswordAuth; 
            _tokenHelper = tokenHelper;
            _logHelper = logHelper;
            _windowsUserAuth = windowsUserAuth; 
            _requisitionMgr = requisitionMgr;
            _sysArgsMgr = sysArgsMgr;
        }
          
        /// <summary>
        /// 获取身份认证的token(外部系统鉴权）
        /// </summary>
        /// <param name="externalAuthDto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        public async Task<string> GetAccessToken(ExternalProviderAuthDto externalAuthDto)
        {
            string providerHost = HttpContext.Connection.RemoteIpAddress.ToString();
            string providerPort = HttpContext.Connection.RemotePort.ToString();
            _logHelper.LogInfo("GetAccessToken", "第三方授权HOST", providerHost);
            var authRes = await _accountPasswordAuth.ExternalValidate(externalAuthDto.ProviderName, externalAuthDto.ProviderSecret, providerHost);
            if (authRes)
            {
                var signature = _tokenHelper.BulidSignature(externalAuthDto.ProviderName);
                var token = _tokenHelper.BulidToken(externalAuthDto.ProviderName, externalAuthDto.ProviderName, signature, "Ext");
                return token;
            }
            else
            {
                throw new BusinessException("身份验证失败,请检查ProviderName,ProviderSecret以及请求的host地址是否正确");
            }
        }

        /// <summary>
        /// casSever获取ticket的url地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetTicketUrl()
        {
            return _windowsUserAuth.GetTicketUrl();
        } 

        /// <summary>
        /// 从cas中获取客户端windows登录账户
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserAuthorizationDto> GetWindowsUser(string ticket)
        {
            var authRes= await _windowsUserAuth.GetUserAccount(ticket);
            if (authRes != null)
            {
                if (authRes.IsVaild)
                {
                    authRes.SystemInfo = await _sysArgsMgr.GetArgs();
                    var signature = _tokenHelper.BulidSignature(authRes.UserId);
                    var token = _tokenHelper.BulidToken(authRes.UserId, authRes.UserName, signature, "Win");
                    HttpContext.Response.Headers.Add("authorization", token);
                    _tokenHelper.SetTokenCache(authRes.UserId, token);
                    return authRes;
                }
                else
                {
                    throw new BusinessException("登录验证失败,该用户已被冻结");
                }

            }
            else
            {
                throw new BusinessException("登录验证失败,未获取到Windows账户信息");
            }
        }

        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserAuthorizationDto> Login(LoginInfo data)
        {  
            var authRes = await _accountPasswordAuth.Validate(data.UserId, data.Password);
            if (authRes != null)
            {
                if (authRes.IsVaild)
                {
                    authRes.SystemInfo = await _sysArgsMgr.GetArgs();
                    var signature = _tokenHelper.BulidSignature(data.UserId);
                    var token = _tokenHelper.BulidToken(authRes.UserId, authRes.UserName, signature, data.Device);
                    HttpContext.Response.Headers.Add("authorization", token);
                    _tokenHelper.SetTokenCache(authRes.UserId, token); 
                    return authRes;
                }
                else
                {
                    throw new BusinessException("登录验证失败,该用户已被冻结"); 
                }
              
            }
            else
            {
                throw new BusinessException("登录验证失败,账号或密码错误"); 
            } 
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public void LoginOut(string userId)
        {
            _tokenHelper.RemoveTokenCache(userId); 
        }

    }
 
}
