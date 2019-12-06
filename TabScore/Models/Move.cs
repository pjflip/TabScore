using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Move
    {
        public int TableNumber { get; }
        public string Direction { get; }

        // Constructor for pairs move
        public Move(string DB, int sectionID, int round, int pairNumber, string dir)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult = null;
                connection.Open();
                string SQLString;
                if (dir == "NS")
                {
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pairNumber}";
                }
                else
                {
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pairNumber}";
                }

                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd1.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd1.Dispose();
                }

                if (queryResult != null)
                {
                    TableNumber = Convert.ToInt32(queryResult);
                    Direction = dir;
                    return;
                }
                else
                {
                    // Pair changes Direction
                    if (dir == "NS")
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pairNumber}";
                    }
                    else
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pairNumber}";
                    }

                    OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            queryResult = cmd2.ExecuteScalar();
                        });
                    }
                    finally
                    {
                        cmd2.Dispose();
                    }

                    if (queryResult != null)
                    {
                        TableNumber = Convert.ToInt32(queryResult);
                        if (dir == "NS")
                        {
                            Direction = "EW";
                        }
                        else
                        {
                            Direction = "NS";
                        }
                    }
                    else   // No move info found - move to sit out
                    {
                        TableNumber = 0;
                        Direction = "";
                    }
                }
            }
        }

        // Constructor for individual move
        public Move(string DB, int sectionID, int round, int playerNumber)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult = null;
                connection.Open();
                string SQLString;

                // Try Direction = North
                SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={playerNumber}";
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
                if (queryResult != null)
                {
                    TableNumber = Convert.ToInt32(queryResult);
                    Direction = "North";
                    return;
                }

                // Try Direction = South
                SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND South={playerNumber}";
                cmd = new OdbcCommand(SQLString, connection);
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
                if (queryResult != null)
                {
                    TableNumber = Convert.ToInt32(queryResult);
                    Direction = "South";
                    return;
                }

                // Try Direction = East
                SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={playerNumber}";
                cmd = new OdbcCommand(SQLString, connection);
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
                if (queryResult != null)
                {
                    TableNumber = Convert.ToInt32(queryResult);
                    Direction = "East";
                    return;
                }

                // Try Direction = West
                SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND West={playerNumber}";
                cmd = new OdbcCommand(SQLString, connection);
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
                if (queryResult != null)
                {
                    TableNumber = Convert.ToInt32(queryResult);
                    Direction = "West";
                    return;
                }
                else   // No move info found - move to sit out
                {
                    TableNumber = 0;
                    Direction = "";
                    return;
                }
            }
        }
    }
}
