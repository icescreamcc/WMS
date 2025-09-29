using DbRepository.Repository.Seed;
using External.Common; 
using External.Log;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System; 
using System.IO;
using System.Linq;
using System.Reflection; 

namespace DbRepository.Repository
{
   public class DbContext
    {  
        private readonly LogHelper _logHelper;

        private  readonly IConfiguration _configuration;

        public DbContext(LogHelper logHelper, IConfiguration configuration)
        {
            _logHelper = logHelper;
            _configuration = configuration;
        }
         
        private SqlSugarClient _db;

       // private object _locker = new object();

        /// <summary>
        /// 根据配置创建数据库实例
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public SqlSugarClient GetDb( string tenantId=null)
        { 
            var connectString = _configuration.GetSection("Sqlsugar:ConnectionString").Value;
            var dbType = _configuration.GetSection("Sqlsugar:DbType").Value; 
            var isDev= bool.Parse(_configuration.GetSection("Sqlsugar:Develop").Value);
            if (!isDev)
            {
                connectString = EncryptionHelper.DesDecrypt(connectString);
            }  
            var conf = new ConnectionConfig();
            conf.ConnectionString = connectString;
            conf.InitKeyType = InitKeyType.Attribute;
            conf.IsAutoCloseConnection = true;
            conf.DbType = DbType.SqlServer;
            conf.ConfigId = tenantId;
            if (dbType == "MySql")
            {
                conf.DbType = DbType.MySql;
            }
            else if (dbType == "Oracle")
            {
                conf.DbType = DbType.Oracle;
            }
            _db = new SqlSugarClient(conf);
            if (isDev)
            {
                _db.Aop.OnLogExecuted = (sql, para) =>
                {
                    Console.WriteLine(sql + "\r\n" + _db.Utilities.SerializeObject(para.ToString()));
                    Console.WriteLine();
                };
            }
            _db.Aop.OnError = (exp =>
            {
                _logHelper.LogError("SqlException", exp.Message, exp.Sql, exp, exp.StackTrace);
            });
            return _db;
        }
         
    }
}
