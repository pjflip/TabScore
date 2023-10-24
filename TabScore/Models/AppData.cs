// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Resources;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using WebGrease.Activities;

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
            public string ID; 
            public string Name;
        }
        private static readonly List<PlayerRecord> PlayerNamesTable = new List<PlayerRecord>();
        private static readonly string PathToTabScoreDBtxt = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
        private static DateTime TabScoreDBTime = DateTime.MinValue;

        public static string Refresh()
        {
            if (!File.Exists(PathToTabScoreDBtxt)) return Strings.ErrorNoTabScoreDBtxt;
            DBConnectionString = File.ReadAllText(PathToTabScoreDBtxt);
            if (DBConnectionString == "") return Strings.ErrorNoDBSelected;

            DateTime lastTabScoreDBtxtUpdateTime = File.GetLastWriteTime(PathToTabScoreDBtxt);
            if (TabScoreDBTime == lastTabScoreDBtxtUpdateTime)  // TabScoreDB.txt has not changed since the last run of AppData.Refresh()
            {
                if (PermanentDBError)
                {
                    // Once a permanent database error has occurred, it needs a re-start of TabScoreStarter.exe to update TabScoreDB.txt 
                    return Strings.ErrorPermanentDB;
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

                    // Check if new event is an individual (in which case there will be a filled field 'South' in the RoundData table)
                    IsIndividual = true;
                    string SQLString = $"SELECT TOP 1 South FROM RoundData";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            object queryResult = cmd.ExecuteScalar();
                            if (queryResult == DBNull.Value || queryResult == null || Convert.ToString(queryResult) == "") IsIndividual = false;
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
                    }

                    // Retrieve global PlayerNames table
                    // Cater for possibility that one or both of ID and strID could be null/blank.  Prefer strID
                    System.Data.DataTable schema = connection.GetSchema("Columns");
                    int numCols = 0;
                    foreach (System.Data.DataRow row in schema.Select("TABLE_NAME = 'PlayerNames'"))
                    {
                       numCols++;
                    }
                    string extraCol = "";
                    if ( numCols == 3)
                    {
                        extraCol = ", StrID ";
                    }
                    SQLString = $"SELECT ID, Name" + extraCol + " FROM PlayerNames";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                object readerID = null;
                                if(!reader.IsDBNull(0))
                                {
                                    readerID = reader.GetValue(0);
                                }
                                object readerStrID = null;
                                if (numCols == 3)
                                {
                                    readerStrID = reader.GetValue(2);
                                }
                                string tempID = "";
                                if (readerStrID != null) tempID = Convert.ToString(readerStrID);
                                if (tempID == "" && readerID != null) tempID = Convert.ToString(Convert.ToInt32(readerID));

                                PlayerRecord playerRecord = new PlayerRecord
                                {
                                    ID = tempID,
                                    Name = reader.GetString(1),
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
                return Strings.ErrorPermanentDB;
            }
            return "";  // Successful refresh
        }

        public static string GetNameFromPlayerNamesTable(string playerID)
        {
            if (PlayerNamesTable.Count == 0)
            {
                return "#" + playerID;
            }
            PlayerRecord player = PlayerNamesTable.Find(x => (x.ID == playerID));
            if (player == null)
            {
                return Strings.Unknown + " #" + playerID;
            }
            else
            {
                return player.Name;
            }
        }
    }
}
