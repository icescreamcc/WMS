using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using Logic.LogicBase;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicCommon
{
   public class SysArgsService: DbOperationHandler
    {
        public SysArgsService(Repository repository) : base(repository)
        {
        }

        /// <summary>
        /// 根据参数Key获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<KeyValueModel> GetValueByKey(string key)
        {
            return await Repository.ClientDb.Queryable<SysArgs>().Where(w=>w.ArgsKey==key).Select(s=>new KeyValueModel { Key = key, Value = s.ArgsValue, Remark = s.Remark } ).SingleAsync(); 
        }

        /// <summary>
        /// 根据参数Key获取字典选项
        /// </summary>
        /// <param name="argskey"></param>
        /// <returns></returns>
        public async Task<Args> GetDictionaryOption(string argskey)
        { 
            var data = await Repository.ClientDb.Queryable<SysArgs>()
                .LeftJoin<SysArgsOptions>((a,o)=>o.ArgsKey==a.ArgsKey)
                .Where((a,o)=>a.ArgsKey== argskey)
                .OrderBy((a, o) => a.Rank)
                .Select((a, o) => new
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
                    OptionId = o.OptionId,
                    OptionKey = o.OptionKey,
                    OptionName = o.OptionName,
                    OptionNameEn = o.OptionNameEn,
                    OptionRank = o.Rank,
                    OptionRemark = o.Remark
                }).ToListAsync();
            var model = new Args();
            if(data!=null&& data.Count > 0)
            {
                model = data.Select(a => new Args
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
                }).FirstOrDefault();
                model.ArgsOptions = data.OrderBy(p=>p.OptionId).Select(d => new ArgsOptions
                {
                    ArgsKey = d.ArgsKey,
                    OptionId = d.OptionId,
                    OptionKey = d.OptionKey,
                    OptionName = d.OptionName,
                    OptionNameEn = d.OptionNameEn,
                    Rank = d.OptionRank,
                    Remark = d.OptionRemark
                }).ToList();
            }
            return model;
        }

        /// <summary>
        /// 获取所有字典选项
        /// </summary>
        /// <param name="argsKey"></param>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetArgsOptions(string argsKey)
        {
           return await Repository.ClientDb.Queryable<SysArgsOptions>().Where(w=>w.ArgsKey== argsKey).Select(s=>new KeyValueModel
           {
               Key=s.OptionKey,
               Value=s.OptionName
           }).ToListAsync(); 
        }

        public async Task<List<KeyValueModel>> GetArgsOptionsBySubGrp(string subGroup)
        {
            return await Repository.ClientDb.Queryable<SysArgs>()
              .InnerJoin<SysArgsOptions>((a, o) => a.ArgsKey == o.ArgsKey)
              .Where((a, o) => a.ArgsSubGroup == subGroup)
              .Select((a, o) => new KeyValueModel
              {
                  Key = o.OptionKey,
                  Value = o.OptionName
              }).ToListAsync();
        }

        /// <summary>
        /// 根据字典选项Key和分组查询Name
        /// </summary>
        /// <param name="subGroup"></param>
        /// <param name="optionKey"></param>
        /// <returns></returns>
        public async Task<KeyValueModel> GetArgsOption(string subGroup,string optionKey)
        {
            return await Repository.ClientDb.Queryable<SysArgs>()
                .InnerJoin<SysArgsOptions>((a,o)=>a.ArgsKey==o.ArgsKey)
                .Where((a,o)=>a.ArgsSubGroup== subGroup && o.OptionKey==optionKey)
                .Select((a, o) => new KeyValueModel
                {
                    Key=o.OptionKey,
                    Value=o.OptionName
                }) .SingleAsync();
        }

        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<Args>> GetDictionarys()
        {
           return await Repository.ClientDb.Queryable<SysArgs>().Where(a => a.ArgsGroup == ArgsGroup.Dic.ToString())
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
                }).ToListAsync();
        }

        /// <summary>
        /// 获取超级管理员账号信息
        /// Key:超级管理员
        /// Value:root
        /// Remark:密码
        /// </summary>
        /// <returns></returns>
        public async Task<KeyValueModel> GetDeveloper()
        {
            var res = await Repository.ClientDb.Queryable<SysArgs>().SingleAsync(x => x.ArgsKey == BusinessConst.DeveloperAccount);
            return new KeyValueModel() { Key = res.ArgsKeyName, Value = res.ArgsValue, Remark = res.Remark };
        }
    }
}
