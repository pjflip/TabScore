using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Player
    {
        public static string UpdateDatabase(string DB, string sectionID, string table, string round, string direction, string playerNumber) 
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string dir = direction.Substring(0, 1);    // Need just N, S, E or W
                string SQLString;
                object queryResult = null;

                connection.Open();
                // Get pair number for this player to set TabScorePairNo field
                if (dir == "N" || dir == "S")
                {
                    SQLString = $"SELECT NSPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                }
                else
                {
                    SQLString = $"SELECT EWPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                }

                OdbcCommand cmd1 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd1.ExecuteScalar();
                    });
                }
                catch (OdbcException)
                {
                    return "Error";
                }
                finally
                {
                    cmd1.Dispose();
                }
                string pairNo = queryResult.ToString();

                // Numbers entered at the start (when round = 1) need to be set as round 0
                if (round == "1")
                {
                    round = "0";
                }

                // Check if PlayerNumbers record exists; if it does update it, if not create it
                SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={sectionID} AND [Table]={table} AND ROUND={round} AND Direction='{dir}'";
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd2.ExecuteScalar();
                    });
                }
                catch (OdbcException)
                {
                    return "Error";
                }
                finally
                {
                    cmd2.Dispose();
                }

                if (queryResult == null)
                {
                    if (playerNumber == "Unknown" || playerNumber == "0" || playerNumber == "")
                    {
                        SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Round, Processed, TimeLog, TabScorePairNo) VALUES ({sectionID}, {table}, '{dir}', '0', {round}, False, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, {pairNo})";
                    }
                    else
                    {
                        string name = Player.GetNameFromNumber(DB, playerNumber);
                        if (name == "Error") return "Error";
                        if (name.Substring(0, 1) == "#")    // PlayerNames table doesn't exist, so let scoring software set the name 
                        {
                            SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Round, Processed, TimeLog, TabScorePairNo) VALUES ({sectionID}, {table}, '{dir}', '{playerNumber}', {round}, False, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, {pairNo})";
                        }
                        else
                        {
                            name = name.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor
                            SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabScorePairNo) VALUES ({sectionID}, {table}, '{dir}', '{playerNumber}', '{name}', {round}, False, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, {pairNo})";
                        }
                    }
                }
                else
                {
                    if (playerNumber == "Unknown" || playerNumber == "0" || playerNumber == "")
                    {
                        SQLString = $"UPDATE PlayerNumbers SET [Number]='0', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, TabScorePairNo={pairNo} WHERE Section={sectionID} AND [Table]={table} AND Round={round} AND Direction='{dir}'";
                    }
                    else
                    {
                        string name = Player.GetNameFromNumber(DB, playerNumber);
                        if (name == "Error") return "Error";
                        if (name.Substring(0, 1) == "#")    // PlayerNames table doesn't exist, so let scoring software set the name 
                        {
                            SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, TabScorePairNo={pairNo} WHERE Section={sectionID} AND [Table]={table} AND Round={round} AND Direction='{dir}'";
                        }
                        else
                        {
                            name = name.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor
                            SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{name}', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, TabScorePairNo={pairNo} WHERE Section={sectionID} AND [Table]={table} AND Round={round} AND Direction='{dir}'";
                        }
                    }
                }

                OdbcCommand cmd3 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd3.ExecuteNonQuery();
                    });
                }
                catch (OdbcException)
                {
                    return "Error";
                }
                finally
                {
                    cmd3.Dispose();
                }
            }
            return "";
        }

        public static string GetNameFromNumber(string DB, string playerNumber)
        {
            if (playerNumber == "0")
            {
                return "Unknown";
            }
            else if (playerNumber == "")
            {
                return "";
            }
            int nameSource = Settings.GetSetting<int>(DB, SettingName.NameSource);
            switch (nameSource)
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
                    if (name == "Error") return "Error";
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
                        return "Error";
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
            OdbcConnectionStringBuilder externalDB = new OdbcConnectionStringBuilder();
            externalDB.Driver = "Microsoft Access Driver (*.mdb)";
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

        public static string GetName(string DB, string sectionID, string table, string round, string pairNo, string direction, bool formatUnknown)
        {
            string number = "###";
            string name = "";
            string errorString = "";

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
                catch (OdbcException)
                {
                    return "Error";
                }
                finally
                {
                    cmd1.Dispose();
                }

                if (queryResult != null)
                {
                    // This duplicates the code in TabScoreStarter
                    OdbcDataReader reader2;
                    SQLString = "SELECT Section, [Table], Direction FROM PlayerNumbers";
                    OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
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
                                catch (OdbcException)
                                {
                                    errorString = "Error";
                                }
                                cmd3.Dispose();
                                if (errorString != "Error")
                                {
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
                                    catch (OdbcException)
                                    {
                                        errorString = "Error";
                                    }
                                    cmd4.Dispose();
                                }
                            }
                            reader2.Close();
                        });
                    }
                    catch (OdbcException)
                    {
                        errorString = "Error";
                    }
                    cmd2.Dispose();
                    if (errorString == "Error") return "Error";  // An error somewhere in all of this ...
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
                        int roundAsInt = Convert.ToInt32(round);
                        while (reader5.Read())
                        {
                            int readerRound = Convert.ToInt32(reader5.GetValue(2));
                            if (readerRound <= roundAsInt && readerRound > biggestRoundSoFar)
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
                catch (OdbcException)
                {
                    errorString = "Error";
                }
                cmd5.Dispose();
                reader5.Close();
                if (errorString == "Error") return "Error";

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
                    catch (OdbcException)
                    {
                        return "Error";
                    }
                    finally
                    {
                        reader6.Close();
                        cmd6.Dispose();
                    }
                }
                cmd1.Dispose();

                if (number == "###")  // Nothing found in either direction!!
                {
                    number = "";
                }
            }
            return FormatName(name, number, formatUnknown);
        }

        // Function to deal with different display format options for blank and unknown names
        private static string FormatName(string name, string number, bool formatUnknown)
        {
            if (formatUnknown)
            {
                if (name == "" || name == "Unknown")
                {
                    if (number == "" || number == "0")
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
            else
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
}