using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // 捕获异常并记录请求信息到日志中
                if (context.Request.Method == "POST")
                {
                    var requestBody = context.Items["RequestBody"] as string; 
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        requestBody = await reader.ReadToEndAsync();
                    }
                    var exstr = ex.Message;
                }
               

                // 在这里进行日志记录的逻辑
                // ...

                // 将异常重新抛出，以便后续的全局异常过滤器可以处理
                throw;
            }
        }
    }
}
