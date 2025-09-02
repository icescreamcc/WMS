using Logic.Sys;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Logic.ScheduleJob
{
    public class ImportIBDDataSyncJob : IJob
    {
        private readonly ImportIBDDataSync _importIBDDataSync;
        public ImportIBDDataSyncJob(ImportIBDDataSync importIBDDataSync)
        {
            _importIBDDataSync = importIBDDataSync;
        }

        public static string JobName => "ImportIBDDataSyncJob";

        public static string JobGroup => "ImportIBDDataSync";
        //public static string CronExp => "0 10 08,17 * * ?";//早上8点10分和下午17点10分执行
        public static string CronExp => "0 10 08,17,14 * * ?";//早上8点10分和下午17点10分执行
                                                           //public static int IntervalInSeconds => 60;
                                                           //public static int IntervalInSeconds => 60 * 60 & 24;
        public static int IntervalInSeconds => 60 ;

        public Task Execute(IJobExecutionContext context)
        {
            _importIBDDataSync.Sync();
            return Task.CompletedTask;
        }

    }
}
