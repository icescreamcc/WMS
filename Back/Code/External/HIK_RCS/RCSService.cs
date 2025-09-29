using External.Common;
using External.HIK_RCS.Dto;
using External.Log;
using Microsoft.Extensions.Configuration;
using Models.Model.Prod;
using Models.Model.Sys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS
{
    public class RCSService
    {
        private readonly string? _rcsHost;

        private readonly HttpHelperAsync _httpHelperAsync;

        public event EventHandler<RCSCallbackArgs>? OnTaskResult;

        public bool IsAgvCallbackSubscribed;

        public RCSService(IConfiguration configuration, HttpHelperAsync httpHelperAsync)
        {
            _rcsHost = configuration.GetSection("RCS:Host").Value;
            _httpHelperAsync = httpHelperAsync;
        }

        public async Task<RCSArgsOutput<string>> ScheduleAgvTaskTest(string reqCode, string taskCode, string taskType, string userCallCode)
        {
            var taskArgs = new RCSArgsAGVTaskInput
            {
                reqCode = reqCode,
                taskTyp = taskType,
                taskCode = taskCode,
                userCallCode = userCallCode
            };
            string path = "/rcms/services/rest/hikRpcService/genAgvSchedulingTask";
            string url = _rcsHost + path;
            return await _httpHelperAsync.RequestPostAsync<RCSArgsAGVTaskInput, RCSArgsOutput<string>>(taskArgs, url);
        }

        /// <summary>
        /// 创建AGV任务
        /// "type": 位置类型00
        ///"taskTyp": 任务模板 LMR
        ///podCode：货架编码
        ///ctnrTyp: 容器类型，CTU为3
        ///reqCode：请求编码（GUID）
        ///taskCode：任务编码（前缀+GUID）
        /// </summary>
        /// <param name="args">code=0表示执行成功</param>
        /// <returns></returns>
        public RCSArgsOutput<string> ScheduleAgvTask(string reqCode, string taskType,string ctnrTyp, string positionType, string taskCode, string startPositionCode, string endPositionCode)
        {
            var taskArgs = new RCSArgsAGVTaskInput
            {
                reqCode = reqCode,
                taskTyp = taskType,
                taskCode = taskCode,
                ctnrTyp= ctnrTyp,
                positionCodePath = new RCS_AGV_Position[]
                 {
                        new RCS_AGV_Position
                        {
                            type=positionType,
                                positionCode=startPositionCode
                        },
                        new RCS_AGV_Position
                        {
                            type=positionType,
                            positionCode=endPositionCode
                        }
                 }
            };
            string path = "/rcms/services/rest/hikRpcService/genAgvSchedulingTask";
            string url = _rcsHost + path;
            return _httpHelperAsync.RequestPost<RCSArgsAGVTaskInput, RCSArgsOutput<string>>(taskArgs, url);
        }

        public RCSArgsOutput<string> ScheduleAgvTaskCollection(string reqCode, string taskType, string ctnrTyp, string taskCode, RCS_AGV_Position[] positions )
        {
            var taskArgs = new RCSArgsAGVTaskInput
            {
                reqCode = reqCode,
                taskTyp = taskType,
                taskCode = taskCode,
                ctnrTyp = ctnrTyp,
                positionCodePath = positions
            };
            string path = "/rcms/services/rest/hikRpcService/genAgvSchedulingTask";
            string url = _rcsHost + path;
            return _httpHelperAsync.RequestPost<RCSArgsAGVTaskInput, RCSArgsOutput<string>>(taskArgs, url);
        }

        /// <summary>
        /// code=0表示执行成功
        /// </summary>
        /// <param name="taskCode">任务编号</param>
        /// <returns></returns>
        public async Task CancelAgvTask(string taskCode)
        {
            var agvInputArgs = new RCSArgsAGVCancelInput
            {
                reqCode = Guid.NewGuid().ToString("N").ToUpper(),
                taskCode = taskCode
            };
            string path = "/rcms/services/rest/hikRpcService/cancelTask";
            string url = _rcsHost + path;
            await _httpHelperAsync.RequestPostAsync<RCSArgsAGVCancelInput, RCSArgsOutput<string>>(agvInputArgs, url);
        }

        /// <summary>
        /// AGV（CTU）取放申请
        /// </summary>
        /// <param name="takCode"></param>
        /// <param name="type">1-取申请通过，2-放申请通过</param>
        /// <returns></returns>
        public RCSArgsOutput<string> AgvAppllyPass(string takCode,int type)
        {
            var inputArgs = new RCSArgsAGVApplyPassInput
            {
                reqCode = Guid.NewGuid().ToString("N").ToUpper(),
                taskCode =takCode,
                type=type.ToString()
            };
            string path = "/rcms/services/rest/hikRpcService/boxApplyPass";
            string url = _rcsHost + path;
            return _httpHelperAsync.RequestPost<RCSArgsAGVApplyPassInput, RCSArgsOutput<string>>(inputArgs, url); 
        }

        /// <summary>
        /// rcs回调
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public RCSArgsOutput<string> AgvCallback(RCSCallbackArgs args)
        {
            if (OnTaskResult != null)
            {
                Task.Run(() =>
                {
                    OnTaskResult(this, args);
                });
            }
            return new RCSArgsOutput<string>
            {
                code = "0",
                reqCode = args.reqCode
            };
        }

        /// <summary>
        /// 查询任务状态
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RCSArgsOutput<RCSOutputData_TaskStatus>> QueryAgvTaskStatus(RCSArgsInput args)
        {
            string path = "/rcms/services/rest/hikRpcService/queryTaskStatus";
            string url = _rcsHost + path;
            var res = await _httpHelperAsync.RequestPostAsync<RCSArgsInput, RCSArgsOutput<RCSOutputData_TaskStatus>>(args, url);
            return res;
        }

        /// <summary>
        /// 查询AGV状态
        /// 地图编码:AA
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RCSArgsOutput<RCSOutputData_AgvStatus[]>> QueryAgvStatus(RCSArgsAGVStatusQueryInput args)
        {
            string path = "/rcms-dps/rest/queryAgvStatus";
            string url = _rcsHost + path;
            var res = await _httpHelperAsync.RequestPostAsync<RCSArgsAGVStatusQueryInput, RCSArgsOutput<RCSOutputData_AgvStatus[]>>(args, url);
            return res;
        }

        /// <summary>
        /// 停止AGV
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RCSArgsOutput<string>> StopAgv(RCSArgsAGVStopInput args)
        {
            string path = "/rcms/services/rest/hikRpcService/stopRobot";
            string url = _rcsHost + path;
            var res = await _httpHelperAsync.RequestPostAsync<RCSArgsAGVStopInput, RCSArgsOutput<string>>(args, url);
            return res;
        }

        /// <summary>
        /// 恢复AGV
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RCSArgsOutput<string>> resumeAgv(RCSArgsAGVStopInput args)
        {
            string path = "/rcms/services/rest/hikRpcService/resumeRobot";
            string url = _rcsHost + path;
            var res = await _httpHelperAsync.RequestPostAsync<RCSArgsAGVStopInput, RCSArgsOutput<string>>(args, url);
            return res;
        }

        /// <summary>
        /// 预调度任务
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<RCSArgsOutput<string>> PerScheduleAgvTask(RCSArgsAGVPerTaskInput args)
        {
            string path = "/rcms/services/rest/hikRpcService/genPreScheduleTask";
            string url = _rcsHost + path;
            var res = await _httpHelperAsync.RequestPostAsync<RCSArgsAGVPerTaskInput, RCSArgsOutput<string>>(args, url);
            return res;
        }
    }
}
