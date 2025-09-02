using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using External.Common.Extension;
using Logic.LogicBase;
using Models.Model.Enum;
using Models.Model.Sys;
using System; 
using System.Threading.Tasks;

namespace Logic.LogicCommon
{
    /// <summary>
    /// 记录业务日志
    /// </summary>
   public class BusinessLogService: DbOperationHandler
    {
        public BusinessLogService(Repository repository) :base(repository)
        {
        }

        public async Task LogWrite(BusinessLog log)
        {
            var model = new SysLogs
            {
                UserId = log.UserId,
                Title=log.Title,
                Message = log.Message,
                LogType = log.LogType,
                Remark = log.Remark,
                DateTime = DateTime.Now.ToStringExtension()
            };
            await Repository.AddAsync(model); 
        }

        public async Task LogWrite(string userId, string userName, string title, LogType logType, string argsKey,string moduleName)
        {
            var model = new SysLogs
            {
                UserId = userId,
                Title = title,
                Message = $"{userName}执行了“{title}”操作",
                LogType = logType.ToString(),
                Remark = argsKey,
                DateTime = DateTime.Now.ToStringExtension(),
                ModuleName = moduleName
            };
            await Repository.AddAsync(model);
        }

        public async Task ExportLogWrite(string userId,string userName,string title, string fileName, string remark=null)
        {
            var model = new SysLogs
            {
                UserId = userId,
                Title= title,
                Message = $"{userName}导出文件:{fileName}",
                LogType = LogType.Export.ToString(),
                Remark = remark,
                DateTime = DateTime.Now.ToStringExtension()
            };
            await Repository.AddAsync(model); 
        }

        public async Task ImportLogWrite(string userId, string userName, string title, string fileName, string remark=null)
        {
            var model = new SysLogs
            {
                UserId = userId,
                Title = title,
                Message = $"{userName}导入文件:{fileName}",
                LogType = LogType.Import.ToString(),
                Remark = remark,
                DateTime = DateTime.Now.ToStringExtension()
            };
            await Repository.AddAsync(model); 
        }

        public async Task AddLogWrite(string userId, string userName, string title, string argsKey)
        {
            var model = new SysLogs
            {
                UserId = userId,
                Title = title,
                Message = $"{userName}执行了{title}操作",
                LogType = LogType.Add.ToString(),
                Remark = argsKey,
                DateTime = DateTime.Now.ToStringExtension()
            }; 
            await Repository.AddAsync(model);
        }

        public async Task UpdateLogWrite(string userId, string userName, string title, string argsKey)
        {
            var model = new SysLogs
            {
                UserId = userId,
                Title = title,
                Message = $"{userName}执行了{title}操作",
                LogType = LogType.Update.ToString(),
                Remark = argsKey,
                DateTime = DateTime.Now.ToStringExtension()
            };
            await Repository.AddAsync(model);
        }

        public async Task DelLogWrite(string userId, string userName, string title, string argsKey)
        {
            var model = new SysLogs
            {
                UserId = userId,
                Title = title,
                Message = $"{userName}执行了{title}操作",
                LogType = LogType.Del.ToString(),
                Remark = argsKey,
                DateTime = DateTime.Now.ToStringExtension()
            };
            await Repository.AddAsync(model);
        }
    }
}
