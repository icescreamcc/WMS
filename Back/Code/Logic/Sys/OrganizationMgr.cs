using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Logic.LogicCommon.FileStorage;
using Models.Model;
using Models.Model.Enum;
using Models.Model.Sys; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{
    /// <summary>
    /// 组织架构业务处理类
    /// </summary>
   public class OrganizationMgr: DataPermissionHandler
    {
        private readonly IFileStorage _fileStorage;

        private readonly BusinessCacheService _cacheService;

        private readonly BusinessCacheItem _businessCacheItem;

        public OrganizationMgr(Repository repository, IFileStorage fileStorage, BusinessCacheService cacheService, BusinessCacheItem businessCacheItem) : base(repository)
        {
            _fileStorage = fileStorage;
            _cacheService = cacheService;
            _businessCacheItem = businessCacheItem;
        }

        /// <summary>
        /// 查询公司组织架构数据并组建成树形结构模型(公司,部门,角色)
        /// </summary>
        /// <returns></returns>
        public async Task<TreeModel> GetOrganizationData()
        { 
            var companyData = await Repository.ClientDb.Queryable<SysCompany>().SingleAsync();
            var deptData = await Repository.ClientDb.Queryable<SysDepartments>().ToListAsync();
            var roleData= await Repository.ClientDb.Queryable<SysRoles>().ToListAsync();
            //公司作为根节点
            var treeRoot = new TreeModel();
            treeRoot.Id = companyData.CompanyId;
            treeRoot.Label = companyData.CompanyName;
            treeRoot.Remark = companyData.CompanyNo;
            treeRoot.Type = OrganizationType.Company.ToString();
            //部门和特殊角色作为2级节点
            treeRoot.Children = deptData.Select(d => new TreeModel { Id = d.DeptId, Label = d.DeptName, Remark = d.DeptNo,Type= OrganizationType.Dept.ToString(), Rank =d.Rank }).ToList();
            var compRoles = roleData.Where(r => r.ParentId == companyData.CompanyId).Select(r => new TreeModel { Id = r.RoleId, Label = r.RoleName, Type= OrganizationType.Role.ToString(), Remark = r.RoleNo,Rank=r.Rank }).ToList();
            if (compRoles != null)
            {
                treeRoot.Children.AddRange(compRoles);
                treeRoot.Children = treeRoot.Children.OrderBy(c => c.Rank).ToList();
            }
            //一般角色作为3级以下节点 
            treeRoot.Children.ForEach(c =>
            {
                _getChildren(c, roleData);
            });
            return treeRoot;
        }

        /// <summary>
        /// 无限迭代构建子级角色
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="data"></param>
        private void _getChildren(TreeModel parent,List<SysRoles> data)
        {
            parent.Children = new List<TreeModel>();
            foreach (var r in data)
            {
                if (r.ParentId == parent.Id.ToString())
                {
                    var child = new TreeModel { Id = r.RoleId, Label = r.RoleName, Remark = r.RoleNo,Type= OrganizationType.Role.ToString(), Rank = r.Rank };
                    parent.Children.Add(child);
                    _getChildren(child, data);
                }
            }
        }

        /// <summary>
        /// 根据组织架构类型查询用户
        /// </summary>
        /// <param name="searchId"></param>
        /// <param name="organizationType"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsersByOrganizationType(string searchId,string organizationType)
        {
            List<User> data;
            if (organizationType== OrganizationType.Company.ToString())
            {
                data=await Repository.ClientDb.Queryable<SysUser>().Where(u => u.IsVaild).Select(u => new User
                {
                    UserId = u.UserId,
                    NickName = u.NickName,
                    Email = u.Email,
                    MobilePhone = u.MobilePhone,
                    Phone = u.Phone,
                    UserName = u.UserName,
                    Wechat = u.Wechat
                }).ToListAsync();
            }
            else if(organizationType == OrganizationType.Dept.ToString())
            {
                data = await Repository.ClientDb.Queryable<SysUser>()
                     .InnerJoin<SysDepartments>((u, d) => u.DeptId == d.DeptId)
                    .Where((u, d) => d.DeptId == searchId && u.IsVaild)
                    //.InnerJoin<SysUserRoles>((u, ur) => u.UserId == ur.UserId)
                    //.InnerJoin<SysRoles>((u, ur, r) => ur.RoleId == r.RoleId)
                    //.Where((u, ur, r) => r.DeptId == searchId && u.IsVaild)
                    .Select(u => new User
                    {
                        UserId = u.UserId,
                        NickName = u.NickName,
                        Email = u.Email,
                        MobilePhone = u.MobilePhone,
                        Phone = u.Phone,
                        UserName = u.UserName,
                        Wechat = u.Wechat
                    }).Distinct().ToListAsync();
            }
            else
            {
                data = await Repository.ClientDb.Queryable<SysUser>()
                    .InnerJoin<SysUserRoles>((u, ur) => u.UserId == ur.UserId)
                    .Where((u, ur) => u.IsVaild && ur.RoleId == searchId).Select(u => new User
                    {
                        UserId = u.UserId,
                        NickName = u.NickName,
                        Email = u.Email,
                        MobilePhone = u.MobilePhone,
                        Phone = u.Phone,
                        UserName = u.UserName,
                        Wechat = u.Wechat
                    }).ToListAsync();
            }
            return data;
        }

        #region 部门增删改
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> AddDept(Department data)
        {
            var existModel = await Repository.Exist<SysDepartments>(d => d.DeptName == data.DeptName || d.DeptNo == data.DeptNo);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前部门名称或编号已存在"); 
            }
            var model = new SysDepartments
            {
                DeptId = GetPrimaryId(),
                DeptNo = data.DeptNo,
                DeptName = data.DeptName,
                CompanyId = data.CompanyId,
                IsVaild = true,
                Rank = data.Rank
            };
            var res = await Repository.AddAsync(model);
            return model.DeptId;
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateDept(Department data)
        {
            var existModel = await Repository.Exist<SysDepartments>(d => d.DeptId == data.DeptId);
            if (!existModel)
            {
                throw new BusinessException("保存失败,当前修改部门不存在"); 
            }
            var model = new SysDepartments
            {
                DeptId = data.DeptId,
                DeptNo = data.DeptNo,
                DeptName = data.DeptName,
                CompanyId = data.CompanyId,
                IsVaild = true,
                Rank = data.Rank
            };
             await Repository.UpdateAsync(model);  
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public async Task DelDept(string deptId)
        {
            var roles = await Repository.ClientDb.Queryable<SysRoles>().Where(r => r.DeptId == deptId).ToListAsync();
            if(roles!=null&& roles.Count > 0)
            {
                var rolesId = roles.Select(r => r.RoleId).ToList();
                Repository.ClientDb.Deleteable(roles).AddQueue();
                Repository.ClientDb.Deleteable<SysUserRoles>(ur => rolesId.Contains(ur.RoleId)).AddQueue();
            }
            Repository.ClientDb.Deleteable<SysDepartments>(d => d.DeptId == deptId).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
        }
        #endregion

        #region 角色增删改
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> AddRole(Role data)
        {
            var existModel = await Repository.Exist<SysRoles>(d => d.RoleNo == data.RoleNo || d.RoleName == data.RoleName);
            if (existModel)
            {
                throw new BusinessException("保存失败,当前角色名称或编号已存在"); 
            }
            
            var model = new SysRoles
            {
                RoleId = GetPrimaryId(),
                RoleName = data.RoleName,
                RoleNo = data.RoleNo,
                ParentId = data.ParentId,
                DeptId= await _findDept(data.ParentId,data.ParentType),
                IsVaild = true,
                Rank = data.Rank
            };
            await Repository.AddAsync(model);
            return model.RoleId;
        }

        /// <summary>
        /// 查找当前角色所属部门,如果上级属于角色,则上级角色所属部门就属于当前角色的部门,如果上级角色未归属部门,则当前角色和上级角色都归属于公司管辖
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        private async Task<string> _findDept(string parentId,string parentType)
        {
            string deptId = null;
            if (parentType == OrganizationType.Role.ToString())
            {
                var parentRole = await Repository.GetSingeAsync<SysRoles>(parentId);
                if (!string.IsNullOrEmpty(parentRole.DeptId))
                {
                    deptId =parentRole.DeptId;
                }
            }
            else if (parentType == OrganizationType.Dept.ToString())
            { 
                deptId = parentId;
            }
            return deptId;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateRole(Role data)
        {
            var existModel = await Repository.Exist<SysRoles>(d => d.RoleId==data.RoleId);
            if (!existModel)
            {
                throw new BusinessException("保存失败,当前角色名不存在"); 
            }
            var model = new SysRoles
            {
                RoleId = data.RoleId,
                RoleName = data.RoleName,
                RoleNo = data.RoleNo,
                ParentId = data.ParentId,
                DeptId = await _findDept(data.ParentId, data.ParentType),
                IsVaild = true,
                Rank = data.Rank
            };
             await Repository.UpdateAsync(model);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DelRole(string roleId)
        { 
            if(roleId== BusinessConst.RoleAdmin)
            {
                throw new BusinessException("删除失败,管理员角色不允许被删除"); 
            }
            Repository.ClientDb.Deleteable<SysRoles>(r=>r.RoleId== roleId||r.ParentId==roleId).AddQueue();
            Repository.ClientDb.Deleteable<SysUserRoles>(r => r.RoleId == roleId).AddQueue();
            await Repository.ClientDb.SaveQueuesAsync();
            _cacheService.RemoveByKeys(_businessCacheItem.UserPermissionKey);
            _cacheService.RemoveByKeys(_businessCacheItem.UserMenusKey);
        }
        #endregion

        #region 企业信息编辑

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <returns></returns>
        public async Task<Company> GetCompanyInfo()
        {
            return await Repository.ClientDb.Queryable<SysCompany>().Select<Company>().SingleAsync();
        }

        /// <summary>
        /// 修改企业信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateCompany(Company data)
        {
            var model = new SysCompany
            {
                CompanyId=data.CompanyId,
                CompanyNo=data.CompanyNo,
                CompanyName=data.CompanyName,
                 Email=data.Email,
                 Phone=data.Phone,
                 Url=data.Url,
                 Logo=data.Logo,
                 Remark=data.Remark
            };
            await Repository.UpdateAsync(model); 
        }

        /// <summary>
        /// 上传企业logo
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> UploadCompanyLogo(string fileName, Stream stream)
        {
            var fileUrl = await _fileStorage.SaveFile(fileName, stream, FileType.Image);
            return fileUrl;
        }

        #endregion
    }
}
