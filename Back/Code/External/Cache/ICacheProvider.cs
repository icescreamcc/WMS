using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace External.Cache
{
    public interface ICacheProvider
    {
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public Task SetString(string key, string value, int expiration=0);

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        public string GetString(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task Remove(string key);

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public  Task SetObject<T>(string key, T obj, int expiration = 0);

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        public T GetObject<T>(string key);

        /// <summary>
        /// 存储list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Task SetList<T>(string key, List<T> list, int expiration = 0);

        /// <summary>
        /// 获取list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string key);

        public T GetOrSetObj<T>(string key, Func<T> callback, int expiration = 0);

        public List<T> GetOrSetList<T>(string key, Func<List<T>> callback, int expiration = 0);
    }
}
