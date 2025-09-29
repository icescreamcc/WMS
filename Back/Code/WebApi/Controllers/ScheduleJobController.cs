using Logic.ScheduleJob;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{ 
    /// <summary>
    /// 后台作业手动触发控制器
    /// </summary>
    public class ScheduleJobController : AnonymousController
    {
        private readonly JobLauncher _jobLauncher;
        public ScheduleJobController(JobLauncher jobLauncher)
        {
            _jobLauncher = jobLauncher;
        }

        [HttpGet]
        public void Start(string jobName = "All")
        {
            _jobLauncher.JobStart(jobName);
        }

        [HttpGet]
        public void Stop(string jobName = "All")
        {
            _jobLauncher.JobStop(jobName);
        }
    }
}
