using DbRepository.Repository.DbModels;
using Logic.BaseInfo;
using Logic.LogicCommon; 
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Baseinfo
{ 
    public class ClientController : AuthTokenController
    {

        private readonly ClientMgr _clientMgr;

        private readonly SelectOptionsService _selectOptionsServer; 

        public ClientController(ClientMgr clientMgr, SelectOptionsService selectOptionsServer)
        {
            _clientMgr = clientMgr;
            _selectOptionsServer = selectOptionsServer; 
        }

        /// <summary>
        /// 获取所有客户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableModel<ClientDetail>> GetClients(string userId, int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            return await _clientMgr.GetClients(userId, pgSize,  pgIndex,  orderFiled,  ConvertOrderType(orderType),  searchKey); 
        }

        /// <summary>
        /// 获取客户表启用的闲置字段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldsManage>> GetEnableSpareFields()
        {
            return await _clientMgr.GetEnableSpareFields(typeof(BaseClients).Name); 
        }

        /// <summary>
        /// 获取客户明细
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<ClientDetail> GetClientDetail(string clientId)
        {
            return await _clientMgr.GetClientDetail(clientId); 
        }

        /// <summary>
        /// 获取客户相关选项的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<object> GetOptions()
        {
            var clientType = await _selectOptionsServer.GetDictionaryOption("ClientType");
            var clientLevel = await _selectOptionsServer.GetDictionaryOption("ClientLevel");
            var provinceOptions = await _selectOptionsServer.GetProvinces();
            return new { ClientTypeOptions =  clientType,ClientLevelOptions=clientLevel,  ProvinceOptions = provinceOptions }; 
        }
         

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加客户", LogType.Add)]
        public async Task AddClient(ClientDetail data)
        {
            await _clientMgr.AddClient(data); 
        }

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改客户", LogType.Update)]
        public async Task UpdateClient(ClientDetail data)
        {
            await _clientMgr.UpdateClient(data); 
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="clientsId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除客户", LogType.Del)]
        public async Task DelClient(string[] clientsId)
        {
            await _clientMgr.DelClient(clientsId); 
        }

        /// <summary>
        /// 导出客户
        /// </summary> 
        /// <param name="searchKey"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPut]
        [BusinessLog("导出客户", LogType.Export)]
        public async Task<string> ExportClients( string searchKey, string orderField, string orderType, List<KeyValueModel> fields)
        {
            var fileUrl = await _clientMgr.ExportClients(searchKey, orderField, ConvertOrderType(orderType), fields); 
            return fileUrl;
        }

        /// <summary>
        /// 获取当前用户导出供应商列表被允许的字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<FieldPermission>> GetAllowField(string userId)
        {
            return await _clientMgr.GetAllowField<BaseClients>(userId); 
        }
    }
}
