using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Response;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors]
    public  class ControllerServer: ControllerBase
    {  
        public static string ConvertOrderType(string orderType,bool asc=true)
        {
            if (string.IsNullOrEmpty(orderType))
            {
                return asc?"asc":"desc";
            }
            if (orderType.IndexOf("ending") >= 0)
            {
                return orderType.Replace("ending", "");
            }
            return orderType;
        }
    }
}
