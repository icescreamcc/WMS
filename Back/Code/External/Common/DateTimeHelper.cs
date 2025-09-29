using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Common
{
   public  class DateTimeHelper
    {
       public static string ConvertToString(DateTime dateTime)
        {
            if (dateTime <= DateTime.Parse("1999-1-1"))
            {
                return "";
            }
            return dateTime.ToString("yyyy-MM-dd hh:mm");
        }

        public static string ConvertToString(object dateTime)
        {
            if (dateTime == null)
            {
                return "";
            }
            else
            {
                DateTime dt;
                DateTime.TryParse(dateTime.ToString(), out dt);
                if (dt <= DateTime.Parse("1999-1-1"))
                {
                    return "";
                }
                return dt.ToString("yyyy-MM-dd hh:mm");
            } 
        }

        public static string ConvertToShotString(DateTime dateTime)
        {
            if (dateTime <= DateTime.Parse("1999-1-1"))
            {
                return "";
            }
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ConvertToShotString(object dateTime)
        {
            if (dateTime == null)
            {
                return "";
            }
            else
            {
                DateTime dt;
                DateTime.TryParse(dateTime.ToString(), out dt);
                if (dt <= DateTime.Parse("1999-1-1"))
                {
                    return "";
                }
                return dt.ToString("yyyy-MM-dd");
            }  
        }

        public static DateTime ConvertToDateTime(object dateTimeStr)
        {
            if (dateTimeStr==null)
            {
                return DateTime.Parse("1900-1-1 00:00:00");
            }
            DateTime dt;
            DateTime.TryParse(dateTimeStr.ToString(),out dt);
            return dt;
        }
    }
}
