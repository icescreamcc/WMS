using External.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Aliyun.SMS
{
   public class ConfigEnvironment
    {
        public ConfigModel Config { get; }

        public ConfigEnvironment(IConfiguration configuration)
        { 
            Config = new ConfigModel();
            Config.KeyID = configuration.GetSection("AliyunSms:KeyID").Value;
            Config.KeySecret = configuration.GetSection("AliyunSms:KeySecret").Value;
            Config.Encrypt = configuration.GetSection("AliyunSms:Encrypt").Value;
            Config.SignName = configuration.GetSection("AliyunSms:SignName").Value; 
        }
    }
}
