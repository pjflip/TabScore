using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Player
    {
        public static void UpdateDatabase(string DB, string sectionID, string table, string round, string direction, string playerNumber) 
        {
            string dir = direction.Substring(0, 1);    // Need just N, S, E or W
            string SQLString;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                // Get pair number for this player to set TabScorePairNo field
                if (dir == "N" || dir == "S")
                {
                    SQLString = $"SELECT NSPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                }
                else
                {
                    SQLString = $"SELECT EWPair FROM RoundData WHERE Section={sectionID} AND [Table]={table} AND ROUND={round}";
                }
                connection.Open();
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                object queryResult = cmd.ExecuteScalar();
                string pairNo = queryResult.ToString();

                // Numbers entered at the start (when round = 1) need to be set as round 0
                if (round == "1")
                {
                    round = "0";
                }

                // Check if PlayerNumbers record exists; if it does update it, if not create it
                SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={sectionID} AND [Table]={table} AND ROUND={round} AND Direction='{dir}'";
                cmd = new OdbcCommand(SQLString, connection);
                queryResult = cmd.ExecuteScalar();
                if (queryResult == null)
                {
                    if (playerNumber == "Unknown" || playerNumber == "0" || playerNumber == "")
                    {
                        SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Round, Processed, TimeLog, TabScorePairNo) VALUES ({sectionID}, {table}, '{dir}', '0', {round}, False, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, {pairNo})";
                    }
                    else
                    {
                        string name = Player.GetNameFromNumber(DB, playerNumber);
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
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
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
            string name;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT Name FROM PlayerNames WHERE ID={playerNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult == null)
                    {
                        name = "Unknown #" + playerNumber;
                    }
                    else
                    {
                        name = queryResult.ToString();
                    }
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S02")  // PlayerNames table does not exist
                    {
                        name = "#" + playerNumber;
                    }
                    else
                    {
                        throw e;
                    }
                }
                cmd.Dispose();
            }
            return name;
        }

        public static string GetName(string DB, string sectionID, string table, string round, string pairNo, string direction, bool formatUnknown)
        {
            string number = "";
            string name = "";
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                // First look for entries in the same direction
                string SQLString = $"SELECT Number, Name, Round FROM PlayerNumbers WHERE Section={sectionID} AND TabScorePairNo={pairNo} AND Direction='{direction}'";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = cmd.ExecuteReader();
                int biggestRoundSoFar = -1;
                int roundAsInt = Convert.ToInt32(round);
                while (reader.Read())
                {
                    int readerRound = Convert.ToInt32(reader.GetValue(2));
                    if (readerRound <= roundAsInt && readerRound > biggestRoundSoFar)
                    {
                        number = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                        {
                            name = reader.GetString(1);
                        }
                        biggestRoundSoFar = readerRound;
                    }
                }
                if (number == "")  // Nothing found so try Round 0 entries in the other direction (for Howell type pairs movement)
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
                    cmd = new OdbcCommand(SQLString, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        number = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                        {
                            name = reader.GetString(1);
                        }
                    }
                }
                reader.Close();
                cmd.Dispose();
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