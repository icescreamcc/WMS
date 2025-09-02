using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicCommon.FileStorage;
using Models.Model.Enum;
using Models.Model.Sys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Logic.Sys
{
    /// <summary>
    /// 系统参数管理
    /// </summary>
   public class SysArgsMgr : DbOperationHandler
    {

        private readonly IFileStorage _fileStorage;
        public SysArgsMgr(Repository repository, IFileStorage fileStorage) : base(repository)
        {
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// 获取系统参数列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Args>> GetArgs()
        { 
            var list = await Repository.ClientDb.Queryable<SysArgs>()
               .Where(a => a.ArgsGroup == ArgsGroup.Args.ToString()&&a.IsVisible)
               .OrderBy(a => a.Rank)
               .Select(a => new Args
               {
                   ArgsId = a.ArgsId,
                   ArgsKey = a.ArgsKey,
                   ArgsKeyName = a.ArgsKeyName,
                   ArgsKeyNameEn = a.ArgsKeyNameEn,
                   ArgsValue = a.ArgsValue,
                   ArgsGroup = a.ArgsGroup,
                   ArgsType = a.ArgsType,
                   Rank = a.Rank,
                   Remark = a.Remark,
                   IsVisible=a.IsVisible
               }).ToListAsync();
            var selectArgs = list.Where(w => w.ArgsType == ArgsType.SingleSelect.ToString()).Select(s => s.Remark).ToList();
            if (selectArgs?.Count > 0)
            {
                var options = await Repository.ClientDb.Queryable<SysArgsOptions>().Where(w => selectArgs.Contains(w.ArgsKey)).Select<ArgsOptions>().ToListAsync();
                list.ForEach(l =>
                {
                    if(l.ArgsType== ArgsType.SingleSelect.ToString())
                    {
                       l.ArgsOptions= options.Where(w=>w.ArgsKey==l.Remark).ToList();
                    }
                });
            }
            return list;
        }

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public async Task UpdateArgs(List<Args> dataList)
        {
            var modelList = new List<SysArgs>();
            foreach (var data in dataList)
            { 
                var model = new SysArgs
                {
                    ArgsId = data.ArgsId,
                    ArgsKey = data.ArgsKey,
                    ArgsKeyName = data.ArgsKeyName,
                    ArgsKeyNameEn = data.ArgsKeyNameEn,
                    ArgsValue = data.ArgsValue,
                    ArgsGroup = data.ArgsGroup,
                    ArgsType = data.ArgsType,
                    Rank = data.Rank,
                    Remark = data.Remark,
                    IsVisible= data.IsVisible
                };
                modelList.Add(model);
            }; 
            await Repository.UpdateAsync(modelList); 
        }

        /// <summary>
        /// 上传系统主页Logo
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> UploadSystemHomePageLogo(string fileName, Stream stream)
        {
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Image);
            return fileUrl;
        }

    }
}
