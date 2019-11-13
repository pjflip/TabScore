using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class DBInfo
    {
        // Check if this is an individual event, when RoundData will have a 'South' column
        public static string IsIndividual(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT TOP 1 South FROM RoundData";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteScalar();
                    return "YES";
                }
                catch (OdbcException)
                {
                    return "NO";
                }
                finally
                {
                    cmd.Dispose();
                }
            } 
        }

        // Find out how many rounds there are
        public static int MaxRounds(string DB, int sectionID)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = SQLString = $"SELECT MAX(Round) FROM RoundData WHERE Section={sectionID}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    return Convert.ToInt32(queryResult);
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
        }
    }
}