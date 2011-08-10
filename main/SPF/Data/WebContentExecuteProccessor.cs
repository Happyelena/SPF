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
    internal class WebContentExecuteProccessor:IContentExecuteProccessor<byte[]>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configElement"></param>
        /// <param name="itemKey"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public byte[] Execute(ConfigurationElement configElement, string itemKey, NameValueCollection paras)
        {
            var webSourceSettings = configElement as WebSourceElement;
            var webSourceItemSettings = webSourceSettings.WebSourceItems[itemKey];
            var webSourceResource = webSourceItemSettings.WebSourceItemProccessors["ContentExecutor"].WebSourceItemProccessorResources;

            var httpVerb = webSourceItemSettings.WebSourceHttpParam.HttpVerb;

            byte[] rawData = null;
            byte[] postData = null;

            postData = encodeCollection(paras);
            Uri sourceUri = getSourceUri(webSourceSettings.BaseUrl, webSourceResource[0].Url);

            rawData = doExecute(sourceUri, httpVerb, webSourceItemSettings.ContentType, postData);

            return rawData;
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
        /// Encode a dictionary of key/value pairs as an HTTP query string.
        /// </summary>
        /// <param name="dict">The dictionary to encode</param>
        private byte[] encodeCollection(NameValueCollection paraCollection)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < paraCollection.Count; i++)
            {
                sb.Append(HttpUtility.UrlEncode(paraCollection.Keys[i]));
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(paraCollection.Get(i)));
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1); // Remove trailing &
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Load Remote source file by post httprequest
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <param name="verb"></param>
        /// <param name="type"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private byte[] doExecute(Uri sourceUri, HttpVerb verb, ContentType type, byte[] postData)
        {
            byte[] sourceContent = null;

            HttpWebRequest request = WebRequest.Create(sourceUri) as HttpWebRequest;

            request.Method = WebSourceRequest.HttpVerbTypeInfo[verb];
            request.ContentType = WebSourceRequest.ContentTypeInfo[type];
            request.ContentLength = postData.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            requestStream.Close();

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
