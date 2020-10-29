// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Traveller : List<Result>
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; private set; }
        public int BoardNumber { get; private set; }
        public bool HandRecord { get; private set; }
        public int PairNS { get; private set; }
        public int PercentageNS { get; private set; }

        private int currentScore;

        public Traveller(TableStatus tableStatus)
        {
            SectionID = tableStatus.SectionID;
            TableNumber = tableStatus.TableNumber;
            BoardNumber = tableStatus.ResultData.BoardNumber;
            PairNS = tableStatus.RoundData.PairNS;

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
                        SQLString = $"SELECT PairNS, PairEW, South, West, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={SectionID} AND Board={BoardNumber}";
                    }
                    else
                    {
                        SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={SectionID} AND Board={BoardNumber}";
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
                                    BoardNumber = BoardNumber,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    South = reader.GetInt32(2),
                                    West = reader.GetInt32(3),
                                    DeclarerNSEW = reader.GetString(4),
                                    Contract = reader.GetString(5),
                                    LeadCard = reader.GetString(6),
                                    TricksTakenSymbol = reader.GetString(7)
                                };
                            }
                            else
                            {
                                result = new Result()
                                {
                                    BoardNumber = BoardNumber,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    DeclarerNSEW = reader.GetString(2),
                                    Contract = reader.GetString(3),
                                    LeadCard = reader.GetString(4),
                                    TricksTakenSymbol = reader.GetString(5)
                                };
                            }
                            if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                            {
                                result.CalculateScore();
                                Add(result);
                                if (result.PairNS == PairNS)  // Get score for current result for calculating percentages
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

            // Sort traveller and calculate percentage for current result
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

            // Determine if there is a hand record to view
            HandRecord = false;
            if (Settings.ShowHandRecord && HandRecords.HandRecordsList.Count > 0)
            {
                HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == SectionID && x.BoardNumber == BoardNumber);
                if (handRecord != null)     
                {
                    HandRecord = true;
                }
                else    // Can't find matching hand record, so try default SectionID = 1
                {
                    handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == BoardNumber);
                    if (handRecord != null)
                    {
                        HandRecord = true;
                    }
                }
            }
        }
    }
}