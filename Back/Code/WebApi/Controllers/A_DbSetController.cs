using DbRepository.Repository.Seed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{ 
    public class A_DbSetController : AnonymousController
    {  
        private readonly SeedHelper _seedHelper;

        public A_DbSetController( SeedHelper seedHelper)
        { 
            _seedHelper = seedHelper;
        }

        /// <summary>
        /// 数据库迁移-初始化数据库
        /// </summary>
        /// <param name="existDb">是否已存在数据库</param>
        [HttpGet]
        public void CreateDatabase(bool existDb)
        {
            _seedHelper.CreateDataBase(existDb);
        }

        /// <summary>
        /// 数据库迁移-新增、修改、删除表
        /// </summary>
        /// <param name="isInitData">是否初始化指定表名的数据</param>
        /// <param name="tableName">新增、修改、删除指定的表结构</param>
        [HttpPost]
        public void AlterTable(bool isInitData, params string[] tableName)
        {
            _seedHelper.AlterTable(isInitData, tableName);
        }
    }
}
