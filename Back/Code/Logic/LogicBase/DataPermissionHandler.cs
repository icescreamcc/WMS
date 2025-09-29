using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common.Extension;
using Models.Model;
using Models.Model.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase
{
    /// <summary>
    /// 处理字段与数据的查询权限
    /// </summary>
   public class DataPermissionHandler : FieldsManageHandler
    {
        public DataPermissionHandler(Repository repository) : base(repository)
        {
        }

        #region 字段权限处理方法
        /// <summary>
        /// 当前用户被拒绝访问的表字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<FieldPermission>> GetDeniedField<T>(string userId) where T : class
        {
            var tableName = typeof(T).Name;
            var res = await Repository.ClientDb.Queryable<SysFieldsManage, SysFieldsPermissionsRole>((p, r) => new object[] { JoinType.Left, p.FieldsManageId == r.FieldsManageId })
                   .Where((p, r) => p.TableName == tableName)
                   .Where((p, r) => SqlFunc.Subqueryable<SysUserRoles>().Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).Contains(r.DeniedRoleId))
                   .Select((p, r) => new FieldPermission { TableName = p.TableName, TableDesc = p.TableDesc, FieldName = p.FieldName, FieldDesc = p.FieldDesc })
                   .Distinct()
                   .ToListAsync();
            return res;
        }

        /// <summary>
        /// 当前用户允许访问的表字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<FieldPermission>> GetAllowField<T>(string userId) where T : class
        {
            var tableName = typeof(T).Name;
            var userRoles = await Repository.ClientDb.Queryable<SysUserRoles>().Where(x => x.UserId == userId).ToListAsync();
            var res = await Repository.ClientDb.Queryable<SysFieldsManage, SysFieldsPermissionsRole>((p, r) => new object[] { JoinType.Left, p.FieldsManageId == r.FieldsManageId })
                   .Where((p, r) => p.TableName == tableName)
                   .OrderBy((p, r) => p.Rank)
                   .Select((p, r) => new FieldPermission { TableName = p.TableName, TableDesc = p.TableDesc, FieldName = p.FieldName, FieldDesc = p.FieldDesc, RoleId = r.DeniedRoleId })
                   .ToListAsync();
            var data = new List<FieldPermission>();
            res.ForEach(x =>
            {
                if (!userRoles.Exists(e => e.RoleId == x.RoleId))
                {
                    data.Add(x);
                }
            });
            return data.Distinct().ToList();
        }

        /// <summary>
        /// 当前用户允许访问并支持导出的表字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="isExport"></param>
        /// <returns></returns>
        public async Task<List<FieldPermission>> GetAllowField<T>(string userId,bool isExport) where T : class
        {
            var tableName = typeof(T).Name;
            var userRoles = await Repository.ClientDb.Queryable<SysUserRoles>().Where(x => x.UserId == userId).ToListAsync();
            var res = await Repository.ClientDb.Queryable<SysFieldsManage, SysFieldsPermissionsRole>((p, r) => new object[] { JoinType.Left, p.FieldsManageId == r.FieldsManageId })
                   .Where((p, r) => p.TableName == tableName&&p.IsExport)
                   .OrderBy((p, r) => p.Rank)
                   .Select((p, r) => new FieldPermission { TableName = p.TableName, TableDesc = p.TableDesc, FieldName = p.FieldName, FieldDesc = p.FieldDesc, RoleId = r.DeniedRoleId })
                   .ToListAsync();
            var data = new List<FieldPermission>();
            res.ForEach(x =>
            {
                if (!userRoles.Exists(e => e.RoleId == x.RoleId))
                {
                    data.Add(x);
                }
            });
            return data.Distinct().ToList();
        }

        /// <summary>
        /// 将被拒绝访问的字段设置为空或默认值
        /// </summary>
        /// <typeparam name="TSource">对应数据库访问的实体类</typeparam>
        /// <typeparam name="TData">映射的业务数据类</typeparam>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <param name="action">可执行回调</param>
        /// <returns></returns>
        public async Task SetDeniedFieldValue<TSource, TData>(string userId, List<TData> data, Action<TData> action = null) where TSource : class where TData : class
        {
            var deniedFields = await GetDeniedField<TSource>(userId);
            data?.ForEach(row =>
            {
                if (deniedFields.Count > 0)
                {
                    var rowType = row.GetType();
                    rowType.GetProperties().ToList().ForEach(p =>
                    {
                        if (deniedFields.Exists(d => d.FieldName == p.Name))
                        {
                            rowType.GetProperty(p.Name).SetValueExtension(row, null);
                        }
                    });
                }
                if (action != null)
                {
                    action(row);
                }
            });
        }

        /// <summary>
        /// 将被拒绝访问的字段设置为空或默认值
        /// </summary>
        /// <typeparam name="TSource">对应数据库访问的实体类</typeparam>
        /// <typeparam name="TData">映射的业务数据类</typeparam>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SetDeniedFieldValue<TSource, TData>(string userId, TData data) where TSource : class where TData : class
        {
            var deniedFields = await GetDeniedField<TSource>(userId);
            if (deniedFields.Count > 0)
            {
                var rowType = data.GetType();
                rowType.GetProperties().ToList().ForEach(p =>
                {
                    if (deniedFields.Exists(d => d.FieldName == p.Name))
                    {
                        rowType.GetProperty(p.Name).SetValueExtension(data, null);
                    }
                });
            }
        }
        #endregion

        #region 数据权限处理方法

        /// <summary>
        /// 用迭代查询方式获取当前用户的所有下属用户
        /// 注：当角色直属公司时则为最高权限角色拥有所有角色的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUnderlingUsers(string userId)
        {
            var curUserRoles = await Repository.ClientDb.Queryable<SysUserRoles>()
                .InnerJoin<SysRoles>((ur,r)=>ur.RoleId==r.RoleId)
                .Where((ur,r) => ur.UserId == userId)
                .Select((ur, r) => new Role {RoleId=r.RoleId,ParentId=r.ParentId }).ToListAsync();
            var companyid = await Repository.ClientDb.Queryable<SysCompany>().Select(x => x.CompanyId).SingleAsync();
            var isHightAuth = curUserRoles.Exists(x => x.ParentId == companyid);
            if (isHightAuth)
            {
                return await Repository.ClientDb.Queryable<SysRoles>()
                    .InnerJoin<SysUserRoles>((r,ur)=>ur.RoleId==r.RoleId)
                    .Select((r, ur)=>ur.UserId).ToListAsync();//.Where((r, ur) => r.ParentId != companyid || ur.UserId==userId)
            }
            else
            {
                var allUnderlingUsers = new List<string> { userId };
                var curRolesId = curUserRoles.Select(r => r.RoleId);
                var curRoleUsers = await Repository.ClientDb.Queryable<SysUserRoles>().Where(w => curRolesId.Contains(w.RoleId)).Select(w => w.UserId).ToListAsync();
                allUnderlingUsers.AddRange(curRolesId);
                _getChildUsers(curRolesId, allUnderlingUsers);
                return await Task.FromResult(allUnderlingUsers);
            } 
        }

        private void _getChildUsers(IEnumerable<string> parents,List<string> allUnderlingUsers)
        {
            var userRoles = Repository.ClientDb.Queryable<SysRoles>()
                .LeftJoin<SysUserRoles>((r, ur) => ur.RoleId == r.RoleId)
                .Where((r, ur) => parents.Contains(r.ParentId)).Select((r, ur)=>new UserRole { RoleId=r.RoleId,UserId= ur.UserId }).ToList();
            if (userRoles?.Count > 0)
            {
                var users = userRoles.Where(x=>!string.IsNullOrEmpty(x.UserId)).Select(x => x.UserId).Distinct();
                var roles = userRoles.Select(x => x.RoleId).Distinct();
                allUnderlingUsers.AddRange(users);
                _getChildUsers(roles, allUnderlingUsers);
            }
            return;
        }
        #endregion
    }
}
