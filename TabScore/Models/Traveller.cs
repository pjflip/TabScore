﻿using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Traveller : List<Result>
    {
        public bool HandRecord { get; private set; }
        public int PairNS { get; private set; }
        public int PercentageNS { get; private set; }
        public int Board { get; private set; }

        private int currentScore;

        public Traveller(string DB, int sectionID, int board, int pairNS, bool individual)
        {
            Board = board;
            PairNS = pairNS;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString;
                OdbcCommand cmd = null;
                OdbcDataReader reader = null;
                try
                {
                    if (individual)
                    {
                        SQLString = $"SELECT PairNS, PairEW, South, West, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={board}";
                    }
                    else
                    {
                        SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={board}";
                    }
                    cmd = new OdbcCommand(SQLString, connection);
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Result result = null;
                            if (individual)
                            {
                                result = new Result()
                                {
                                    Board = board,
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
                                    Board = board,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    NSEW = reader.GetString(2),
                                    Contract = reader.GetString(3),
                                    LeadCard = reader.GetString(4),
                                    TricksTakenSymbol = reader.GetString(5)
                                };
                            }
                            if (result.Contract.Length > 2)  // Testing for corrupt ReceivedData table
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
            if (Settings.GetSetting<bool>(DB, SettingName.ShowPercentage))
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

            if (Settings.GetSetting<bool>(DB, SettingName.ShowHandRecord))
            {
                HandRecord hr = new HandRecord(DB, sectionID, board);
                if (hr.NorthSpades == "###")
                {
                    HandRecord = false;
                    if (sectionID != 1)    // Try default Section 1 hand records
                    {
                        hr = new HandRecord(DB, 1, sectionID);
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