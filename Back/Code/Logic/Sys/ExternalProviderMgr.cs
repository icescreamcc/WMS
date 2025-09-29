using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model.Sys;
using Models.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using External.Common;

namespace Logic.Sys
{
    public class ExternalProviderMgr : DbOperationHandler
    {
        private readonly IMapper _mapper;

        public ExternalProviderMgr(Repository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<TableModel<ExternalProviderDto>> GetProviders(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "ProviderName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var data = Repository.ClientDb.Queryable<SysExternalProvider>()
                  .Where(w => w.ProviderName.Contains(searchKey))
                  .Select(s => new ExternalProviderDto { ProviderName = s.ProviderName, ProviderSecretKey = s.ProviderSecretKey, ProviderSecret = s.ProviderSecret, ProviderHost = s.ProviderHost, Remark = s.Remark,IsValid=s.IsValid})
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<ExternalProviderDto>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }

        public async Task AddProvider(ExternalProviderDto input)
        {
            var exist = Repository.ClientDb.Queryable<SysExternalProvider>().Any(a => a.ProviderName == input.ProviderName);
            if (exist)
            {
                throw new BusinessException("保存失败,当前账号已存在");
            }
            var entity = _mapper.Map<SysExternalProvider>(input);
            entity.ProviderSecret = EncryptionHelper.DesEncrypt(entity.ProviderName, entity.ProviderSecretKey);
            await Repository.AddAsync(entity);
        }

        public async Task UpdateProvider(ExternalProviderDto input)
        {
            var entity = _mapper.Map<SysExternalProvider>(input);
            entity.ProviderSecret = EncryptionHelper.DesEncrypt(entity.ProviderName, entity.ProviderSecretKey);
            await Repository.UpdateAsync(entity);
        }

        public async Task DelProvider(string[] providerName)
        {
            Repository.ClientDb.Deleteable<SysExternalProvider>(u => providerName.Contains(u.ProviderName)).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
    }
}
