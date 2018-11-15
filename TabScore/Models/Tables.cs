using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Tables
    {
        public static bool LogonTable(string DB, string sectionID, string table)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                object queryResult = cmd.ExecuteScalar();
                if (queryResult == null || queryResult.ToString() == "1")
                {
                    cmd.Dispose();
                    return false;
                }
                else
                {
                    SQLString = $"UPDATE Tables SET LogOnOff=1 WHERE Section={sectionID} AND [Table]={table}";
                    cmd = new OdbcCommand(SQLString, connection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return true;
                }
            }
        }

        public static void LogoffTable(string DB, string sectionID, string table)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"UPDATE Tables SET LogOnOff=2 WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}