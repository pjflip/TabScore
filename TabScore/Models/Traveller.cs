// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Traveller : List<TravellerResult>
    {
        public int TabletDeviceNumber { get; set; }
        public int BoardNumber { get; private set; }
        public bool HandRecord { get; private set; }
        public string PercentageNS { get; private set; }
        public string PercentageEW { get; private set; }
        public bool FromView { get; set; }

        public Traveller(int tabletDeviceNumber, TableStatus tableStatus)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            BoardNumber = tableStatus.ResultData.BoardNumber;
            List<Result> boardResultsList = new List<Result>();

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
                        SQLString = $"SELECT PairNS, PairEW, South, West, [NS/EW], Contract, LeadCard, Result, Remarks FROM ReceivedData WHERE Section={tableStatus.SectionID} AND Board={BoardNumber}";
                    }
                    else
                    {
                        SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result, Remarks FROM ReceivedData WHERE Section={tableStatus.SectionID} AND Board={BoardNumber}";
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
                                    NumberNorth = reader.GetInt32(0),
                                    NumberEast = reader.GetInt32(1),
                                    NumberSouth = reader.GetInt32(2),
                                    NumberWest = reader.GetInt32(3),
                                    DeclarerNSEW = reader.GetString(4),
                                    Contract = reader.GetString(5),
                                    LeadCardDB = reader.GetString(6),
                                    TricksTakenSymbol = reader.GetString(7),
                                    Remarks = reader.GetString(8)
                                };
                            }
                            else
                            {
                                result = new Result()
                                {
                                    BoardNumber = BoardNumber,
                                    NumberNorth = reader.GetInt32(0),
                                    NumberEast = reader.GetInt32(1),
                                    DeclarerNSEW = reader.GetString(2),
                                    Contract = reader.GetString(3),
                                    LeadCardDB = reader.GetString(4),
                                    TricksTakenSymbol = reader.GetString(5),
                                    Remarks = reader.GetString(6)
                                };
                            }
                            if (result.Contract.Length > 2)  // Testing for valid and uncorrupted contract
                            {
                                result.CalculateScore();
                            }
                            if (!(result.Remarks == "" || result.Remarks == "Wrong direction") || result.Contract.Length > 2)  // Some form of valid result
                            {
                                boardResultsList.Add(result);
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

            // Set maximum match points based on total number of results (including Not Played) for Neuberg formula
            int resultsForThisBoard = boardResultsList.Count;
            int matchPointsMax = 2 * resultsForThisBoard - 2;

            // Get list of all scorable results for matchpoint calculation
            List<Result> currentBoardScoresList = boardResultsList.FindAll(x => x.ContractLevel >= 0 && x.BoardNumber == BoardNumber);
            int scoresForThisBoard = currentBoardScoresList.Count;

            foreach (Result result in boardResultsList)
            {
                TravellerResult travellerResult = new TravellerResult()
                {
                    NumberNorth = result.NumberNorth,
                    NumberEast = result.NumberEast,
                    NumberSouth = result.NumberSouth,
                    NumberWest = result.NumberWest,
                    Contract = result.ContractTravellerDisplay,
                    DeclarerNSEW = result.DeclarerNSEW,
                    LeadCard = result.LeadCardDisplay,
                    TricksTaken = result.TricksTakenSymbol,
                };
                if (result.Remarks != "" && result.Remarks != "Wrong direction")
                {
                    // No scorable contract, so look for arbitral percentages
                    string[] temp = result.Remarks.Split(new char[] { '%', '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length == 2 && uint.TryParse(temp[0], out uint tempInt0) && uint.TryParse(temp[1], out uint tempInt1))
                    {
                        travellerResult.ScoreNS = $"<span style=\"color:red\">{temp[0]}%</span>";
                        travellerResult.ScoreEW = $"<span style=\"color:red\">{temp[1]}%</span>";
                        travellerResult.SortPercentage = Convert.ToDouble(temp[0]);
                        if (result.NumberNorth == tableStatus.RoundData.NumberNorth)
                        {
                            PercentageNS = temp[0] + "%";
                            PercentageEW = temp[1] + "%";
                        }
                    }
                    else  // Can't work out percentages
                    {
                        travellerResult.ScoreNS = travellerResult.ScoreEW = "<span style=\"color:red\">???</span>";
                        travellerResult.SortPercentage = 0.0;
                        if (result.NumberNorth == tableStatus.RoundData.NumberNorth)
                        {
                            PercentageNS = PercentageEW = "???";
                            travellerResult.Highlight = true;
                        }
                    }
                }
                else  // Genuine result
                {
                    if (result.Score > 0)
                    {
                        travellerResult.ScoreNS = result.Score.ToString();
                        travellerResult.ScoreEW = "";
                    }
                    else if (result.Score < 0)
                    {
                        travellerResult.ScoreEW = (-result.Score).ToString();
                        travellerResult.ScoreNS = "";
                    }

                    if (matchPointsMax == 0)
                    {
                        travellerResult.SortPercentage = 50.0;
                    }
                    else
                    {
                        // Apply Neuberg formula here
                        int matchpoints = 2 * currentBoardScoresList.FindAll(x => x.Score < result.Score).Count + currentBoardScoresList.FindAll(x => x.Score == result.Score).Count - 1;
                        double neuberg = ((matchpoints + 1) * resultsForThisBoard / (double)scoresForThisBoard) - 1.0;
                        travellerResult.SortPercentage = neuberg / matchPointsMax * 100.0;
                    }
                    if (result.NumberNorth == tableStatus.RoundData.NumberNorth)
                    {
                        int intPercentageNS = Convert.ToInt32(travellerResult.SortPercentage);
                        PercentageNS = Convert.ToString(intPercentageNS) + "%";
                        PercentageEW = Convert.ToString(100 - intPercentageNS) + "%";
                        travellerResult.Highlight = true;
                    }

                    if (result.Remarks == "Wrong direction")
                    {
                        travellerResult.NumberNorth = result.NumberEast;
                        travellerResult.NumberEast = result.NumberNorth;
                        travellerResult.NumberSouth = result.NumberWest;
                        travellerResult.NumberWest = result.NumberSouth;
                    }
                }
                Add(travellerResult);
            }

            Sort((x, y) => y.SortPercentage.CompareTo(x.SortPercentage));  // Sort traveller into descending percentage order
            if (!Settings.ShowPercentage) PercentageNS = "";   // Don't show percentage

            // Determine if there is a hand record to view
            HandRecord = false;
            if (Settings.ShowHandRecord && HandRecords.HandRecordsList.Count > 0)
            {
                HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == tableStatus.SectionID && x.BoardNumber == BoardNumber);
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