using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using SPF.Configuration;

namespace SPF.Data
{
    internal class SqlContentLoadProccessor : IContentLoadProccessor<DataTable>
    {
        public DataTable Load(System.Configuration.ConfigurationElement configElement, string itemKey)
        {
            var sqlSourecSettings = configElement as SqlSourceElement;
            var sqlConnectionProfile = sqlSourecSettings.ConnectionPorfile;
            var sqlSourceItemSettings = sqlSourecSettings.SqlSourceItems[itemKey];
            var sqlSourceItemLoadProccessorSettings = sqlSourceItemSettings.SqlSourceProccessors["ContentLoader"];
            var sqlSourceContentLoadSettings = sqlSourceItemLoadProccessorSettings.SqlSourceItemLoadCommands["SqlCommand"];

            string connectionString = ConfigurationManager.ConnectionStrings[sqlConnectionProfile].ConnectionString;
            string sqlCommandText = sqlSourceContentLoadSettings.CommandText.Replace("//r//n", String.Empty);
            CommandType commandType = sqlSourceContentLoadSettings.CommandType;

            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, connection))
                {
                    try
                    {
                        sqlCommand.CommandType = commandType;
                        connection.Open();
                        SqlDataAdapter sqlDataAdpater = new SqlDataAdapter(sqlCommand);
                        sqlDataAdpater.Fill(data);
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
            return data;
        }

        public DataTable Load(System.Configuration.ConfigurationElement configElement, string itemKey, NameValueCollection paras)
        {
            var sqlSourecSettings = configElement as SqlSourceElement;
            var sqlConnectionProfile = sqlSourecSettings.ConnectionPorfile;
            var sqlSourceItemSettings = sqlSourecSettings.SqlSourceItems[itemKey];
            var sqlSourceItemLoadProccessorSettings = sqlSourceItemSettings.SqlSourceProccessors["ContentLoader"];
            var sqlSourceContentLoadSettings = sqlSourceItemLoadProccessorSettings.SqlSourceItemLoadCommands["SqlCommand"];
            var sqlSourceItemArgs = sqlSourceItemLoadProccessorSettings.SqlSourceItemArgs;

            string connectionString = ConfigurationManager.ConnectionStrings[sqlConnectionProfile].ConnectionString;
            string sqlCommandText = getSqlCommandText(sqlSourceContentLoadSettings.CommandText, paras, sqlSourceItemArgs).Replace("\r\n", String.Empty);
            CommandType commandType = sqlSourceContentLoadSettings.CommandType;

            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, connection))
                {
                    try
                    {
                        sqlCommand.CommandType = commandType;
                        connection.Open();
                        SqlDataAdapter sqlDataAdpater = new SqlDataAdapter(sqlCommand);
                        sqlDataAdpater.Fill(data);
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
            return data;
        }

        #region

        /// <summary>
        /// Replace the args in sql command
        /// </summary>
        /// <param name="orginalSqlCommand"></param>
        /// <param name="paras"></param>
        /// <param name="sqlArgs"></param>
        /// <returns></returns>
        private string getSqlCommandText(string orginalSqlCommand, NameValueCollection paras, SqlSourceItemArgsElementCollection sqlArgs)
        {
            foreach (SqlSourceItemArgsElement sqlArg in sqlArgs)
            {
                foreach (string key in paras.AllKeys)
                {
                    if (sqlArg.Value.Contains("${"))
                    {
                        orginalSqlCommand = orginalSqlCommand.Replace(String.Concat("${", sqlArg.Key, "}"), paras[key]);
                    }
                    else
                    {
                        orginalSqlCommand = orginalSqlCommand.Replace(String.Concat("${", sqlArg.Key, "}"), sqlArg.Value);
                    }
                }
            }
            return orginalSqlCommand;
        }

        #endregion
    }
}
