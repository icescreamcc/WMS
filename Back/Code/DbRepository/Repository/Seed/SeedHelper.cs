using External.Common;
using External.Log;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed
{
   public class SeedHelper
    {
        private readonly IConfiguration _configuration;

        private readonly LogHelper _logHelper;

        public SeedHelper(LogHelper logHelper, IConfiguration configuration)
        {
            _logHelper = logHelper;
            _configuration = configuration;
        }
         

        /// <summary>
        /// 创建默认数据库并生成数据表
        /// </summary>
        /// <param name="stringDefaultLength"></param> 
        public void CreateDataBase(bool existDb, int stringDefaultLength = 100)
        {  
            var connectString = _configuration.GetSection("Sqlsugar:ConnectionString").Value;
            var dbType = _configuration.GetSection("Sqlsugar:DbType").Value;
            var isDev = bool.Parse(_configuration.GetSection("Sqlsugar:Develop").Value);
            if (!isDev)
            {
                connectString = EncryptionHelper.DesDecrypt(connectString);
            }
            var conf = new ConnectionConfig();
            conf.ConnectionString = connectString;
            conf.InitKeyType = InitKeyType.Attribute;
            conf.IsAutoCloseConnection = true;
            conf.DbType = DbType.SqlServer;
            if (dbType == "MySql")
            {
                conf.DbType = DbType.MySql;
            }
            else if (dbType == "Oracle")
            {
                conf.DbType = DbType.Oracle;
            }
            var db = new SqlSugarClient(conf);
            db.CodeFirst.SetStringDefaultLength(stringDefaultLength);
            var tables = _getModels();
            if (existDb)
            {
                var oldTables = db.DbMaintenance.GetTableInfoList();
                if (oldTables != null)
                {
                    oldTables.ForEach(t =>
                    {
                        db.DbMaintenance.DropTable(t.Name);
                    });
                } 
            }
            else
            {
                db.DbMaintenance.CreateDatabase();
            } 
            db.CodeFirst.InitTables(tables);
            CreateInitData(db);
            
        }

        public void AlterTable(bool isInitData, params string[] tableName )
        {
            var connectString = _configuration.GetSection("Sqlsugar:ConnectionString").Value;
            var dbType = _configuration.GetSection("Sqlsugar:DbType").Value;
            var isDev = bool.Parse(_configuration.GetSection("Sqlsugar:Develop").Value);
            if (!isDev)
            {
                connectString = EncryptionHelper.DesDecrypt(connectString);
            }
            var conf = new ConnectionConfig();
            conf.ConnectionString = connectString;
            conf.InitKeyType = InitKeyType.Attribute;
            conf.IsAutoCloseConnection = true;
            conf.DbType = DbType.SqlServer;
            if (dbType == "MySql")
            {
                conf.DbType = DbType.MySql;
            }
            else if (dbType == "Oracle")
            {
                conf.DbType = DbType.Oracle;
            }
            var db = new SqlSugarClient(conf);
            var tables = _getModels();
            var alters = tables.Where(w => tableName.Contains(w.Name)).ToArray();
            db.Aop.OnLogExecuted = (sql, para) =>
            {
                _logHelper.LogDbMigration(string.Join(',', tableName), sql + "\r\n", db.Utilities.SerializeObject(para.ToString()));
                Console.WriteLine("DbMigration:" + sql + "\r\n" + db.Utilities.SerializeObject(para.ToString()));
                Console.WriteLine();
            }; 
            if(alters?.Length > 0)
            { 
                if (isInitData)
                {
                    db.DbMaintenance.DropTable(alters);
                    db.CodeFirst.InitTables(alters);
                    CreateInitData(db, tableName);
                }
                else
                {
                    db.CodeFirst.InitTables(alters);
                }
            } 
        }

        public void CreateInitData(SqlSugarClient db,params string[] tableName)
        {
            var seedClassNames= new List<string>();
            foreach(var name in tableName)
            {
                seedClassNames.Add(name + "InitData"); 
            }
            var types = Assembly.Load("Repository").GetTypes();
            var seedTypes = types.ToList().Where(x => typeof(IDbInitData).IsAssignableFrom(x)&&!x.IsInterface&&!x.IsAbstract).WhereIF(tableName?.Length>0,x=> seedClassNames.Contains(x.Name)).ToArray(); 
            foreach (var type in seedTypes)
            { 
                var instence = (IDbInitData)Activator.CreateInstance(type);
                instence.CreateTableInitData(db);
            }
            db.SaveQueues();
        }

        /// <summary>
        /// 获取需要映射成表的实体类
        /// </summary>
        /// <returns></returns>
        private Type[] _getModels()
        { 
            var types = Assembly.Load("Repository").GetTypes();
            return types.ToList().Where(x => x.FullName.Contains("DbModels")).ToArray();
        }
    }
}
