using System.Data.Odbc;

namespace TabScore.Models
{
    public static class PlayerID
    {
        public static string GetNameFromPID(string DB, string PID)
        {
            object queryResult;
            string Name;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                OdbcCommand cmd = new OdbcCommand("SELECT Name FROM PlayerNames WHERE ID=" + PID, connection);
                connection.Open();
                queryResult = cmd.ExecuteScalar();
                if (queryResult == null)
                {
                    Name = "Unknown #" + PID;
                }
                else
                {
                    Name = queryResult.ToString();
                }
                cmd.Dispose();
            }
            return Name;
        }
    }
}