using Logic.PlanMaterial;
using Logic.Sys;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ScheduleJob.Job
{
    public class FinishedProductOrdersDataSyncJob : IJob
    {
        private readonly MaterialRequirementPlanMgr _materialRequirementPlanMgr;

        public FinishedProductOrdersDataSyncJob(MaterialRequirementPlanMgr materialRequirementPlanMgr)
        {
            _materialRequirementPlanMgr = materialRequirementPlanMgr;
        }

        public static string JobName => "FinishedProductOrdersDataSyncJob";

        public static string JobGroup => "DataSyncJob";

        public static string CronExp => "";

        public static int IntervalInSeconds => 600;


        public Task Execute(IJobExecutionContext context)
        {
            _materialRequirementPlanMgr.GetFinishedProductOrdersFormPMS();
            _materialRequirementPlanMgr.SetMaterialRequirementExceed(); 
            return Task.CompletedTask;
        }
    }
}
