using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPF
{
    public enum SourceType
    {
        WebDataSource,
        SqlDataSource,
    }

    interface ISourceContent<R>
    {
        bool Match(string key, string originalKey);

        R LoadContent();
    }
}
