using Logic.LogicCommon;
using Logic.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Response;
using WebApi.Token;
using Microsoft.Extensions.Configuration;

namespace WebApi.Filter
{
    public class AuthorizeExternalFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly TokenHelper _tokenHelper;

        private readonly UserMgr _userMgr;

        private readonly SysArgsService _sysArgsHelper;

        private readonly IConfiguration _configuration;

        public AuthorizeExternalFilter(TokenHelper tokenHelper, UserMgr userMgr, SysArgsService sysArgsHelper, IConfiguration configuration)
        {
            _tokenHelper = tokenHelper;
            _userMgr = userMgr;
            _sysArgsHelper = sysArgsHelper;
            _configuration = configuration;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var desc = context.ActionDescriptor as ControllerActionDescriptor;
            var headers = context.HttpContext.Request.Headers;
            var res = new ResponseResult();
            if (headers.ContainsKey("authorization"))
            {
                string token = headers["authorization"];
                var tokenModel = _tokenHelper.GetTokenInfo(token);
                if (DateTime.Now <= tokenModel.Expiration)
                {
                    if (context.HttpContext.Request.Method == HttpMethods.Post || context.HttpContext.Request.Method == HttpMethods.Put || context.HttpContext.Request.Method == HttpMethods.Delete)
                    {
                        if (context.HttpContext.Request.ContentType.IndexOf("multipart/form-data") < 0)
                        {
                            var requestBody = string.Empty;
                            context.HttpContext.Request.EnableBuffering();
                            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                            using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
                            {
                                requestBody = await reader.ReadToEndAsync();
                                context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                            }
                            context.HttpContext.Items["RequestBody"] = requestBody;
                        }
                    }
                    var newToken = _tokenHelper.BulidToken(tokenModel.UserId, tokenModel.UserName, tokenModel.Signature, tokenModel.Device); 
                    var adminAccount = await _sysArgsHelper.GetDeveloper();
                    if (tokenModel.UserId == adminAccount.Value.ToString())
                    {
                        context.HttpContext.Response.Headers.Add("authorization", newToken);
                        return;
                    }
                    var attr = desc.MethodInfo.GetCustomAttributes(true);
                    var isExternalApi = attr.Any(a => a is ExternalApiAttribute);
                    if (!isExternalApi)
                    {
                        res.Status = ResponseStatus.Denied.ToString();
                        res.Message = "请求被拒绝,当前请求非对外开放的接口或没有相关授权";
                        context.Result = new JsonResult(res);
                        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    }
                    context.HttpContext.Response.Headers.Add("authorization", newToken);
                    //var permissionMenus = await _userMgr.GetPermissionMenus(tokenModel.UserId); 
                    //if (!permissionMenus.Exists(x => x.CtrlName == desc.ControllerName && x.ActionName == desc.ActionName))
                    //{
                    //    res.Status = ResponseStatus.Denied.ToString();
                    //    res.Message = "请求被拒绝,本次请求没未授权";
                    //    context.Result = new JsonResult(res);
                    //    context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    //}
                    //context.HttpContext.Response.Headers.Add("authorization", newToken);
                }
                else
                {
                    res.Status = ResponseStatus.Denied.ToString();
                    res.Message = "请求被拒绝,验证已过期,请重新登录";
                    context.Result = new JsonResult(res);
                    context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                }
            }
            else
            {
                res.Status = ResponseStatus.Denied.ToString();
                res.Message = "请求被拒绝,未携带验证票据";
                context.Result = new JsonResult(res);
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            }
        }
    }
}
