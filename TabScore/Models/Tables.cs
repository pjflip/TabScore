using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Tables
    {
        public static int IsLoggedOn(string DB, string sectionID, string table)
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                catch (OdbcException)
                {
                    return -1;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return Convert.ToInt32(queryResult);
        }

        public static int Logon(string DB, string sectionID, string table)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"UPDATE Tables SET LogOnOff=1 WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                catch (OdbcException)
                {
                   return -1;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return 0;
        }
    }
}