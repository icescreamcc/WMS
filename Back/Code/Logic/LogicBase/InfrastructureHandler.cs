using DbRepository.Repository;
using External.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase
{
    /// <summary>
    /// 用于业务逻辑中常用关键字（包括字典表SysArgs中的Key）及方法
    /// </summary>
    public class InfrastructureHandler
    {
        /// <summary>
        /// 创建Guid作为主键
        /// </summary>
        /// <returns></returns>
        public string GetPrimaryId()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 创建一个有序ID作为主键
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public string GetPrimaryId( string prefix, string lastId)
        {
            if (string.IsNullOrEmpty(lastId))
            {
                return prefix + "10000001";
            }
            else
            {
               var num= lastId.Substring(prefix.Length,lastId.Length- prefix.Length);
                return prefix + (int.Parse(num) + 1).ToString();
            }
          
        }

        /// <summary>
        /// 用于日期范围查询时转换起始日期
        /// </summary>
        /// <param name="dateStart"></param>
        /// <returns></returns>
        public DateTime GetDateStart(string dateStart)
        { 
            if (string.IsNullOrEmpty(dateStart))
            {
               return DateTime.Parse("1900-1-1");
            }
            return DateTimeHelper.ConvertToDateTime(dateStart);
        }

        /// <summary>
        /// 用于日期范围查询时转换结束日期
        /// </summary>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DateTime GetDateEnd(string dateEnd)
        { 
            if (string.IsNullOrEmpty(dateEnd))
            {
              return DateTime.Parse("3000-1-1");
            }
            return DateTimeHelper.ConvertToDateTime(dateEnd);
        }
    }
}
