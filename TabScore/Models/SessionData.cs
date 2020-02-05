using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class SessionData  // A class for bridge session data
    {
        public int TableNumber { get; set; }
        public int SectionID { get; set; }
        public string SectionLetter { get; set; }
        public string SectionTableString { get; set; }
        public int NumTables { get; set; }
        public int MissingPair { get; set; }
        public bool IsIndividual { get; private set; }

        public SessionData(string DB)
        {
            TableNumber = 0;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT TOP 1 South FROM RoundData";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteScalar();
                    IsIndividual = true;
                }
                catch (OdbcException)
                {
                    IsIndividual = false;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }

        public int TableLogonStatus(string DB)
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section={SectionID} AND [Table]={TableNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return Convert.ToInt32(queryResult);
        }

        public void LogonTable(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"UPDATE Tables SET LogOnOff=1 WHERE Section={SectionID} AND [Table]={TableNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
    }
}