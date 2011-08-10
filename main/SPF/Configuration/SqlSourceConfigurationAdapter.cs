using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SPF.Configuration
{
    internal sealed class SqlSourceConfigurationAdapter:IConfiguratonAdapter<SqlSourceConfiguration>
    {
        public SqlSourceConfiguration Fill()
        {
            try
            {
                var sqlSourceConfig = (ConfigurationManager.GetSection("SourceContent/SqlSourceContent") as SqlSourceConfiguration);
                return sqlSourceConfig;
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
