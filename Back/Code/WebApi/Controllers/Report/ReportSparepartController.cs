using Logic.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Enum;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Report
{ 
    public class ReportSparepartController : AuthTokenController
    {
        private readonly ReportSparepartMgr _reportSparepartMgr;

        private const string _moduleName = "备件报表";

        public ReportSparepartController(ReportSparepartMgr reportSparepartMgr)
        {
            _reportSparepartMgr = reportSparepartMgr;
        } 
    }
}
