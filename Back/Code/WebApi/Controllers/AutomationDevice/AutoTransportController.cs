using Logic.AutomationDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.AutomationDevice;
using Models.Model.Enum;
using Models.Model.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.AutomationDevice
{ 
    public class AutoTransportController : AuthTokenController
    {
        private readonly AutoTransportHandler _autoTransportHandler;

        private readonly AutoTransportInfrastructureMgr _autoTransportInfrastructureMgr;

        public AutoTransportController(AutoTransportHandler autoTransportHandler, AutoTransportInfrastructureMgr autoTransportInfrastructureMgr)
        { 
            _autoTransportHandler = autoTransportHandler;
            _autoTransportInfrastructureMgr = autoTransportInfrastructureMgr;
        }

        [HttpPost]
        [Skip]
        public void ConnectAllPLC(List<AutoProdDeviceDto> plcDeviceArr)
        { 
            _autoTransportHandler.ConnectAllPLC(plcDeviceArr);
        }

        [HttpPost]
        [Skip]
        public void CloseAllPLC(List<AutoProdDeviceDto> plcDeviceArr)
        {
            _autoTransportHandler.CloseAllPLC(plcDeviceArr);
        }

        [HttpGet]
        [Skip]
        public int GetDeviceState(string deviceNo)
        {
          return  _autoTransportHandler.GetDeviceState(deviceNo);
        }

        [HttpGet]
        [Skip]
        public void SendDeviceRollOut(string deviceNo)
        {
            _autoTransportHandler.SendDeviceRollOut(deviceNo);
        }

        [HttpGet]
        [Skip]
        public void SendDeviceRollIn(string deviceNo)
        {
            _autoTransportHandler.SendDeviceRollIn(deviceNo);
        }

        [HttpGet]
        [Skip]
        public bool IsAllowAutoTransport()
        {
            string clientIP = HttpContext.Connection.RemoteIpAddress.ToString();
            return _autoTransportInfrastructureMgr.IsAllowAutoTransport(clientIP);
        }

        [HttpGet]
        [Skip]
        public async Task<List<RequisitionGoodsDetail>> GetHisTask(string orderNo)
        {
            return await _autoTransportInfrastructureMgr.GetHisTask(orderNo);
        }

        [HttpGet]
        [Skip]
        public object GetAutoTransportInfrastructureInfo(string goodsClassifyGroup)
        {
           return _autoTransportInfrastructureMgr.GetAutoTransportInfrastructureInfo(goodsClassifyGroup);
        }

        [HttpGet]
        [Skip]
        public void ExecuteTransTask(int taskId)
        {
            _autoTransportHandler.ExecuteTransTask(taskId);
        }

        [HttpGet]
        [Skip]
        public List<AutoProdTaskTrackingDto> GetPendingExecTransTask(string goodsClassifyGroup,string userId)
        {
            return _autoTransportHandler.GetPendingExecTransTask(goodsClassifyGroup, userId);
        }
         
    }
}
