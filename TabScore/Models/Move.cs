using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Move
    {
        public int Table { get; }

        public string Direction { get; }

        public Move(string DB, int sectionID, int round, int pairOrPlayerNumber, bool individual, string dir = "")
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult = null;
                connection.Open();
                string SQLString;
                if (individual)
                {

                    // Try Direction = North
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pairOrPlayerNumber}";
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
                        Table = -1;
                        cmd.Dispose();
                        return;
                    }
                    if (queryResult != null)
                    {
                        Table = Convert.ToInt32(queryResult);
                        Direction = "North";
                        cmd.Dispose();
                        return;
                    }

                    // Try Direction = South
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND South={pairOrPlayerNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            queryResult = cmd.ExecuteScalar();
                        });
                    }
                    catch (OdbcException)
                    {
                        Table = -1;
                        cmd.Dispose();
                        return;
                    }
                    if (queryResult != null)
                    {
                        Table = Convert.ToInt32(queryResult);
                        Direction = "South";
                        cmd.Dispose();
                        return;
                    }

                    // Try Direction = East
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pairOrPlayerNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            queryResult = cmd.ExecuteScalar();
                        });
                    }
                    catch (OdbcException)
                    {
                        Table = -1;
                        cmd.Dispose();
                        return;
                    }
                    if (queryResult != null)
                    {
                        Table = Convert.ToInt32(queryResult);
                        Direction = "East";
                        cmd.Dispose();
                        return;
                    }

                    // Try Direction = West
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND West={pairOrPlayerNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            queryResult = cmd.ExecuteScalar();
                        });
                    }
                    catch (OdbcException)
                    {
                        Table = -1;
                        cmd.Dispose();
                        return;
                    }
                    if (queryResult != null)
                    {
                        Table = Convert.ToInt32(queryResult);
                        Direction = "West";
                        cmd.Dispose();
                        return;
                    }
                    else   // No move info found - move to sit out
                    {
                        Table = 0;
                        Direction = "";
                        return;
                    }
                }

                else  // Not individual event
                {
                    if (dir == "NS")
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pairOrPlayerNumber}";
                    }
                    else
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pairOrPlayerNumber}";
                    }

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
                        Table = -1;
                    }
                    finally
                    {
                        cmd1.Dispose();
                    }

                    if (queryResult != null)
                    {
                        Table = Convert.ToInt32(queryResult);
                        Direction = dir;
                        return;
                    }
                    else
                    {
                        // Pair changes Direction
                        if (dir == "NS")
                        {
                            SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND EWPair={pairOrPlayerNumber}";
                        }
                        else
                        {
                            SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sectionID} AND Round={round} AND NSPair={pairOrPlayerNumber}";
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
                            Table = -1;
                        }
                        finally
                        {
                            cmd2.Dispose();
                        }

                        if (queryResult != null)
                        {
                            Table = Convert.ToInt32(queryResult);
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
                            Table = 0;
                            Direction = "";
                        }
                    }
                }
            }
        }
    }
}
