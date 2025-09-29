using External.Common;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MQ
{
    public class RabbitMQClient : IDisposable
    {
        private readonly IConnection _connection;

        private readonly string _expiration;

        public RabbitMQClient( IConfiguration configuration)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetSection("RabbitMQ:Host").Value,
                UserName = EncryptionHelper.DesDecrypt(configuration.GetSection("RabbitMQ:UserName").Value),
                Password = EncryptionHelper.DesDecrypt(configuration.GetSection("RabbitMQ:Password").Value)
            };
            _expiration = configuration.GetSection("RabbitMQ:Expiration").Value;
            _connection = factory.CreateConnection();
        }

        public IModel CreateModel()
        {
            return _connection.CreateModel();
        }

       

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="isPersistent">是否持久化到磁盘</param>
        /// <param name="priority">优先级0~9</param>
        /// <param name="message">消息体</param>
        public void SendMessage<T>(string exchangeName, string routingKey, string queueName, bool isPersistent, byte priority, T data) where T:class
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, isPersistent);
                channel.QueueDeclare(queueName, isPersistent, false, false, null);
                string json = JsonSerializer.Serialize(data);
                var body = Encoding.UTF8.GetBytes(json); 
                var properties = channel.CreateBasicProperties();
                properties.Persistent = isPersistent; // 设置消息持久性
                properties.Priority = priority; // 设置消息优先级0~9
                properties.Expiration = _expiration; // 设置消息过期时间（1小时）
                channel.BasicPublish(exchangeName, routingKey, false, properties, body);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="exchangeName">绑定的交换机名称</param>
        /// <param name="routingKey">绑定的路由key</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="durable">是否持久化</param>
        /// <param name="callback">接收到消息后回调处理函数</param>
        public void ReceiveMessage<T>(string exchangeName, string routingKey, string queueName, bool durable, Action<T> callback) where T : class
        {
            using(var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queueName, durable, false, false, null);
                //channel.QueueBind(queueName, exchangeName, routingKey);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var data = JsonSerializer.Deserialize<T>(message);
                    // 处理接收到的消息
                    if (callback != null)
                    {
                        callback(data);
                    } 
                    //channel.BasicAck(ea.DeliveryTag, true); // 确认消息已被消费 
                };

                channel.BasicConsume(queueName, true, consumer);
            } 
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
