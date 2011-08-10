using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website
{
    public partial class TestDelegate : System.Web.UI.Page
    {
        public delegate string ConcatDelegate(string a, string b);

        public string add(string a,string b)
        {
            return a+b;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConcatDelegate myConcatDelegate = new ConcatDelegate(add);
            Response.Write(myConcatDelegate("a", "b"));
        }

    }
}