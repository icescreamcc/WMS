using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace External.Cache
{
    public class MemoryCacheProvider: ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        private int _expiration { get; set; }
        public MemoryCacheProvider(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _expiration = int.Parse(configuration.GetSection("Cache:Expiration").Value);
        }

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task SetString(string key, string value, int expiration = 0)
        {
            _expiration = expiration > 0 ? expiration: _expiration;
            _memoryCache.Set(key, value, TimeSpan.FromSeconds(_expiration));
            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        public string GetString(string key)
        {
            if (_memoryCache.TryGetValue(key, out string str))
            {
                return str;
            }
            return "";
        }


        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public async Task SetObject<T>(string key, T obj, int expiration = 0)
        {
            _expiration = expiration > 0 ? expiration : _expiration;
            _memoryCache.Set(key, obj, TimeSpan.FromSeconds(_expiration));
            await Task.CompletedTask;
        }
         
        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        public T GetObject<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T obj))
            {
                return obj;
            }
            return default;
        }

        public async Task Remove(string key)
        {
            _memoryCache.Remove(key);
            await Task.CompletedTask;
        }

        public async Task RemoveContains(string key)
        { 
            _memoryCache.Remove(key);
            await Task.CompletedTask;
        }

        public async Task SetList<T>(string key, List<T> list, int expiration = 0)
        {
            _expiration = expiration > 0 ? expiration : _expiration;
            _memoryCache.Set(key, list, TimeSpan.FromSeconds(_expiration));
            await Task.CompletedTask;
        }

        public List<T> GetList<T>(string key)
        { 
            if(_memoryCache.TryGetValue(key, out List<T> list))
            {
                return list;
            }
            return default;
        }

        public T GetOrSetObj<T>(string key, Func<T> callback, int expiration = 0)
        {
            _expiration = expiration > 0 ? expiration : _expiration;
            return _memoryCache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_expiration);
                return callback();
            });
        }

        public List<T> GetOrSetList<T>(string key, Func<List<T>> callback, int expiration = 0)
        {
            _expiration = expiration > 0 ? expiration : _expiration;
            return _memoryCache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_expiration);
                return callback();
            });
        }
    } 
}
