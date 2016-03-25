using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace AppExtension
{
    /// <summary>
    /// Cache
    /// </summary>
    public class CMSDataCache
    {
        private static System.Web.Caching.Cache _cache;

        /// <summary>
        /// Initialize cache
        /// </summary>
        static CMSDataCache()
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// Get cache data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return _cache[key];
        }

        /// <summary>
        /// Remove cache data
        /// </summary>
        /// <param name="key"></param>
        public static object Remove(string key)
        {
            return _cache.Remove(key);
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator cacheEnum = _cache.GetEnumerator();

            while (cacheEnum.MoveNext())
            {
                _cache.Remove(cacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// Insert cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert(string key, object value)
        {
            _cache.Insert(key, value);
        }

        /// <summary>
        /// Insert cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        public static void Insert(string key, object value, int seconds)
        {
            _cache.Insert(key, value, null, DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Insert cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        public static void Insert(string key, object value, CacheDependency dependencies)
        {
            _cache.Insert(key, value, dependencies);
        }

        /// <summary>
        /// Insert cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        /// <param name="seconds"></param>
        public static void Insert(string key, object value, CacheDependency dependencies, int seconds)
        {
            _cache.Insert(key, value, dependencies, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, seconds));
        }

        /// <summary>
        /// Insert cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        /// <param name="priority"></param>
        public static void Insert(string key, object value, CacheDependency dependencies, CacheItemPriority priority)
        {
            _cache.Insert(key, value, dependencies, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, priority, null);
        }

        /// <summary>
        /// Insert cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        /// <param name="priority"></param>
        public static void Insert(string key, object value, int seconds, CacheItemPriority priority)
        {
            _cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, seconds), priority, null);
        }

        /// <summary>
        /// Insert cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        /// <param name="seconds"></param>
        /// <param name="priority"></param>
        public static void Insert(string key, object value, CacheDependency dependencies, int seconds,
                                  CacheItemPriority priority)
        {
            _cache.Insert(key, value, dependencies, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, seconds), priority, null);
        }

        /// <summary>
        /// Insert local cache data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void InsertLocal(string key, object value)
        {
            HttpContext context = HttpContext.Current;
            context.Items.Add(key, value);
        }

        /// <summary>
        /// Remove local cache data
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveLocal(string key)
        {
            HttpContext context = HttpContext.Current;
            context.Items.Remove(key);
        }

        /// <summary>
        /// Get local cache data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetLocal(string key)
        {
            HttpContext context = HttpContext.Current;
            return context.Items[key];
        }

        /// <summary>
        /// Clear local cache data
        /// </summary>
        public static void ClearLocal()
        {
            HttpContext context = HttpContext.Current;
            context.Items.Clear();
        }
    }
}