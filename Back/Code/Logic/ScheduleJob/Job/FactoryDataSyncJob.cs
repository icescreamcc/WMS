using Logic.BaseInfo;
using Logic.Sys;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ScheduleJob
{
    public class FactoryDataSyncJob : IJob
    {
        private readonly FactoryDataSyncMgr _factoryDataSyncMgr;
        public FactoryDataSyncJob(FactoryDataSyncMgr factoryDataSyncMgr)
        {
            _factoryDataSyncMgr = factoryDataSyncMgr;
        }

        public static string JobName => "FactoryDataSyncJob";

        public static string JobGroup => "DataSyncJob";

        public static string CronExp => "";

        public static int IntervalInSeconds => 600;


        public Task Execute(IJobExecutionContext context)
        {
            _factoryDataSyncMgr.DataSync();
            return Task.CompletedTask;
        }

    }
}
