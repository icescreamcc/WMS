using External.Log;
using Logic.LogicBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks; 
using WebApi.Response;
using WebApi.Token;

namespace WebApi.Filter
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly LogHelper _logger;

        private readonly TokenHelper _tokenHelper;
        public ExceptionFilter(LogHelper logger, TokenHelper tokenHelper)
        {
            _logger = logger;
            _tokenHelper = tokenHelper;
        }
          
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                var userId = string.Empty;
                string token = context.HttpContext.Request.Headers["authorization"];
                if (token != null)
                {
                    var tokenModel = _tokenHelper.GetTokenInfo(token);
                    userId = tokenModel?.UserId ?? "";
                }
               
                var desc = context.ActionDescriptor as ControllerActionDescriptor; 
                var args = string.Empty;
                if (context.HttpContext.Request.Method == HttpMethods.Get || context.HttpContext.Request.Query?.Count > 0)
                {
                    args = context.HttpContext.Request.QueryString.Value.TrimStart('?');
                    var enArgs = string.Empty;
                    if (External.Common.EncryptionHelper.TryBase64Encoded(args, out enArgs))
                    {
                        args = enArgs;
                    }
                }
                if (context.HttpContext.Request.Method == HttpMethods.Post || context.HttpContext.Request.Method == HttpMethods.Put || context.HttpContext.Request.Method == HttpMethods.Delete)
                {
                    if (context.HttpContext.Request.ContentLength > 0)
                    {
                        if (context.HttpContext.Request.ContentType.IndexOf("multipart/form-data") < 0)
                        {
                            args += "&" + context.HttpContext.Items["RequestBody"];
                        }
                        else
                        {
                            //foreach (var key in context.HttpContext.Request.Form.Keys)
                            //{
                            //    args += "&" + key + "=" + context.HttpContext.Request.Form[key].ToString();
                            //}
                            //args = args.TrimStart('&'); 
                        }
                    }
                } 
                var res = new ResponseResult(); 
                res.Status = ResponseStatus.Error.ToString();
                if (context.Exception.GetType().Name == typeof(BusinessException).Name)
                {
                    res.Message = context.Exception.Message;
                }
                else
                {
                    Console.WriteLine(context.Exception.Message);
                    Console.WriteLine(context.Exception.StackTrace);
                    res.Message = "请求发生错误,详细信息请联系管理员查看错误日志";
                    _logger.LogError(userId, desc.ControllerName, desc.ActionName, context.Exception.Message, args, context.Exception, context.Exception.StackTrace);
                }
                Console.WriteLine("异常描述:"+context.Exception.Message+",堆栈信息:"+ context.Exception.StackTrace);
                context.Result = new JsonResult(res);
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                await Task.CompletedTask;
            }
        }
    }
}
