using Logic.Sys;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.UserModel;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ScheduleJob
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;

        protected readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceScopeFactory  serviceScopeFactory, IServiceProvider serviceProvider)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var job = provider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }
        }

        //public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        //{
        //    var job = _serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        //    return job;
        //}

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}
