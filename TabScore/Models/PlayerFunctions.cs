using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class PlayerFunctions
    {
        public static void UpdateDatabase(string DB, int sectionID, int table, int round, string direction, string playerNumber, bool individual) 
        {
            string dir = direction.Substring(0, 1);    // Need just N, S, E or W
            string SQLString = null;
            object queryResult = null;
            string pairNo = null;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                // Get pair number for this player to set TabScorePairNo field
                if (individual)
                {
                    switch (dir)
                    {
                        case "N":
                            SQLString = $"SELECT NSPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                            break;
                        case "S":
                            SQLString = $"SELECT South FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                            break;
                        case "E":
                            SQLString = $"SELECT EWPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                            break;
                        case "W":
                            SQLString = $"SELECT West FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                            break;
                    }
                }
                else
                {
                    switch (dir)
                    {
                        case "N":
                        case "S":
                            SQLString = $"SELECT NSPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                            break;
                        case "E":
                        case "W":
                            SQLString = $"SELECT EWPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                            break;
                    }
                }

                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
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
                pairNo = queryResult.ToString();   // Doubles as player number for individual events
            }

            // Numbers entered at the start (when round = 1) need to be set as round 0
            if (round == 1)
            {
                round = 0;
            }

            // Set the name for this number
            string name;
            if (playerNumber == "Unknown" || playerNumber == "0" || playerNumber == "")
            {
                playerNumber = "0";
                name = "";
            }
            else
            {
                name = PlayerFunctions.GetNameFromNumber(DB, playerNumber);
                if (name.Substring(0, 1) == "#")    // PlayerNames table doesn't exist, so let scoring software set the name 
                {
                    name = "";
                }
                else
                {
                    name = name.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor
                }
            }

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();

                // Check if PlayerNumbers entry exists already; if it does update it, if not create it
                SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={sectionID} AND [Table]={table} AND ROUND={round} AND Direction='{dir}'";
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd2.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd2.Dispose();
                }
                if (queryResult == null)
                {
                    SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabScorePairNo) VALUES ({sectionID}, {table}, '{dir}', '{playerNumber}', '{name}', {round}, False, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, {pairNo})";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{name}', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, TabScorePairNo={pairNo} WHERE Section={sectionID} AND [Table]={table} AND Round={round} AND Direction='{dir}'";
                }
                OdbcCommand cmd3 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd3.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd3.Dispose();
                }
            }
        }

        public static string GetNameFromNumber(string DB, string playerNumber)
        {
            switch (new Settings(DB).NameSource)
            {
                case 0:
                    return GetNameFromPlayerNamesTable(DB, playerNumber);
                case 1:
                    return GetNameFromExternalDatabase(playerNumber);
                case 2:
                    return "";
                case 3:
                    string name;
                    name = GetNameFromPlayerNamesTable(DB, playerNumber);
                    if (name == "" || name.Substring(0,1) == "#" || (name.Length >= 7 && name.Substring(0,7) == "Unknown"))
                    {
                        return GetNameFromExternalDatabase(playerNumber);
                    }
                    else
                    {
                        return name;
                    }
                default:
                    return "";

            }
        }

        private static string GetNameFromPlayerNamesTable(string DB, string playerNumber)
        {
            string name;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult = null;
                connection.Open();
                string SQLString = $"SELECT Name FROM PlayerNames WHERE ID={playerNumber}";

                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S02")  // PlayerNames table does not exist
                    {
                        name = "#" + playerNumber;
                    }
                    else
                    {
                        throw (e);
                    }
                }
                finally
                {
                    cmd.Dispose();
                }

                if (queryResult == null)
                {
                    name = "Unknown #" + playerNumber;
                }
                else
                {
                    name = queryResult.ToString();
                }
            }
            return name;
        }

        private static string GetNameFromExternalDatabase(string playerNumber)
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

        public static string GetName(string DB, int sectionID, int round, int pairNo, string direction)
        {
            string number = "###";
            string name = "";

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult = null;
                connection.Open();

                // Check to see if TabScorePairNo exists (it may get overwritten), and if not recreate it
                string SQLString = $"SELECT 1 FROM PlayerNumbers WHERE TabScorePairNo IS NULL";
                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
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
                    // This duplicates the code in TabScoreStarter
                    SQLString = "SELECT Section, [Table], Direction FROM PlayerNumbers";
                    OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
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
                                if (tempDirection == "N" || tempDirection == "S")
                                {
                                    SQLString = $"SELECT NSPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                }
                                else
                                {
                                    SQLString = $"SELECT EWPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                }

                                OdbcCommand cmd3 = new OdbcCommand(SQLString, connection);
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
                                OdbcCommand cmd4 = new OdbcCommand(SQLString, connection);
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

                // First look for entries in the same direction
                SQLString = $"SELECT Number, Name, Round FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={pairNo} AND Direction='{direction}'";
                OdbcCommand cmd5 = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader5 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader5 = cmd5.ExecuteReader();
                        int biggestRoundSoFar = -1;
                        while (reader5.Read())
                        {
                            int readerRound = Convert.ToInt32(reader5.GetValue(2));
                            if (readerRound <= round && readerRound > biggestRoundSoFar)
                            {
                                if (!reader5.IsDBNull(0))
                                {
                                    number = reader5.GetString(0);
                                }
                                if (!reader5.IsDBNull(1))
                                {
                                    name = reader5.GetString(1);
                                }
                                biggestRoundSoFar = readerRound;
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
                    string otherDirection;
                    switch (direction)
                    {
                        case "N":
                            otherDirection = "E";
                            break;
                        case "S":
                            otherDirection = "W";
                            break;
                        case "E":
                            otherDirection = "N";
                            break;
                        case "W":
                            otherDirection = "S";
                            break;
                        default:
                            otherDirection = "";
                            break;
                    }
                    SQLString = $"SELECT Number, Name FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={pairNo} AND Direction='{otherDirection}' AND Round=0";
                    OdbcCommand cmd6 = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader6 = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader6 = cmd6.ExecuteReader();
                            while (reader6.Read())
                            {
                                if (!reader6.IsDBNull(0))
                                {
                                    number = reader6.GetString(0);
                                }
                                if (!reader6.IsDBNull(1))
                                {
                                    name = reader6.GetString(1);
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
            }
            return FormatName(name, number);
        }

        public static string GetNameIndividual(string DB, int sectionID, int round, int playerNo)
        {
            string number = "###";
            string name = "";

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                object queryResult = null;
                connection.Open();

                // Check to see if TabScorePairNo exists (it may get overwritten), and if not recreate it
                string SQLString = $"SELECT 1 FROM PlayerNumbers WHERE TabScorePairNo IS NULL";
                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
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
                    // This duplicates the code in TabScoreStarter
                    SQLString = "SELECT Section, [Table], Direction FROM PlayerNumbers";
                    OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
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

                                OdbcCommand cmd3 = new OdbcCommand(SQLString, connection);
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
                                OdbcCommand cmd4 = new OdbcCommand(SQLString, connection);
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

                // Now find name from TabScorePairNo
                SQLString = $"SELECT Number, Name, Round FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={playerNo}";
                OdbcCommand cmd5 = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader5 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader5 = cmd5.ExecuteReader();
                        int biggestRoundSoFar = -1;
                        while (reader5.Read())
                        {
                            int readerRound = Convert.ToInt32(reader5.GetValue(2));
                            if (readerRound <= round && readerRound > biggestRoundSoFar)
                            {
                                if (!reader5.IsDBNull(0))
                                {
                                    number = reader5.GetString(0);
                                }
                                if (!reader5.IsDBNull(1))
                                {
                                    name = reader5.GetString(1);
                                }
                                biggestRoundSoFar = readerRound;
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
    }
}