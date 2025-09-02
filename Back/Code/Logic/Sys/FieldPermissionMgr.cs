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

namespace Logic.Sys
{
    /// <summary>
    /// 表字段权限管理
    /// </summary>
  public  class FieldPermissionMgr: DbOperationHandler
    {
        public FieldPermissionMgr(Repository repository) : base(repository) { }

        /// <summary>
        /// 获取所有表及字段信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetPermissionTbFields()
        {
            var data = await Repository.ClientDb.Queryable<SysFieldsManage>()
                .Where(x => x.Remark == FielsManageRemark.PermissionField.ToString() || x.Remark == FielsManageRemark.All.ToString())
                .Where(x=>x.IsEnable).OrderBy(x=>x.Rank).ToListAsync();
            var treeRoot = data.GroupBy(d => new { d.TableName, d.TableDesc }).Select(m => new TreeModel { Id =m.Key.TableName, Label = m.Key.TableDesc}).ToList();
            treeRoot.ForEach(r =>
            {
                r.Children = data.Where(m => m.TableName == r.Id.ToString()).Select(m => new TreeModel { Id = m.FieldsManageId, Label = m.FieldDesc ,Remark=m.FieldName }).ToList();
            });
            return treeRoot;
        }

        /// <summary>
        /// 获取指定表字段信息
        /// </summary>
        /// <param name="fieldsManageId"></param>
        /// <returns></returns>
        public async Task<FieldPermissions> GetPermissioField(string fieldsManageId)
        {
            var data = await Repository.ClientDb.Queryable<SysFieldsManage>()
                .LeftJoin<SysFieldsPermissionsRole>((p, r) => p.FieldsManageId == r.FieldsManageId)
                .Where((p, r) => p.FieldsManageId == fieldsManageId)
                .Select((p, r) => new
                { 
                    TableName = p.TableName,
                    TableDesc = p.TableDesc,
                    FieldName = p.FieldName,
                    FieldDesc = p.FieldDesc,
                    DeniedRoleId = r.DeniedRoleId
                }).ToListAsync();
            var model = data.Select(d => new FieldPermissions
            {
                FieldManageId = fieldsManageId,
                TableName = d.TableName,
                TableDesc = d.TableDesc,
                FieldName = d.FieldName,
                FieldDesc = d.FieldDesc
            }).FirstOrDefault();
            model.PermissionsRoles = data.Select(d => new FieldsPermissionsRole
            {
                DeniedRoleId = d.DeniedRoleId
            }).ToList();
            return model;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles()
        {
            return await Repository.ClientDb.Queryable<SysRoles>().Select(r => new Role
            { 
                RoleId = r.RoleId,
                RoleName = r.RoleName
            }).ToListAsync(); 
        }

        /// <summary>
        /// 修改表字段权限
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SetPermissioFieldRole(FieldPermissions data)
        {
            Repository.ClientDb.Deleteable<SysFieldsPermissionsRole>(d => d.FieldsManageId == data.FieldManageId).AddQueue();
            if(data.PermissionsRoles!=null&& data.PermissionsRoles.Count > 0)
            {
                var list = new List<SysFieldsPermissionsRole>();
                data.PermissionsRoles.ForEach(r =>
                {
                    list.Add(new SysFieldsPermissionsRole { FieldsManageId = data.FieldManageId, DeniedRoleId = r.DeniedRoleId });
                });
                Repository.ClientDb.Insertable(list).AddQueue();
            }
            await Repository.ClientDb.SaveQueuesAsync(); 
        }
    }
}
