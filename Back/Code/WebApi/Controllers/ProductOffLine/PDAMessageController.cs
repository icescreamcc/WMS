using Logic.ProductOffLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Sys;
using Models.Model;
using System.Threading.Tasks;
using WebApi.Filter;
using System.Collections.Generic;

namespace WebApi.Controllers.ProductOffLine
{ 
    /// <summary>
    /// PDA消息查询
    /// </summary>
    public class PDAMessageController : AuthTokenController
    {
        private readonly PDAMessageMgr  _pdaMessageMgr;

        public PDAMessageController(PDAMessageMgr pdaMessageMgr)
        {
            _pdaMessageMgr = pdaMessageMgr;
        }

        /// <summary>
        /// 消息分页查询
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<TableModel<Message>> GetMessages(int pgSize, int pgIndex,string userId)
        {
            return await _pdaMessageMgr.GetMessages(pgSize, pgIndex,userId);
        }

        /// <summary>
        /// 获取未读消息的数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<int> GetNotReadMessageCount(string userId)
        {
            return await _pdaMessageMgr.GetNotReadMessageCount(userId);
        }

        /// <summary>
        /// 将未读消息修改成已读
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Skip]
        public async Task SetRead(int[] messageId,string userId)
        {
            await _pdaMessageMgr.SetRead(messageId, userId);
        }

        /// <summary>
        /// 获取订阅消息的类型
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<KeyValueModel>> GetSubscribeMessageType(string userId)
        {
            return await _pdaMessageMgr.GetSubscribeMessageType(userId);
        }

        /// <summary>
        /// 设置订阅消息类型
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="messageTypes"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SubmitSubscribeMessageType(string[] messageTypes, string userId)
        {
            await _pdaMessageMgr.SetSubscribeMessageType(userId, messageTypes);
        }
    }
}
