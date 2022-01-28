// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License


using System;
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
                    string SQLString = $"SELECT [Table], NSPair, EWPair, LowBoard, HighBoard, South, West FROM RoundData WHERE Section={sectionID} AND Round={roundNumber}";
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
                                    NumberNorth = reader.GetInt32(1),
                                    NumberEast = reader.GetInt32(2),
                                    LowBoard = reader.GetInt32(3),
                                    HighBoard = reader.GetInt32(4),
                                    NumberSouth = reader.GetInt32(5),
                                    NumberWest = reader.GetInt32(6)
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
                    string SQLString = $"SELECT [Table], NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Round={roundNumber}";
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
                                    NumberNorth = reader.GetInt32(1),
                                    NumberEast = reader.GetInt32(2),
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

        public Move GetMove(int tableNumber, int pairNumber, Direction direction)
        {
            Move move = new Move()
            {
                PairNumber = pairNumber,
            };
            Round round;
            if (AppData.IsIndividual)
            {
                move.DisplayDirection = Enum.GetName(typeof(Direction), direction);

                // Try Direction = North
                round = Find(x => x.NumberNorth == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = Direction.North;
                    move.DisplayNewDirection = "North";
                    move.Stay = (move.NewTableNumber == tableNumber && direction == Direction.North);
                    return move;
                }

                // Try Direction = South
                round = Find(x => x.NumberSouth == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = Direction.South;
                    move.DisplayNewDirection = "South";
                    move.Stay = (move.NewTableNumber == tableNumber && direction == Direction.South);
                    return move;
                }

                // Try Direction = East
                round = Find(x => x.NumberEast == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = Direction.East;
                    move.DisplayNewDirection = "East";
                    move.Stay = (move.NewTableNumber == tableNumber && direction == Direction.East);
                    return move;
                }

                // Try Direction = West
                round = Find(x => x.NumberWest == pairNumber);
                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = Direction.West;
                    move.DisplayNewDirection = "West";
                    move.Stay = (move.NewTableNumber == tableNumber && direction == Direction.West);
                    return move;
                }

                else   // No move info found - move to phantom table
                {
                    move.NewTableNumber = 0;
                    move.NewDirection = Direction.Sitout;
                    move.DisplayNewDirection = "Sitout";
                    move.Stay = false;
                    return move;
                }
            }
            else   // Pairs - if there is a sitout pair (at an imaginary sitout table, eg a rover), it works like East
            {
                if (direction == Direction.North)
                {
                    round = Find(x => x.NumberNorth == pairNumber);
                    move.DisplayDirection = "North/South";
                }
                else
                {
                    round = Find(x => x.NumberEast == pairNumber);
                    move.DisplayDirection = "East/West";
                }

                if (round != null)
                {
                    move.NewTableNumber = round.TableNumber;
                    move.NewDirection = direction;
                    move.DisplayNewDirection = move.DisplayDirection;
                }
                else
                {
                    // Pair changes Direction
                    if (direction == Direction.North)
                    {
                        round = Find(x => x.NumberEast == pairNumber);
                    }
                    else
                    {
                        round = Find(x => x.NumberNorth == pairNumber);
                    }

                    if (round != null)
                    {
                        move.NewTableNumber = round.TableNumber;
                        if (direction == Direction.North)
                        {
                            move.NewDirection = Direction.East;
                            move.DisplayNewDirection = "East/West";
                        }
                        else
                        {
                            move.NewDirection = Direction.North;
                            move.DisplayNewDirection = "North/South";
                        }
                    }
                    else   // No move info found - move to phantom table
                    {
                        move.NewTableNumber = 0;
                        move.NewDirection = Direction.Sitout;
                        move.DisplayNewDirection = "Sitout";
                    }
                }
                move.Stay = (move.NewTableNumber == tableNumber && move.NewDirection == direction);
                return move;
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
