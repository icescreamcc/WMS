using External.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using System.Linq;
using System.Security.Claims;
using WebApi.Filter;
using WebApi.Response;

namespace WebApi.Controllers
{ 
    [TypeFilter(typeof(AuthorizeTokenFilter))]
    public class AuthTokenController : ControllerServer
    {  
    }
}
