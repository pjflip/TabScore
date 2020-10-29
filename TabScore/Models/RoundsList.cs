// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class RoundsList : List<Round>
    {
        public RoundsList(int sectionID, int roundNumber)
        {
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard, South, West FROM RoundData WHERE Section={sectionID} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Round round = new Round()
                                {
                                    TableNumber = reader.GetInt32(0),
                                    PairNS = reader.GetInt32(1),
                                    PairEW = reader.GetInt32(2),
                                    LowBoard = reader.GetInt32(3),
                                    HighBoard = reader.GetInt32(4),
                                    South = reader.GetInt32(5),
                                    West = reader.GetInt32(6)
                                };
                                Add(round);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT Table, NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Round round = new Round()
                                {
                                    TableNumber = reader.GetInt32(0),
                                    PairNS = reader.GetInt32(1),
                                    PairEW = reader.GetInt32(2),
                                    LowBoard = reader.GetInt32(3),
                                    HighBoard = reader.GetInt32(4),
                                };
                                Add(round);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
            }
        }

        public Move GetMove(int tableNumber, int pairNumber, string direction)
        {
            Move move = new Move()
            {
                PairNumber = pairNumber,
                Direction = direction
            };
            Round round = new Round();
            if (!AppData.IsIndividual)  // Pairs
            {
                if (direction == "NS")
                {
                    round = Find(x => x.PairNS == pairNumber);
                }
                else
                {
                    round = Find(x => x.PairEW == pairNumber);
                }
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = direction;
                }
                else
                {
                    // Pair changes Direction
                    if (direction == "NS")
                    {
                        round = Find(x => x.PairEW == pairNumber);
                    }
                    else
                    {
                        round = Find(x => x.PairNS == pairNumber);
                    }

                    if (round != null)
                    {
                        move.NewTableNumber = round.TableNumber;
                        if (direction == "NS")
                        {
                            move.NewDirection = "EW";
                        }
                        else
                        {
                            move.NewDirection = "NS";
                        }
                    }
                    else   // No move info found - move to sit out
                    {
                        move.NewTableNumber = 0;
                        move.NewDirection = "";
                    }
                }
                move.Stay = (move.NewTableNumber == tableNumber && move.NewDirection == direction);
                return move;
            }
            else   // Individual
            {
                // Try Direction = North
                round = Find(x => x.PairNS == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = "North";
                    move.Stay = (move.NewTableNumber == tableNumber && move.NewDirection == direction);
                    return move;
                }

                // Try Direction = South
                round = Find(x => x.South == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = "South";
                    move.Stay = (move.NewTableNumber == tableNumber && move.NewDirection == direction);
                    return move;
                }

                // Try Direction = East
                round = Find(x => x.PairEW == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = "East";
                    move.Stay = (move.NewTableNumber == tableNumber && move.NewDirection == direction);
                    return move;
                }

                // Try Direction = West
                round = Find(x => x.West == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = "West";
                    move.Stay = (move.NewTableNumber == tableNumber && move.NewDirection == direction);
                    return move;
                }

                else   // No move info found - move to sit out
                {
                    move.NewTableNumber = 0;
                    move.NewDirection = "";
                    move.Stay = false;
                    return move;
                }
            }
        }

        public int GetBoardsNewTableNumber(int tableNumber, int lowBoard)
        {
            // Get a list of all possible tables to which boards could move
            List<Round> tableList = FindAll(x => x.LowBoard == lowBoard);
            if (tableList.Count == 0)
            {
                // No table, so move to relay table
                return 0;
            }
            else if (tableList.Count == 1)
            {
                // Just one table, so use it
                return tableList[0].TableNumber;
            }
            else
            {
                // Find the next table down to which the boards could move
                tableList.Sort((x, y) => x.TableNumber.CompareTo(y.TableNumber));
                Round boardMoveTable = tableList.FindLast(x => x.TableNumber < tableNumber);
                if (boardMoveTable != null)
                {
                    return boardMoveTable.TableNumber;
                }

                // Next table down must be highest table number in the list
                return tableList[tableList.Count - 1].TableNumber;
            }
        }
    }
}
