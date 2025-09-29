using Logic.Sys;
using Quartz; 
using System; 
using System.Threading.Tasks;

namespace Logic.ScheduleJob
{
   public class UserDataSyncJob : IJob
    {
        private readonly UserDataSync _userDataSync;
        public UserDataSyncJob(UserDataSync userDataSync)
        {
            _userDataSync = userDataSync;
        }

        public static string JobName => "UserDataSyncJob";

        public static string JobGroup => "DataSyncJob";

        public static string CronExp => "";

        public static int IntervalInSeconds => 60;
         

        public  Task Execute(IJobExecutionContext context)
        {
             _userDataSync.Sync(); 
             return Task.CompletedTask;
        }
         
    }
}
