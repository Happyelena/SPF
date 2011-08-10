using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace SPF.Data.DataCache
{
    public class WebDataCache<T>:DataCache<T>
    {
        public WebDataCache() { }

        public WebDataCache(string key)
        {
            Key = key;
        }

        public WebDataCache(string key,T value,int duration,CacheItemPriority cacheItemPriority,CacheItemUpdateCallback cacheItemUpdateCallBackHandler)
        {
            Key = key;
            Value = value;
            Duration = duration;
            DataCacheItemPriority = cacheItemPriority;
            CacheItemUpdateCallBackHandler = cacheItemUpdateCallBackHandler;
        }
    }
}
