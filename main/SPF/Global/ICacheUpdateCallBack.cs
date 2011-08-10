using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace SPF.Data
{
    interface ICacheUpdateCallBack
    {
        void CacheCallBack(string key,CacheItemUpdateReason cacheItemUpdateReason,out object value,out CacheDependency dependency ,out DateTime dateTime,out TimeSpan timeSpan);
    }
}
