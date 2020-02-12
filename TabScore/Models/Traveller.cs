using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Traveller : List<Result>
    {
        public bool HandRecord { get; private set; }
        public int PairNS { get; private set; }
        public int PercentageNS { get; private set; }
        public int BoardNumber { get; private set; }

        private int currentScore;

        public Traveller(int sectionID, int boardNumber, int pairNS)
        {
            BoardNumber = boardNumber;
            PairNS = pairNS;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString;
                OdbcCommand cmd = null;
                OdbcDataReader reader = null;
                try
                {
                    if (AppData.IsIndividual)
                    {
                        SQLString = $"SELECT PairNS, PairEW, South, West, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={boardNumber}";
                    }
                    else
                    {
                        SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={boardNumber}";
                    }
                    cmd = new OdbcCommand(SQLString, connection);
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Result result = null;
                            if (AppData.IsIndividual)
                            {
                                result = new Result()
                                {
                                    BoardNumber = boardNumber,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    South = reader.GetInt32(2),
                                    West = reader.GetInt32(3),
                                    NSEW = reader.GetString(4),
                                    Contract = reader.GetString(5),
                                    LeadCard = reader.GetString(6),
                                    TricksTakenSymbol = reader.GetString(7)
                                };
                            }
                            else
                            {
                                result = new Result()
                                {
                                    BoardNumber = boardNumber,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    NSEW = reader.GetString(2),
                                    Contract = reader.GetString(3),
                                    LeadCard = reader.GetString(4),
                                    TricksTakenSymbol = reader.GetString(5)
                                };
                            }
                            if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                            {
                                result.CalculateScore();
                                Add(result);
                                if (result.PairNS == pairNS)  // Get score for current result for calculating percentages
                                {
                                    currentScore = result.Score;
                                }
                            }
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            };

            Sort((x, y) => y.Score.CompareTo(x.Score));
            if (Settings.ShowPercentage)
            {
                if (Count == 1)
                {
                    PercentageNS = 50;
                }
                else
                {
                    int currentMP = 2 * FindAll((x) => x.Score < currentScore).Count + FindAll((x) => x.Score == currentScore).Count - 1;
                    PercentageNS = Convert.ToInt32(50.0 * currentMP / (Count - 1));
                }
            }
            else
            {
                PercentageNS = -1;   // Don't show percentage
            }

            if (Settings.ShowHandRecord)
            {
                HandRecord hr = new HandRecord(sectionID, boardNumber);
                if (hr.NorthSpades == "###")
                {
                    HandRecord = false;
                    if (sectionID != 1)    // Try default Section 1 hand records
                    {
                        hr = new HandRecord(1, sectionID);
                        if (hr.NorthSpades != "###")
                        {
                            HandRecord = true;
                        }
                    }
                }
                else
                {
                    HandRecord = true;
                }
            }
            else
            {
                HandRecord = false;
            }
        }
    }
}