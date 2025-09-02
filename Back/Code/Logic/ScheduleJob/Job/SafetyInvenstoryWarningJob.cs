using Logic.Inventory;
using Logic.PlanMaterial;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Purchase;

namespace Logic.ScheduleJob
{
    public class SafetyInvenstoryWarningJob : IJob
    {
        private readonly SafetyWarningRecordMgr _safetyWarningRecordMgr;
        private readonly ReceivingOrderMgr _sReceivingOrderMgr;

        public SafetyInvenstoryWarningJob(SafetyWarningRecordMgr safetyWarningRecordMgr, ReceivingOrderMgr receivingOrderMgr)
        {
            _safetyWarningRecordMgr = safetyWarningRecordMgr;
            _sReceivingOrderMgr = receivingOrderMgr; 
        }

        public static string JobName => "SafetyInvenstoryWarningJob";

        public static string JobGroup => "InvJob";

        //public static string CronExp => "0 0 10 ? * MON *";  //0 30 10 ? * * *  

        //public static string CronExp => "0 14 14,15 * * ?";
        //public static string CronExp => "0 0 7,12 * * ?";
        public static string CronExp => "0 0 8,17 * * ?";

        public static int IntervalInSeconds => 60 * 60 & 24;

        public Task Execute(IJobExecutionContext context)
        {
            _safetyWarningRecordMgr.CreateSafetyWarningRecord();
            _sReceivingOrderMgr.CreateASNWarningRecord(); // 创建ASN预警记录
            return Task.CompletedTask;
        }
    }
}
