using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;


namespace SPF.Data.DataCache
{
    public class SqlDataCache<T>:DataCache<T>
    {
        public SqlDataCache() { }

        public SqlDataCache(string key)
        {
            Key = key;
        }

        public SqlDataCache(string key, T value, int duration, CacheItemPriority cacheItemPriority)
        {
            Key = key;
            Value = value;
            Duration = duration;
            DataCacheItemPriority = cacheItemPriority;
        }

        /// <summary>
        /// Insert a cache copy without callback handler
        /// </summary>
        public new void Add()
        {
            if (IsAlive(Key))
            {
                return;
            }
            else
            {
                System.Web.HttpRuntime.Cache.Insert(Key, Value, null, DateTime.Now.AddSeconds(Duration), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
    }
}
