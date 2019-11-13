using System.Data.Odbc;

namespace TabScore.Models
{
    public static class BoardMove
    {
        public static string GetBoardMoveInfo(string DB, int sectionID, int round, int lowBoard)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult = null;
                string SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND LowBoard={lowBoard}";
                connection.Open();

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
                    return "Error";
                }
                finally
                {
                    cmd.Dispose();
                }

                if (queryResult != null)
                {
                    return queryResult.ToString();
                }
                else
                {
                    return "0";
                }
            }
        }
    }
}