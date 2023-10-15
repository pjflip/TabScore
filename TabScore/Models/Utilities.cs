// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Resources;
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
        North,
        East,
        South,
        West,
        Sitout, 
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
                catch { }
                cmd.Dispose();
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
                    cmd.Dispose();
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
                catch { }
                cmd.Dispose();
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
                catch { }
                cmd.Dispose();
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
                    return Strings.N;
                case 1:
                    return Strings.E;
                case 2:
                    return Strings.S;
                case 3:
                    return Strings.W;
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

        // Set the header for views
        public static string HeaderString(TabletDeviceStatus tabletDeviceStatus, TableStatus tableStatus, int boardNumber = 0)
        {
            if (boardNumber == -1)  // At ShowBoards, so don't colour header string
            {
                if (AppData.IsIndividual)
                {
                    return $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth} v {tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}";
                }
                else
                {
                    return $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {Strings.N}{Strings.S} {tableStatus.RoundData.NumberNorth} v {Strings.E}{Strings.W} {tableStatus.RoundData.NumberEast}";
                }
            }
            else
            {
                if (boardNumber == 0) boardNumber = tableStatus.ResultData.BoardNumber;  // Board number not specified, so get it from table status
                if (AppData.IsIndividual)
                {
                    return $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {ColourPairByVulnerability("NS", boardNumber, $"{tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth}")} v {ColourPairByVulnerability("EW", boardNumber, $"{tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}")}";
                }
                else
                {
                    return $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {ColourPairByVulnerability("NS", boardNumber, $"{Strings.N}{Strings.S} {tableStatus.RoundData.NumberNorth}")} v {ColourPairByVulnerability("EW", boardNumber, $"{Strings.E}{Strings.W} {tableStatus.RoundData.NumberEast}")}";
                }
            }
        }

        // Calculate the number of seconds to show on the round timer 
        public static int SetTimerSeconds(TabletDeviceStatus tabletDeviceStatus)
        {
            RoundTimer roundTimer = AppData.RoundTimerList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.RoundNumber == tabletDeviceStatus.RoundNumber);
            if (roundTimer == null)  // Round not yet started, so create initial timer data for this section and round 
            {
                DateTime startTime = DateTime.Now;
                TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
                if (tableStatus == null) return -1;  // No data, so don't show timer
                int secondsPerRound = (tableStatus.RoundData.HighBoard - tableStatus.RoundData.LowBoard + 1) * Settings.SecondsPerBoard + Settings.AdditionalSecondsPerRound;
                AppData.RoundTimerList.Add(new RoundTimer
                {
                    SectionID = tabletDeviceStatus.SectionID,
                    RoundNumber = tabletDeviceStatus.RoundNumber,
                    StartTime = startTime,
                    SecondsPerRound = secondsPerRound
                });
                return secondsPerRound;  // Timer shows full time for the round
            }
            else
            {
                int timerSeconds = roundTimer.SecondsPerRound - Convert.ToInt32(DateTime.Now.Subtract(roundTimer.StartTime).TotalSeconds);
                if (timerSeconds < 0) timerSeconds = 0;
                return timerSeconds;  // Timer shows time remaining in this round 
            }
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
            if (cardValue == "1") cardValue = "T";   // Use T for 10 as comparing against PBN strings

            switch (tableStatus.ResultData.DeclarerNSEW)
            {
                case "N":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.EastSpadesPBN.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.EastHeartsPBN.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.EastDiamondsPBN.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.EastClubsPBN.Contains(cardValue)) return true;
                            break;
                    }
                    break;
                case "S":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.WestSpadesPBN.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.WestHeartsPBN.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.WestDiamondsPBN.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.WestClubsPBN.Contains(cardValue)) return true;
                            break;
                    }
                    break;
                case "E":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.SouthSpadesPBN.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.SouthHeartsPBN.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.SouthDiamondsPBN.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.SouthClubsPBN.Contains(cardValue)) return true;
                            break;
                    }
                    break;
                case "W":
                    switch (cardSuit)
                    {
                        case "S":
                            if (handRecord.NorthSpadesPBN.Contains(cardValue)) return true;
                            break;
                        case "H":
                            if (handRecord.NorthHeartsPBN.Contains(cardValue)) return true;
                            break;
                        case "D":
                            if (handRecord.NorthDiamondsPBN.Contains(cardValue)) return true;
                            break;
                        case "C":
                            if (handRecord.NorthClubsPBN.Contains(cardValue)) return true;
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