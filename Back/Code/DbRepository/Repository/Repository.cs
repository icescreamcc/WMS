using Microsoft.Extensions.Configuration;
using Models.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository
{
   public class Repository 
    {

        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext) 
        {
            _dbContext = dbContext; 
        }
         
        /// <summary>
        /// 数据库实例
        /// </summary>
        public SqlSugarClient ClientDb { get; set; }

        /// <summary>
        /// SASS模式下指定租户的数据库
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public SqlSugarClient SetTenantDb(string tenantId)
        {
            ClientDb= _dbContext.GetDb(tenantId);
            return ClientDb;
        }

        public OrderByType GetOrderType(string orderType)
        {
            orderType = orderType.ToLower();
            return orderType == "asc" ? OrderByType.Asc : OrderByType.Desc;
        }

        public bool Add<T>(T model) where T : class,new()
        {
            return  ClientDb.Insertable(model).ExecuteCommand()>0;
        }

        public async Task<bool> AddAsync<T>(T model) where T : class, new()
        {
            return await ClientDb.Insertable(model).ExecuteCommandAsync() > 0;
        }

        public bool AddList<T>(List<T> list) where T : class, new()
        {
            return  ClientDb.Insertable(list).ExecuteCommand() > 0;
        }

        public async Task<bool> AddListAsync<T>(List<T> list) where T : class, new()
        {
            return await ClientDb.Insertable(list).ExecuteCommandAsync() > 0;
        }

        public async Task<int> AddRetrueId<T>(T model) where T : class, new()
        {
            return await ClientDb.Insertable(model).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            return await ClientDb.Deleteable(whereLambda).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync<T>(dynamic Id) where T : class, new()
        {
            return await ClientDb.Deleteable(Id).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync<T>(T model) where T : class, new()
        {
            return await ClientDb.Deleteable(model).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync<T>(List<T> list) where T : class, new()
        {
            return await ClientDb.Deleteable(list).ExecuteCommandAsync() > 0;
        }

        public async Task<T> GetModelAsync<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            return await ClientDb.Queryable<T>().SingleAsync(whereLambda);
        }

        public bool Update<T>(T model) where T : class, new()
        {
            return  ClientDb.Updateable(model).ExecuteCommand() > 0;
        }

        public async Task<bool> UpdateAsync<T>(T model) where T : class, new()
        {
            return await ClientDb.Updateable(model).ExecuteCommandAsync() > 0;
        }

        public bool Update<T>(List<T> list) where T : class, new()
        {
            return  ClientDb.Updateable(list).ExecuteCommand() > 0;
        }

        public async Task<bool> UpdateAsync<T>(List<T> list) where T : class, new()
        {
            return await ClientDb.Updateable(list).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Update<T>(Expression<Func<T, object>> columns, Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            return await ClientDb.Updateable<T>().UpdateColumns(columns).Where(whereLambda).ExecuteCommandAsync() > 0;
        }

        public T GetSinge<T>(object primaryKey)
        {
            return  ClientDb.Queryable<T>().InSingle(primaryKey);
        }

        public async Task<T> GetSingeAsync<T>(object primaryKey)
        {
            return await ClientDb.Queryable<T>().InSingleAsync(primaryKey);
        }

        public async Task<bool> Exist<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            return await ClientDb.Queryable<T>().AnyAsync(whereLambda);
        }

        public async Task<TableModel<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            var res = await ClientDb.Queryable<T>().Where(whereLambda).ToListAsync();
            return new TableModel<T>() { Total = res.Count, Rows = res };
        }

        public  TableModel<T> GetListByPages<T>(Expression<Func<T, bool>> whereLambda, int pageIndex, int pageSize) where T : class, new()
        {
            var total = 0;
            var res=  ClientDb.Queryable<T>().Where(whereLambda).ToPageList(pageIndex, pageSize,ref total);
            return new TableModel<T>() { Total = total, Rows = res };
        }

        public  TableModel<T> GetListByPages<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda, OrderByType orderType, int pageIndex, int pageSize) where T : class, new()
        {
            var total = 0;
            var res =  ClientDb.Queryable<T>().Where(whereLambda).OrderBy(orderLambda,orderType).ToPageList(pageIndex, pageSize,ref total);
            return new TableModel<T>() { Total = total, Rows = res };
        }

        public async Task<DataTable> QueryBySqlAsync(string sql,Dictionary<string,object> param)
        { 
            var paramList = new List<SugarParameter>();
            if (param != null)
            {
                foreach (var dic in param)
                {
                    paramList.Add(new SugarParameter(dic.Key, dic.Value));
                }
            }
            return await ClientDb.Ado.GetDataTableAsync(sql, paramList);
        }

        public async Task<DataTable> QueryByProcedureAsync(string procedureName, Dictionary<string, object> param)
        {
            var paramList = new List<SugarParameter>();
            if (param != null)
            {
                foreach (var dic in param)
                {
                    paramList.Add(new SugarParameter(dic.Key, dic.Value));
                }
            }
            return await ClientDb.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, paramList);
        }
    }
}
