// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    // PlayerNames provides an single internal copy of the database PlayerNames table (if it exists) that can be read in all sessions
    public static class PlayerNames
    {
        private class PlayerRecord
        {
            public string Name;
            public int Number;
        }

        private static readonly List<PlayerRecord> PlayerNamesTable = new List<PlayerRecord>();

        public static void Refresh()
        {
            PlayerNamesTable.Clear();
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT Name, ID FROM PlayerNames";

                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        OdbcDataReader reader = cmd.ExecuteReader();
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
                    cmd.Dispose();
                }
            }
        }

        public static string GetNameFromNumber(int playerNumber)
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