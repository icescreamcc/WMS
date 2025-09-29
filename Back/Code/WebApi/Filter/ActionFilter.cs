using Logic.LogicCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System; 
using System.Threading.Tasks;
using WebApi.Response;
using WebApi.Token;

namespace WebApi.Filter
{
    public class ActionFilter : IAsyncActionFilter
    {
        private readonly BusinessLogService _businessLog;

        private readonly TokenHelper _tokenHelper;

        private readonly IConfiguration _configuration;

        public ActionFilter(BusinessLogService businessLog, TokenHelper tokenHelper, IConfiguration configuration)
        {
            _businessLog = businessLog;
            _tokenHelper = tokenHelper;
            _configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _setRequestArgs(context);
            var resultContext = await next();
            _setResponseData(context, resultContext); 
            var enable = _configuration.GetSection("Logging:BusinessLog").GetValue<bool>("Enable");
            if (enable)
                _logWrite(context);
        }

        private void _setRequestArgs(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                if (context.HttpContext.Request.Query.Count > 0)
                {
                    var args = context.HttpContext.Request.QueryString.Value.TrimStart('?');
                    var enArgs = string.Empty;
                    if (External.Common.EncryptionHelper.TryBase64Encoded(args, out enArgs))
                    {
                        var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(enArgs);
                        foreach (var parameter in context.ActionDescriptor.Parameters)
                        {
                            if (queryParams.ContainsKey(parameter.Name))
                            {
                                var parameterValue = queryParams[parameter.Name][0];
                                if (parameter.ParameterType == typeof(int))
                                {
                                    context.ActionArguments[parameter.Name] = int.Parse(parameterValue);
                                }
                                else if (parameter.ParameterType == typeof(double))
                                {
                                    context.ActionArguments[parameter.Name] = double.Parse(parameterValue);
                                }
                                else if (parameter.ParameterType == typeof(float))
                                {
                                    context.ActionArguments[parameter.Name] = float.Parse(parameterValue);
                                }
                                else if (parameter.ParameterType == typeof(decimal))
                                {
                                    context.ActionArguments[parameter.Name] = decimal.Parse(parameterValue);
                                }
                                else if (parameter.ParameterType == typeof(long))
                                {
                                    context.ActionArguments[parameter.Name] = long.Parse(parameterValue);
                                }
                                else if (parameter.ParameterType == typeof(short))
                                {
                                    context.ActionArguments[parameter.Name] = short.Parse(parameterValue);
                                }
                                else if (parameter.ParameterType == typeof(bool))
                                {
                                    context.ActionArguments[parameter.Name] = bool.Parse(parameterValue);
                                }
                                else if (parameter.ParameterType == typeof(DateTime))
                                {
                                    context.ActionArguments[parameter.Name] = DateTime.Parse(parameterValue);
                                }
                                else
                                {
                                    context.ActionArguments[parameter.Name] = parameterValue== "undefined"?"": parameterValue;
                                }
                            }
                        }
                        context.HttpContext.Request.QueryString = Microsoft.AspNetCore.Http.QueryString.Create(queryParams);
                    }
                }
            }
        }

        private void _setResponseData(ActionExecutingContext context, ActionExecutedContext resultContext)
        {
            if (resultContext.Result != null)
            {
                var desc = context.ActionDescriptor as ControllerActionDescriptor;
                var attr = desc.MethodInfo.GetCustomAttributes(true);
                foreach (var a in attr)
                {
                    if (a is OriginalResponseAttribute)
                    {
                        return;
                    }
                }
                var actionName = (context.ActionDescriptor as ControllerActionDescriptor).ActionName;
                var responseResult = new ResponseResult();
                responseResult.Status = ResponseStatus.Success.ToString();
                if (actionName.IndexOf("Add") ==0 || actionName.IndexOf("Update")==0|| actionName.IndexOf("Submit") == 0 | actionName.IndexOf("Save") == 0)
                {
                    responseResult.Message = "保存成功";
                }
                else if (actionName.IndexOf("Del") ==0)
                {
                    responseResult.Message = "删除成功";
                }
                else if (actionName.IndexOf("Approval") ==0)
                {
                    responseResult.Message = "审批成功";
                }
                if (resultContext.Result is ObjectResult objectResult)
                {
                    responseResult.Data = objectResult.Value;
                }
                else if (resultContext.Result is EmptyResult emptyResult)
                {
                    responseResult.Data = null;
                }
                else if (resultContext.Result is JsonResult jsonResult)
                {
                    responseResult.Data = jsonResult.Value;
                }
                else if (resultContext.Result is ContentResult contentResult)
                {
                    responseResult.Data = contentResult.Content;
                }
                resultContext.Result = new JsonResult(responseResult)
                {
                    StatusCode = 200
                };
            }
        }

        private void _logWrite(ActionExecutingContext context)
        {

            var desc = context.ActionDescriptor as ControllerActionDescriptor; 
            var attr = desc.MethodInfo.GetCustomAttributes(true); 
            foreach (var a in attr)
            {
                if (a is BusinessLogAttribute businessLogAttribute)
                {
                    var args = string.Empty;
                    if (context.HttpContext.Request.Method == HttpMethods.Get|| context.HttpContext.Request.Query?.Count > 0)
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
                                args += "&"+context.HttpContext.Items["RequestBody"];
                            } 
                        }
                    }
                    string token = context.HttpContext.Request.Headers["authorization"];
                    var tokenModel = _tokenHelper.GetTokenInfo(token);
                    var userId = tokenModel?.UserId ?? "";
                    var userName = tokenModel?.UserName ?? "";
                    _ = _businessLog.LogWrite(userId, userName, businessLogAttribute.Titile, businessLogAttribute.LogType, args, businessLogAttribute.ModuleName);
                    break;
                }
            }
        }
    }
}
