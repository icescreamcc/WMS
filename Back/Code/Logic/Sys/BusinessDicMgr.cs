using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using Logic.LogicBase;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{
    /// <summary>
    /// 企业字典管理
    /// </summary>
   public class BusinessDicMgr: DbOperationHandler
    {
        public BusinessDicMgr(Repository repository) : base(repository)
        {
        }

        /// <summary>
        /// 获取所有字典列表
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<List<Args>> GetArgs(string searchKey)
        {
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var list = new List<Args>(); 
            var data = await Repository.ClientDb.Queryable<SysArgs>()
                .LeftJoin<SysArgsOptions>((a, o) => a.ArgsKey == o.ArgsKey)
                .Where((a,o) => a.ArgsGroup == ArgsGroup.Dic.ToString())
                .Where((a,o)=>a.ArgsKey.Contains(searchKey)||a.ArgsKeyName.Contains(searchKey))
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
            var primaryData = data.GroupBy(a => a.ArgsKey).ToList();
            primaryData.ForEach(p =>
            {
                var model = data.Where(d => d.ArgsKey == p.Key)
                .GroupBy(d => new { d.ArgsId, d.ArgsKey, d.ArgsKeyName, d.ArgsKeyNameEn, d.ArgsValue, d.ArgsGroup, d.ArgsType, d.Rank, d.Remark })
                .Select(d => new Args
                {
                    ArgsId = d.Key.ArgsId,
                    ArgsKey = d.Key.ArgsKey,
                    ArgsKeyName = d.Key.ArgsKeyName,
                    ArgsKeyNameEn = d.Key.ArgsKeyNameEn,
                    ArgsValue = d.Key.ArgsValue,
                    ArgsGroup = d.Key.ArgsGroup,
                    ArgsType = d.Key.ArgsType,
                    Rank = d.Key.Rank,
                    Remark = d.Key.Remark
                }).Single();
                if (model.ArgsType == ArgsType.SingleSelect.ToString() )
                {
                    model.ArgsOptions = data.Where(d => d.ArgsKey == model.ArgsKey).OrderBy(d => d.OptionRank).Select(d => new ArgsOptions
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
                list.Add(model);
            });
            return list;
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddArgs(Args data)
        {
            var existModel = await Repository.Exist<SysArgs>(a => a.ArgsKey == data.ArgsKey);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前字典编码已存在"); 
            }
            var model = new SysArgs
            { 
                ArgsKey = data.ArgsKey,
                ArgsKeyName = data.ArgsKeyName,
                ArgsKeyNameEn = data.ArgsKeyNameEn,
                ArgsValue = data.ArgsValue,
                ArgsGroup = ArgsGroup.Dic.ToString(),
                ArgsType = data.ArgsType,
                Rank = data.Rank,
                IsVisible=true,
                Remark = data.Remark
            };
            Repository.ClientDb.Insertable(model).AddQueue();
            if (data.ArgsOptions?.Count > 0)
            {
                if(data.ArgsOptions.GroupBy(d => d.OptionKey).Count()!= data.ArgsOptions.Count)
                {
                    throw new BusinessException("保存失败,当前字典选项存在多个相同选项编码"); 
                }
                if (data.ArgsOptions.GroupBy(d => d.OptionName).Count() != data.ArgsOptions.Count)
                {
                    throw new BusinessException("保存失败,当前字典选项存在多个相同选项名称"); 
                }
                var options = data.ArgsOptions.Select(o => new SysArgsOptions
                {
                    ArgsKey = data.ArgsKey, 
                    OptionKey = o.OptionKey,
                    OptionName = o.OptionName,
                    OptionNameEn = o.OptionNameEn,
                    Rank = o.Rank,
                    Remark = o.Remark
                }).ToList();
                Repository.ClientDb.Insertable(options).AddQueue();
            }
             await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateArgs(Args data)
        {
            var existModel = await Repository.Exist<SysArgs>(a => a.ArgsId == data.ArgsId);
            if (!existModel)
            {
                throw new BusinessException("保存失败,当前字典ID不存在"); 
            }
            var model = new SysArgs
            {
                ArgsId = data.ArgsId,
                ArgsKey = data.ArgsKey,
                ArgsKeyName = data.ArgsKeyName,
                ArgsKeyNameEn = data.ArgsKeyNameEn,
                ArgsValue = data.ArgsValue,
                ArgsGroup = ArgsGroup.Dic.ToString(),
                ArgsType = data.ArgsType,
                Rank = data.Rank,
                IsVisible = true,
                Remark = data.Remark
            };
            Repository.ClientDb.Updateable(model).AddQueue();
            if (data.ArgsOptions?.Count > 0)
            {
                if (data.ArgsOptions.GroupBy(d => d.OptionKey).Count() != data.ArgsOptions.Count)
                {
                    throw new BusinessException("保存失败,当前字典选项存在多个相同选项编码"); 
                }
                if (data.ArgsOptions.GroupBy(d => d.OptionName).Count() != data.ArgsOptions.Count)
                {
                    throw new BusinessException("保存失败,当前字典选项存在多个相同选项名称"); 
                }
                var options = data.ArgsOptions.Select(o => new SysArgsOptions
                {
                    ArgsKey = data.ArgsKey,
                    OptionId = o.OptionId,
                    OptionKey = o.OptionKey,
                    OptionName = o.OptionName,
                    OptionNameEn = o.OptionNameEn,
                    Rank = o.Rank,
                    Remark = o.Remark
                }).ToList();
                Repository.ClientDb.Deleteable<SysArgsOptions>(o => o.ArgsKey == model.ArgsKey).AddQueue();
                Repository.ClientDb.Insertable(options).AddQueue();
            }
             await Repository.ClientDb.SaveQueuesAsync();
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="argsId"></param>
        /// <returns></returns>
        public async Task DelArgs(string argsKey)
        {
            Repository.ClientDb.Deleteable<SysArgs>(o => o.ArgsKey == argsKey).AddQueue();
            Repository.ClientDb.Deleteable<SysArgsOptions>(o => o.ArgsKey == argsKey).AddQueue();
             await Repository.ClientDb.SaveQueuesAsync();
        }
    }
}
