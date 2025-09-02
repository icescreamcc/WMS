using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Response
{
    public class ResponseResult
    { 
        public string Status { get; set; } 

        public string Message { get; set; }

        public dynamic Data { get; set; } 
    }
}
