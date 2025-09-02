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
   public interface IRepository<T> where T:class
    {
        Task<bool> Add(T model);

        Task<bool> AddList(List<T> list);

        Task<int> AddRetrueId(T model);

        Task<bool> Delete(Expression<Func<T, bool>> whereLambda);

        Task<bool> Delete(dynamic Id);

        Task<bool> Delete(T model);

        Task<bool> Delete(List<T> list);

        Task<bool> Update(T model);

        Task<bool> Update(Expression<Func<T, object>> columns, Expression<Func<T, bool>> whereLambda);

        Task<T> GetModel(Expression<Func<T, bool>> whereLambda);

        Task<TableModel<T>>  GetList(Expression<Func<T, bool>> whereLambda);

        Task<TableModel<T>> GetListByPages(Expression<Func<T, bool>> whereLambda, int pageIndex,int pageSize);

        Task<TableModel<T>> GetListByPages(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda, OrderByType orderType, int pageIndex, int pageSize);

        Task<DataTable> QueryBySql(string sql, Dictionary<string, object> param);

        Task<DataTable> QueryByProcedure(string procedureName, Dictionary<string, object> param);
    }
}
