using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace External.Cache
{
    public class RedisProvider : ICacheProvider
    {
        private readonly RedisClient _redisClient;

        public RedisProvider(RedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task SetString(string key, string value, int expiration = 0)
        {
            _redisClient.SetStringKey(key, value);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        public string GetString(string key)
        {
            return _redisClient.GetStringKey(key); 
        }


        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public async Task SetObject<T>(string key, T obj, int expiration = 0)
        {
            _redisClient.SetStringKey(key, obj);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        public T GetObject<T>(string key)
        {
            return _redisClient.GetStringKey<T>(key); 
        }

        public async Task SetList<T>(string key, List<T> list, int expiration = 0)
        {
            _redisClient.AddList<T>(key, list);
            await Task.CompletedTask;
        }

        public List<T> GetList<T>(string key)
        {
           return _redisClient.GetList<T>(key);
        }

        public async Task Remove(string key)
        {
             await _redisClient.Remove(key); 
        }

        public T GetOrSetObj<T>(string key, Func<T> callback, int expiration = 0)
        {
            var obj = _redisClient.GetStringKey<T>(key);
            if (obj == null)
            {
                obj = callback();
                _redisClient.SetStringKey(key, obj);
            }
            return obj;
        }

        public List<T> GetOrSetList<T>(string key, Func<List<T>> callback, int expiration = 0)
        {
            var obj = _redisClient.GetStringKey<List<T>>(key);
            if (obj == null)
            {
                obj = callback();
                _redisClient.SetStringKey(key, obj);
            }
            return obj;
        }
    }
}
