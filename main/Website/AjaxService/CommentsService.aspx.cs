using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPF.Sql;

namespace Website.AjaxService
{
    public partial class CommentsService : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string opType = Request.QueryString["op"].ToString();
            string msg = String.Empty;
            switch(opType.ToLower())
            {
                case ("addcomments"):
                    {
                        string userId = Request["userId"].ToString().Trim();
                        string articleId = Request["articleId"].ToString().Trim();
                        string commentsContent = Request["commentsContent"].ToString().Trim();
                        NameValueCollection paras = new NameValueCollection()
                        {
                            {"userId",userId},
                            {"articleId",articleId},
                            {"commentsContent",commentsContent},
                        };
                        SqlSourceContent<object> sqlcontent = new SqlSourceContent<object>()
                        {
                            SourceKey = "SqlCommand",
                            ItemKey = "AddComment/articleId=" + articleId + "&userId=" + userId,
                            Paras=paras,
                        };
                        if (sqlcontent.ExecuteTransaction())
                        {
                            msg = "success";
                        }
                        else
                        {
                            msg = "failed";
                        }
                        Response.Write(msg);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}