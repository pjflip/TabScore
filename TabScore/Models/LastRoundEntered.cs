using System.Data.Odbc;

namespace TabScore.Models
{
    public class LastRoundEntered
    {
        public static int Get(string DB, int sectionID, int table)
        {
            int maxRound = 1;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT Round FROM ReceivedData WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int thisRound = reader.GetInt32(0);
                            if (thisRound > maxRound)
                            {
                                maxRound = thisRound;
                            }
                        }
                    });
                }
                catch (OdbcException)
                {
                    return -1;
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
            return maxRound;
        }
    }
}