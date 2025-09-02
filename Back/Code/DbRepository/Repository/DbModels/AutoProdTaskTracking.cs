using DbRepository.Repository.BaseModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("AutoProdTaskTracking", "AGV自动化任务追踪表")]
    public class AutoProdTaskTracking
    {
        [SugarColumn(Length = 50, IsIdentity =true, IsPrimaryKey = true)]
        public int TaskId{ get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "任务编码")]
        public string TaskCode { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "AGV任务请求码")]
        public string AGVReqCode { get; set; }

        [SugarColumn(Length = 100,IsNullable =true,  ColumnDescription = "需求产线")]
        public string LineNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "业务单号(出库单号或入库单号)")]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "物料分类")]
        public string GoodsClassifyGroup { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "业务类型")]
        public string BusinessType { get; set; } 

        [SugarColumn(Length = 5000, ColumnDescription = "运送物料信息(Json字符串)")]
        public string GoodsInfo { get; set; }
         
        [SugarColumn(Length = 100, ColumnDescription = "任务类型(出库或入库)")]
        public string TaskType { get; set; }

        [SugarColumn(Length = 100,IsNullable =true, ColumnDescription = "AGV动作类型(取料箱/还料箱)")]
        public string ActionType { get; set; }

        [SugarColumn(ColumnDescription = "是否处于执行中")]
        public bool IsExecuting { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "任务模式(单次任务/多次任务)")]
        public string TaskModel { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "送料箱时任务状态")]
        public string TaskStatus { get; set; }

        [SugarColumn( ColumnDescription = "是否已还回料箱")]
        public bool IsReturn { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "还料箱时任务状态")]
        public string ReturnTaskStatus { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "起始地设备编号")]
        public string StartingDeviceNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "起始地设备类型")]
        public string StartingDeviceType { get; set; }

        [SugarColumn(Length = 100,IsNullable =true, ColumnDescription = "起始地AGV仓位标识点")]
        public string StartingAGVPositionNo{ get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "起始地仓位编码")]
        public string StartingBinNo { get; set; }

        [SugarColumn(ColumnDescription = "起始地仓位序号")]
        public int StartingBinRank { get; set; }
          
        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "目的地设备编号")]
        public string DestinationDeviceNo { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "目的地设备类型")]
        public string DestinationDeviceType { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "目的地AGV仓位标识点")]
        public string DestinationAGVPositionNo { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "目的地仓位编码")]
        public string DestinationBinNo { get; set; }

        [SugarColumn(ColumnDescription = "目的地仓位序号")]
        public int DestinationBinRank { get; set; } 
           
        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "AGV编号")]
        public string AGVNo { get; set; }

        [SugarColumn(ColumnDescription = "任务创建时间")]
        public DateTime TaskCreateTime { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "任务开始时间")]
        public string TaskStartTime { get; set; }

        [SugarColumn(Length = 50, IsNullable = true, ColumnDescription = "任务结束时间")]
        public string TaskEndTime { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "操作人ID")]
        public string OperatorId { get; set; }

        [SugarColumn(Length = 50,  ColumnDescription = "操作人姓名")]
        public string OperatorName { get; set; }
    }
}
