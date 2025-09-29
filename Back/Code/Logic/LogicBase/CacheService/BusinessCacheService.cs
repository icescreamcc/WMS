using External.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase.CacheService
{
   public class BusinessCacheService
    {
        private  ICacheProvider _cacheProvider { get; }

        public BusinessCacheService(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }  

        private string _getKey(string cacheItem, string userId)
        {
            return $"cache_{cacheItem}_{userId}";
        }

        public void RemoveBykey(string cacheItem,string userId)
        {
            var key = _getKey(cacheItem, userId);
            _ = _cacheProvider.Remove(key);
        }


        public void RemoveByKeys(string cacheItem)
        {
            var keys = _cacheProvider.GetList<string>(cacheItem);
            if (keys != null)
            {
                foreach(var key in keys)
                {
                    _ = _cacheProvider.Remove(key);
                }
            } 
        }

        public void SetObject<T>(string cacheItem, string userId,T data,int expiration=0)
        {
            var key = _getKey(cacheItem, userId);
            _= _cacheProvider.SetObject(key,data, expiration);
        }

        public void SetObjectAndKey<T>(string cacheItem, string userId, T data, int expiration = 0)
        {
            var key = _getKey(cacheItem, userId);
            var keys = _cacheProvider.GetList<string>(cacheItem);
            if (keys != null)
            {
                if (!keys.Exists(e => e == key))
                    keys.Add(key);
            }
            else
            {
                keys = new List<string> { key };
            }
            _ = _cacheProvider.SetList(cacheItem, keys, expiration);
            _ = _cacheProvider.SetObject(key, data, expiration);
        }

        public void SetList<T>(string cacheItem, string userId, List<T> data, int expiration = 0)
        {
            var key = _getKey(cacheItem, userId);
            _ = _cacheProvider.SetList(key, data, expiration);
        }

        /// <summary>
        /// 缓存数据同时将Key缓存到一个以cacheItem为key的集合中
        /// 1.当只需要移除某个userId的缓存则用RemoveByKey方法
        /// 2.当需要移除所有cacheItem项的缓存时需要用RemoveByKeys方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheItem"></param>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <param name="expiration"></param>
        public void SetListAndKey<T>(string cacheItem, string userId, List<T> data, int expiration = 0)
        {
            var key = _getKey(cacheItem, userId);
            var keys = _cacheProvider.GetList<string>(cacheItem);
            if (keys != null)
            {
                if (!keys.Exists(e => e == key))
                    keys.Add(key);
            }
            else
            {
                keys = new List<string> { key };
            }
            _ = _cacheProvider.SetList(cacheItem, keys, expiration);
            _ = _cacheProvider.SetList(key, data, expiration);
        }

        public T GetObject<T> (string cacheItem, string userId)
        {
            var key = _getKey(cacheItem, userId);
            return _cacheProvider.GetObject<T>(key);
        }

        public List<T> GetList<T>(string cacheItem, string userId)
        {
            var key = _getKey(cacheItem, userId);
            return _cacheProvider.GetList<T>(key);
        }

        public void SetString(string cacheItem, string userId, string value)
        {
            var key = _getKey(cacheItem, userId);
            _ = _cacheProvider.SetString(key, value);
        }

        public string GetString(string cacheItem, string userId)
        {
            var key = _getKey(cacheItem, userId);
            return _cacheProvider.GetString(key);
        }
    }
}
