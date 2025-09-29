using Logic.LogicBase; 
using Logic.Sys; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model; 
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter; 

namespace WebApi.Controllers.Sys
{
    public class OrganizationController : AuthTokenController
    {
        private readonly OrganizationMgr _organizetionMgr;

        private const string _moduleName = "组织架构管理";

        public OrganizationController(OrganizationMgr organizetionMgr)
        {
            _organizetionMgr = organizetionMgr; 
        }

        /// <summary>
        /// 查询公司组织架构数据并组建成树形结构模型
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("查看组织架构信息", Models.Model.Enum.LogType.Read, _moduleName)]
        public async Task<TreeModel> GetOrganizationData()
        {
            return await _organizetionMgr.GetOrganizationData(); 
        }

        /// <summary>
        /// 根据组织架构类型查询用户
        /// </summary>
        /// <param name="searchId"></param>
        /// <param name="organizationType"></param>
        /// <returns></returns>
        [HttpGet]
        [Skip]
        public async Task<List<User>> GetUsersByOrganizationType(string searchId, string organizationType)
        {
            return await _organizetionMgr.GetUsersByOrganizationType(searchId, organizationType);  
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加部门", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task<string> AddDept(Department data)
        {
            var res = await _organizetionMgr.AddDept(data); 
            return res;
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改部门", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateDept(Department data)
        {
             await _organizetionMgr.UpdateDept(data); 
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除部门", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelDept(string deptId)
        {
            await _organizetionMgr.DelDept(deptId); 
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("添加角色", Models.Model.Enum.LogType.Add, _moduleName)]
        public async Task<string> AddRole(Role data)
        {
            var res = await _organizetionMgr.AddRole(data); 
            return res;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改角色", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateRole(Role data)
        {
             await _organizetionMgr.UpdateRole(data); 
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [BusinessLog("删除角色", Models.Model.Enum.LogType.Del, _moduleName)]
        public async Task DelRole(string roleId)
        {
            await _organizetionMgr.DelRole(roleId); 
        }

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Company> GetCompanyInfo()
        {
            return await _organizetionMgr.GetCompanyInfo(); 
        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改公司信息", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateCompany(Company data)
        { 
             await _organizetionMgr.UpdateCompany(data); 
        }

        /// <summary>
        /// 上传公司LOGO
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Skip]
        public async Task<string> UploadCompanyLogo()
        { 
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024*5)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        var fileName = $"CompanyLogo{DateTime.Now.Ticks}.{file.FileName.Split('.')[1]}";
                        using (var stream=file.OpenReadStream())
                        {
                            return await _organizetionMgr.UploadCompanyLogo(fileName, stream); 
                        } 
                    }
                    throw new BusinessException("上传文件不属于图片类型"); 
                }
                throw new BusinessException("Logo图片不能大于5M"); 
            }
            throw new BusinessException("未获取到文件信息"); 
        }
    }
}
