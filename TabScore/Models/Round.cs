// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Round
    {
        public int RoundNumber { get; private set; }
        public int TableNumber { get; set; }
        public int PairNS { get; set; }   // Doubles as North player number for individuals
        public int PairEW { get; set; }   // Doubles as East player number for individuals
        public int LowBoard { get; set; }
        public int HighBoard { get; set; }
        public int South { get; set; }
        public int West { get; set; }
        public string NameNorth { get; private set; }
        public string NameSouth { get; private set; }
        public string NameEast { get; private set; }
        public string NameWest { get; private set; }

        // Constructor used for creating lists
        public Round() { }
        
        // Database read constructor
        public Round(SessionData sessionData, int roundNumber)
        {
            RoundNumber = roundNumber;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                PairNS = 0;
                connection.Open();
                CheckTabScorePairNos(connection);
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={sessionData.SectionID} AND Table={sessionData.TableNumber} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
                                South = reader.GetInt32(2);
                                West = reader.GetInt32(3);
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
                    NameNorth = GetNameFromPlayerNumbersTableIndividual(connection, sessionData.SectionID, RoundNumber, PairNS);
                    NameSouth = GetNameFromPlayerNumbersTableIndividual(connection, sessionData.SectionID, RoundNumber, South);
                    NameEast = GetNameFromPlayerNumbersTableIndividual(connection, sessionData.SectionID, RoundNumber, PairEW);
                    NameWest = GetNameFromPlayerNumbersTableIndividual(connection, sessionData.SectionID, RoundNumber, West);
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sessionData.SectionID} AND Table={sessionData.TableNumber} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
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
                    NameNorth = GetNameFromPlayerNumbersTable(connection, sessionData.SectionID, RoundNumber, PairNS, "N");
                    NameSouth = GetNameFromPlayerNumbersTable(connection, sessionData.SectionID, RoundNumber, PairNS, "S");
                    NameEast = GetNameFromPlayerNumbersTable(connection, sessionData.SectionID, RoundNumber, PairEW, "E");
                    NameWest = GetNameFromPlayerNumbersTable(connection, sessionData.SectionID, RoundNumber, PairEW, "W");
                }
            }
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
                            SQLString = $"UPDATE PlayerNumbers SET TabScorePairNo={TSpairNo} WHERE Section={tempSectionID.ToString()} AND [Table]={tempTable.ToString()} AND Direction='{tempDirection}'";
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

        private static string GetNameFromPlayerNumbersTable(OdbcConnection conn, int sectionID, int round, int pairNo, string dir)
        {
            string number = "###";
            string name = "";
            
            // First look for entries in the same direction
            // If the player has changed (eg in teams), we need the latest name applicable to this round
            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={pairNo} AND Direction='{dir}'";
            OdbcCommand cmd5 = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader5 = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader5 = cmd5.ExecuteReader();
                    DateTime latestTimeLog = new DateTime(2010, 1, 1);
                    while (reader5.Read())
                    {
                        try
                        {
                            int readerRound = reader5.GetInt32(2);
                            DateTime timeLog;
                            if(reader5.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader5.GetDateTime(3); 
                            }
                            if (readerRound <= round && timeLog >= latestTimeLog)
                            {
                                number = reader5.GetString(0);
                                name = reader5.GetString(1);
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
                reader5.Close();
                cmd5.Dispose();
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
                OdbcCommand cmd6 = new OdbcCommand(SQLString, conn);
                OdbcDataReader reader6 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader6 = cmd6.ExecuteReader();
                        DateTime latestTimeLog = new DateTime(2010, 1, 1);
                        while (reader6.Read())
                        {
                            try
                            {
                                DateTime timeLog;
                                if (reader6.IsDBNull(2))
                                {
                                    timeLog = new DateTime(2010, 1, 1);
                                }
                                else
                                {
                                    timeLog = reader6.GetDateTime(2);
                                }
                                if (timeLog >= latestTimeLog)
                                {
                                    number = reader6.GetString(0);
                                    name = reader6.GetString(1);
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
                    reader6.Close();
                    cmd6.Dispose();
                }
            }

            if (number == "###")  // Nothing found in either direction!!
            {
                number = "";
            }
            return FormatName(name, number);
        }

        private static string GetNameFromPlayerNumbersTableIndividual(OdbcConnection conn, int sectionID, int round, int playerNo)
        {
            string number = "###";
            string name = "";

            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={playerNo}";
            OdbcCommand cmd5 = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader5 = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader5 = cmd5.ExecuteReader();
                    DateTime latestTimeLog = new DateTime(2010, 1, 1);
                    while (reader5.Read())
                    {
                        try
                        {
                            int readerRound = reader5.GetInt32(2);
                            DateTime timeLog;
                            if (reader5.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else 
                            {
                                timeLog = reader5.GetDateTime(3);
                            }
                            if (readerRound <= round && timeLog >= latestTimeLog)
                            {
                                number = reader5.GetString(0);
                                name = reader5.GetString(1);
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
                cmd5.Dispose();
                reader5.Close();
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

        public void UpdatePlayer(SessionData sessionData, string direction, int playerNumber)
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
                        playerName = PlayerNames.GetNameFromNumber(playerNumber);
                        break;
                    case 1:
                        playerName = GetNameFromExternalDatabase(playerNumber);
                        break;
                    case 2:
                        playerName = "";
                        break;
                    case 3:
                        string name;
                        name = PlayerNames.GetNameFromNumber(playerNumber);
                        if (name == "" || name.Substring(0, 1) == "#" || (name.Length >= 7 && name.Substring(0, 7) == "Unknown"))
                        {
                            playerName = GetNameFromExternalDatabase(playerNumber);
                        }
                        else
                        {
                            playerName = name;
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
                        pairNumber = PairNS;
                        break;
                    case "S":
                        NameSouth = playerName;
                        pairNumber = South;
                        break;
                    case "E":
                        NameEast = playerName;
                        pairNumber = PairEW;
                        break;
                    case "W":
                        NameWest = playerName;
                        pairNumber = West;
                        break;
                }
            }
            else
            {
                switch (dir)
                {
                    case "N":
                        NameNorth = playerName;
                        pairNumber = PairNS;
                        break;
                    case "S":
                        NameSouth = playerName;
                        pairNumber = PairNS;
                        break;
                    case "E":
                        NameEast = playerName;
                        pairNumber = PairEW;
                        break;
                    case "W":
                        NameWest = playerName;
                        pairNumber = PairEW;
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
                string SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={sessionData.SectionID} AND [Table]={sessionData.TableNumber} AND ROUND={roundNumber} AND Direction='{dir}'";
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
                    SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabScorePairNo) VALUES ({sessionData.SectionID}, {sessionData.TableNumber}, '{dir}', '{playerNumber}', '{playerName}', {roundNumber}, False, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, {pairNumber})";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{playerName}', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, TabScorePairNo={pairNumber} WHERE Section={sessionData.SectionID} AND [Table]={sessionData.TableNumber} AND Round={roundNumber} AND Direction='{dir}'";
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
