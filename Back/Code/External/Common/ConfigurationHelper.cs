using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Common
{
    /// <summary>
    /// json配置文件帮助类
    /// </summary>
   public class ConfigurationHelper
    {
        private static IConfiguration _configuration { get; set; }

        public ConfigurationHelper(string fileName,params string [] filePath)
        {
            var rootPath = Directory.GetCurrentDirectory().Replace("WebApi", "");
            var fullPath = @$"{rootPath}";
            foreach (string path in filePath)
            {
                fullPath += @$"\\{path}";
            } 
            _configuration = new ConfigurationBuilder().SetBasePath(fullPath).AddJsonFile(fileName + ".json").Build();
        } 

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///根据key 获取值
        /// </summary>
        /// <param name="key">格式:cofig:prop</param>
        /// <returns></returns>
        public  string GetSection(string key)
        {
            var value = _configuration[key];
            return value;
        }
    }
}
