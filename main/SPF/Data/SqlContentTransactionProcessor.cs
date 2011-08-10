using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using SPF;
using SPF.Configuration;

namespace SPF.Data
{
    internal class SqlContentTransactionProcessor : IContentExecuteProccessor<bool>
    {
        public bool Execute(ConfigurationElement configElement, string itemKey, NameValueCollection paras)
        {
            var transactionExecuteResult = false;

            var sqlSourecSettings = configElement as SqlSourceElement;
            var sqlConnectionProfile = sqlSourecSettings.ConnectionPorfile;
            var sqlSourceItemSettings = sqlSourecSettings.SqlSourceItems[itemKey];
            var sqlSourceItemExecuteProccessorSettings = sqlSourceItemSettings.SqlSourceProccessors["TranscationExecuter"];
            var sqlSourceContentExecuteSettings = sqlSourceItemExecuteProccessorSettings.SqlSourceItemLoadCommands;
            var sqlSourceItemArgs = sqlSourceItemExecuteProccessorSettings.SqlSourceItemArgs;

            string connectionString = ConfigurationManager.ConnectionStrings[sqlConnectionProfile].ConnectionString;
            string[] sqlCommandArray = getSqlCommandCollection(sqlSourceContentExecuteSettings, paras, sqlSourceItemArgs);

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlTransaction sqlTransaction;
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlConn.Open();
                    sqlTransaction = sqlConn.BeginTransaction();
                    sqlCommand.Connection = sqlConn;
                    sqlCommand.Transaction = sqlTransaction;
                    try
                    {
                        foreach (string sqlCommandStr in sqlCommandArray)
                        {
                            sqlCommand.CommandText = sqlCommandStr;
                            sqlCommand.ExecuteNonQuery();
                        }
                        sqlTransaction.Commit();
                        transactionExecuteResult = true;
                    }
                    catch (SqlException sqlEx)
                    {
                        sqlTransaction.Rollback();
                        throw sqlEx;
                    }
                }
            }
            return transactionExecuteResult;
        }


        #region

        private string[] getSqlCommandCollection(SqlSourceItemLoadCommandElementCollection sqlSourceContentExecuteSettings, NameValueCollection paras, SqlSourceItemArgsElementCollection sqlSourceItemArgs)
        {
            int sqlCommandCount = sqlSourceContentExecuteSettings.Count;
            string[] sqlCommandArray = new string[sqlCommandCount];
            for(int i=0;i<sqlCommandCount;i++)
            {
                sqlCommandArray[i] = sqlSourceContentExecuteSettings[i].CommandText;
            }
            for (int j = 0; j < sqlCommandCount; j++)
            {
                foreach (SqlSourceItemArgsElement arg in sqlSourceItemArgs)
                {
                    if (sqlCommandArray[j].Contains(String.Concat("${", arg.Key, "}")))
                    {
                        if (arg.Value.Contains(String.Concat("${", arg.Key, "}")))
                        {
                            sqlCommandArray[j] = sqlCommandArray[j].Replace(String.Concat("${", arg.Key, "}"), paras[arg.Key]).Replace("\r\n", String.Empty);
                        }
                        else
                        {
                            sqlCommandArray[j] = sqlCommandArray[j].Replace(String.Concat("${", arg.Key, "}"), arg.Value).Replace("\r\n", String.Empty);
                        }
                    }
                }
            }
            return sqlCommandArray;
        }

        #endregion
    }
}
