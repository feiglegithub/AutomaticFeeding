//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：CacheManager.cs
//   创建时间：2018-11-29 9:51
//   作    者：
//   说    明：
//   修改时间：2018-11-29 9:51
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using NJIS.FPZWS.Common;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Cache
{
    public class CacheManager : Singleton<CacheManager>
    {
        private readonly object _locker = new object();
        private readonly MemoryCache Cache = MemoryCache.Default; //声明缓存类

        /// <summary>
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">数据对象</param>
        /// <param name="policy">过期时间</param>
        private bool SetToCache(string key, object value, CacheItemPolicy policy)
        {
            lock (_locker)
            {
                Cache.Set(key, value, policy);
                return true;
            }
        }

        /// <summary>
        ///     添加到缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>结果状态</returns>
        public bool Set(string key, object value)
        {
            var policy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.NotRemovable
            };
            return SetToCache(key, value, policy);
        }

        /// <summary>
        ///     设置缓存绝对时间过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        public bool Set(string key, object value, DateTimeOffset expiresIn)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = expiresIn
            };
            return SetToCache(key, value, policy);
        }

        /// <summary>
        ///     设置缓存指定时间未访问过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        public bool Set(string key, object value, TimeSpan expiresIn)
        {
            var policy = new CacheItemPolicy
            {
                SlidingExpiration = expiresIn
            };
            return SetToCache(key, value, policy);
        }

        /// <summary>
        ///     设置缓存，没有其他重载方法，第一个参数name是我们的缓存的名字，第二个参数是我们需要缓存的对象，第三个是我们的过期时间默认7200秒
        /// </summary>
        /// <param name="name">缓存的名字</param>
        /// <param name="ovlaue">需要缓存的值</param>
        /// <param name="seconds">过期时间</param>
        public bool Set(string name, object ovlaue, int seconds = 60 * 60 * 24 * 3)
        {
            if (seconds <= 0) throw new ArgumentOutOfRangeException(nameof(seconds));
            var policy = new CacheItemPolicy {AbsoluteExpiration = DateTime.Now.AddSeconds(seconds)};


            return SetToCache(name, ovlaue, policy);
        }

        /// <summary>
        ///     获取键的集合
        /// </summary>
        /// <returns>键的集合</returns>
        public ICollection<string> GetCacheKeys()
        {
            lock (_locker)
            {
                var items = Cache.AsEnumerable();
                return items.Select(m => m.Key).ToList();
            }
        }

        /// <summary>
        ///     清除实例
        /// </summary>
        public void Clear()
        {
            ClearCache();
        }

        /// <summary>
        ///     清除实例
        /// </summary>
        private void ClearCache()
        {
            lock (_locker)
            {
                Cache.ToList().ForEach(m => Cache.Remove(m.Key));
            }
        }


        /// <summary>
        ///     数据对象从缓存对象中移除
        /// </summary>
        /// <param name="key">键</param>
        private bool RemoveFromCache(string key)
        {
            lock (_locker)
            {
                if (Cache.Contains(key))
                {
                    Cache.Remove(key);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        ///     数据对象从缓存对象中移除
        /// </summary>
        /// <param name="key">键</param>
        public bool Remove(string key)
        {
            return RemoveFromCache(key);
        }

        /// <summary>
        ///     判断缓存中是否有此对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public bool ContainKey(string key)
        {
            lock (_locker)
            {
                return Cache.Contains(key);
            }
        }

        /// <summary>
        ///     从缓存中获取对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>缓存对象</returns>
        private object GetFromCache(string key)
        {
            lock (_locker)
            {
                if (Cache.Contains(key))
                {
                    return Cache[key];
                }

                return null;
            }
        }


        /// <summary>
        ///     获取缓存，传入缓存名字即可
        /// </summary>
        /// <param name="name">缓存的名字</param>
        /// <returns></returns>
        public object Get(string name)
        {
            return GetFromCache(name);
        }

        /// <summary>
        ///     获取缓存，传入缓存名字即可
        /// </summary>
        /// <param name="name">缓存的名字</param>
        /// <returns></returns>
        public T Get<T>(string name)
        {
            var obj = GetFromCache(name);
            if (obj is T)
            {
                return (T) obj;
            }

            return default(T);
        }

        /// <summary>
        ///     获取缓存尺寸
        /// </summary>
        /// <returns>缓存尺寸</returns>
        public long Size()
        {
            return GetCacheSize();
        }

        /// <summary>
        ///     获取缓存尺寸
        /// </summary>
        /// <returns>缓存尺寸</returns>
        private long GetCacheSize()
        {
            lock (_locker)
            {
                return Cache.GetCount();
            }
        }

        /// <summary>
        ///     获取缓存对象集合
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <returns>缓存对象集合</returns>
        private ICollection<T> GetValues<T>()
        {
            lock (_locker)
            {
                var items = Cache.AsEnumerable();
                return items.Select(m => (T) m.Value).ToList();
            }
        }
    }
}