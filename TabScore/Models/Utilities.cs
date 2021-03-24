// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
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

    public static class Utilities
    {
        // Find out how many rounds there are in the event
        // Need to re-query database in case rounds are added/removed by scoring program
        public static int TableLogonStatus(int sectionID, int tableNumber)
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section={sectionID} AND [Table]={tableNumber}";
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

        public static void LogonTable(int sectionID, int tableNumber)
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

        public static int NumberOfRoundsInEvent(int sectionID)
        {
            object queryResult = null;
            using(OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = SQLString = $"SELECT MAX(Round) FROM RoundData WHERE Section={sectionID}";
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
        public static int GetLastRoundWithResults(int sectionID, int table)
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT MAX(Round) FROM ReceivedData WHERE Section={sectionID} AND [Table]={table}";
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
                    PairString = $"<a style=\"color:red\">{pair}</a>";
                }
                else
                {
                    PairString = $"<a style=\"color:green\">{pair}</a>";
                }
            }
            else
            {
                if (EWVulnerability[(boardNo - 1) % 16])
                {
                    PairString = $"<a style=\"color:red\">{pair}</a>";
                }
                else
                {
                    PairString = $"<a style=\"color:green\">{pair}</a>";
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

        public static string GetNameFromPlayerNumbersTable(OdbcConnection conn, int sectionID, int round, int pairNo, string dir)
        {
            string number = "###";
            string name = "";

            // First look for entries in the same direction
            // If the player has changed (eg in teams), there will be more than one PlayerNumbers record for this pair number and direction
            // We need the most recently added name applicable to this round
            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={pairNo} AND Direction='{dir}'";
            OdbcCommand cmd1 = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader1 = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader1 = cmd1.ExecuteReader();
                    DateTime latestTimeLog = new DateTime(2010, 1, 1);
                    while (reader1.Read())
                    {
                        try
                        {
                            int readerRound = reader1.GetInt32(2);
                            DateTime timeLog;
                            if (reader1.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader1.GetDateTime(3);
                            }
                            if (readerRound <= round && timeLog >= latestTimeLog)
                            {
                                number = reader1.GetString(0);
                                name = reader1.GetString(1);
                                latestTimeLog = timeLog;
                            }
                        }
                        catch   // Record found, but format cannot be parsed
                        {
                            if (number == "###") number = "";
                        }
                    }
                });
            }
            finally
            {
                reader1.Close();
                cmd1.Dispose();
            }

            if (number == "###")  // Nothing found so try Round 0 entries in the other direction (for Howell type pairs movement)
            {
                string otherDir;
                switch (dir)
                {
                    case "N":
                        otherDir = "E";
                        break;
                    case "S":
                        otherDir = "W";
                        break;
                    case "E":
                        otherDir = "N";
                        break;
                    case "W":
                        otherDir = "S";
                        break;
                    default:
                        otherDir = "";
                        break;
                }
                SQLString = $"SELECT Number, Name, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={pairNo} AND Direction='{otherDir}' AND Round=0";
                OdbcCommand cmd2 = new OdbcCommand(SQLString, conn);
                OdbcDataReader reader2 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader2 = cmd2.ExecuteReader();
                        DateTime latestTimeLog = new DateTime(2010, 1, 1);
                        while (reader2.Read())
                        {
                            try
                            {
                                DateTime timeLog;
                                if (reader2.IsDBNull(2))
                                {
                                    timeLog = new DateTime(2010, 1, 1);
                                }
                                else
                                {
                                    timeLog = reader2.GetDateTime(2);
                                }
                                if (timeLog >= latestTimeLog)
                                {
                                    number = reader2.GetString(0);
                                    name = reader2.GetString(1);
                                    latestTimeLog = timeLog;
                                }
                            }
                            catch   // Record found, but format cannot be parsed
                            {
                                if (number == "###") number = "";
                            }
                        }
                    });
                }
                finally
                {
                    reader2.Close();
                    cmd2.Dispose();
                }
            }

            if (number == "###")  // Nothing found in either direction!!
            {
                number = "";
            }
            return FormatName(name, number);
        }

        public static string GetNameFromPlayerNumbersTableIndividual(OdbcConnection conn, int sectionID, int round, int playerNo)
        {
            string number = "###";
            string name = "";

            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={playerNo}";
            OdbcCommand cmd = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader = cmd.ExecuteReader();
                    DateTime latestTimeLog = new DateTime(2010, 1, 1);
                    while (reader.Read())
                    {
                        try
                        {
                            int readerRound = reader.GetInt32(2);
                            DateTime timeLog;
                            if (reader.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader.GetDateTime(3);
                            }
                            if (readerRound <= round && timeLog >= latestTimeLog)
                            {
                                number = reader.GetString(0);
                                name = reader.GetString(1);
                                latestTimeLog = timeLog;
                            }
                        }
                        catch  // Record found, but format cannot be parsed
                        {
                            if (number == "###") number = "";
                        }
                    }
                });
            }
            finally
            {
                cmd.Dispose();
                reader.Close();
            }

            if (number == "###")  // Nothing found
            {
                number = "";
            }

            return FormatName(name, number);
        }

        // Function to deal with different display format options for blank and unknown names
        private static string FormatName(string name, string number)
        {
            if (name == "" || name == "Unknown")
            {
                if (number == "")
                {
                    return "";
                }
                else if (number == "0")
                {
                    return "Unknown";
                }
                else
                {
                    return "Unknown #" + number;
                }
            }
            else
            {
                return name;
            }
        }

        public static string GetNameFromExternalDatabase(int playerNumber)
        {
            string name = "";
            OdbcConnectionStringBuilder externalDB = new OdbcConnectionStringBuilder { Driver = "Microsoft Access Driver (*.mdb)" };
            externalDB.Add("Dbq", @"C:\Bridgemate\BMPlayerDB.mdb");
            externalDB.Add("Uid", "Admin");
            using (OdbcConnection connection = new OdbcConnection(externalDB.ToString()))
            {
                object queryResult = null;
                string SQLString = $"SELECT Name FROM PlayerNameDatabase WHERE ID={playerNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                        if (queryResult == null)
                        {
                            name = "Unknown #" + playerNumber;
                        }
                        else
                        {
                            name = queryResult.ToString();
                        }
                    });
                }
                catch (OdbcException)  // If we can't read the external database for whatever reason...
                {
                    name = "#" + playerNumber;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return name;
        }
    }
}