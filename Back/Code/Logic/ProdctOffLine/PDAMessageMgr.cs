using DbRepository.Repository;
using Logic.LogicBase;
using Models.Model.Prod;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbRepository.Repository.DbModels;
using SqlSugar;
using AutoMapper;
using Models.Model.Sys;
using Logic.LogicCommon;
using Models.Model.Enum;
using External.Common;
using NPOI.Util;
using System.Reflection;
using System.Reflection.Metadata;

namespace Logic.ProductOffLine
{
    public class PDAMessageMgr : DbOperationHandler
    {
        private readonly IMapper _mapper; 

        public PDAMessageMgr(Repository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper; 
        }

        public async Task<TableModel<Message>> GetMessages(int pgSize, int pgIndex,string userId)
        {
            var msgSubscribe = await Repository.ClientDb.Queryable<SysMessageSubscribe>().Where(w => w.UserId == userId).Select(s=>s.MessageType).ToListAsync();
            if(msgSubscribe?.Count > 0)
            {
                int total = 0;
                var queryData = Repository.ClientDb.Queryable<SysMessage>()
                    .Where(w => msgSubscribe.Contains(w.MessageType))
                    .OrderByDescending(o => o.CreateDate)
                    .Select(s=>new Message
                    {
                        MessageId=s.MessageId,
                        MessageType=s.MessageType,
                        Sender=s.Sender,
                        Content=s.Content,
                        Remark=s.Remark,
                        CreateDate=s.CreateDate,
                        IsRead=SqlFunc.Subqueryable<SysMessageReadUser>().Where(w=>w.UserId == userId&&w.MessageId==s.MessageId).Any()
                    })
                    .ToPageList(pgIndex, pgSize, ref total);
                queryData.ForEach(f => f.MessageType = EnumHelper.GetDescFromEnumVal<MessageType>(f.MessageType));
                var res = new TableModel<Message>() { Total = total, Rows = queryData };
                return await Task.FromResult(res);
            }
            else
            {
                return new TableModel<Message>() { Total = 0, Rows = new List<Message>() }; 
            }
        }

        public async Task<int> GetNotReadMessageCount(string userId)
        {
            var msgSubscribe = await Repository.ClientDb.Queryable<SysMessageSubscribe>().Where(w => w.UserId == userId).Select(s => s.MessageType).ToListAsync();
            if (msgSubscribe?.Count > 0)
            {
                return await Repository.ClientDb.Queryable<SysMessage>()
                    .CountAsync(w =>  msgSubscribe.Contains(w.MessageType)&& SqlFunc.Subqueryable<SysMessageReadUser>().Where(sw => sw.UserId == userId && sw.MessageId == w.MessageId).NotAny());
            }
            return 0;
        }

        public async Task SetRead(int[] messageId,string userId)
        { 
            Repository.ClientDb.Deleteable<SysMessageReadUser>(w => w.UserId == userId && messageId.Contains(w.MessageId)).AddQueue();
            var addData = new List<SysMessageReadUser>();
            foreach (var item in messageId)
            {
                addData.Add(new SysMessageReadUser { UserId = userId, MessageId = item });
            } 
            Repository.ClientDb.Insertable(addData).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }

        public async Task<List<KeyValueModel>> GetSubscribeMessageType(string userId)
        {
            var msgSubscribe = await Repository.ClientDb.Queryable<SysMessageSubscribe>().Where(w => w.UserId == userId).Select(s => s.MessageType).ToListAsync();
            var types =new List<string> { MessageType.InStorage.ToString(), MessageType.OutStorage.ToString(), MessageType.AllocationStorage.ToString() };
            var allMsg = EnumHelper.GetEnumValNames<MessageType>().Where(w => types.Contains(w.Key)).ToList(); 
            allMsg.ForEach(all =>
            {
                if (msgSubscribe.Exists(e => e == all.Key.ToString()))
                {
                    all.Remark = true;
                } 
            }); 
            return allMsg;
        }

        public async Task SetSubscribeMessageType(string userId,string[] messageTypes)
        {
            Repository.ClientDb.Deleteable<SysMessageSubscribe>(d => d.UserId == userId).AddQueue();
            var entitys = new List<SysMessageSubscribe>();
            foreach(var type in messageTypes)
            {
                entitys.Add(new SysMessageSubscribe { UserId = userId, MessageType = type });
            }
            Repository.ClientDb.Insertable(entitys).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
         
    }
}
