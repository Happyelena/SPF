using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using SPF.Configuration;

namespace SPF.Data
{
    internal class SqlContentExecuteProcessor : IContentExecuteProccessor<int>
    {
        public int Execute(System.Configuration.ConfigurationElement configElement, string itemKey, NameValueCollection paras)
        {
            int effectedRowCount = 0;

            var sqlSourecSettings = configElement as SqlSourceElement;
            var sqlConnectionProfile = sqlSourecSettings.ConnectionPorfile;
            var sqlSourceItemSettings = sqlSourecSettings.SqlSourceItems[itemKey];
            var sqlSourceItemExecuteProccessorSettings = sqlSourceItemSettings.SqlSourceProccessors["ContentExecuter"];
            var sqlSourceContentExecuteSettings = sqlSourceItemExecuteProccessorSettings.SqlSourceItemLoadCommands["SqlCommand"];
            var sqlSourceItemArgs = sqlSourceItemExecuteProccessorSettings.SqlSourceItemArgs;

            string connectionString = ConfigurationManager.ConnectionStrings[sqlConnectionProfile].ConnectionString;
            string sqlCommandText = getSqlCommandText(sqlSourceContentExecuteSettings.CommandText, paras, sqlSourceItemArgs).Replace("\r\n", String.Empty);
            CommandType commandType = sqlSourceContentExecuteSettings.CommandType;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, connection))
                {
                    try
                    {
                        sqlCommand.CommandType = commandType;
                        connection.Open();
                        if (sqlCommandText.Contains("SELECT @@IDENTITY"))
                        {
                            effectedRowCount = int.Parse(sqlCommand.ExecuteScalar().ToString().Trim());
                        }
                        else
                        {
                            effectedRowCount = sqlCommand.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new Exception("SqlContentLoadProccessor exception:", sqlEx);
                    }
                    catch (InvalidOperationException invalidOpEx)
                    {
                        throw new InvalidOperationException("SqlContentLoadProccessor exception:", invalidOpEx);
                    }
                }
            }
            return effectedRowCount;
        }

        #region
        private string getSqlCommandText(string orginalSqlCommand, NameValueCollection paras, SqlSourceItemArgsElementCollection sqlArgs)
        {
            foreach (SqlSourceItemArgsElement sqlArg in sqlArgs)
            {
                if (orginalSqlCommand.Contains(String.Concat("${", sqlArg.Key,"}")))
                {
                    orginalSqlCommand = orginalSqlCommand.Replace(String.Concat("${", sqlArg.Key, "}"), paras[sqlArg.Key]);
                }
                else
                {
                    orginalSqlCommand = orginalSqlCommand.Replace(String.Concat("${", sqlArg.Key, "}"), sqlArg.Value);
                }
            }
            return orginalSqlCommand;
        }
        #endregion

    }
}