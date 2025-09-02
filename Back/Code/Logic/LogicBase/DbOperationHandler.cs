using DbRepository.Repository;
using External.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase
{
    /// <summary>
    /// 业务逻辑层基类,处理SASS模式下不同租户的数据库的连接
    /// </summary>
   public class DbOperationHandler: InfrastructureHandler
    {
        public  Repository Repository { get; } 

        public string TenantId { get; set; }

        public DbOperationHandler(Repository repository)
        {
            Repository = repository;
            Repository.SetTenantDb(TenantId); 
        }
    }
}
