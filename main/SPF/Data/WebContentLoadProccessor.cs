using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SPF.Configuration;
using SPF.Web;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Web;

namespace SPF.Data
{
    internal class WebContentLoadProccessor:IContentLoadProccessor<byte[]>
    {
        /// <summary>
        /// Load a specific web resource without parameters
        /// This method requires the HttpVerb to be "GET"
        /// </summary>
        /// <param name="configElement"></param>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public byte[] Load(ConfigurationElement configElement, string itemKey)
        {
            var webSourceSettings = configElement as WebSourceElement;
            var webSourceItemSettings = webSourceSettings.WebSourceItems[itemKey];
            var webSourceResource = webSourceItemSettings.WebSourceItemProccessors["ContentLoader"].WebSourceItemProccessorResources;

            var httpVerb = webSourceItemSettings.WebSourceHttpParam.HttpVerb;

            byte[] rawData = null;

            Uri sourceUri = getSourceUri(webSourceSettings.BaseUrl, webSourceResource[0].Url);

            if (sourceUri.IsFile)
            {
                rawData = loadLocalSource(sourceUri);
                return rawData;
            }
            else
            {
                rawData = loadRemoteSource(sourceUri, httpVerb, webSourceItemSettings.ContentType);
                return rawData;
            }
        }

        /// <summary>
        /// Load a specific web resource within parameters
        /// </summary>
        /// <param name="configElement"></param>
        /// <param name="itemKey"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public byte[] Load(ConfigurationElement configElement, string itemKey, NameValueCollection paras)
        {
            var webSourceSettings = configElement as WebSourceElement;
            var webSourceItemSettings = webSourceSettings.WebSourceItems[itemKey];
            var webSourceResource = webSourceItemSettings.WebSourceItemProccessors["ContentLoader"].WebSourceItemProccessorResources;

            var httpVerb = webSourceItemSettings.WebSourceHttpParam.HttpVerb;

            byte[] rawData = null;

            Uri sourceUri = getSourceUri(webSourceSettings.BaseUrl, webSourceResource[0].Url,paras);

            if (sourceUri.IsFile)
            {
                rawData = loadLocalSource(sourceUri);
                return rawData;
            }
            else
            {
                rawData = loadRemoteSource(sourceUri, httpVerb, webSourceItemSettings.ContentType);
                return rawData;
            }
        }

        #region

        /// <summary>
        /// Combine baseUri and relativeUri without parameters
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="relativeUri"></param>
        /// <returns></returns>
        private Uri getSourceUri(string baseUri, string relativeUri)
        {
            return new Uri(String.Concat(baseUri, relativeUri));
        }

        /// <summary>
        /// Combine baseUri and relativeUri within parameters
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="relativeUri"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        private Uri getSourceUri(string baseUri, string relativeUri, NameValueCollection paras)
        {
            try
            {
                for (int i = 0; i < paras.Count; i++)
                {
                    string key = paras.GetKey(i);
                    string value = HttpUtility.UrlEncode(paras.Get(i));
                    relativeUri = relativeUri.Replace(String.Concat("${", key, "}"), value);
                }
                return new Uri(String.Concat(baseUri, relativeUri));
            }
            catch (ArgumentOutOfRangeException argOutofRangeEx)
            {
                throw new ArgumentOutOfRangeException("parameters out of range:", argOutofRangeEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException("parameter exception:", argEx);
            }
            catch (Exception commonEx)
            {
                throw new Exception("common exception:", commonEx);
            }
        }

        /// <summary>
        /// Load local file in IO stream
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <returns></returns>
        private byte[] loadLocalSource(Uri sourceUri)
        {
            byte[] sourceContent = null;
            try
            {
                string filePath = sourceUri.AbsolutePath.ToString();
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        StreamReader sReader = new StreamReader(fileStream);
                        sourceContent = UTF8Encoding.UTF8.GetBytes(sReader.ReadToEnd());
                    }
                }
                else
                {
                    sourceContent = null;
                }
                return sourceContent;
            }
            catch (IOException ioEx)
            {
                throw new IOException("IO Error:", ioEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException("ArgumentNull Error:", argEx);
            }
            catch (Exception commonEx)
            {
                throw new Exception("Common Error:", commonEx);
            }
        }

        /// <summary>
        /// Load Remote source file by httprequest
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <param name="type"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        private byte[] loadRemoteSource(Uri sourceUri, HttpVerb verb, ContentType type)
        {
            byte[] sourceContent = null;

            HttpWebRequest request = WebRequest.Create(sourceUri) as HttpWebRequest;

            request.Method = WebSourceRequest.HttpVerbTypeInfo[verb];
            request.ContentType = WebSourceRequest.ContentTypeInfo[type];

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader sReader = new StreamReader(response.GetResponseStream());
                    sourceContent = UTF8Encoding.UTF8.GetBytes(sReader.ReadToEnd());
                }
                return sourceContent;
            }
            catch (WebException webEx)
            {
                throw new WebException("Server Error:", webEx);
            }
            catch (Exception commonEx)
            {
                throw new Exception("Common Error:", commonEx);
            }
        }

        #endregion
    }
}
