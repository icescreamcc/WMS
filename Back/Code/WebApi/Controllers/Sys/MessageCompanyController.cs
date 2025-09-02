using Logic.Sys; 
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Sys; 
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Sys
{ 
    public class MessageCompanyController : AuthTokenController
    {
        private readonly MessageCompanyMgr _messageCompanyMgr; 

        public MessageCompanyController(MessageCompanyMgr messageCompanyMgr)
        {
            _messageCompanyMgr = messageCompanyMgr; 
        }

        /// <summary>
        /// 获取系统参数列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableModel<MessageCompany>> GetMessages(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd)
        {
            return await _messageCompanyMgr.GetMessages( pgSize,  pgIndex,  orderFiled, ConvertOrderType(orderType,false),  searchKey, dateStart, dateEnd); 
        }

        /// <summary>
        /// 查询最新发布的公告明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<MessageCompany> GetNewMessageDetail()
        {
            return await _messageCompanyMgr.GetNewMessageDetail(); 
        }

        /// <summary>
        /// 添加(发布)公告
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加(发布)企业公告", Models.Model.Enum.LogType.Add)]
        public async Task AddMessage(MessageCompany data)
        {
             await _messageCompanyMgr.AddMessage(data); 
        }

        /// <summary>
        /// 修改公告
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改(发布)企业公告", Models.Model.Enum.LogType.Update)]
        public async Task UpdateMessage(MessageCompany data)
        {
             await _messageCompanyMgr.UpdateMessage(data); 
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("删除企业公告", Models.Model.Enum.LogType.Del)]
        public async Task DelMessage(int[] msgId)
        {
             await _messageCompanyMgr.DelMessage(msgId); 
        }
    }
}
