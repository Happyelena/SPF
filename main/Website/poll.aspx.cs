#undef web

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SPF.Sql;

namespace Website
{
    public partial class poll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
#if web
            string xmlText = @"<?xml version='1.0' encoding='utf-8' ?>
                               <root>
                               <data><![CDATA[This is the test data]]></data>
                               </root>";
            Response.Write(xmlText);
#else
            string libName = Request["txtLibName"].ToString().Trim();
            string libAddress = Request["txtLibAddress"].ToString().Trim();
            string libPhone = Request["txtLibPhone"].ToString().Trim();
            NameValueCollection paras = new NameValueCollection()
            {
                {"libName",libName},
                {"libAddress",libAddress},
                {"libPhone",libPhone},
            };
            SqlSourceContent<object> sqlsourceContent = new SqlSourceContent<object>()
            {
                SourceKey = "SqlCommand",
                ItemKey = "AddLibInfo",
                Paras = paras
            };
            int identifyValue = sqlsourceContent.ExecuteNonQueryReturnIdentifyValue();
            if (identifyValue > 0)
            {
                Response.Write("Success and the identify value is" + identifyValue.ToString());
            }
            else
            {
                Response.Write("Failed");
            }
#endif
        }
    }
}