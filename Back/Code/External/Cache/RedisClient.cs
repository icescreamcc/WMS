using External.Common;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace External.Cache
{
   public class RedisClient
    { 

        private static ConnectionMultiplexer redis { get; set; }
         

        private IDatabase db { get; set; } 

        private int _expiration { get; set; }

        public RedisClient(IConfiguration configuration)
        {
            string connectString = configuration.GetSection("Redis:ConnectionString").Value;
            _expiration = int.Parse(configuration.GetSection("Cache:Expiration").Value); 
            connectString = EncryptionHelper.DesDecrypt(connectString);
            redis = ConnectionMultiplexer.Connect(connectString);
            db = redis.GetDatabase();
        }

        #region String 
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="value">保存的值</param> 
        public bool SetStringKey(string key, string value, int expiration = 0)
        {
            _expiration = expiration > 0 ? expiration : _expiration;
            return db.StringSet(key, value, TimeSpan.FromSeconds(_expiration));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        public RedisValue GetStringKey(string key)
        {
            return db.StringGet(key);
        }


        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        public T GetStringKey<T>(string key)
        {
            if (db == null)
            {
                return default;
            }
            var value = db.StringGet(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value); 
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <param name="obj"></param>
        public bool SetStringKey<T>(string key, T obj, int expiration = 0)
        {
            if (db == null)
            {
                return false;
            }
            _expiration = expiration > 0 ? expiration : _expiration;
            string json = JsonSerializer.Serialize(obj);
            return db.StringSet(key, json, TimeSpan.FromSeconds(_expiration));
        }

        #endregion
        /// <summary>
        /// 将一个泛型List添加到缓存中
        /// </summary>
        /// <typeparam name="T">泛型T</typeparam>
        /// <param name="listkey">Key</param>
        /// <param name="list">list</param>
        /// <param name="db_index">数据库序号，不传默认为0</param>
        /// <returns></returns>
        public bool AddList<T>(string listkey, List<T> list, int expiration = 0)
        {
            if (db == null)
            {
                return false;
            }
            if (list?.Count > 0)
            {
                _expiration = expiration > 0 ? expiration : _expiration;
                var redisValues = list.Select(item => (RedisValue)JsonSerializer.Serialize(item)).ToArray();
                db.KeyExpire(listkey, TimeSpan.FromSeconds(_expiration));
                return db.ListRightPush(listkey, redisValues) > 0;
            }
            return false;
        }

        /// <summary>
        /// 通过指定Key值获取泛型List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listkey"></param>
        /// <param name="db_index"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string listkey)
        {
            if (db == null)
            {
                return null;
            }

            RedisValue[] redisValues = db.ListRange(listkey);
            var list = new List<T>(); 
            foreach (var redisValue in redisValues)
            {
                T item = JsonSerializer.Deserialize<T>(redisValue);
                list.Add(item);
            } 
            return list;
        }
         
        public bool GetKeyExists(string listkey)
        {
            if (db == null)
            {
                return false;
            }
            if (db.KeyExists(listkey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除指定List<T>中满足条件的元素
        /// </summary>
        /// <param name="listkey">Key</param>
        /// <param name="func">lamdba表达式</param>
        /// <param name="db_index">数据库序号，不传默认为0</param>
        /// <returns></returns>
        public bool DelListByLambda<T>(string listkey, Func<T, bool> func)
        {
            if (db == null)
            {
                return false;
            }
            if (db.KeyExists(listkey))
            {
                var value = db.StringGet(listkey);
                if (!string.IsNullOrEmpty(value))
                {
                    var list = JsonSerializer.Deserialize<List<T>>(value);
                    if (list.Count > 0)
                    {
                        list = list.SkipWhile<T>(func).ToList();
                        value = JsonSerializer.Serialize(list);
                        return db.StringSet(listkey, value);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取指定List<T>中满足条件的元素
        /// </summary>
        /// <param name="listkey">Key</param>
        /// <param name="func">lamdba表达式</param>
        /// <param name="db_index">数据库序号，不传默认为0</param>
        /// <returns></returns>
        public List<T> GetListByLambda<T>(string listkey, Func<T, bool> func)
        { 
            if (db == null)
            {
                return new List<T>();
            }
            if (db.KeyExists(listkey))
            {
                var value = db.StringGet(listkey);
                if (!string.IsNullOrEmpty(value))
                {
                    var list = JsonSerializer.Deserialize<List<T>>(value);
                    if (list.Count > 0)
                    {
                        list = list.Where(func).ToList();
                        return list;
                    }
                    else
                    {
                        return new List<T>();
                    }
                }
                else
                {
                    return new List<T>();
                }
            }
            else
            {
                return new List<T>();
            }
        } 

        public async Task<bool> Remove(string key)
        {
           return await db.KeyDeleteAsync(key);
        }
    }
}
