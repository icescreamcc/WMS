using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Common.Extension
{
   public static class DateTimeExtension
    {
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStringExtension(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShotStringExtension(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// yyyy年MM月dd日 HH时mm分ss秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStringCHExtension(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy年MM月dd日 HH时mm分ss秒");
        }

        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShotStringCHExtension(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy年MM月dd日");
        }

        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStringNoSignExtension(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }

        public static int ToStringYYMMExtension(this DateTime dateTime)
        {
            return int.Parse(dateTime.ToString("yyyyMM"));
        }
    }
}
