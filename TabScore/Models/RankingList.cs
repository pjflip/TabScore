// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class RankingList : List<Ranking>
    {
        public int TabletDeviceNumber { get; set; }
        public int RoundNumber { get; set; }
        public int NumberNorth { get; set; } = 0;
        public int NumberEast { get; set; } = 0;
        public int NumberSouth { get; set; } = 0;
        public int NumberWest { get; set; } = 0;
        public bool FinalRankingList { get; set; } = false;

        public RankingList(int tabletDeviceNumber)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            RoundNumber = tabletDeviceStatus.RoundNumber;

            // Set player numbers to highlight appropriate rows of ranking list
            if (AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID).TabletDevicesPerTable == 1)
            {
                TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
                NumberNorth = tableStatus.RoundData.NumberNorth;
                NumberEast = tableStatus.RoundData.NumberEast;
                NumberSouth = tableStatus.RoundData.NumberSouth;
                NumberWest = tableStatus.RoundData.NumberWest;
            }
            else  // More than one tablet device per table
            {
                // Only need to highlight one row entry, so use NumberNorth as proxy
                NumberNorth = tabletDeviceStatus.PairNumber;
            }

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT Orientation, Number, Score, Rank FROM Results WHERE Section={tabletDeviceStatus.SectionID}";

                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader1 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader1 = cmd.ExecuteReader();
                        while (reader1.Read())
                        {
                            Ranking ranking = new Ranking
                            {
                                Orientation = reader1.GetString(0),
                                PairNo = reader1.GetInt32(1),
                                Score = reader1.GetString(2),
                                Rank = reader1.GetString(3)
                            };
                            ranking.ScoreDecimal = Convert.ToDouble(ranking.Score);
                            Add(ranking);
                        }
                        reader1.Close();
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Any error other than results table doesn't exist
                    {
                        throw (e);
                    }
                }
                cmd.Dispose();

                if (Count == 0)  // Results table either doesn't exist or contains no entries, so try to calculate rankings
                {
                    if (AppData.IsIndividual)
                    {
                        InsertRange(0, CalculateIndividualRankingFromReceivedData(tabletDeviceStatus.SectionID));
                    }
                    else
                    {
                        InsertRange(0, CalculateRankingFromReceivedData(tabletDeviceStatus.SectionID));
                    }
                }

                // Make sure that ranking list is sorted into presentation order
                Sort((x, y) =>
                {
                    int sortValue = y.Orientation.CompareTo(x.Orientation);    // N's first then E's
                    if (sortValue == 0) sortValue = y.ScoreDecimal.CompareTo(x.ScoreDecimal);
                    if (sortValue == 0) sortValue = x.PairNo.CompareTo(y.PairNo);
                    return sortValue;
                });
            }
        }

        private static List<Ranking> CalculateRankingFromReceivedData(int sectionID)
        {
            // Uses Neuberg formula for match point pairs to create ranking list based on data in ReceivedData table for this section
            // This might include score adjustments
            // If anything goes wrong, this function returns null and no ranking list is shown

            int Winners = AppData.SectionsList.Find(x => x.SectionID == sectionID).Winners;
            if (Winners == 0)
            {
                // Winners not set, so no chance of calculating ranking
                return null;
            }

            List<Result> resultsList = new List<Result>();
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                // No Results table and Winners = 1 or 2, so use ReceivedData to calculate ranking
                connection.Open();
                string SQLString = $"SELECT Board, PairNS, PairEW, [NS/EW], Contract, Result, Remarks FROM ReceivedData WHERE Section={sectionID}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Result result = new Result()
                            {
                                BoardNumber = reader.GetInt32(0),
                                NumberNorth = reader.GetInt32(1),
                                NumberEast = reader.GetInt32(2),
                                DeclarerNSEW = reader.GetString(3),
                                Contract = reader.GetString(4),
                                TricksTakenSymbol = reader.GetString(5),
                                Remarks = reader.GetString(6)
                            };
                            if (result.Contract.Length > 2)  // Testing for valid and uncorrupted contract
                            {
                                result.CalculateScore();
                            }
                            if (!(result.Remarks == "" || result.Remarks == "Wrong direction") || result.Contract.Length > 2)  // Some form of valid result
                            {
                                resultsList.Add(result);
                            }
                        }
                    });
                }
                catch (OdbcException)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                }
            }

            // Create a list of how many times each board has been played in order to find maximum value for any board
            List<ResultsPerBoard> resultsPerBoardList = new List<ResultsPerBoard>();
            int maxResultsPerBoard = 1;
            foreach (Result result in resultsList)
            {
                ResultsPerBoard resultsPerBoard = resultsPerBoardList.Find(x => x.BoardNumber == result.BoardNumber);
                if (resultsPerBoard == null)
                {
                    resultsPerBoardList.Add(new ResultsPerBoard() { BoardNumber = result.BoardNumber, NumberOfResults = 1 });
                }
                else
                {
                    resultsPerBoard.NumberOfResults++;
                    if (maxResultsPerBoard < resultsPerBoard.NumberOfResults) maxResultsPerBoard = resultsPerBoard.NumberOfResults;
                }
            }
            int matchPointsMax = 2 * maxResultsPerBoard - 2;

            // Calculate MPs 
            foreach (Result result in resultsList)
            {
                if (result.ContractLevel >= 0)  // The current result has a genuine score, so there is at least one genuine score for this board
                {
                    List<Result> currentBoardScoresList = resultsList.FindAll(x => x.ContractLevel >= 0 && x.BoardNumber == result.BoardNumber);
                    int scoresForThisBoard = currentBoardScoresList.Count;
                    int matchpoints = 2 * currentBoardScoresList.FindAll(x => x.Score < result.Score).Count + currentBoardScoresList.FindAll(x => x.Score == result.Score).Count - 1;

                    // Apply Neuberg formula here
                    double neuberg = ((matchpoints + 1) * maxResultsPerBoard / (double)scoresForThisBoard) - 1.0;
                    if (result.Remarks == "Wrong direction")
                    {
                        result.MatchpointsEW = neuberg;
                        result.MatchpointsNS = matchPointsMax - neuberg;
                    }
                    else  // Normal case
                    {
                        result.MatchpointsNS = neuberg;
                        result.MatchpointsEW = matchPointsMax - neuberg;
                    }
                }
                else
                {
                    // No scorable contract, so look for arbitral score
                    try
                    {
                        string[] temp = result.Remarks.Split(new char[] { '%', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        int percentageNS = Convert.ToInt32(temp[0]);
                        int percentageEW = Convert.ToInt32(temp[1]);
                        result.MatchpointsNS = matchPointsMax * percentageNS / 100.0;
                        result.MatchpointsEW = matchPointsMax * percentageEW / 100.0;
                    }
                    catch
                    {
                        // Can't work out percentages, so make it 50%/50%
                        result.MatchpointsNS = result.MatchpointsEW = matchPointsMax / 2.0;
                    }
                }
            }

            List<Ranking> rankingList = new List<Ranking>();
            if (Winners == 1)
            {
                // Add up MPs for each pair, creating Ranking List entries as we go
                foreach (Result result in resultsList)
                {
                    Ranking rankingListFind = rankingList.Find(x => x.PairNo == result.NumberNorth);
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNo = result.NumberNorth,
                            Orientation = "0",
                            MP = result.MatchpointsNS,
                            MPMax = matchPointsMax
                        };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsNS;
                        rankingListFind.MPMax += matchPointsMax;
                    }
                    rankingListFind = rankingList.Find(x => x.PairNo == result.NumberEast);
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNo = result.NumberEast,
                            Orientation = "0",
                            MP = result.MatchpointsEW,
                            MPMax = matchPointsMax
                    };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsEW;
                        rankingListFind.MPMax += matchPointsMax;
                    }
                }
                    
                // Calculate percentages
                foreach (Ranking ranking in rankingList)
                {
                    if (ranking.MPMax == 0)
                    {
                        ranking.ScoreDecimal = 50.0;
                    }
                    else
                    {
                        ranking.ScoreDecimal = 100.0 * ranking.MP / ranking.MPMax;
                    }
                    ranking.Score = ranking.ScoreDecimal.ToString("0.##");
                }
                    
                // Calculate ranking
                foreach (Ranking ranking in rankingList)
                {
                    double currentScoreDecimal = ranking.ScoreDecimal;
                    int rank = rankingList.FindAll(x => x.ScoreDecimal > currentScoreDecimal).Count + 1;
                    ranking.Rank = rank.ToString();
                    if (rankingList.FindAll(x => x.ScoreDecimal == currentScoreDecimal).Count > 1)
                    {
                        ranking.Rank += "=";
                    }
                }
            }
            else    // Winners = 2
            {
                // Add up MPs for each pair, creating Ranking List entries as we go
                foreach (Result result in resultsList)
                {
                    Ranking rankingListFind = rankingList.Find(x => x.PairNo == result.NumberNorth && x.Orientation == "N");
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNo = result.NumberNorth,
                            Orientation = "N",
                            MP = result.MatchpointsNS,
                            MPMax = matchPointsMax
                        };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsNS;
                        rankingListFind.MPMax += matchPointsMax;
                    }
                    rankingListFind = rankingList.Find(x => x.PairNo == result.NumberEast && x.Orientation == "E");
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNo = result.NumberEast,
                            Orientation = "E",
                            MP = result.MatchpointsEW,
                            MPMax = matchPointsMax
                        };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsEW;
                        rankingListFind.MPMax += matchPointsMax;
                    }
                }
                    
                // Calculate percentages
                foreach (Ranking ranking in rankingList)
                {
                    if (ranking.MPMax == 0)
                    {
                        ranking.ScoreDecimal = 50.0;
                    }
                    else
                    {
                        ranking.ScoreDecimal = 100.0 * ranking.MP / ranking.MPMax;
                    }
                    ranking.Score = ranking.ScoreDecimal.ToString("0.##");
                }

                // Sort and calculate rank within Orientation subsections - matching ranks are show by an '='
                foreach (Ranking ranking in rankingList)
                {
                    double currentScoreDecimal = ranking.ScoreDecimal;
                    string currentOrientation = ranking.Orientation;
                    int rank = rankingList.FindAll(x => x.Orientation == currentOrientation && x.ScoreDecimal > currentScoreDecimal).Count + 1;
                    ranking.Rank = rank.ToString();
                    if (rankingList.FindAll(x => x.Orientation == currentOrientation && x.ScoreDecimal == currentScoreDecimal).Count > 1)
                    {
                        ranking.Rank += "=";
                    }
                }
            }
            return rankingList;
        }

        private static List<Ranking> CalculateIndividualRankingFromReceivedData(int sectionID)
        {
            // Uses Neuberg formula for match point pairs to create ranking list based on data in ReceivedData table
            // This might include score adjustments
            // If anything goes wrong, this function returns null and no ranking list is shown

            List<Result> resultsList = new List<Result>();
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT Board, PairNS, PairEW, South, West, [NS/EW], Contract, Result, Remarks FROM ReceivedData WHERE Section={sectionID}";

                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Result result = new Result()
                            {
                                BoardNumber = reader.GetInt32(0),
                                NumberNorth = reader.GetInt32(1),
                                NumberEast = reader.GetInt32(2),
                                NumberSouth = reader.GetInt32(3),
                                NumberWest = reader.GetInt32(4),
                                DeclarerNSEW = reader.GetString(5),
                                Contract = reader.GetString(6),
                                TricksTakenSymbol = reader.GetString(7),
                                Remarks = reader.GetString(8)
                            };
                            if (result.Contract.Length > 2)  // Testing for valid and uncorrupted contract
                            {
                                result.CalculateScore();
                            }
                            if (!(result.Remarks == "" || result.Remarks == "Wrong direction") || result.Contract.Length > 2)  // Some form of valid result
                            {
                                resultsList.Add(result);
                            }
                        }
                    });
                }
                catch (OdbcException)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                }
            }

            // Create a list of how many times each board has been played in order to find maximum value for any board
            List<ResultsPerBoard> resultsPerBoardList = new List<ResultsPerBoard>();
            int maxResultsPerBoard = 1;
            foreach (Result result in resultsList)
            {
                ResultsPerBoard resultsPerBoard = resultsPerBoardList.Find(x => x.BoardNumber == result.BoardNumber);
                if (resultsPerBoard == null)
                {
                    resultsPerBoardList.Add(new ResultsPerBoard() { BoardNumber = result.BoardNumber, NumberOfResults = 1 });
                }
                else
                {
                    resultsPerBoard.NumberOfResults++;
                    if (maxResultsPerBoard < resultsPerBoard.NumberOfResults) maxResultsPerBoard = resultsPerBoard.NumberOfResults;
                }
            }
            int matchPointsMax = 2 * maxResultsPerBoard - 2;

            // Calculate MPs 
            foreach (Result result in resultsList)
            {
                if (result.ContractLevel >= 0)  // The current result has a genuine score, so there is at least one genuine score for this board
                {
                    List<Result> currentBoardScoresList = resultsList.FindAll(x => x.ContractLevel >= 0 && x.BoardNumber == result.BoardNumber);
                    int scoresForThisBoard = currentBoardScoresList.Count;
                    int matchpoints = 2 * currentBoardScoresList.FindAll(x => x.Score < result.Score).Count + currentBoardScoresList.FindAll(x => x.Score == result.Score).Count - 1;

                    // Apply Neuberg formula here
                    double neuberg = ((matchpoints + 1) * maxResultsPerBoard / (double)scoresForThisBoard) - 1.0;
                    if (result.Remarks == "Wrong direction")
                    {
                        result.MatchpointsEW = neuberg;
                        result.MatchpointsNS = matchPointsMax - neuberg;
                    }
                    else  // Normal case
                    {
                        result.MatchpointsNS = neuberg;
                        result.MatchpointsEW = matchPointsMax - neuberg;
                    }
                }
                else
                {
                    // No scorable contract, so look for arbitral score
                    try
                    {
                        string[] temp = result.Remarks.Split(new char[] { '%', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        int percentageNS = Convert.ToInt32(temp[0]);
                        int percentageEW = Convert.ToInt32(temp[1]);
                        result.MatchpointsNS = matchPointsMax * percentageNS / 100.0;
                        result.MatchpointsEW = matchPointsMax * percentageEW / 100.0;
                    }
                    catch
                    {
                        // Can't work out percentages, so make it 50%/50%
                        result.MatchpointsNS = result.MatchpointsEW = matchPointsMax / 2.0;
                    }
                }
            }

            // Add up MPs for each player, creating Ranking List entries as we go
            List<Ranking> rankingList = new List<Ranking>();
            foreach (Result result in resultsList)
            {
                Ranking rankingListFind = rankingList.Find(x => x.PairNo == result.NumberNorth);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.NumberNorth,
                        Orientation = "0",
                        MP = result.MatchpointsNS,
                        MPMax = matchPointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsNS;
                    rankingListFind.MPMax += matchPointsMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNo == result.NumberEast);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.NumberEast,
                        Orientation = "0",
                        MP = result.MatchpointsEW,
                        MPMax = matchPointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsEW;
                    rankingListFind.MPMax += matchPointsMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNo == result.NumberSouth);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.NumberSouth,
                        Orientation = "0",
                        MP = result.MatchpointsNS,
                        MPMax = matchPointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsNS;
                    rankingListFind.MPMax += matchPointsMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNo == result.NumberWest);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.NumberWest,
                        Orientation = "0",
                        MP = result.MatchpointsEW,
                        MPMax = matchPointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsEW;
                    rankingListFind.MPMax += matchPointsMax;
                }
            }
                
            // Calculate percentages
            foreach (Ranking ranking in rankingList)
            {
                if (ranking.MPMax == 0)
                {
                    ranking.ScoreDecimal = 50.0;
                }
                else
                {
                    ranking.ScoreDecimal = 100.0 * ranking.MP / ranking.MPMax;
                }
                ranking.Score = ranking.ScoreDecimal.ToString("0.##");
            }

            // Calculate ranks - matching ranks are show by an '='
            rankingList.Sort((x, y) => y.ScoreDecimal.CompareTo(x.ScoreDecimal));
            foreach (Ranking ranking in rankingList)
            {
                double currentScoreDecimal = ranking.ScoreDecimal;
                int rank = rankingList.FindAll(x => x.ScoreDecimal > currentScoreDecimal).Count + 1;
                ranking.Rank = rank.ToString();
                if (rankingList.FindAll(x => x.ScoreDecimal == currentScoreDecimal).Count > 1)
                {
                    ranking.Rank += "=";
                }
            }

            return rankingList;
        }
    }
}
