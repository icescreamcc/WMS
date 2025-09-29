using Models.Model.Sys;
using System;
using System.Collections.Generic;

namespace Models.Model
{
    /// <summary>
    /// 用于分页查询列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableModel<T> where T :class
    {
        public TableModel()
        {

        }

        public TableModel(int total,List<T> rows)
        {
            Total = total;
            Rows = rows; 
        }
        /// <summary>
        /// 数据总行数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 数据体
        /// </summary>
        public List<T> Rows { get; set; }

        public int Sum { get; set; }

        public int Count { get; set; }
    }
}
