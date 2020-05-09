// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;
using System.IO;

namespace TabScore.Models
{
    // AppData is a global class that applies accross all sessions
    public static class AppData
    {
        public static string DBConnectionString { get; private set; }
        public static bool IsIndividual { get; private set; }

        private static string PathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
        private static DateTime TabScoreDBTime;

        public static void Refresh()
        {
            if (File.Exists(PathToTabScoreDB)) 
            {
                // Only do an update if TabScoreStarter has updated TabScoreDB.txt
                DateTime lastWriteTime = File.GetLastWriteTime(PathToTabScoreDB);
                if (lastWriteTime > TabScoreDBTime)
                {
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

                        // Check if new event is an individual (in which case there will be a field 'South' in the RoundData table)
                        using (OdbcConnection connection = new OdbcConnection(DBConnectionString))
                        {
                            connection.Open();
                            string SQLString = $"SELECT TOP 1 South FROM RoundData";
                            OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                            int attempts = 3;
                            try
                            {
                                while (true)
                                {
                                    try
                                    {
                                        attempts--;
                                        cmd.ExecuteScalar();
                                        IsIndividual = true;
                                        break;
                                    }
                                    catch (OdbcException e)
                                    {
                                        if (e.Errors.Count == 1 && e.Errors[0].SQLState == "07002")   // Field 'South' doesn't exist
                                        {
                                            IsIndividual = false;
                                            break;
                                        }
                                        if (attempts <= 0) throw e;
                                        Random r = new Random();
                                        System.Threading.Thread.Sleep(r.Next(300, 500));
                                    }
                                }
                            }
                            finally 
                            {
                                cmd.Dispose();
                            }
                        }

                        // Update global PlayerNames table
                        PlayerNames.Refresh();
                    }
                }
            }
        }
    }
}