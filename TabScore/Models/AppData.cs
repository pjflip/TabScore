﻿// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;

namespace TabScore.Models
{
    // AppData is a global class that applies accross all sessions
    // It includes a list of sections, a copy of the PlayerNames table (if it exists), and the current status of each table
    public static class AppData
    {
        public static string DBConnectionString { get; private set; }
        public static bool IsIndividual { get; private set; }
        public static List<Section> SectionsList = new List<Section>();
        public static List<TableStatus> TableStatusList = new List<TableStatus>();

        private class PlayerRecord
        {
            public string Name;
            public int Number;
        }
        private static readonly List<PlayerRecord> PlayerNamesTable = new List<PlayerRecord>();
        private static readonly string PathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
        private static DateTime TabScoreDBTime;

        public static void Refresh()
        {
            if (File.Exists(PathToTabScoreDB))
            {
                // Only do an update if TabScoreStarter has updated TabScoreDB.txt
                DateTime lastWriteTime = File.GetLastWriteTime(PathToTabScoreDB);
                if (lastWriteTime > TabScoreDBTime)
                {
                    // Clear table status list
                    TableStatusList.Clear();

                    string pathToDB = File.ReadAllText(PathToTabScoreDB);
                    TabScoreDBTime = lastWriteTime;
                    if (pathToDB == "")
                    {
                        DBConnectionString = "";
                    }
                    else
                    {
                        // Set database connection string
                        OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
                        cs.Driver = "Microsoft Access Driver (*.mdb)";
                        cs.Add("Dbq", pathToDB);
                        cs.Add("Uid", "Admin");
                        DBConnectionString = cs.ToString();

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
                            SQLString = "SELECT ID, Letter, Tables, MissingPair FROM Section";
                            SectionsList.Clear();
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
                                            MissingPair = reader.GetInt32(3)
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
                            PlayerNamesTable.Clear();
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
                }
            }
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
