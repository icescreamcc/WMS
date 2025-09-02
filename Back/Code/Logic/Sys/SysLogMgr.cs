using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using Logic.LogicBase;
using Models.Model;
using Models.Model.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{
   public class SysLogMgr: DbOperationHandler
    {
        public SysLogMgr(Repository repository):base(repository)
        { }

        /// <summary>
        /// 分页查询操作日志
        /// </summary>
        /// <param name="pgSize"></param>
        /// <param name="pgIndex"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderType"></param>
        /// <param name="searchKey"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public async Task<TableModel<BusinessLog>> GetLogs(int pgSize, int pgIndex, string orderFiled, string orderType, string searchKey, string dateStart, string dateEnd)
        {
            int total = 0;
            orderFiled = string.IsNullOrEmpty(orderFiled) ? "DateTime" : orderFiled;
            orderFiled = orderFiled== "UserName" ? "u.UserName" : orderFiled;
            searchKey = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
            var data = Repository.ClientDb.Queryable<SysLogs>()
                  .LeftJoin<SysUser>((l,u)=>l.UserId==u.UserId)
                  .Where((l, u) => l.Title.Contains(searchKey) || l.Message.Contains(searchKey) || l.Remark.Contains(searchKey)||u.UserName.Contains(searchKey))
                  .Where((l, u) => SqlFunc.ToDate(l.DateTime) >= GetDateStart(dateStart) && SqlFunc.ToDate(l.DateTime) <= GetDateEnd(dateEnd))
                  .Select((l, u) => new BusinessLog { LogId=l.LogId, Title=l.Title, Message=l.Message, LogType=l.LogType, DateTime=l.DateTime,UserId=u.UserId, UserName=u.UserName, Remark = l.Remark })
                  .OrderBy($"{orderFiled} {orderType}")
                  .ToPageList(pgIndex, pgSize, ref total);
            var res = new TableModel<BusinessLog>() { Total = total, Rows = data };
            return await Task.FromResult(res);
        }
    }
}
