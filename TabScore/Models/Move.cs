using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Move
    {
        public int NewTableNumber { get; private set; }
        public string Direction { get; private set; }
        public string NewDirection { get; private set; }
        public bool Stay { get; private set; }
        public int PairNumber { get; private set; }

        public Move(SessionData sessionData, int newRoundNumber, int pairNumber, string direction)
        {
            PairNumber = pairNumber;
            Direction = direction;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                object queryResult = null;
                connection.Open();
                string SQLString;

                if (!AppData.IsIndividual)  // Pairs
                {
                    if (direction == "NS")
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND NSPair={pairNumber}";
                    }
                    else
                    {
                        SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND EWPair={pairNumber}";
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
                        NewTableNumber = Convert.ToInt32(queryResult);
                        NewDirection = direction;
                    }
                    else
                    {
                        // Pair changes Direction
                        if (direction == "NS")
                        {
                            SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND EWPair={pairNumber}";
                        }
                        else
                        {
                            SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND NSPair={pairNumber}";
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
                            NewTableNumber = Convert.ToInt32(queryResult);
                            if (direction == "NS")
                            {
                                NewDirection = "EW";
                            }
                            else
                            {
                                NewDirection = "NS";
                            }
                        }
                        else   // No move info found - move to sit out
                        {
                            NewTableNumber = 0;
                            NewDirection = "";
                        }
                    }
                    Stay = (NewTableNumber == sessionData.TableNumber && NewDirection == Direction);
                }
                else   // Individual
                {
                    // Try Direction = North
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND NSPair={pairNumber}";
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
                        NewTableNumber = Convert.ToInt32(queryResult);
                        NewDirection = "North";
                        Stay = (NewTableNumber == sessionData.TableNumber && NewDirection == Direction);
                        return;
                    }

                    // Try Direction = South
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND South={pairNumber}";
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
                        NewTableNumber = Convert.ToInt32(queryResult);
                        NewDirection = "South";
                        Stay = (NewTableNumber == sessionData.TableNumber && NewDirection == Direction);
                        return;
                    }

                    // Try Direction = East
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND EWPair={pairNumber}";
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
                        NewTableNumber = Convert.ToInt32(queryResult);
                        NewDirection = "East";
                        Stay = (NewTableNumber == sessionData.TableNumber && NewDirection == Direction);
                        return;
                    }

                    // Try Direction = West
                    SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND West={pairNumber}";
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
                        NewTableNumber = Convert.ToInt32(queryResult);
                        NewDirection = "West";
                        Stay = (NewTableNumber == sessionData.TableNumber && NewDirection == Direction);
                        return;
                    }
                    else   // No move info found - move to sit out
                    {
                        NewTableNumber = 0;
                        NewDirection = "";
                        Stay = false;
                        return;
                    }
                }
            }
        }
    }
}
