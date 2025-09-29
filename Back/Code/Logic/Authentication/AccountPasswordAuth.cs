using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Log;
using Logic.LogicBase;
using Logic.LogicCommon;
using Logic.LogicCommon.FileStorage;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Authentication
{
    /// <summary>
    /// 普通账号密码验证
    /// </summary>
   public class AccountPasswordAuth: DbOperationHandler
    {
        private readonly SysArgsService _sysArgsHelper; 

        public AccountPasswordAuth(Repository repository, SysArgsService sysArgsHelper) :base (repository)
        {
            _sysArgsHelper = sysArgsHelper;   
        }

        public async Task<UserAuthorizationDto> Validate(string userId, string password)
        { 
            var adminAccount = await _sysArgsHelper.GetDeveloper();
            if (userId== adminAccount.Value.ToString() &&  password == EncryptionHelper.DesDecrypt(adminAccount.Remark.ToString()))
            {
                return new UserAuthorizationDto { UserId = userId, UserName = adminAccount.Key.ToString(),IsVaild=true };
            }
            password = EncryptionHelper.MD5Encrypt(password);
            return await Repository.ClientDb.Queryable<SysUser>() 
                .LeftJoin<SysDepartments>((u,d)=>u.DeptId==d.DeptId)
                .Where((u) => (u.AuthAccount == userId||u.UserCode==userId||u.DomainName==userId) && u.Password == password)
                .Select((u,d)=>new UserAuthorizationDto { UserId=u.UserId,UserName=u.UserName,NickName=u.NickName,DeptId=d.DeptId,DeptName=d.DeptName,IsVaild=u.IsVaild }).SingleAsync();
        }

        public async Task<bool> ExternalValidate(string providerName, string providerSecret, string host)
        {
            return await Repository.ClientDb.Queryable<SysExternalProvider>()
                .AnyAsync((u) => u.ProviderName == providerName && u.ProviderSecret == providerSecret && u.ProviderHost == host);
        }

    }
}
