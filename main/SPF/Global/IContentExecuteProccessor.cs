using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SPF.Data
{
    /// <summary>
    /// Interface for all kinds of content execute class
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="P"></typeparam>
    public interface IContentExecuteProccessor<R>:IProccessor
    {
        R Execute(ConfigurationElement configElement, string itemKey, NameValueCollection paras);
    }
}
