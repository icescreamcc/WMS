using External.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Aliyun.OSS
{
   public class OssConfigs
    {
        public ConfigModel Config { get; }
          
        public OssConfigs(IConfiguration configuration)
        { 
            Config = new ConfigModel();
            Config.KeyID = configuration.GetSection("AliyunOss:KeyID").Value;
            Config.KeySecret = configuration.GetSection("AliyunOss:KeySecret").Value;
            Config.Encrypt = configuration.GetSection("AliyunOss:Encrypt").Value;
            Config.EndPoint = configuration.GetSection("AliyunOss:EndPoint").Value;
            Config.BucketName = configuration.GetSection("AliyunOss:BucketName").Value;
            Config.BucketUrl = configuration.GetSection("AliyunOss:BucketUrl").Value;
            Config.RootFolder = configuration.GetSection("AliyunOss:RootFolder").Value; 
        }
    }
}
