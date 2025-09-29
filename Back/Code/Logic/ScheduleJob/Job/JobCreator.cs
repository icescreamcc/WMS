using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ScheduleJob
{
   public class JobCreator
    {

        private readonly StdSchedulerFactory  _stdSchedulerFactory; 

        protected readonly IServiceProvider _serviceProvider;

        public JobCreator(StdSchedulerFactory stdSchedulerFactory, IServiceProvider serviceProvider)
        {
            _stdSchedulerFactory = stdSchedulerFactory;
            _serviceProvider = serviceProvider; 
        }

        private JobKey _createJobKey(string jobName, string jobGroup)
        {
            return new JobKey(jobName, jobGroup);
        }
         

        /// <summary>
        /// 将作业添加到调度容器中并设置执行规则
        /// </summary>
        /// <returns></returns>
        public async void CreateJobsAndStart<T>(string jobName,string jobGroup,int interval,string cron="") where T : IJob
        {
            var jobKeyDemo = _createJobKey(jobName, jobGroup);
            var schedulers = await _stdSchedulerFactory.GetAllSchedulers();
            var isJobExist = schedulers.Any(scheduler => scheduler.GetJobDetail(jobKeyDemo).Result != null);
            if (!isJobExist)
            {
                var scheduler = await _stdSchedulerFactory.GetScheduler();  
                scheduler.JobFactory = _serviceProvider.GetService<IJobFactory>();
                var jobDetail = JobBuilder.Create<T>()
                                      .WithIdentity(jobKeyDemo)
                                      .Build();
                if (string.IsNullOrEmpty(cron))
                {
                    var trigger1 = TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).RepeatForever())
                    .StartNow().Build();
                    await scheduler.ScheduleJob(jobDetail, trigger1);
                }
                else
                {
                    var trigger1 = TriggerBuilder.Create()
                       .WithCronSchedule(cron)
                       .StartNow().Build();
                    await scheduler.ScheduleJob(jobDetail, trigger1);
                } 
                await scheduler.Start();
            } 
        }
         

        /// <summary>
        /// 停止作业
        /// </summary>

        public async void JobStop(string jobName, string jobGroup)
        {
            var jobKeyDemo = _createJobKey(jobName, jobGroup);
            var schs = await _stdSchedulerFactory.GetAllSchedulers();
            var demoJobScheduler = schs.SingleOrDefault(scheduler => scheduler.GetJobDetail(jobKeyDemo) != null);
            if (demoJobScheduler != null)
            { 
                _= demoJobScheduler.Shutdown();
            } 
        }
    }
}
