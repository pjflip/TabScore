using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Move
    {
        public static MoveClass GetMoveInfo(string DB, string sectionID, string round, string pair, string dir)
        {
            MoveClass move = new MoveClass();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString;
                object queryResult = null;
                if (dir == "NS")
                {
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pair}";
                }
                else
                {
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pair}";
                }
                connection.Open();

                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd1.ExecuteScalar();
                    });
                }
                catch (OdbcException)
                {
                    move.Table = "Error";
                    return move;
                }
                finally
                {
                    cmd1.Dispose();
                }

                if (queryResult != null)
                {
                    move.Table = queryResult.ToString();
                    move.Direction = dir;
                }
                else
                {
                    // Pair changes direction
                    if (dir == "NS")
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pair}";
                    }
                    else
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pair}";
                    }

                    OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            queryResult = cmd2.ExecuteScalar();
                        });
                    }
                    catch (OdbcException)
                    {
                        move.Table = "Error";
                        return move;
                    }
                    finally
                    {
                        cmd2.Dispose();
                    }

                    if (queryResult != null)
                    {
                        move.Table = queryResult.ToString();
                        if (dir == "NS")
                        {
                            move.Direction = "EW";
                        }
                        else
                        {
                            move.Direction = "NS";
                        }
                    }
                    else   // No move info found - session complete
                    {
                        move.Table = "0";
                        move.Direction = "";
                    }
                }
            }
            return move;
        }

        public static string GetBoardMoveInfo(string DB, string sectionID, string round, string lowBoard)
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