using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External.MQ;

namespace Logic.LogicBase.MQService
{
    public class BusinessMQService
    {
        private readonly RabbitMQClient _rabbitMQClient;

        public BusinessMQService(RabbitMQClient rabbitMQClient)
        {
            _rabbitMQClient = rabbitMQClient;
        }

        public async Task DataSend<T>(BusinessMQConfig mqConfig, T data) where T : class
        {
            _rabbitMQClient.SendMessage(mqConfig.ExchangeName, mqConfig.RoutingKey, mqConfig.QueueName, mqConfig.IsPersistent, mqConfig.Priority, data);
            await Task.CompletedTask;
        }

        public async Task DataReceived<T>(BusinessMQConfig mqConfig, Action<T> callback) where T : class
        {
            _rabbitMQClient.ReceiveMessage(mqConfig.ExchangeName, mqConfig.RoutingKey, mqConfig.QueueName, mqConfig.IsPersistent, callback);
            await Task.CompletedTask;
        }
    }
}
