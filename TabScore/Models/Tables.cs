using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Tables
    {
        public static bool IsLoggedOn(string DB, string sectionID, string table)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                object queryResult = cmd.ExecuteScalar();
                cmd.Dispose();
                if (queryResult.ToString() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void Logon(string DB, string sectionID, string table)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"UPDATE Tables SET LogOnOff=1 WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}