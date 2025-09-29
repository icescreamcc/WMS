using External.HIK_RCS;
using External.HIK_RCS.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Prod;
using WebApi.Filter;

namespace WebApi.Controllers.AutomationDevice
{
    public class AGVTaskNotificationController : AnonymousController
    {
        private readonly RCSService _rcsService;

        public AGVTaskNotificationController(RCSService rcsService)
        {
            _rcsService = rcsService;
        }

        [HttpPost]
        [OriginalResponse]
        public RCSArgsOutput<string> agvCallback(RCSCallbackArgs args)
        {
            return _rcsService.AgvCallback(args);
        }
    }
}
