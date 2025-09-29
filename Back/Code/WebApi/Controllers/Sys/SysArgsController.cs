using Logic.LogicBase;
using Logic.Sys;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Sys
{ 
    public class SysArgsController : AuthTokenController
    {
        private readonly SysArgsMgr _sysArgsMgr;

        private const string _moduleName = "系统参数管理";

        public SysArgsController(SysArgsMgr sysArgsMgr)
        {
            _sysArgsMgr = sysArgsMgr; 
        }

        /// <summary>
        /// 获取系统参数列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Args>> GetArgs()
        {
            return await _sysArgsMgr.GetArgs(); 
        }

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        [HttpPost]
        [BusinessLog("修改参数", Models.Model.Enum.LogType.Update, _moduleName)]
        public async Task UpdateArgs(List<Args> dataList)
        {
            await _sysArgsMgr.UpdateArgs(dataList);
            var args = dataList.Select(x => x.ArgsKeyName).ToList(); 
        }


        /// <summary>
        /// 上传系统主页Logo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Skip]
        public async Task<string> UploadSystemHomePageLogo()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length / 1024 < 1024 * 10)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        var fileName = $"SystemHomePageLogo{DateTime.Now.Ticks}.{file.FileName.Split('.')[1]}";
                        using (var stream = file.OpenReadStream())
                        {
                            return await _sysArgsMgr.UploadSystemHomePageLogo(fileName, stream);
                        }
                    }
                    throw new BusinessException("上传文件不属于图片类型");
                }
                throw new BusinessException("Logo图片不能大于10M");
            }
            throw new BusinessException("未获取到文件信息");
        }


    }
}
