using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SPF.Configuration;
using SPF.Data;
using SPF.Data.DataCache;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Caching;

namespace SPF.Web
{
    public abstract class WebSourceContent<T> : ISourceContent<byte[]>,ICacheUpdateCallBack
    {
        private WebSourceConfiguration webSourceConfig;

        public string SourceKey { get; set; }

        public string ItemKey { get; set; }

        public NameValueCollection Paras { get; set; }

        public SourceType SourceType
        {
            get
            {
                return SourceType.WebDataSource;
            }
            private set { }

        }

        public delegate T TransformDelgate(byte[] rawData);

        /// <summary>
        /// Load WebSourceConfiguration Settings
        /// </summary>
        protected WebSourceConfiguration WebSourceConfig
        {
            get
            {
                if (webSourceConfig == null)
                {
                    webSourceConfig = new WebSourceConfigurationAdapter().Fill();
                }
                return webSourceConfig;
            }
            private set { }
        }

        public WebSourceContent() { }

        public bool Match(string key, string originalKey)
        {
            if (Regex.IsMatch(key, originalKey, RegexOptions.IgnorePatternWhitespace) || Regex.IsMatch(key, originalKey, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[] LoadContent()
        {
            byte[] rawData = null;
            try
            {
                foreach (WebSourceElement webSourceElement in WebSourceConfig.WebSourceCollection)
                {
                    if (Match(SourceKey, webSourceElement.Key))
                    {
                        foreach (WebSourceItemElement webSourceItemElement in webSourceElement.WebSourceItems)
                        {
                            if (Match(ItemKey, webSourceItemElement.Key))
                            {
                                string proccessorType = webSourceItemElement.WebSourceItemProccessors["ContentLoader"].Type;
                                var contentLoadProccessor = Activator.CreateInstance(Type.GetType(proccessorType)) as WebContentLoadProccessor;
                                if (Paras == null)
                                {
                                    rawData = contentLoadProccessor.Load(webSourceElement, webSourceItemElement.Key);
                                }
                                else
                                {
                                    rawData = contentLoadProccessor.Load(webSourceElement, webSourceItemElement.Key, Paras);
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException("webcontent load null exception:", nullEx);
            }
            catch (Exception commmonEx)
            {
                throw new Exception(String.Concat("webcontent load common exception:", commmonEx.Message));
            }
            return rawData;
        }

        public T LoadContent(TransformDelgate transformDelegate)
        {
            string key = String.Concat(SourceKey, "_", ItemKey);
            WebDataCache<T> dataCache = new WebDataCache<T>(key);
            if (dataCache.IsAlive(key))
            {
                return (T)dataCache.Get(key);
            }
            else
            {
                T value = transformDelegate(this.LoadContent());
                foreach (WebSourceElement webSourceElement in WebSourceConfig.WebSourceCollection)
                {
                    if (Match(SourceKey, webSourceElement.Key))
                    {
                        if (!webSourceElement.WebSourceCache.CacheEnableTag)
                        {
                            break;
                        }
                        else
                        {
                            int sourceCacheDuration = webSourceElement.WebSourceCache.CacheDuration;
                            foreach (WebSourceItemElement webSourceItemElement in webSourceElement.WebSourceItems)
                            {
                                if (Match(ItemKey, webSourceItemElement.Key))
                                {
                                    if (!webSourceItemElement.WebSourceItemCache.CacheEnableTag)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        int sourceItemCacheDuration = (webSourceItemElement.WebSourceItemCache.CacheDuration == 0) ? sourceCacheDuration : webSourceItemElement.WebSourceItemCache.CacheDuration;
                                        CacheItemPriority cacheItemPriority = webSourceItemElement.WebSourceItemCache.CacheItemPriority;
                                        CacheItemUpdateCallback cacheItemUpdateCallbackHandler;
                                        if (webSourceItemElement.WebSourceItemCache.FallBackTag)
                                        {
                                            cacheItemUpdateCallbackHandler = new CacheItemUpdateCallback(CacheCallBack);
                                            WebDataCache<T> webDataCache = new WebDataCache<T>(key, value, sourceItemCacheDuration, cacheItemPriority, cacheItemUpdateCallbackHandler);
                                            webDataCache.Add();
                                        }
                                        else
                                        {
                                            System.Web.HttpRuntime.Cache.Insert(key, value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return value;
            }

        }

        public byte[] SubmitContent()
        {
            byte[] rawData = null;
            try
            {
                foreach (WebSourceElement webSourceElement in WebSourceConfig.WebSourceCollection)
                {
                    if (Match(SourceKey, webSourceElement.Key))
                    {
                        foreach (WebSourceItemElement webSourceItemElement in webSourceElement.WebSourceItems)
                        {
                            if (Match(ItemKey, webSourceItemElement.Key))
                            {
                                string proccessorType = webSourceItemElement.WebSourceItemProccessors["ContentExecutor"].Type;
                                var contentLoadProccessor = Activator.CreateInstance(Type.GetType(proccessorType)) as WebContentExecuteProccessor;
                                rawData = contentLoadProccessor.Execute(webSourceElement, webSourceItemElement.Key, Paras);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException("webcontent submit null exception:", nullEx);
            }
            catch (Exception commmonEx)
            {
                throw new Exception(String.Concat("webcontent submit common exception:", commmonEx.Message));
            }
            return rawData;
        }

        public T SubmitContent(TransformDelgate transformDelegate)
        {
            return transformDelegate(this.SubmitContent());
        }

        public abstract void CacheCallBack(string key, CacheItemUpdateReason cacheItemUpdateReason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration);
    }
}
