using Logic.ScheduleJob.Job;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ScheduleJob
{
    /// <summary>
    /// 所有的后台作业在这里启动
    /// </summary>
   public class JobLauncher
    {
        private readonly JobCreator _demoJobCreator;

        private readonly IConfiguration _configuration;

        public JobLauncher(JobCreator demoJobCreator, IConfiguration configuration)
        {
            _demoJobCreator = demoJobCreator; 
            _configuration = configuration;
        }

        public void JobStart(string jobName="All")
        {
            var sign = _configuration.GetSection("Factory:sign").Value;
            var appName= _configuration.GetSection("AppConfig:AppName").Value;
            //if (sign == "dl") { 
            //    if (jobName == "All")
            //    {
            //        _demoJobCreator.CreateJobsAndStart<FactoryDataSyncJob>(FactoryDataSyncJob.JobName, FactoryDataSyncJob.JobGroup, FactoryDataSyncJob.IntervalInSeconds);
            //        _demoJobCreator.CreateJobsAndStart<UserDataSyncJob>(UserDataSyncJob.JobName, UserDataSyncJob.JobGroup, UserDataSyncJob.IntervalInSeconds);
            //        if (appName == "IWMS")
            //        {
            //            //_demoJobCreator.CreateJobsAndStart<FinishedProductOrdersDataSyncJob>(FinishedProductOrdersDataSyncJob.JobName, FinishedProductOrdersDataSyncJob.JobGroup, FinishedProductOrdersDataSyncJob.IntervalInSeconds);
            //            _demoJobCreator.CreateJobsAndStart<ImportIBDDataSyncJob>(ImportIBDDataSyncJob.JobName, ImportIBDDataSyncJob.JobGroup, ImportIBDDataSyncJob.IntervalInSeconds, ImportIBDDataSyncJob.CronExp);
            //        }
            //        _demoJobCreator.CreateJobsAndStart<SafetyInvenstoryWarningJob>(SafetyInvenstoryWarningJob.JobName, SafetyInvenstoryWarningJob.JobGroup, SafetyInvenstoryWarningJob.IntervalInSeconds, SafetyInvenstoryWarningJob.CronExp);
            //    }
            //    else
            //    {
            //        if(jobName==typeof(FactoryDataSyncJob).Name)
            //            _demoJobCreator.CreateJobsAndStart<FactoryDataSyncJob>(FactoryDataSyncJob.JobName, FactoryDataSyncJob.JobGroup, FactoryDataSyncJob.IntervalInSeconds);
            //        if (jobName == typeof(UserDataSyncJob).Name)
            //            _demoJobCreator.CreateJobsAndStart<UserDataSyncJob>(UserDataSyncJob.JobName, UserDataSyncJob.JobGroup, UserDataSyncJob.IntervalInSeconds);
            //        if (jobName == typeof(FinishedProductOrdersDataSyncJob).Name)
            //            _demoJobCreator.CreateJobsAndStart<FinishedProductOrdersDataSyncJob>(FinishedProductOrdersDataSyncJob.JobName, FinishedProductOrdersDataSyncJob.JobGroup, FinishedProductOrdersDataSyncJob.IntervalInSeconds);
            //        if (jobName == typeof(FinishedProductOrdersDataSyncJob).Name)
            //            _demoJobCreator.CreateJobsAndStart<SafetyInvenstoryWarningJob>(SafetyInvenstoryWarningJob.JobName, SafetyInvenstoryWarningJob.JobGroup, SafetyInvenstoryWarningJob.IntervalInSeconds);
            //    }
            //}
            //else if (sign == "w3")
            //{
            //    if (jobName == typeof(UserDataSyncJob).Name)
            //        _demoJobCreator.CreateJobsAndStart<UserDataSyncJob>(UserDataSyncJob.JobName, UserDataSyncJob.JobGroup, UserDataSyncJob.IntervalInSeconds);
            //}
        }

        public void JobStop(string jobName = "All")
        {
            var sign = _configuration.GetSection("Factory:sign").Value;
            if (jobName == "All")
            { 
                    _demoJobCreator.JobStop(FactoryDataSyncJob.JobName, FactoryDataSyncJob.JobGroup); 
                    _demoJobCreator.JobStop(UserDataSyncJob.JobName, UserDataSyncJob.JobGroup);
                   _demoJobCreator.JobStop(FinishedProductOrdersDataSyncJob.JobName, FinishedProductOrdersDataSyncJob.JobGroup);
                   _demoJobCreator.JobStop(SafetyInvenstoryWarningJob.JobName, SafetyInvenstoryWarningJob.JobGroup);
            }
            else
            {
                if (jobName == typeof(FactoryDataSyncJob).Name)
                    _demoJobCreator.JobStop(FactoryDataSyncJob.JobName, FactoryDataSyncJob.JobGroup);
                if (jobName == typeof(UserDataSyncJob).Name)
                    _demoJobCreator.JobStop(UserDataSyncJob.JobName, UserDataSyncJob.JobGroup);
                if (jobName == typeof(FinishedProductOrdersDataSyncJob).Name)
                    _demoJobCreator.JobStop(FinishedProductOrdersDataSyncJob.JobName, FinishedProductOrdersDataSyncJob.JobGroup);
                if (jobName == typeof(SafetyInvenstoryWarningJob).Name)
                    _demoJobCreator.JobStop(SafetyInvenstoryWarningJob.JobName, SafetyInvenstoryWarningJob.JobGroup);
            }
         
        }
    }
}
