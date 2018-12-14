using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class PlayerNumber
    {
        public static void UpdateDB(string DB, string sectionID, string table, string direction, string playerNumber) 
        {
            string dir = direction.Substring(0, 1);    // Need just N, S, E or W
            string SQLString;
            if (playerNumber == "Unknown" || playerNumber == "0")
            {
                SQLString = $"UPDATE PlayerNumbers SET [Number]='0', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}# WHERE Section={sectionID} AND [Table]={table} AND Direction='{dir}'";
            }
            else
            {
                string name = PlayerNumber.GetNameFromPlayerNumber(DB, playerNumber);
                name = name.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor
                if (name.Substring(0,1) == "#")    // PlayerNames table doesn't exist, so let scoring software set the name 
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}# WHERE Section={sectionID} AND [Table]={table} AND Direction='{dir}'";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{name}', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}# WHERE Section={sectionID} AND [Table]={table} AND Direction='{dir}'";
                }
            }

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }

        public static string GetNameFromPlayerNumber(string DB, string playerNumber)
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
    }
}