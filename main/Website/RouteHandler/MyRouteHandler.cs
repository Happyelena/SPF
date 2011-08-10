using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;

namespace Website
{
    public class MyRouteHandler:IRouteHandler
    {

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            foreach (KeyValuePair<string, object> token in requestContext.RouteData.Values)
            {
                requestContext.HttpContext.Items.Add(token.Key, token.Value);
            }
            IHttpHandler result = BuildManager.CreateInstanceFromVirtualPath("~/TestResult.aspx", typeof(TestResult)) as IHttpHandler;
            return result;
        }
    }
}