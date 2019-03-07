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
            List<TravellerResultClass> trList = new List<TravellerResultClass>();
            int Winners = 0;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT Orientation, Number, Score, Rank FROM Results WHERE Section={sectionID}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        RankingListClass rl = new RankingListClass();
                        rl.Orientation = reader.GetString(0);
                        rl.PairNo = reader.GetInt32(1).ToString();
                        rl.Score = reader.GetString(2);
                        rl.Rank = reader.GetString(3);

                        rList.Add(rl);
                    }
                    reader.Close();
                    cmd.Dispose();
                    return rList;
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S02")
                    {
                        // Results table doesn't exist, so check Winners
                        SQLString = $"SELECT Winners FROM Section WHERE ID={sectionID}";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            object queryResult = cmd.ExecuteScalar();
                            Winners = Convert.ToInt32(queryResult);
                            if (Winners == 0)
                            {
                                // Winners not set, so no chance of calculating ranking
                                cmd.Dispose();
                                return null;
                            }
                        }
                        catch (OdbcException e2)
                        {
                            if (e.Errors.Count == 1 && e2.Errors[0].SQLState == "42S22")
                            {
                                // Winners column does not exist, so no chance of calculating ranking
                                cmd.Dispose();
                                return null;
                            }
                            else
                            {
                                throw e2;
                            }
                        }
                    }
                }

                // No Results table and Winners = 1 or 2, so use ReceivedData to calculate ranking

                SQLString = $"SELECT Board, PairNS, PairEW, [NS/EW], Contract, Result FROM ReceivedData WHERE Section={sectionID}";
                cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    TravellerResultClass tr = new TravellerResultClass()
                    {
                        Board = reader2.GetInt32(0).ToString(),
                        PairNS = reader2.GetInt32(1).ToString(),
                        PairEW = reader2.GetInt32(2).ToString(),
                        NSEW = reader2.GetString(3),
                        Contract = reader2.GetString(4),
                        TricksTakenSymbol = reader2.GetString(5)
                    };
                    trList.Add(tr);
                }
                reader2.Close();
                cmd.Dispose();
            }  // End using; close connection

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
