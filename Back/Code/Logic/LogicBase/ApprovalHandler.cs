using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common.Extension;
using External.Common;
using Models.Model.Enum;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase
{
   public class ApprovalHandler: DataPermissionHandler
    {
        public ApprovalHandler(Repository repository) :base(repository)
        { 
        }

        /// <summary>
        /// 查询指定数据类型审批流
        /// </summary>
        /// <param name="approvalDataType"></param>
        /// <returns></returns>
        public async Task<List<ApprovalProcessModel>> GetApprovalProcess(string approvalDataType)
        {
            return await Repository.ClientDb.Queryable<ApprovalProcess>()
                .InnerJoin<SysUserRoles>((a, ur) => a.ApproverRole == ur.RoleId)
                .InnerJoin<SysRoles>((a,ur,r)=>ur.RoleId==r.RoleId)
                .Select((a, ur, r) => new ApprovalProcessModel { ApproverId = ur.UserId, ApproverRole = a.ApproverRole,ApproverRoleName=r.RoleName, ApprovalModel = a.ApprovalModel, IsLastApproval = a.IsLastApproval, Rank = a.Rank })
                .Where(a => a.DataType == approvalDataType).ToListAsync();
        }

        /// <summary>
        /// 查询当前用户审批相关信息
        /// </summary>
        /// <param name="approvalDataType"></param>
        /// <returns>审核人员用户ID列表</returns>
        public async Task<List<ApprovalProcessModel>> GetApprover(string approvalDataType, string approverId)
        {
            return await Repository.ClientDb.Queryable<ApprovalProcess>()
                    .InnerJoin<SysUserRoles>((a, ur) => a.ApproverRole == ur.RoleId)
                    .InnerJoin<SysRoles>((a, ur, r) => r.RoleId == ur.RoleId)
                    .Where((a, ur, r) => a.DataType == approvalDataType && ur.UserId == approverId)
                    .Select((a, ur, r) => new ApprovalProcessModel { ApprovalModel = a.ApprovalModel, ApproverRole = a.ApproverRole, ApproverRoleName = r.RoleName, IsLastApproval = a.IsLastApproval, Rank = a.Rank, ApproverId = ur.UserId })
                    .ToListAsync();
        }

        /// <summary>
        /// 获取下一级审批人信息
        /// </summary>
        /// <param name="primaryId"></param>
        /// <param name="approvalDataType"></param>
        /// <returns></returns>
        public async Task<List<ApprovalProcessModel>> GetNextApprover(string primaryId, ApprovalDataType approvalDataType)
        {
            var process = await Repository.ClientDb.Queryable<ApprovalProcess>()
                    .InnerJoin<SysUserRoles>((a, ur) => a.ApproverRole == ur.RoleId)
                    .InnerJoin<SysRoles>((a, ur, r) => r.RoleId == ur.RoleId)
                    .InnerJoin<SysUser>((a, ur, r, u) => ur.UserId == u.UserId)
                    .Where((a, ur, r, u) => a.DataType == approvalDataType.ToString())
                    .Select((a, ur, r, u) => new ApprovalProcessModel
                    {
                        ApprovalModel = a.ApprovalModel,
                        ApproverRole = a.ApproverRole,
                        ApproverRoleName = r.RoleName,
                        IsLastApproval = a.IsLastApproval,
                        Rank = a.Rank,
                        ApproverId = ur.UserId,
                        ApproverName=u.UserName,
                        ApproverEmail=u.Email
                    }).ToListAsync();
            var approvalHis = await Repository.ClientDb.Queryable<ApprovalHis>().Where(w => w.DataType == approvalDataType.ToString() && w.PrimaryId == primaryId).OrderBy(w => w.ApprovalRank).ToListAsync();
            var firstProc = process.OrderBy(w => w.Rank).First();
            if (firstProc.ApprovalModel == ApprovalModel.Any.ToString())
            {
                if (approvalHis?.Count == 0)
                {
                    return process; 
                } 
            }
            else
            {
                if (approvalHis?.Count == 0)
                {
                    return process.Where(w => w.Rank == firstProc.Rank).ToList();
                }
                else
                {
                    var lastHis = approvalHis.OrderBy(w => w.ApprovalRank).Last();
                    if (lastHis.ApprovalStatus != ApprovalStatus.Reject.ToString())
                    {
                        return process.Where(w => w.Rank == lastHis.ApprovalRank + 1).ToList();
                    } 
                } 
            }
            return null;
        }

        /// <summary>
        /// 获取待审批数据的数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="approvalDataType"></param>
        /// <returns></returns>
        public async Task<int> GetApprovalCount(string userId, string approvalDataType)
        {
            return await Task.FromResult(0);
        }

        /// <summary>
        /// 查询审批记录
        /// </summary>
        /// <param name="primaryId"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        /// <param name="createDate"></param>
        /// <param name="approvalStatus"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<List<ApprovalHisModel>> GetApprovalHis(string primaryId,string createUserId,string createUserName,DateTime createDate, string approvalStatus,string remark, ApprovalDataType dataType)
        { 
            var createUserRole = await Repository.ClientDb.Queryable<SysUserRoles>()
                .InnerJoin<SysRoles>((ur, r) => ur.RoleId == r.RoleId)
                .Where((ur, r) => ur.UserId == createUserId)
                .Select((ur, r) => r.RoleName)
                .FirstAsync();
            var curApprovalStatus = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(approvalStatus);
            var approvalList = new List<ApprovalHisModel>
            {
                new ApprovalHisModel
                {
                    PrimaryId=primaryId,
                    ApprovalDate=createDate.ToStringExtension(),
                    ApproverName=createUserName,
                    ApproverRoleName=createUserRole,
                    ApprovalStatusDesc="提交订单",
                    Opinion=$"当前状态：{curApprovalStatus}"
                }
            };
            if (approvalStatus != ApprovalStatus.NoApproval.ToString())
            {
                var process = await Repository.ClientDb.Queryable<ApprovalProcess>()
              .LeftJoin<SysRoles>((p, r) => p.ApproverRole == r.RoleId)
              .LeftJoin<SysUserRoles>((p, r, ur) => p.ApproverRole == ur.RoleId)
              .LeftJoin<SysUser>((p, r, ur, u) => u.UserId == ur.UserId && u.IsVaild)
              .Where((p, r, ur, u) => p.DataType == dataType.ToString())
              .OrderBy((p, r, ur, u) => p.Rank)
              .Select((p, r, ur, u) => new
              {
                  p.ApproverRole,
                  r.RoleName,
                  u.UserId,
                  u.UserName,
                  p.Rank,
                  p.IsLastApproval,
                  p.ApprovalModel
              }).ToListAsync();
                var hisQuery = await Repository.ClientDb.Queryable<ApprovalHis>().Where(w => w.DataType == dataType.ToString() && w.PrimaryId == primaryId).OrderBy(w => w.ApprovalRank).Select<ApprovalHisModel>().ToListAsync();
                var rankList = process.Select(s => s.Rank).Distinct().ToList();
                foreach (var rank in rankList)
                {
                    var curHis = hisQuery.Where(w => w.ApprovalRank == rank).ToList();
                    if (curHis?.Count > 0)
                    {
                        curHis.ForEach(f =>
                        {
                            approvalList.Add(new ApprovalHisModel
                            {
                                PrimaryId = primaryId,
                                ApprovalDate = f.ApprovalDate,
                                ApproverName = f.ApproverName,
                                ApproverRoleName = f.ApproverRoleName,
                                ApprovalStatus = f.ApprovalStatus,
                                ApprovalStatusDesc = EnumHelper.GetDescFromEnumVal<ApprovalStatus>(f.ApprovalStatus),
                                Opinion = f.Opinion
                            });
                        });
                    }
                    else
                    {
                        var curPro = process.Where(w => w.Rank == rank).ToList();
                        approvalList.Add(new ApprovalHisModel
                        {
                            PrimaryId = primaryId,
                            ApprovalDate = "",
                            ApproverName = string.Join(',', curPro.Select(s => s.UserName).Distinct()),
                            ApproverRoleName = string.Join(',', curPro.Select(s => s.RoleName).Distinct()),
                            ApprovalStatus = ApprovalStatus.Pending.ToString(),
                            ApprovalStatusDesc = EnumHelper.GetDescFromEnum(ApprovalStatus.Pending),
                            Opinion = ""
                        });
                    }
                }
            }
            return approvalList;
        }
    }
}
