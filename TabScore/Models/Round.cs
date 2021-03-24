// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Round
    {
        public int TableNumber { get; set; }
        public int RoundNumber { get; private set; }
        public int NumberNorth { get; set; }   // Applies to NS pair in pairs and teams
        public int NumberEast { get; set; }   // Applies to EW pair in pairs and teams
        public int NumberSouth { get; set; }
        public int NumberWest { get; set; }
        public string NameNorth { get; private set; }
        public string NameSouth { get; private set; }
        public string NameEast { get; private set; }
        public string NameWest { get; private set; }
        public bool BlankName { get; private set; }
        public int LowBoard { get; set; }
        public int HighBoard { get; set; }

        // Constructor used for creating lists
        public Round() { }
        
        // Get specific round info
        public Round(int sectionID, int tableNumber, int roundNumber)
        {
            RoundNumber = roundNumber;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                NumberNorth = 0;
                connection.Open();
                CheckTabScorePairNos(connection);
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
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
                    NameNorth = GetNameFromPlayerNumbersTableIndividual(connection, sectionID, roundNumber, NumberNorth);
                    NameSouth = GetNameFromPlayerNumbersTableIndividual(connection, sectionID, roundNumber, NumberSouth);
                    NameEast = GetNameFromPlayerNumbersTableIndividual(connection, sectionID, roundNumber, NumberEast);
                    NameWest = GetNameFromPlayerNumbersTableIndividual(connection, sectionID, roundNumber, NumberWest);
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
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
                    NameNorth = GetNameFromPlayerNumbersTable(connection, sectionID, roundNumber, NumberNorth, "N");
                    NameSouth = GetNameFromPlayerNumbersTable(connection, sectionID, roundNumber, NumberNorth, "S");
                    NameEast = GetNameFromPlayerNumbersTable(connection, sectionID, roundNumber, NumberEast, "E");
                    NameWest = GetNameFromPlayerNumbersTable(connection, sectionID, roundNumber, NumberEast, "W");
                }
            }
            BlankName = NameNorth == "" || NameEast == "" || NameSouth == "" || NameWest == "";
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
                // TabScorePairNo doesn't exist, so recreate it.  This duplicates the code in TabScoreStarter
                SQLString = "SELECT Section, [Table], Direction FROM PlayerNumbers";
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
                            if (AppData.IsIndividual)
                            {
                                switch (tempDirection)
                                {
                                    case "N":
                                        SQLString = $"SELECT NSPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "S":
                                        SQLString = $"SELECT South FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "E":
                                        SQLString = $"SELECT EWPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "W":
                                        SQLString = $"SELECT West FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                }
                            }
                            else
                            {
                                switch (tempDirection)
                                {
                                    case "N":
                                    case "S":
                                        SQLString = $"SELECT NSPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "E":
                                    case "W":
                                        SQLString = $"SELECT EWPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
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
                            SQLString = $"UPDATE PlayerNumbers SET TabScorePairNo={TSpairNo} WHERE Section={tempSectionID} AND [Table]={tempTable} AND Direction='{tempDirection}'";
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

        private static string GetNameFromPlayerNumbersTable(OdbcConnection conn, int sectionID, int roundNumber, int pairNo, string dir)
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
                            int readerRoundNumber = reader1.GetInt32(2);
                            DateTime timeLog;
                            if(reader1.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader1.GetDateTime(3); 
                            }
                            if (readerRoundNumber <= roundNumber && timeLog >= latestTimeLog)
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

        private static string GetNameFromPlayerNumbersTableIndividual(OdbcConnection conn, int sectionID, int roundNumber, int playerNo)
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
                            if (readerRoundNumber <= roundNumber && timeLog >= latestTimeLog)
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

        public void UpdatePlayer(int sectionID, int tableNumber, string direction, int playerNumber)
        {
            // First get name, and update the Round
            string playerName = "";
            string dir = direction.Substring(0, 1);    // Need just N, S, E or W
            
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
                switch (dir)
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
                switch (dir)
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
            int roundNumber = RoundNumber;
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
                string SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={sectionID} AND [Table]={tableNumber} AND ROUND={roundNumber} AND Direction='{dir}'";
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
                if (queryResult == DBNull.Value || queryResult == null)
                {
                    SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabScorePairNo) VALUES ({sectionID}, {tableNumber}, '{dir}', '{playerNumber}', '{playerName}', {roundNumber}, False, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, {pairNumber})";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{playerName}', Processed=False, TimeLog=#{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, TabScorePairNo={pairNumber} WHERE Section={sectionID} AND [Table]={tableNumber} AND Round={roundNumber} AND Direction='{dir}'";
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
