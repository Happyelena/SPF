using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Text;
using SPF;
using SPF.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;

namespace Website
{
    public class ArticleTransformer:ITransformProccessor<ArticleInfo,byte[]>
    {

        public ArticleInfo Transform(System.Configuration.ConfigurationElement configElement, byte[] rawData)
        {
            return Transform(configElement, rawData, null);
        }

        public ArticleInfo Transform(System.Configuration.ConfigurationElement configElement, byte[] rawData, NameValueCollection paras)
        {
            string xmlDataStr = UTF8Encoding.UTF8.GetString(rawData);
            XElement xElement = XElement.Parse(xmlDataStr);

            XPathNavigator xNav = xElement.CreateNavigator();
            ArticleInfo article = new ArticleInfo()
            {
                Title = xNav.SelectSingleNode("data").Value,
            };
            return article;
        }
    }
}