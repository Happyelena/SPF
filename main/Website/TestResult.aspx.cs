#define web

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Specialized;
using System.Text;

using SPF.Configuration;
using SPF.Web;
using SPF.Sql;

namespace Website
{
    public partial class TestResult : System.Web.UI.Page
    {
        protected string testResult { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = (Request.QueryString["id"] == null) ? "1" : Request.QueryString["id"];
            //string id = Page.RouteData.Values["id"].ToString();
            NameValueCollection paras = new NameValueCollection()
            {
                {"id",id},
            };
#if web
            XmlWebSourceContent<string> webContent = new XmlWebSourceContent<string>()
            {
                SourceKey = "StaticContent",
                ItemKey = "article?id=" + id,
                Paras = paras,
            };
            testResult = webContent.TransformData;
#else
            SqlSourceContent<UserInfo> sqlContent = new SqlSourceContent<UserInfo>()
            {
                SourceKey = "SqlCommand",
                ItemKey = "GetUserById/id=" + id,
                Paras = paras,
            };
            testResult = (sqlContent.TransformedData as UserInfo).UName;
#endif

        }
    }
}