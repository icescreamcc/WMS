using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common.Extension;
using Logic.LogicBase;
using Models.Model;
using Models.Model.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{
   public class MessageCompanyMgr: DbOperationHandler
    {
        public MessageCompanyMgr(Repository repository) : base(repository)
        {
        }

        /// <summary>
        /// 分页查询所有企业公告
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<TableModel<MessageCompany>> GetMessages(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey,string dateStart,string dateEnd)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "DateTime" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var data = Repository.ClientDb.Queryable<SysMessageCompany>()
                  .Where(m => m.Content.Contains(searchKey)||m.Remark.Contains(searchKey))
                  .Where(m=> SqlFunc.ToDate(m.DateTime)>= GetDateStart(dateStart)&&SqlFunc.ToDate(m.DateTime)<= GetDateEnd(dateEnd))
                  .Select(m => new MessageCompany { MessageId = m.MessageId, Content = m.Content, DateTime = m.DateTime, IsPublishCurrent = m.IsPublishCurrent, Remark = m.Remark })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res= new TableModel<MessageCompany>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        /// <summary>
        /// 查询最新发布的公告明细
        /// </summary>
        /// <returns></returns>
        public async Task<MessageCompany> GetNewMessageDetail()
        {
            return await Repository.ClientDb.Queryable<SysMessageCompany>().Select(x => new MessageCompany { MessageId = x.MessageId, Content = x.Content, Remark = x.Remark, DateTime = x.DateTime }).SingleAsync(x => x.IsPublishCurrent);
        }

        /// <summary>
        /// 添加企业公告
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddMessage(MessageCompany data)
        {
            var date = DateTime.Now.ToStringExtension();
            var model = new SysMessageCompany
            {
                Content = data.Content,
                DateTime = date,
                IsPublishCurrent = data.IsPublishCurrent,
                Remark = data.Remark
            }; 
            var updateDic = new Dictionary<string, object>();
            updateDic.Add("IsPublishCurrent", false);
            Repository.ClientDb.Updateable<SysMessageCompany>(updateDic).Where(s=>s.DateTime!= date).AddQueue();
            Repository.ClientDb.Insertable(model).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 修改企业公告
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateMessage(MessageCompany data)
        {
            var existModel = await Repository.Exist<SysMessageCompany>(m=>m.MessageId== data.MessageId);
            if (!existModel)
            {
                throw new BusinessException("保存失败,当前公告ID不存在或已删除");
            }
            var date = DateTime.Now.ToStringExtension();
            var model = new SysMessageCompany
            {
                MessageId=data.MessageId,
                Content = data.Content,
                DateTime = date,
                IsPublishCurrent = data.IsPublishCurrent,
                Remark = data.Remark
            };
            var updateDic = new Dictionary<string, object>();
            updateDic.Add("IsPublishCurrent", false);
            Repository.ClientDb.Updateable<SysMessageCompany>(updateDic).Where(s => s.DateTime != date).AddQueue();
            Repository.ClientDb.Updateable(model).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync(); 
        }

        /// <summary>
        /// 删除企业公告
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public async Task DelMessage(int [] msgId)
        {
             await Repository.DeleteAsync<SysMessageCompany>(m => msgId.Contains(m.MessageId)); 
        }
    }
}
