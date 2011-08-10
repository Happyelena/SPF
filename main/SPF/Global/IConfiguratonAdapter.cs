using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPF.Configuration
{
    /// <summary>
    /// Fill in the specific confuguration section profile
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IConfiguratonAdapter<T>
    {
        T Fill();
    }
}
