using Logic.Authentication; 
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Baseinfo;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers
{
    [TypeFilter(typeof(AuthorizeExternalFilter))] 
    public class ExternalApiController : ControllerServer
    { 
    }
}
