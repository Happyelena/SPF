using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SPF.Data;

namespace SPF.Web
{
    /// <summary>
    /// Http request Verb enumeration
    /// </summary>
    public enum HttpVerb
    {
        GET,
        PUT,
        POST,
        DELETE,
    }

    /// <summary>
    /// Http request Content-Type enumeration
    /// </summary>
    public enum ContentType
    {
        RSS,
        POX,
        ATOM,
        JSON,
        HTML,
        WEBFORM,
    }


    /// <summary>
    /// This is web data request entity class
    /// </summary>
    internal class WebSourceRequest
    {
        public static readonly Dictionary<ContentType, string> ContentTypeInfo = new Dictionary<ContentType, string>()
        {
            {ContentType.RSS,"application/rss+xml"},
            {ContentType.POX,"text/xml"},
            {ContentType.ATOM,"application/atom+xml"},
            {ContentType.JSON,"application/json"},
            {ContentType.HTML,"text/html"},
            {ContentType.WEBFORM,"application/x-www-form-urlencoded"},
        };

        public static readonly Dictionary<HttpVerb, string> HttpVerbTypeInfo = new Dictionary<HttpVerb, string>()
        {
            {HttpVerb.GET,"GET"},
            {HttpVerb.POST,"POST"},
            {HttpVerb.PUT,"PUT"},
            {HttpVerb.DELETE,"DELETE"},
        };
    }

}
