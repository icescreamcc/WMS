using Logic.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Inv;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Inv
{ 
    public class StorageExternalController : ExternalApiController
    {
        private readonly RequisitionMgr _requisitionMgr;

        public StorageExternalController(RequisitionMgr requisitionMgr)
        {
            _requisitionMgr = requisitionMgr;
        }

        [HttpPost]
        [ExternalApi]
        public async Task AddReqisitionOrderByExternal(ProductReqisitionInputExternalDto data)
        {
            await _requisitionMgr.AddReqisitionOrderByExternal(data);
        }
    }
}
