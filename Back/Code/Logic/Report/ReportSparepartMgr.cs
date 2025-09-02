using DbRepository.Repository;
using Logic.LogicBase;
using Logic.LogicCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Report
{
    public class ReportSparepartMgr : DbOperationHandler
    {
        private readonly SysArgsService _sysArgsService;

        public ReportSparepartMgr(Repository repository, SysArgsService sysArgsService) : base(repository)
        {
            _sysArgsService = sysArgsService;
        }
 
    }
}
