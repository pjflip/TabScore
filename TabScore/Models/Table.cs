using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Table
    {
        public int LogonStatus { get; private set; }
        
        private readonly int TableNumber;
        private readonly int SectionID;

        public Table(string DB, int sectionID, int tableNumber)
        {
            TableNumber = tableNumber;
            SectionID = sectionID;

            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section={sectionID} AND [Table]={tableNumber}";
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
            LogonStatus = Convert.ToInt32(queryResult);
        }

        public void Logon(string DB)
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
                    LogonStatus = 1;
                    cmd.Dispose();
                }
            }
        }
    }
}