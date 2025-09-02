using External.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class DecryptMiddleware
    {
        private readonly RequestDelegate _next; 

        public DecryptMiddleware(RequestDelegate next)
        {
            _next = next; 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "GET")
            {
                if (context.Request.Query.Count > 0)
                {
                   var args = context.Request.QueryString.Value.TrimStart('?');
                    var enArgs = string.Empty;
                    if (EncryptionHelper.TryBase64Encoded(args,out enArgs))
                    { 
                        var queryParams = QueryHelpers.ParseQuery(enArgs); 
                        context.Request.QueryString = QueryString.Create(queryParams);
                    } 
                } 
            } 
            await _next(context);
        }
         
    }
     
}
