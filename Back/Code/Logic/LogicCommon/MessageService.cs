using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicCommon
{
    public class MessageService : DbOperationHandler
    {
        public MessageService(Repository repository) : base(repository)
        {
        }

        public async Task CreateMessage(string sender, string content, string remark, MessageType messageType, string receiver="", string link = "")
        {
            var msg = new SysMessage
            {
                Sender = sender,
                MessageType=messageType.ToString(),
                Content = content,
                Remark = remark,
                Link = link,
                Receiver = receiver,
                CreateDate = DateTime.Now 
            };
            await Repository.ClientDb.Insertable(msg).ExecuteCommandAsync();
        }
    }
}
