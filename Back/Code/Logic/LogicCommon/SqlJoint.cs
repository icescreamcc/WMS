using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicCommon
{
    /// <summary>
    /// 用于sql脚本拼接
    /// </summary>
   public class SqlJoint
    {
        /// <summary>
        /// 用于导出查询时拼接出select后面的字段(目前仅用于数据导出)
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="types">表对应的实体类type</param>
        /// <returns></returns>
        public static string GetSelectFieldFormat(KeyValueModel field, List<Type> types)
        {
            foreach (var t in types)
            {
                var tbName = t.Name;
                var props = t.GetProperties().ToList();
                string fieldStr = "";
                if (props.Exists(u => u.Name == field.Key.ToString()))
                {
                    fieldStr = $"{tbName}.{field.Key} as {field.Value},";
                    return fieldStr;
                }
            }
            return "";
        }

        /// <summary>
        /// 用于查询时order by后面的拼接(目前仅用于数据导出)
        /// </summary>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="types">表对应的实体类type</param>
        /// <returns></returns>
        public static string GetOrderFieldFormat(string orderField, string orderType, List<Type> types)
        {
            if (string.IsNullOrEmpty(orderField))
            {
                return "";
            }
            foreach (var t in types)
            {
                var tbName = t.Name;
                var props = t.GetProperties().ToList();
                string fieldStr = "";
                if (props.Exists(u => u.Name == orderField))
                {
                    fieldStr = $"order by {tbName}.{orderField} {orderType}";
                    return fieldStr;
                }
            }
            return "";
        }
    }
}
