// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Round
    {
        public int TableNumber { get; set; }
        public int NumberNorth { get; set; } = 0;  // Applies to NS pair in pairs and teams
        public int NumberEast { get; set; } = 0;  // Applies to EW pair in pairs and teams
        public int NumberSouth { get; set; }
        public int NumberWest { get; set; }
        public string NameNorth { get; private set; }
        public string NameSouth { get; private set; }
        public string NameEast { get; private set; }
        public string NameWest { get; private set; }
        public bool GotAllNames { get; private set; }
        public int LowBoard { get; set; }
        public int HighBoard { get; set; }

        // Constructor for lists
        public Round() { }

        // Database read constructor
        public Round(TableStatus tableStatus)
        {
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={tableStatus.SectionID} AND Table={tableStatus.TableNumber} AND Round={tableStatus.RoundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                NumberNorth = reader.GetInt32(0);
                                NumberEast = reader.GetInt32(1);
                                NumberSouth = reader.GetInt32(2);
                                NumberWest = reader.GetInt32(3);
                                LowBoard = reader.GetInt32(4);
                                HighBoard = reader.GetInt32(5);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={tableStatus.SectionID} AND Table={tableStatus.TableNumber} AND Round={tableStatus.RoundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                NumberNorth = NumberSouth = reader.GetInt32(0);
                                NumberEast = NumberWest = reader.GetInt32(1);
                                LowBoard = reader.GetInt32(2);
                                HighBoard = reader.GetInt32(3);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
            }

            // Check for use of missing pair in Section table and set player numbers to 0 if necessary
            int missingPair = AppData.SectionsList.Find(x => x.SectionID == tableStatus.SectionID).MissingPair;
            if (NumberNorth == missingPair) NumberNorth = NumberSouth = 0;
            if (NumberEast == missingPair) NumberEast = NumberWest = 0;
            return;
        }

        public void UpdateNames(TableStatus tableStatus)
        {
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                CheckTabScorePairNos(connection);
                if (AppData.IsIndividual)
                {
                        NameNorth = GetNameFromPlayerNumbersTableIndividual(connection, tableStatus, NumberNorth);
                        NameSouth = GetNameFromPlayerNumbersTableIndividual(connection, tableStatus, NumberSouth);
                        NameEast = GetNameFromPlayerNumbersTableIndividual(connection, tableStatus, NumberEast);
                        NameWest = GetNameFromPlayerNumbersTableIndividual(connection, tableStatus, NumberWest);
                }
                else  // Not individual
                {
                        NameNorth = GetNameFromPlayerNumbersTable(connection, tableStatus, NumberNorth, "N");
                        NameSouth = GetNameFromPlayerNumbersTable(connection, tableStatus, NumberNorth, "S");
                        NameEast = GetNameFromPlayerNumbersTable(connection, tableStatus, NumberEast, "E");
                        NameWest = GetNameFromPlayerNumbersTable(connection, tableStatus, NumberEast, "W");
                }
            }

            GotAllNames = (NumberNorth == 0 || (NameNorth != "" && NameSouth != "")) && (NumberEast == 0 || (NameEast != "" && NameWest != ""));
            return;
        }

        private static void CheckTabScorePairNos(OdbcConnection conn)
        {
            object queryResult = null;

            // Check to see if TabScorePairNo exists (it may get overwritten if the scoring program recreates the PlayerNumbers table)
            string SQLString = $"SELECT 1 FROM PlayerNumbers WHERE TabScorePairNo IS NULL";
            OdbcCommand cmd1 = new OdbcCommand(SQLString, conn);
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    queryResult = cmd1.ExecuteScalar();
                });
            }
            finally
            {
                cmd1.Dispose();
            }

            if (queryResult != null)
            {
                // TabScorePairNo doesn't exist, so recreate it
                SQLString = "SELECT Section, [Table], Direction, Round FROM PlayerNumbers";
                OdbcCommand cmd2 = new OdbcCommand(SQLString, conn);
                OdbcDataReader reader2 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader2 = cmd2.ExecuteReader();
                        while (reader2.Read())
                        {
                            int tempSectionID = reader2.GetInt32(0);
                            int tempTable = reader2.GetInt32(1);
                            string tempDirection = reader2.GetString(2);
                            int tempRoundNumber = reader2.GetInt32(3);
                            int queryRoundNumber = tempRoundNumber;
                            if (queryRoundNumber == 0) queryRoundNumber = 1;
                            if (AppData.IsIndividual)
                            {
                                switch (tempDirection)
                                {
                                    case "N":
                                        SQLString = $"SELECT NSPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND={queryRoundNumber}";
                                        break;
                                    case "S":
                                        SQLString = $"SELECT South FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND={queryRoundNumber}";
                                        break;
                                    case "E":
                                        SQLString = $"SELECT EWPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND={queryRoundNumber}";
                                        break;
                                    case "W":
                                        SQLString = $"SELECT West FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND={queryRoundNumber}";
                                        break;
                                }
                            }
                            else
                            {
                                switch (tempDirection)
                                {
                                    case "N":
                                    case "S":
                                        SQLString = $"SELECT NSPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND={queryRoundNumber}";
                                        break;
                                    case "E":
                                    case "W":
                                        SQLString = $"SELECT EWPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND={queryRoundNumber}";
                                        break;
                                }
                            }
                            OdbcCommand cmd3 = new OdbcCommand(SQLString, conn);
                            try
                            {
                                ODBCRetryHelper.ODBCRetry(() =>
                                {
                                    queryResult = cmd3.ExecuteScalar();
                                });
                            }
                            finally
                            {
                                cmd3.Dispose();
                            }
                            string TSpairNo = queryResult.ToString();
                            SQLString = $"UPDATE PlayerNumbers SET TabScorePairNo={TSpairNo} WHERE Section={tempSectionID} AND [Table]={tempTable} AND Direction='{tempDirection}' AND Round={tempRoundNumber}";
                            OdbcCommand cmd4 = new OdbcCommand(SQLString, conn);
                            try
                            {
                                ODBCRetryHelper.ODBCRetry(() =>
                                {
                                    cmd4.ExecuteNonQuery();
                                });
                            }
                            finally
                            {
                                cmd4.Dispose();
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
        }

        private static string GetNameFromPlayerNumbersTable(OdbcConnection conn, TableStatus tableStatus, int pairNo, string dir)
        {
            if (pairNo == 0) return "";
            string number = "";
            string name = "";
            DateTime latestTimeLog = new DateTime(2010, 1, 1);

            // First look for entries in the same direction
            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={tableStatus.SectionID} AND TabScorePairNo={pairNo} AND Direction='{dir}'";
            OdbcCommand cmd = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            int readerRoundNumber = reader.GetInt32(2);
                            DateTime timeLog;
                            if (reader.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader.GetDateTime(3);
                            }
                            if (readerRoundNumber <= tableStatus.RoundNumber && timeLog >= latestTimeLog)
                            {
                                number = reader.GetString(0);
                                name = reader.GetString(1);
                                latestTimeLog = timeLog;
                            }
                        }
                        catch { }  // Record found, but format cannot be parsed
                    }
                });
            }
            finally 
            {
                reader.Close();
            }

            if (AppData.SectionsList.Find(x => x.SectionID == tableStatus.SectionID).Winners == 1)  // If a one-winner pairs movement, we also need to check the other direction 
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
                SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={tableStatus.SectionID} AND TabScorePairNo={pairNo} AND Direction='{otherDir}'";
                cmd = new OdbcCommand(SQLString, conn);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            try
                            {
                                int readerRoundNumber = reader.GetInt32(2);
                                DateTime timeLog;
                                if (reader.IsDBNull(3))
                                {
                                    timeLog = new DateTime(2010, 1, 1);
                                }
                                else
                                {
                                    timeLog = reader.GetDateTime(3);
                                }
                                if (readerRoundNumber <= tableStatus.RoundNumber && timeLog >= latestTimeLog)
                                {
                                    number = reader.GetString(0);
                                    name = reader.GetString(1);
                                    latestTimeLog = timeLog;
                                }
                            }
                            catch { } // Record found, but format cannot be parsed
                        }
                    });
                }
                finally
                {
                    reader.Close();
                }
            }
            cmd.Dispose();
            return FormatName(name, number);
        }

        private static string GetNameFromPlayerNumbersTableIndividual(OdbcConnection conn, TableStatus tableStatus, int playerNo)
        {
            if (playerNo == 0) return "";
            string number = "";
            string name = "";
            DateTime latestTimeLog = new DateTime(2010, 1, 1);

            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={tableStatus.SectionID} AND TabScorePairNo={playerNo}";
            OdbcCommand cmd = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            int readerRoundNumber = reader.GetInt32(2);
                            DateTime timeLog;
                            if (reader.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader.GetDateTime(3);
                            }
                            if (readerRoundNumber <= tableStatus.RoundNumber && timeLog >= latestTimeLog)
                            {
                                number = reader.GetString(0);
                                name = reader.GetString(1);
                                latestTimeLog = timeLog;
                            }
                        }
                        catch { } // Record found, but format cannot be parsed
                    }
                });
            }
            finally
            {
                cmd.Dispose();
                reader.Close();
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

        public void UpdatePlayer(int sectionID, int tableNumber, Direction direction, int roundNumber, int playerNumber)
        {
            // First get name, and update the Round
            string playerName = "";
            string directionLetter = Enum.GetName(typeof(Direction), direction).Substring(0, 1);    // Need just N, S, E or W
            
            if (playerNumber == 0)
            {
                playerName = "Unknown";
            }
            else
            {
                switch (Settings.NameSource)
                {
                    case 0:
                        playerName = AppData.GetNameFromPlayerNamesTable(playerNumber);
                        break;
                    case 1:
                        playerName = GetNameFromExternalDatabase(playerNumber);
                        break;
                    case 2:
                        playerName = "";
                        break;
                    case 3:
                        playerName = AppData.GetNameFromPlayerNamesTable(playerNumber);
                        if (playerName == "" || playerName.Substring(0, 1) == "#" || (playerName.Length >= 7 && playerName.Substring(0, 7) == "Unknown"))
                        {
                            playerName = GetNameFromExternalDatabase(playerNumber);
                        }
                        break;
                }
            }

            int pairNumber = 0;
            if (AppData.IsIndividual)
            {
                switch (directionLetter)
                {
                    case "N":
                        NameNorth = playerName;
                        pairNumber = NumberNorth;
                        break;
                    case "S":
                        NameSouth = playerName;
                        pairNumber = NumberSouth;
                        break;
                    case "E":
                        NameEast = playerName;
                        pairNumber = NumberEast;
                        break;
                    case "W":
                        NameWest = playerName;
                        pairNumber = NumberWest;
                        break;
                }
            }
            else
            {
                switch (directionLetter)
                {
                    case "N":
                        NameNorth = playerName;
                        pairNumber = NumberNorth;
                        break;
                    case "S":
                        NameSouth = playerName;
                        pairNumber = NumberNorth;
                        break;
                    case "E":
                        NameEast = playerName;
                        pairNumber = NumberEast;
                        break;
                    case "W":
                        NameWest = playerName;
                        pairNumber = NumberEast;
                        break;
                }
            }

            // Now update the PlayerNumbers table in the database
            // Numbers entered at the start (when round = 1) need to be set as round 0 in the database
            if (roundNumber == 1)
            {
                roundNumber = 0;
            }
            if (playerNumber == 0)
            {
                playerName = "";
            }
            playerName = playerName.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                object queryResult = null;

                // Check if PlayerNumbers entry exists already; if it does update it, if not create it
                string SQLString = $"SELECT Section FROM PlayerNumbers WHERE Section={sectionID} AND [Table]={tableNumber} AND Round={roundNumber} AND Direction='{directionLetter}'";
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
                if (queryResult == null)
                {
                    SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabScorePairNo) VALUES ({sectionID}, {tableNumber}, '{directionLetter}', '{playerNumber}', '{playerName}', {roundNumber}, False, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, {pairNumber})";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{playerName}', Processed=False, TimeLog=#{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, TabScorePairNo={pairNumber} WHERE Section={sectionID} AND [Table]={tableNumber} AND Round={roundNumber} AND Direction='{directionLetter}'";
                }
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd2.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd2.Dispose();
                }
            }
            return;
        }

        private static string GetNameFromExternalDatabase(int playerNumber)
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
