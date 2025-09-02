using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Log
{
   public class LogHelper
    {
        private readonly ILogger<LogHelper> _logger;

        public LogHelper(ILogger<LogHelper> logger)
        {
            _logger = logger;
        }

        public void LogError(string eventName,string message, string args, Exception ex, string stackTrace)
        {
            _logger.LogError(ex,_logFormate(eventName, args, message, stackTrace));
        }

        public void LogError(string userId, string ctrlName, string actionName, string content, string args,Exception ex, string stackTrace)
        {
            _logger.LogError(ex,_logFormate("GlobalException", userId, ctrlName, actionName, args, content, stackTrace));
        }

        public void LogInfo(string eventName, string message, string args)
        {
            _logger.LogInformation(_logFormate(eventName,message,args));
        }

        public void LogInfo(string userId, string ctrlName, string actionName, string content, string args)
        {
            _logger.LogInformation(_logFormate("GlobalRequest", userId, ctrlName, actionName, args, content));
           
        }

        public void LogDbMigration(string name, string sql, string args)
        {
            _logger.LogInformation($"【DbMigration:{name},SQL:{sql},Args={args}】");
        }

        private string _logFormate(string eventName, string userId, string ctrlName, string actionName, string args, string message,  string stackTrace)
        {
            return $"[Event:{eventName},User:{userId},Controller:{ctrlName},Action:{actionName},Args:{args},Message:{message}],StackTrace:{stackTrace}";
        }

        private string _logFormate(string eventName, string userId, string ctrlName, string actionName, string args, string message)
        {
            return $"[Event:{eventName},User:{userId},Controller:{ctrlName},Action:{actionName},Args:{args},Message:{message}]";
        }

        private string _logFormate(string eventName, string args, string message,string stackTrace)
        {
            return $"[Event:{eventName},Args:{args},Message={message}],StackTrace:{stackTrace}";
        }

        private string _logFormate(string eventName, string args, string message)
        {
            return $"[Event:{eventName},Args:{args},Message={message}]";
        }
    }
}
