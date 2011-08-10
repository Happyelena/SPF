using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPF.Configuration;
using SPF.Data;
using SPF.Data.DataCache;

namespace SPF.Web
{
    public class JsonWebSourceContent<T>:WebSourceContent<T>
    {
        private JSONObject rawData;

        private string rawDataStr;

        private T transformedData;

        public JSONObject RawData
        {
            get
            {
                if (rawData == null)
                {
                    rawData = outputRawData(base.LoadContent());
                }
                return rawData;
            }
            private set { }
        }

        public string RawDataString
        {
            get
            {
                if (rawDataStr == null&&rawData!=null)
                {
                    rawDataStr = rawData.ToDisplayableString();
                }
                return rawDataStr;
            }
            private set { }
        }

        public T TransformData
        {
            get
            {
                if (transformedData == null)
                {
                    transformedData = base.LoadContent(outputTransformedData);
                }
                return transformedData;
            }
            private set { }
        }

        public JSONObject GetSubmitResponseString()
        {
            return outputRawData(base.SubmitContent());
        }

        public T GetTransformSubmitResponse()
        {
            return base.SubmitContent(getTransformSubmitResponse);
        }

        public override void CacheCallBack(string key, System.Web.Caching.CacheItemUpdateReason cacheItemUpdateReason, out object expensiveObject, out System.Web.Caching.CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration)
        {
            key = String.Concat(SourceKey, "_", ItemKey);
            WebDataCache<T> dataCache =new WebDataCache<T>(key);
            try
            {
                expensiveObject = TransformData;
                if (expensiveObject == null)
                    expensiveObject = dataCache.Get(key);
            }
            catch
            {
                // Instead of throwing errors, use the old cache.
                expensiveObject = dataCache.Get(key);
            }
            int sourceItemCacheDuration = 0;
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
                                    sourceItemCacheDuration = (webSourceItemElement.WebSourceItemCache.CacheDuration == 0) ? sourceCacheDuration : webSourceItemElement.WebSourceItemCache.CacheDuration;
                                }
                            }
                        }
                    }
                }
            }
            absoluteExpiration = DateTime.Now.AddSeconds(sourceItemCacheDuration);
            dependency = null;
            slidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
        }

        #region

        /// <summary>
        /// Output the raw data
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private JSONObject outputRawData(byte[] rawData)
        {
            return JSONObject.CreateFromString(UTF8Encoding.UTF8.GetString(rawData));
        }

        /// <summary>
        /// Output the transformed data
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private T outputTransformedData(byte[] rawData)
        {
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
                                var proccessorCollection = webSourceItemElement.WebSourceItemProccessors;
                                var proccessor = Activator.CreateInstance(Type.GetType(proccessorCollection[0].Type)) as ITransformProccessor<T, byte[]>;
                                if (Paras == null)
                                {
                                    transformedData = proccessor.Transform(proccessorCollection[0], rawData);
                                }
                                else
                                {
                                    transformedData = proccessor.Transform(proccessorCollection[0], rawData, Paras);
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
            return transformedData;
        }

        private T getTransformSubmitResponse(byte[] responseData)
        {
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
                                var proccessorCollection = webSourceItemElement.WebSourceItemProccessors;
                                var proccessor = Activator.CreateInstance(Type.GetType(proccessorCollection[0].Type)) as ITransformProccessor<T, byte[]>;
                                transformedData = proccessor.Transform(proccessorCollection[0], responseData, Paras);
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
            return transformedData;
        }

        #endregion
    }
}
