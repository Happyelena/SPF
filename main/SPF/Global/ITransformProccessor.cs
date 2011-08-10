using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SPF
{
    public interface ITransformProccessor<T,R>:IProccessor
    {
        T Transform(ConfigurationElement configElement, R rawData);

        T Transform(ConfigurationElement configElement, R rawData, NameValueCollection paras);
    }
}
