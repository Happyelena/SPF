using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SPF.Data;

namespace SPF.Configuration
{
    internal sealed class WebSourceConfigurationAdapter : IConfiguratonAdapter<WebSourceConfiguration>
    {
        public WebSourceConfiguration Fill()
        {
            try
            {
                var webSourceConfig = (ConfigurationManager.GetSection("SourceContent/WebSourceContent") as WebSourceConfiguration);
                return webSourceConfig;
            }
            catch (ConfigurationException configEx)
            {
                throw configEx;
            }
            catch (Exception commonEx)
            {
                throw commonEx;
            }
        }
    }
}
