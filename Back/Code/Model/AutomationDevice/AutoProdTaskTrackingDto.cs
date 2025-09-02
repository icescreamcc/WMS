using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class AutoProdTaskTrackingDto
    {
        public int TaskId { get; set; }
         
        public string TaskCode { get; set; }
         
        public string AGVReqCode { get; set; }
         
        public string LineNo { get; set; }
         
        public string OrderNo { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string BusinessType { get; set; }
         
        public string GoodsInfo { get; set; }
         
        public string TaskType { get; set; }

        public bool IsExecuting { get; set; }

        public string ActionType { get; set; }
         
        public string TaskModel { get; set; }
         
        public string TaskStatus { get; set; }

        public string StatusDesc { get; set; }

        public bool IsReturn { get; set; }
         
        public string ReturnTaskStatus { get; set; }

        public string StartingDeviceNo { get; set; }
         
        public string StartingDeviceType { get; set; }
         
        public string StartingAGVPositionNo { get; set; }
         
        public string StartingBinNo { get; set; }
         
        public int StartingBinRank { get; set; }
         
        public string DestinationDeviceNo { get; set; }
         
        public string DestinationDeviceType { get; set; }
         
        public string DestinationAGVPositionNo { get; set; }
         
        public string DestinationBinNo { get; set; }
         
        public int DestinationBinRank { get; set; }
         
        public string AGVNo { get; set; }
         
        public DateTime TaskCreateTime { get; set; }
         
        public string TaskStartTime { get; set; }
         
        public string TaskEndTime { get; set; }
         
        public string OperatorId { get; set; }
         
        public string OperatorName { get; set; }
    }
}
