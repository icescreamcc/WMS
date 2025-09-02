using DbRepository.Repository;
using DbRepository.Repository.DbModels; 
using External.Common;
using Logic.LogicBase;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{
   public class ApprovalMgr: DbOperationHandler
    {
        public ApprovalMgr(Repository repository) : base(repository)
        { 
        }

        /// <summary>
        /// 获取审批项目列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetApprovalSubject()
        {
            var data = EnumHelper.GetEnumValNames<ApprovalDataType>().Select(x=>new TreeModel {Id=x.Key,Label=x.Value.ToString() }).ToList();
            return await Task.FromResult(data);
        }

        /// <summary>
        /// 获取审批模式
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValueModel>> GetApprovalModels()
        {
            var data = EnumHelper.GetEnumValNames<ApprovalModel>().Select(x => new KeyValueModel { Key = x.Key, Value = x.Value}).ToList();
            return await Task.FromResult(data);
        }

        /// <summary>
        /// 根据审批数据类型获取审批流程
        /// </summary>
        /// <param name="approvalDataType"></param>
        /// <returns></returns>
        public async Task<List<ApprovalProcessModel>> GetProcess(string approvalDataType)
        {
            return await Repository.ClientDb.Queryable<ApprovalProcess>()
                .InnerJoin<SysRoles>((a,r)=>r.RoleId==a.ApproverRole)
                .Where((a, r) => a.DataType == approvalDataType)
                .Select((a, r) => new ApprovalProcessModel { ApproverRole= a.ApproverRole, ApproverRoleName=r.RoleName, DataType=a.DataType, IsLastApproval=a.IsLastApproval, ProcessId=a.ProcessId, Rank=a.Rank, ApprovalModel=a.ApprovalModel}).ToListAsync();
        }

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles()
        {
            return await Repository.ClientDb.Queryable<SysRoles>().Select(r => new Role { RoleId = r.RoleId, RoleNo = r.RoleNo, RoleName = r.RoleName }).ToListAsync();
        }
         
        /// <summary>
        /// 保存审批设置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateProcess(List<ApprovalProcessModel> data)
        {
            var type = data.Select(a => a.DataType).First(); 
            var models = data.Select(a => new ApprovalProcess { ApproverRole = a.ApproverRole, DataType = a.DataType, IsLastApproval = a.IsLastApproval, Rank = a.Rank, ApprovalModel=a.ApprovalModel }).ToList();
            Repository.ClientDb.Deleteable<ApprovalProcess>(a => type==a.DataType).AddQueue();
            Repository.ClientDb.Insertable(models).AddQueue();
             await Repository.ClientDb.SaveQueuesAsync();
        }
    }
}
