using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class RankingList
    {
        public static List<RankingListClass> GetRankingList(string DB, string sectionID)
        {
            List<RankingListClass> rList = new List<RankingListClass>();
            int Winners = 0;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT Orientation, Number, Score, Rank FROM Results WHERE Section={sectionID}";

                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader1 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader1 = cmd1.ExecuteReader();
                        while (reader1.Read())
                        {
                            RankingListClass rl = new RankingListClass();
                            rl.Orientation = reader1.GetString(0);
                            rl.PairNo = reader1.GetInt32(1).ToString();
                            rl.Score = reader1.GetString(2);
                            rl.Rank = reader1.GetString(3);

                            rList.Add(rl);
                        }
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S02")
                    {
                        // Results table doesn't exist, so check Winners
                        object queryResult = null;
                        SQLString = $"SELECT Winners FROM Section WHERE ID={sectionID}";
                        OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                        try
                        {
                            ODBCRetryHelper.ODBCRetry(() =>
                            {
                                queryResult = cmd2.ExecuteScalar();
                            });
                            Winners = Convert.ToInt32(queryResult);
                            if (Winners == 0)
                            {
                                // Winners not set, so no chance of calculating ranking
                                return null;
                            }
                            else
                            {
                                // No Results table and Winners = 1 or 2, so use ReceivedData to calculate ranking
                                return CalculateRankingFromReceivedData(DB, sectionID, Winners);
                            }
                        }
                        catch (OdbcException)
                        {
                            // If Winners column doesn't exist, or any other error, can't calculate ranking
                            return null;
                        }
                        finally
                        {
                            cmd2.Dispose();
                        }
                    }
                }
                finally
                {
                    reader1.Close();
                    cmd1.Dispose();
                }
                return rList;
            }
        }

        private static List<RankingListClass> CalculateRankingFromReceivedData(string DB, string sectionID, int Winners)
        {
            List<RankingListClass> rList = new List<RankingListClass>();
            List<TravellerResultClass> trList = new List<TravellerResultClass>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT Board, PairNS, PairEW, [NS/EW], Contract, Result FROM ReceivedData WHERE Section={sectionID}";

                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TravellerResultClass tr = new TravellerResultClass()
                            {
                                Board = reader.GetInt32(0).ToString(),
                                PairNS = reader.GetInt32(1).ToString(),
                                PairEW = reader.GetInt32(2).ToString(),
                                NSEW = reader.GetString(3),
                                Contract = reader.GetString(4),
                                TricksTakenSymbol = reader.GetString(5)
                            };
                            if (tr.Contract.Length > 2)  // Testing for corrupt ReceivedData table
                            {
                                trList.Add(tr);
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

            foreach (TravellerResultClass tr in trList)
            {
                // Use ResultClass Score method to calculate score
                ResultClass res = new ResultClass()
                {
                    SectionID = sectionID,
                    Board = tr.Board,
                };
                if (tr.Contract == "PASS")
                {
                    res.ContractLevel = "PASS";
                    res.NSEW = "";
                    res.TricksTakenSymbol = "";
                }
                else
                {
                    string[] temp = tr.Contract.Split(' ');
                    res.ContractLevel = temp[0];
                    res.ContractSuit = temp[1];
                    if (temp.Length > 2) res.ContractX = temp[2];
                    else res.ContractX = "NONE";
                    res.NSEW = tr.NSEW;
                    res.TricksTakenSymbol = tr.TricksTakenSymbol;
                }
                tr.Score = res.Score();
            }

            // Calculate MPs 
            List<TravellerResultClass> currentBoardList = new List<TravellerResultClass>();
            string currentBoard;
            int currentScore;
            foreach (TravellerResultClass tr in trList)
            {
                currentScore = tr.Score;
                currentBoard = tr.Board;
                currentBoardList = trList.FindAll(x => x.Board == currentBoard);
                tr.MPNS = 2 * currentBoardList.FindAll(x => x.Score < currentScore).Count + currentBoardList.FindAll(x => x.Score == currentScore).Count - 1;
                tr.MPMax = 2 * currentBoardList.Count - 2;
                tr.MPEW = tr.MPMax - tr.MPNS;
            }

            if (Winners == 1)
            {
                // Add up MPs for each pair, creating Ranking List entries as we go
                foreach (TravellerResultClass tr in trList)
                {
                    RankingListClass rListFind = rList.Find(x => x.PairNo == tr.PairNS);
                    if (rListFind == null)
                    {
                        RankingListClass r = new RankingListClass()
                        {
                            PairNo = tr.PairNS,
                            Orientation = "0",
                            MP = tr.MPNS,
                            MPMax = tr.MPMax
                        };
                        rList.Add(r);
                    }
                    else
                    {
                        rListFind.MP += tr.MPNS;
                        rListFind.MPMax += tr.MPMax;
                    }
                    rListFind = rList.Find(x => x.PairNo == tr.PairEW);
                    if (rListFind == null)
                    {
                        RankingListClass r = new RankingListClass()
                        {
                            PairNo = tr.PairEW,
                            Orientation = "0",
                            MP = tr.MPEW,
                            MPMax = tr.MPMax
                        };
                        rList.Add(r);
                    }
                    else
                    {
                        rListFind.MP += tr.MPEW;
                        rListFind.MPMax += tr.MPMax;
                    }
                }
                // Calculate percentages
                foreach (RankingListClass r in rList)
                {
                    if (r.MPMax == 0)
                    {
                        r.ScoreDecimal = 50.0;
                    }
                    else
                    {
                        r.ScoreDecimal = 100.0 * r.MP / r.MPMax;
                    }
                    r.Score = r.ScoreDecimal.ToString("0.##");
                }
                // Calculate ranking
                rList.Sort((x, y) => y.ScoreDecimal.CompareTo(x.ScoreDecimal));
                foreach (RankingListClass r in rList)
                {
                    double currentScoreDecimal = r.ScoreDecimal;
                    int rank = rList.FindAll(x => x.ScoreDecimal > currentScoreDecimal).Count + 1;
                    r.Rank = rank.ToString();
                    if (rList.FindAll(x => x.ScoreDecimal == currentScoreDecimal).Count > 1)
                    {
                        r.Rank += "=";
                    }
                }
            }
            else    // Winners = 2
            {
                // Add up MPs for each pair, creating Ranking List entries as we go
                foreach (TravellerResultClass tr in trList)
                {
                    RankingListClass rListFind = rList.Find(x => x.PairNo == tr.PairNS && x.Orientation == "N");
                    if (rListFind == null)
                    {
                        RankingListClass r = new RankingListClass()
                        {
                            PairNo = tr.PairNS,
                            Orientation = "N",
                            MP = tr.MPNS,
                            MPMax = tr.MPMax
                        };
                        rList.Add(r);
                    }
                    else
                    {
                        rListFind.MP += tr.MPNS;
                        rListFind.MPMax += tr.MPMax;
                    }
                    rListFind = rList.Find(x => x.PairNo == tr.PairEW && x.Orientation == "E");
                    if (rListFind == null)
                    {
                        RankingListClass r = new RankingListClass()
                        {
                            PairNo = tr.PairEW,
                            Orientation = "E",
                            MP = tr.MPEW,
                            MPMax = tr.MPMax
                        };
                        rList.Add(r);
                    }
                    else
                    {
                        rListFind.MP += tr.MPEW;
                        rListFind.MPMax += tr.MPMax;
                    }
                }
                // Calculate percentages
                foreach (RankingListClass r in rList)
                {
                    if (r.MPMax == 0)
                    {
                        r.ScoreDecimal = 50.0;
                    }
                    else
                    {
                        r.ScoreDecimal = 100.0 * r.MP / r.MPMax;
                    }
                    r.Score = r.ScoreDecimal.ToString("0.##");
                }
                // Sort and calculate ranking within Orientation subsections
                rList.Sort((x, y) => {
                    var ret = y.Orientation.CompareTo(x.Orientation);    // N's first then E's
                    if (ret == 0) ret = y.ScoreDecimal.CompareTo(x.ScoreDecimal);
                    return ret;
                });
                foreach (RankingListClass r in rList)
                {
                    double currentScoreDecimal = r.ScoreDecimal;
                    string currentOrientation = r.Orientation;
                    int rank = rList.FindAll(x => x.Orientation == currentOrientation && x.ScoreDecimal > currentScoreDecimal).Count + 1;
                    r.Rank = rank.ToString();
                    if (rList.FindAll(x => x.Orientation == currentOrientation && x.ScoreDecimal == currentScoreDecimal).Count > 1)
                    {
                        r.Rank += "=";
                    }
                }
            }
            return rList;
        }
    }
}
