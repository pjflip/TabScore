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
                object queryResult;
                if (dir == "NS")
                {
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pair}";
                }
                else
                {
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pair}";
                }
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                queryResult = cmd.ExecuteScalar();
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
                    cmd = new OdbcCommand(SQLString, connection);
                    queryResult = cmd.ExecuteScalar();
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
                cmd.Dispose();
            }
            return move;
        }

        public static string GetBoardMoveInfo(string DB, string sectionID, string round, string lowBoard)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult;
                string SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND LowBoard={lowBoard}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                queryResult = cmd.ExecuteScalar();
                cmd.Dispose();
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