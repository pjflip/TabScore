using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class RankingList : List<Ranking>
    {
        public int RoundNumber { get; set; }
        public int PairNS { get; set; }   // Doubles as North player number for individuals
        public int PairEW { get; set; }   // Doubles as East player number for individuals
        public int South { get; set; }
        public int West { get; set; }
        
        public RankingList(string DB, int sectionID, bool individual)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT Orientation, Number, Score, Rank FROM Results WHERE Section={sectionID}";

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
                    });
                    reader1.Close();
                    cmd.Dispose();
                    if (Count == 0)  // Results table exists but is empty
                    {
                        if (individual)
                        {
                            InsertRange(0, CalculateIndividualRankingFromReceivedData(DB, sectionID));
                        }
                        else
                        {
                            InsertRange(0, CalculateRankingFromReceivedData(DB, sectionID));
                        }
                    }
                }
                catch (OdbcException e)
                {
                    reader1.Close();
                    cmd.Dispose();
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S02")  // Results table doesn't exist
                    {
                        if (individual)
                        {
                            InsertRange(0, CalculateIndividualRankingFromReceivedData(DB, sectionID));
                        }
                        else
                        {
                            InsertRange(0, CalculateRankingFromReceivedData(DB, sectionID));
                        }
                    }
                    else
                    {
                        throw (e);
                    }
                }
                Sort((x, y) =>
                {
                    var ret = y.Orientation.CompareTo(x.Orientation);    // N's first then E's
                    if (ret == 0) ret = y.ScoreDecimal.CompareTo(x.ScoreDecimal);
                    if (ret == 0) ret = x.PairNo.CompareTo(y.PairNo);
                    return ret;
                });
            }
        }

        private static List<Ranking> CalculateRankingFromReceivedData(string DB, int sectionID)
        {
            List<Ranking> rankingList = new List<Ranking>();
            List<Result> resList = new List<Result>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                int Winners = 0;
                connection.Open();

                // Check Winners
                object queryResult = null;
                string SQLString = $"SELECT Winners FROM Section WHERE ID={sectionID}";
                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd1.ExecuteScalar();
                    });
                    Winners = Convert.ToInt32(queryResult);
                }
                catch (OdbcException)
                {
                    // If Winners column doesn't exist, or any other error, can't calculate ranking
                    return null;
                }
                finally
                {
                    cmd1.Dispose();
                }

                if (Winners == 0)
                {
                    // Winners not set, so no chance of calculating ranking
                    return null;
                }

                // No Results table and Winners = 1 or 2, so use ReceivedData to calculate ranking
                SQLString = $"SELECT Board, PairNS, PairEW, [NS/EW], Contract, Result FROM ReceivedData WHERE Section={sectionID}";
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd2.ExecuteReader();
                        while (reader.Read())
                        {
                            Result result = new Result()
                            {
                                BoardNumber = reader.GetInt32(0),
                                PairNS = reader.GetInt32(1),
                                PairEW = reader.GetInt32(2),
                                NSEW = reader.GetString(3),
                                Contract = reader.GetString(4),
                                TricksTakenSymbol = reader.GetString(5)
                            };
                            if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                            {
                                result.CalculateScore();
                                resList.Add(result);
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
                    cmd2.Dispose();
                }

                // Calculate MPs 
                List<Result> currentBoardResultList = new List<Result>();
                int currentBoard;
                int currentScore;
                foreach (Result result in resList)
                {
                    currentScore = result.Score;
                    currentBoard = result.BoardNumber;
                    currentBoardResultList = resList.FindAll(x => x.BoardNumber == currentBoard);
                    result.MPNS = 2 * currentBoardResultList.FindAll(x => x.Score < currentScore).Count + currentBoardResultList.FindAll(x => x.Score == currentScore).Count - 1;
                    result.MPMax = 2 * currentBoardResultList.Count - 2;
                    result.MPEW = result.MPMax - result.MPNS;
                }

                if (Winners == 1)
                {
                    // Add up MPs for each pair, creating Ranking List entries as we go
                    foreach (Result result in resList)
                    {
                        Ranking rankingListFind = rankingList.Find(x => x.PairNo == result.PairNS);
                        if (rankingListFind == null)
                        {
                            Ranking ranking = new Ranking()
                            {
                                PairNo = result.PairNS,
                                Orientation = "0",
                                MP = result.MPNS,
                                MPMax = result.MPMax
                            };
                            rankingList.Add(ranking);
                        }
                        else
                        {
                            rankingListFind.MP += result.MPNS;
                            rankingListFind.MPMax += result.MPMax;
                        }
                        rankingListFind = rankingList.Find(x => x.PairNo == result.PairEW);
                        if (rankingListFind == null)
                        {
                            Ranking ranking = new Ranking()
                            {
                                PairNo = result.PairEW,
                                Orientation = "0",
                                MP = result.MPEW,
                                MPMax = result.MPMax
                            };
                            rankingList.Add(ranking);
                        }
                        else
                        {
                            rankingListFind.MP += result.MPEW;
                            rankingListFind.MPMax += result.MPMax;
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
                    foreach (Result result in resList)
                    {
                        Ranking rankingListFind = rankingList.Find(x => x.PairNo == result.PairNS && x.Orientation == "N");
                        if (rankingListFind == null)
                        {
                            Ranking ranking = new Ranking()
                            {
                                PairNo = result.PairNS,
                                Orientation = "N",
                                MP = result.MPNS,
                                MPMax = result.MPMax
                            };
                            rankingList.Add(ranking);
                        }
                        else
                        {
                            rankingListFind.MP += result.MPNS;
                            rankingListFind.MPMax += result.MPMax;
                        }
                        rankingListFind = rankingList.Find(x => x.PairNo == result.PairEW && x.Orientation == "E");
                        if (rankingListFind == null)
                        {
                            Ranking ranking = new Ranking()
                            {
                                PairNo = result.PairEW,
                                Orientation = "E",
                                MP = result.MPEW,
                                MPMax = result.MPMax
                            };
                            rankingList.Add(ranking);
                        }
                        else
                        {
                            rankingListFind.MP += result.MPEW;
                            rankingListFind.MPMax += result.MPMax;
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
                    // Sort and calculate ranking within Orientation subsections
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
        }

        private static List<Ranking> CalculateIndividualRankingFromReceivedData(string DB, int sectionID)
        {
            List<Ranking> rankingList = new List<Ranking>();
            List<Result> resList = new List<Result>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT Board, PairNS, PairEW, South, West, [NS/EW], Contract, Result FROM ReceivedData WHERE Section={sectionID}";

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
                                PairNS = reader.GetInt32(1),
                                PairEW = reader.GetInt32(2),
                                South = reader.GetInt32(3),
                                West = reader.GetInt32(4),
                                NSEW = reader.GetString(5),
                                Contract = reader.GetString(6),
                                TricksTakenSymbol = reader.GetString(7)
                            };
                            if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                            {
                                result.CalculateScore();
                                resList.Add(result);
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
                    cmd.Dispose();
                }
            }

            // Calculate MPs 
            List<Result> currentBoardResultList = new List<Result>();
            int currentBoard;
            int currentScore;
            foreach (Result result in resList)
            {
                currentScore = result.Score;
                currentBoard = result.BoardNumber;
                currentBoardResultList = resList.FindAll(x => x.BoardNumber == currentBoard);
                result.MPNS = 2 * currentBoardResultList.FindAll(x => x.Score < currentScore).Count + currentBoardResultList.FindAll(x => x.Score == currentScore).Count - 1;
                result.MPMax = 2 * currentBoardResultList.Count - 2;
                result.MPEW = result.MPMax - result.MPNS;
            }

            // Add up MPs for each pair, creating Ranking List entries as we go
            foreach (Result result in resList)
            {
                Ranking rankingListFind = rankingList.Find(x => x.PairNo == result.PairNS);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.PairNS,
                        Orientation = "0",
                        MP = result.MPNS,
                        MPMax = result.MPMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MPNS;
                    rankingListFind.MPMax += result.MPMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNo == result.PairEW);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.PairEW,
                        Orientation = "0",
                        MP = result.MPEW,
                        MPMax = result.MPMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MPEW;
                    rankingListFind.MPMax += result.MPMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNo == result.South);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.South,
                        Orientation = "0",
                        MP = result.MPNS,
                        MPMax = result.MPMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MPNS;
                    rankingListFind.MPMax += result.MPMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNo == result.West);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNo = result.West,
                        Orientation = "0",
                        MP = result.MPEW,
                        MPMax = result.MPMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MPEW;
                    rankingListFind.MPMax += result.MPMax;
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
