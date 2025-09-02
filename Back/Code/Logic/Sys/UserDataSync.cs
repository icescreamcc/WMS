using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common; 
using External.Log;
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Microsoft.Extensions.Configuration;
using Models.Model.Sys; 
using Newtonsoft.Json; 
using System.Collections.Generic; 
using System.Linq; 

namespace Logic.Sys
{
    public class UserDataSync  : ExternalApiHandler
    {  
        private readonly string _porvider;

        private readonly string _secret;

        private readonly string _host;

        private  string _accessToken;

        private readonly IMapper _mapper;

        private readonly LogHelper _logHelper;  

        public UserDataSync(Repository repository, IConfiguration configuration, IMapper mapper, LogHelper logHelper, BusinessCacheService businessCacheService, BusinessCacheItem businessCacheItem, HttpHelperAsync httpHelperAsync) : base(repository, businessCacheService, businessCacheItem, httpHelperAsync)
        {
            var porvider = "";
            var secret = "";
            var host = "";
            var sign = configuration.GetSection("Factory:sign").Value;
            if (sign == "dl")
            {
                _porvider = configuration.GetSection("UserPlatform:Porvider").Value;
                _secret = configuration.GetSection("UserPlatform:Secret").Value;
                _host = configuration.GetSection("UserPlatform:Host").Value;
            }
            else
            if (sign == "w3")
            {
                _porvider = configuration.GetSection("UserPlatformW3:Porvider").Value;
                _secret = configuration.GetSection("UserPlatformW3:Secret").Value;
                _host = configuration.GetSection("UserPlatformW3:Host").Value;
            }

            //_porvider =configuration.GetSection("UserPlatform:Porvider").Value;
            //_secret = configuration.GetSection("UserPlatform:Secret").Value;
            //_host= configuration.GetSection("UserPlatform:Host").Value;
            _mapper = mapper;
            _logHelper = logHelper;  
        }

        public  void Sync()
        {
            _logHelper.LogInfo("UserDataSync.Sync", "开始同步用户部门数据", null); 
            if (string.IsNullOrEmpty(_accessToken))
            { 
                _accessToken =  GetAccessToken(_host, _porvider,_secret); 
            }
            if (!string.IsNullOrEmpty(_accessToken))
            { 
                var userTask =  _getUsers(); 
                if(userTask != null)
                {
                    //更新User表 
                    var localUserData = Repository.ClientDb.Queryable<SysUser>().ToList();
                    var userEntityData = _mapper.Map<List<SysUser>>(userTask);
                    if (localUserData?.Count == 0)
                    {
                        Repository.ClientDb.Insertable(userEntityData).ExecuteCommand(); 
                    }
                    else
                    {
                        var insertData = new List<SysUser>();
                        var updateData = new List<SysUser>();
                        foreach (var user in userEntityData)
                        {
                            if (!localUserData.Exists(u => u.UserId == user.UserId))
                            {
                                insertData.Add(user);
                            }
                            if (localUserData.Exists(u => u.UserId == user.UserId && u.EditDate != user.EditDate))
                            {
                                updateData.Add(user);
                            }
                        }
                        if (insertData.Count() > 0)
                        {
                            Repository.ClientDb.Insertable(insertData).ExecuteCommand();
                        }
                        if (updateData.Count() > 0)
                        {
                            Repository.ClientDb.Updateable(updateData).ExecuteCommand();
                        } 
                    }
                    _logHelper.LogInfo("UserDataSync.Sync", "用户数据同步完成", $"本地数据为{localUserData.Count},同步数据为{userEntityData.Count}");
                } 
                //更新Dept表
                var deptTask = _getDepts();
                if (deptTask != null)
                {
                    var localDeptData = Repository.ClientDb.Queryable<SysDepartments>().ToList();
                    var deptEntityData = _mapper.Map<List<SysDepartments>>(deptTask);
                    if (localDeptData.Count == 0)
                    {
                        Repository.ClientDb.Insertable(deptEntityData).ExecuteCommand(); 
                    }
                    else
                    {
                        var insertData = new List<SysDepartments>();
                        var updateData = new List<SysDepartments>();
                        foreach (var dept in deptEntityData)
                        {
                            if (!localDeptData.Exists(e => e.DeptId == dept.DeptId))
                            {
                                insertData.Add(dept);
                            }
                            if (localDeptData.Exists(e => e.DeptId == dept.DeptId && (e.DeptNo != dept.DeptNo || e.DeptName != dept.DeptName || e.Rank != dept.Rank || e.IsVaild != dept.IsVaild)))
                            {
                                updateData.Add(dept);
                            }
                        }
                        if (insertData.Count() > 0)
                        {
                            Repository.ClientDb.Insertable(insertData).ExecuteCommand();
                        }
                        if (updateData.Count() > 0)
                        {
                            Repository.ClientDb.Updateable(updateData).ExecuteCommand();
                        } 
                    }
                    _logHelper.LogInfo("UserDataSync.Sync", "部门数据同步完成", $"本地数据为{localDeptData.Count},同步数据为{deptEntityData.Count}");
                } 
            }
            else
            {
                _logHelper.LogInfo("UserDataSync.Sync", "同步数据失败，未获取到Token", null);
            }
        } 

        private List<UserDetail> _getUsers()
        {
            var url = _host + "/api/DataExternalApi/GetAllUsers";
            var header = new Dictionary<string, string>
            {
                { "authorization", _accessToken }
            };
            var resDtp = HttpHelper.RequestGet<ExternalResponseDto>(url, header);
            if (resDtp?.data != null)
            {
                var dataJson = JsonConvert.SerializeObject(resDtp.data);
                var dataList = JsonConvert.DeserializeObject<List<UserDetail>>(dataJson);
                return dataList;
            }
            return null; 
        } 

        private List<Department> _getDepts()
        {
            var url = _host + "/api/DataExternalApi/GetDepts";
            var header = new Dictionary<string, string>
            {
                { "authorization", _accessToken }
            };
            var resDtp = HttpHelper.RequestGet<ExternalResponseDto>(url, header);
            if (resDtp?.data != null)
            {
                var dataJson = JsonConvert.SerializeObject(resDtp.data);
                var dataList = JsonConvert.DeserializeObject<List<Department>>(dataJson);
                return dataList;
            }
            return null; 
        } 
    }
}
