// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;

namespace TabScore.Models
{
    // AppData is a global class that applies accross all sessions
    // It includes a list of sections, a copy of the PlayerNames table (if it exists), the start time for each round, and the current status of each table and each tablet device
    public static class AppData
    {
        public static string DBConnectionString { get; private set; }
        public static bool IsIndividual { get; private set; }
        public static List<Section> SectionsList = new List<Section>();
        public static List<TableStatus> TableStatusList = new List<TableStatus>();
        public static List<TabletDeviceStatus> TabletDeviceStatusList = new List<TabletDeviceStatus>();
        public static List<RoundTimer> RoundTimerList = new List<RoundTimer>();
        public static bool PermanentDBError = false;

        private class PlayerRecord
        {
            public string Name;
            public int Number;
        }
        private static readonly List<PlayerRecord> PlayerNamesTable = new List<PlayerRecord>();
        private static readonly string PathToTabScoreDBtxt = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
        private static DateTime TabScoreDBTime = DateTime.MinValue;

        public static string Refresh()
        {
            if (!File.Exists(PathToTabScoreDBtxt)) return "File TabScoreDB.txt doesn't exist.  Please re-start TabScoreStarter.exe";
            DBConnectionString = File.ReadAllText(PathToTabScoreDBtxt);
            if (DBConnectionString == "") return "No Scoring Database selected; please wait until the Tournament Director tells you to start";

            DateTime lastTabScoreDBtxtUpdateTime = File.GetLastWriteTime(PathToTabScoreDBtxt);
            if (TabScoreDBTime == lastTabScoreDBtxtUpdateTime)  // TabScoreDB.txt has not changed since the last run of AppData.Refresh()
            {
                if (PermanentDBError)
                {
                    // Once a permanent database error has occurred, it needs a re-start of TabScoreStarter.exe to update TabScoreDB.txt 
                    return "Permanent database connection error.  Please check format and access permissions for Scoring Database file, and re-start TabScoreStarter.exe";
                }
                else
                {
                    // No refresh required as it has already been done by another tablet device
                    return "";
                }
            }

            // TabScoreStarter.exe has updated TabScoreDB.txt with a new database connection string, so hopefully any error has been resolved.
            PermanentDBError = false;
            TabScoreDBTime = lastTabScoreDBtxtUpdateTime;

            // Clear all application-wide lists
            TableStatusList.Clear();
            TabletDeviceStatusList.Clear();
            RoundTimerList.Clear();
            SectionsList.Clear();
            PlayerNamesTable.Clear();

            try
            {
                using (OdbcConnection connection = new OdbcConnection(DBConnectionString))
                {
                    connection.Open();

                    // Check if new event is an individual (in which case there will be a field 'South' in the RoundData table)
                    string SQLString = $"SELECT TOP 1 South FROM RoundData";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            cmd.ExecuteScalar();
                            IsIndividual = true;
                        });
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count > 1 || e.Errors[0].SQLState != "07002")   // Error other than field 'South' doesn't exist
                        {
                            throw (e);
                        }
                        else
                        {
                            IsIndividual = false;
                        }
                    }
                    finally
                    {
                        cmd.Dispose();
                    }

                    // Create list of sections
                    SQLString = "SELECT ID, Letter, Tables, MissingPair, Winners FROM Section";
                    cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Section s = new Section
                                {
                                    SectionID = reader.GetInt32(0),
                                    SectionLetter = reader.GetString(1),
                                    NumTables = reader.GetInt32(2),
                                    MissingPair = reader.GetInt32(3),
                                    Winners = reader.GetInt32(4)
                                };
                                SectionsList.Add(s);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }

                    // Retrieve global PlayerNames table
                    SQLString = $"SELECT Name, ID FROM PlayerNames";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                PlayerRecord playerRecord = new PlayerRecord
                                {
                                    Name = reader.GetString(0),
                                    Number = reader.GetInt32(1)
                                };
                                PlayerNamesTable.Add(playerRecord);
                            };
                        });
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than PlayerNames table does not exist
                        {
                            throw (e);
                        }
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
            }
            catch
            {
                PermanentDBError = true;
                return "Permanent database connection error.  Please check format and access permissions for Scoring Database file, and re-start TabScoreStarter.exe";
            }
            return "";  // Successful refresh
        }

        public static string GetNameFromPlayerNamesTable(int playerNumber)
        {
            if (PlayerNamesTable.Count == 0)
            {
                return "#" + playerNumber;
            }
            PlayerRecord player = PlayerNamesTable.Find(x => (x.Number == playerNumber));
            if (player == null)
            {
                return "Unknown #" + playerNumber;
            }
            else
            {
                return player.Name;
            }
        }
    }
}
