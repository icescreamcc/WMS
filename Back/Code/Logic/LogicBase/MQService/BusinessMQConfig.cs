using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase.MQService
{
   public class BusinessMQConfig
    {
        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }

        public string QueueName { get; set; }

        public byte Priority { get; set; }

        public bool IsPersistent { get; set; }

        public BusinessMQConfig(string itemName,  byte priority = 1, bool isPersistent=true)
        {
            ExchangeName = "exchange_" + itemName;
            RoutingKey = "queue_" + itemName;
            QueueName = "queue_" + itemName;
            Priority = priority;
            IsPersistent = isPersistent;
        }
    }
}
