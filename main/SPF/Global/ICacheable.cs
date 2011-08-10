using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace SPF.Data
{
    interface ICacheable
    {
        /// <summary>
        /// Add a key/value item into cache.
        /// </summary>
        void Add();

        /// <summary>
        /// Get cache value by key
        /// </summary>
        /// <param name="key"></param>
        Object Get(string key);

        /// <summary>
        /// Judge whether the cache is alive
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsAlive(string key);

        void Remove(string key);

        void RemoveAll();
    }
}
