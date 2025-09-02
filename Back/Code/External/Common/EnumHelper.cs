using Models.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace External.Common
{
   public class EnumHelper
    {
        /// <summary>
        /// 根据枚举值获取对应的描述
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescFromEnum(Enum en)
        {
            Type t = en.GetType();
            string key = en.ToString();
            MemberInfo[] menber = t.GetMember(key);
            if (menber != null && menber.Length > 0)
            {
                object[] attrs = menber[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return key;
        }

        /// <summary>
        /// 根据枚举值字符串获取对应的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetDescFromEnumVal<T>(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return "";
            }
            Enum e = Enum.Parse(typeof(T), val) as Enum;
            return GetDescFromEnum(e);
        }
         

        /// <summary>
        /// 获取指定枚举所有的值以及对应的描述，用Key/Value的形式保存为对象集合  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValueModel> GetEnumValNames<T>()
        {
            List<KeyValueModel> list = new List<KeyValueModel>();
            string[] keys = Enum.GetNames(typeof(T));
            foreach (string key in keys)
            {
                var obj = new KeyValueModel { Key = key, Value = GetDescFromEnumVal<T>(key) };
                list.Add(obj);
            }
            return list;
        }
    }
}
