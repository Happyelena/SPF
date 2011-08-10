using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;


namespace SPF.Data
{
    /// <summary>
    /// Interface for all kinds of content loader class
    /// </summary>
    /// <typeparam name="R">RawData when content is loaded</typeparam>
    /// <typeparam name="P">Parameters</typeparam>
    interface IContentLoadProccessor<R>:IProccessor
    {
        R Load(ConfigurationElement configElement, string itemKey);

        R Load(ConfigurationElement configElement, string itemKey, NameValueCollection paras);
    }
}
