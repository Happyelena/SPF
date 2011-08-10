using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace SPF.Data.DataCache
{
    public abstract class DataCache<T> : ICacheable
    {
        /// <summary>
        /// Cache Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Cache Value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Cache Duration
        /// After this duration , the cache value will be expired
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Cache Priority
        /// Default is Normal
        /// </summary>
        public CacheItemPriority DataCacheItemPriority { get; set; }

        public CacheItemUpdateCallback CacheItemUpdateCallBackHandler { get; set; }

        public virtual void Add()
        {
            if (IsAlive(Key))
            {
                return;
            }
            else
            {
                System.Web.HttpRuntime.Cache.Insert(Key, Value, null, DateTime.Now.AddSeconds(Duration), System.Web.Caching.Cache.NoSlidingExpiration,CacheItemUpdateCallBackHandler);
            }
        }

        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public bool IsAlive(string key)
        {
            if (Get(key) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        public void RemoveAll()
        {
            foreach (DictionaryEntry entry in HttpRuntime.Cache)
            {
                HttpRuntime.Cache.Remove(entry.Key.ToString());
            }
        }
    }
}
