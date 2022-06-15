// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public enum LeadValidationOptions
    {
        Validate,
        Warning,
        NoWarning
    }

    public enum ButtonOptions
    {
        OKEnabled,
        OKEnabledAndBack,
        OKDisabled,
        OKDisabledAndBack
    }

    public enum Direction
    {
        Sitout,
        North,
        East,
        South,
        West,
        Null
    }
    public enum HandRecordPerspectiveButtonOptions
    {
        None,
        NSEW,
        NS,
        EW
    }

    public static class Utilities
    {
        // Set table status in "Tables" table.  Not needed in TabScore, but complies with BridgeMate spec
        public static void RegisterTable(int sectionID, int tableNumber)
        {
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"UPDATE Tables SET LogOnOff=1 WHERE Section={sectionID} AND [Table]={tableNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
        
        // Test read and write to the scoring database
        public static bool IsDatabaseOK()
        {
            try
            {
                using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
                {
                    connection.Open();
                    int logOnOff = 0;
                    string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section=1 AND [Table]=1";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            logOnOff = Convert.ToInt32(cmd.ExecuteScalar());
                        });
                        SQLString = $"UPDATE Tables SET LogOnOff={logOnOff} WHERE Section=1 AND [Table]=1";
                        cmd = new OdbcCommand(SQLString, connection);
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            cmd.ExecuteNonQuery();
                        });
                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        // Find out how many rounds there are in the event
        // Need to re-query database in case rounds are added/removed by scoring program
        public static int NumberOfRoundsInEvent(int sectionID)
        {
            object queryResult = null;
            using(OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT MAX(Round) FROM RoundData WHERE Section={sectionID}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return Convert.ToInt32(queryResult);
        }

        // Get the last round that has any results entered for it
        public static int GetLastRoundWithResults(int sectionID)
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT MAX(Round) FROM ReceivedData WHERE Section={sectionID}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            if (queryResult == DBNull.Value || queryResult == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(queryResult);
            }
        }

        // Get the dealer based on board number for standard boards
        public static string GetDealerForBoard(int boardNumber)
        {
            switch ((boardNumber - 1) % 4)
            {
                case 0:
                    return "N";
                case 1:
                    return "E";
                case 2:
                    return "S";
                case 3:
                    return "W";
                default:
                    return "#";
            }
        }

        // Used for setting vulnerability by board number
        public static readonly bool[] NSVulnerability = { false, true, false, true, true, false, true, false, false, true, false, true, true, false, true, false };
        public static readonly bool[] EWVulnerability = { false, false, true, true, false, true, true, false, true, true, false, false, true, false, false, true };

        // Apply html styles to colour the pair number based on vulnerability
        public static string ColourPairByVulnerability(string dir, int boardNo, string pair)
        {
            string PairString;
            if (dir == "NS")
            {
                if (NSVulnerability[(boardNo - 1) % 16])
                {
                    PairString = $"<span style=\"color:red\">{pair}</span>";
                }
                else
                {
                    PairString = $"<span style=\"color:green\">{pair}</span>";
                }
            }
            else
            {
                if (EWVulnerability[(boardNo - 1) % 16])
                {
                    PairString = $"<span style=\"color:red\">{pair}</span>";
                }
                else
                {
                    PairString = $"<span style=\"color:green\">{pair}</span>";
                }
            }
            return PairString;
        }

        // Validate the lead card against the hand record
        public static bool ValidateLead(TableStatus tableStatus, string card)
        {
            if (HandRecords.HandRecordsList.Count == 0) return true;    // No hand records to validate against
            if (card == "SKIP") return true;    // Lead card entry has been skipped, so no validation

            HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == tableStatus.SectionID && x.BoardNumber == tableStatus.ResultData.BoardNumber);
            if (handRecord == null)     // Can't find matching hand record, so try default SectionID = 1
            {
                handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == tableStatus.ResultData.BoardNumber);
                if (handRecord == null) return true;    // Still no match, so no validation possible
            }

            string cardSuit = card.Substring(0, 1);
            string cardValue = card.Substring(1, 1);
            if (cardValue == "1")    // Account for different representations of '10'
            {
                cardValue = "T";
            }

            switch (tableStatus.ResultData.DeclarerNSEW)
            {
                case "N":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.EastSpades.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.EastHearts.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.EastDiamonds.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.EastClubs.Contains(cardValue)) return true;
                            break;
                    }
                    break;
                case "S":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.WestSpades.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.WestHearts.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.WestDiamonds.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.WestClubs.Contains(cardValue)) return true;
                            break;
                    }
                    break;
                case "E":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.SouthSpades.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.SouthHearts.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.SouthDiamonds.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.SouthClubs.Contains(cardValue)) return true;
                            break;
                    }
                    break;
                case "W":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.NorthSpades.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.NorthHearts.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.NorthDiamonds.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.NorthClubs.Contains(cardValue)) return true;
                            break;
                    }
                    break;
            }
            return false;
        }

        // Set the number of tablet devices per table - possibly different for each section depending on the movements
        public static void SetTabletDevicesPerTable()
        {
            foreach (Section section in AppData.SectionsList)
            {
                // Default TabletDevicesPerTable = 1
                if (Settings.TabletDevicesMove)
                {
                    if (AppData.IsIndividual)
                    {
                        section.TabletDevicesPerTable = 4;
                    }
                    else
                    {
                        if (section.Winners == 1) section.TabletDevicesPerTable = 2;
                    }
                }
            }
        }


    }
}