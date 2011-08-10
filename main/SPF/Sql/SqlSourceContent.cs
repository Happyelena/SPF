using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SPF.Data.DataCache;
using SPF.Configuration;
using SPF.Data;

namespace SPF.Sql
{
    public class SqlSourceContent<T> : ISourceContent<DataTable>
    {
        private T transformedData;
        private SqlSourceConfiguration sqlSourceConfig;

        public string SourceKey { get; set; }

        public string ItemKey { get; set; }

        public NameValueCollection Paras { get; set; }

        public SourceType SourceType
        {
            get
            {
                return SourceType.SqlDataSource;
            }
            private set { }

        }

        public T TransformedData
        {
            get
            {
                if (transformedData == null)
                {
                    transformedData = this.LoadTransformedContent();
                }
                return transformedData;
            }
            private set { }
        }

        /// <summary>
        /// Load SqlSourceConfiguration Settings
        /// </summary>
        protected SqlSourceConfiguration SqlSourceConfig
        {
            get
            {
                if (sqlSourceConfig == null)
                {
                    sqlSourceConfig = new SqlSourceConfigurationAdapter().Fill();
                }
                return sqlSourceConfig;
            }
            private set { }
        }

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

        public DataTable LoadContent()
        {
            DataTable data = new DataTable();
            try
            {
                foreach (SqlSourceElement sqlSourceElement in SqlSourceConfig.SqlSourceElements)
                {
                    if (Match(SourceKey, sqlSourceElement.Key))
                    {
                        foreach (SqlSourceItemElement sqlSourceItemElement in sqlSourceElement.SqlSourceItems)
                        {
                            if (Match(ItemKey, sqlSourceItemElement.Key))
                            {
                                string proccessorType = sqlSourceItemElement.SqlSourceProccessors["ContentLoader"].Type;
                                var contentLoadProccessor = Activator.CreateInstance(Type.GetType(proccessorType)) as SqlContentLoadProccessor;
                                if (Paras == null)
                                {
                                    data = contentLoadProccessor.Load(sqlSourceElement, sqlSourceItemElement.Key);
                                }
                                else
                                {
                                    data = contentLoadProccessor.Load(sqlSourceElement, sqlSourceItemElement.Key, Paras);
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException("sqlcontent load null exception:", nullEx);
            }
            catch (Exception commmonEx)
            {
                throw new Exception(String.Concat("sqlcontent load common exception:", commmonEx.Message));
            }
            return data;
        }

        public T LoadTransformedContent()
        {
            try
            {
                string key = String.Concat(SourceKey, "_", ItemKey);
                SqlDataCache<T> sqlDataCache = new SqlDataCache<T>(key);
                if (sqlDataCache.IsAlive(key))
                {
                    transformedData = (T)sqlDataCache.Get(key);
                }
                else
                {
                    foreach (SqlSourceElement sqlSourceElement in SqlSourceConfig.SqlSourceElements)
                    {
                        if (Match(SourceKey, sqlSourceElement.Key))
                        {
                            foreach (SqlSourceItemElement sqlSourceItemElement in sqlSourceElement.SqlSourceItems)
                            {
                                bool IsCacheEnabled = sqlSourceItemElement.SqlSourceItemCacheElement.EnableCacheTag;
                                int sourceItemCacheDuration = IsCacheEnabled ? sqlSourceItemElement.SqlSourceItemCacheElement.CacheDuration : 0;
                                if (Match(ItemKey, sqlSourceItemElement.Key))
                                {
                                    DataTable rawData = this.LoadContent();
                                    var proccessorCollection = sqlSourceItemElement.SqlSourceProccessors;
                                    string processorName = proccessorCollection[0].Name;
                                    if (processorName.Equals("SqlObjectMappingProccessor"))
                                    {
                                        var objectType = sqlSourceItemElement.Type;
                                        NameValueCollection paras = new NameValueCollection()
                                    {
                                        {"ObjectType",objectType},
                                    };
                                        var proccessor = new SqlObjectMappingProccessor<T>();
                                        transformedData = proccessor.Transform(proccessorCollection[0], rawData, paras);
                                    }
                                    else
                                    {
                                        var proccessorType = sqlSourceItemElement.SqlSourceProccessors[0].Type;
                                        var proccessor = Activator.CreateInstance(Type.GetType(proccessorType)) as ITransformProccessor<T, DataTable>;
                                        if (Paras == null)
                                        {
                                            transformedData = proccessor.Transform(proccessorCollection[0], rawData);
                                        }
                                        else
                                        {
                                            transformedData = proccessor.Transform(proccessorCollection[0], rawData, Paras);
                                        }
                                    }
                                    if (IsCacheEnabled)
                                    {
                                        SqlDataCache<T> dataCache = new SqlDataCache<T>()
                                        {
                                            Key=key,
                                            Value=transformedData,
                                            Duration=sourceItemCacheDuration,
                                            DataCacheItemPriority=CacheItemPriority.Normal,
                                        };
                                        dataCache.Add();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException("sqlcontent load null exception:", nullEx);
            }
            catch (Exception commmonEx)
            {
                throw new Exception(String.Concat("sqlcontent load common exception:", commmonEx.Message));
            }
            return transformedData;
        }

        public bool ExecuteNonQuery()
        {
            bool executeResult = false;
            try
            {
                foreach (SqlSourceElement sqlSourceElement in SqlSourceConfig.SqlSourceElements)
                {
                    if (Match(SourceKey, sqlSourceElement.Key))
                    {
                        foreach (SqlSourceItemElement sqlSourceItemElement in sqlSourceElement.SqlSourceItems)
                        {
                            if (Match(ItemKey, sqlSourceItemElement.Key))
                            {
                                var proccessorType = sqlSourceItemElement.SqlSourceProccessors[0].Type;
                                var proccessor = Activator.CreateInstance(Type.GetType(proccessorType)) as SqlContentExecuteProcessor;
                                if (proccessor.Execute(sqlSourceElement, sqlSourceItemElement.Key, Paras)>0)
                                {
                                    executeResult = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException("sqlcontent load null exception:", nullEx);
            }
            catch (Exception commmonEx)
            {
                throw new Exception(String.Concat("sqlcontent load common exception:", commmonEx.Message));
            }
            return executeResult;
        }

        public int ExecuteNonQueryReturnIdentifyValue()
        {
            int effectedRowsCount = 0;
            try
            {
                foreach (SqlSourceElement sqlSourceElement in SqlSourceConfig.SqlSourceElements)
                {
                    if (Match(SourceKey, sqlSourceElement.Key))
                    {
                        foreach (SqlSourceItemElement sqlSourceItemElement in sqlSourceElement.SqlSourceItems)
                        {
                            if (Match(ItemKey, sqlSourceItemElement.Key))
                            {
                                var proccessorType = sqlSourceItemElement.SqlSourceProccessors[0].Type;
                                var proccessor = Activator.CreateInstance(Type.GetType(proccessorType)) as SqlContentExecuteProcessor;
                                effectedRowsCount = proccessor.Execute(sqlSourceElement, sqlSourceItemElement.Key, Paras);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException("sqlcontent load null exception:", nullEx);
            }
            catch (Exception commmonEx)
            {
                throw new Exception(String.Concat("sqlcontent load common exception:", commmonEx.Message));
            }
            return effectedRowsCount;
        }

        public bool ExecuteTransaction()
        {
            bool executeResult = false;
            try
            {
                foreach (SqlSourceElement sqlSourceElement in SqlSourceConfig.SqlSourceElements)
                {
                    if (Match(SourceKey, sqlSourceElement.Key))
                    {
                        foreach (SqlSourceItemElement sqlSourceItemElement in sqlSourceElement.SqlSourceItems)
                        {
                            if (Match(ItemKey, sqlSourceItemElement.Key))
                            {
                                var proccessorType = sqlSourceItemElement.SqlSourceProccessors[0].Type;
                                var proccessor = Activator.CreateInstance(Type.GetType(proccessorType)) as SqlContentTransactionProcessor;
                                if (proccessor.Execute(sqlSourceElement, sqlSourceItemElement.Key, Paras) )
                                {
                                    executeResult = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException("sqlcontent load null exception:", nullEx);
            }
            catch (Exception commmonEx)
            {
                throw new Exception(String.Concat("sqlcontent load common exception:", commmonEx.Message));
            }
            return executeResult;
        }
    }

}
