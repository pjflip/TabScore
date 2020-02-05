using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Player
    {
        private int PlayerNumber;
        private string PlayerName;
        private int PairNumber;
        private readonly string DB;
        private readonly int SectionID;
        private readonly int TableNumber;
        private readonly int RoundNumber;
        private readonly bool Individual;
        private readonly string Dir;

        public Player(string DB, int sectionID, int tableNumber, Round round, string direction, int playerNumber, bool individual, int nameSource)
        {
            this.DB = DB;
            SectionID = sectionID;
            TableNumber = tableNumber;
            Individual = individual;

            RoundNumber = round.RoundNumber;
            // Numbers entered at the start (when round = 1) need to be set as round 0
            if (RoundNumber == 1)
            {
                RoundNumber = 0;
            }

            Dir = direction.Substring(0, 1);    // Need just N, S, E or W
            if (Individual)
            {
                switch (Dir)
                {
                    case "N":
                        PairNumber = round.PairNS;
                        break;
                    case "S":
                        PairNumber = round.South;
                        break;
                    case "E":
                        PairNumber = round.PairEW;
                        break;
                    case "W":
                        PairNumber = round.West;
                        break;
                }
            }
            else
            {
                switch (Dir)
                {
                    case "N":
                    case "S":
                        PairNumber = round.PairNS;
                        break;
                    case "E":
                    case "W":
                        PairNumber = round.PairEW;
                        break;
                }
            }

            PlayerNumber = playerNumber;
            // Set the name for this number
            if (playerNumber == 0)
            {
                PlayerName = "";
            }
            else
            {
                switch (nameSource)
                {
                    case 0:
                        PlayerName = GetNameFromPlayerNamesTable(DB, playerNumber);
                        break;
                    case 1:
                        PlayerName = GetNameFromExternalDatabase(playerNumber);
                        break;
                    case 2:
                        PlayerName = "";
                        break;
                    case 3:
                        string name;
                        name = GetNameFromPlayerNamesTable(DB, playerNumber);
                        if (name == "" || name.Substring(0, 1) == "#" || (name.Length >= 7 && name.Substring(0, 7) == "Unknown"))
                        {
                            PlayerName = GetNameFromExternalDatabase(playerNumber);
                        }
                        else
                        {
                            PlayerName = name;
                        }
                        break;
                    default:
                        PlayerName = "";
                        break;
                }
                if (PlayerName.Substring(0, 1) == "#")    // PlayerNames table doesn't exist, so let scoring software set the name 
                {
                    PlayerName = "";
                }
                else
                {
                    PlayerName = PlayerName.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor
                }
            }
        }

        public void UpdateDatabase()
        {
            string SQLString = null;
            object queryResult = null;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();

                // Check if PlayerNumbers entry exists already; if it does update it, if not create it
                SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={SectionID} AND [Table]={TableNumber} AND ROUND={RoundNumber} AND Direction='{Dir}'";
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
                if (queryResult == null)
                {
                    SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabScorePairNo) VALUES ({SectionID}, {TableNumber}, '{Dir}', '{PlayerNumber}', '{PlayerName}', {RoundNumber}, False, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, {PairNumber})";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{PlayerNumber}', [Name]='{PlayerName}', Processed=False, TimeLog=#{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, TabScorePairNo={PairNumber} WHERE Section={SectionID} AND [Table]={TableNumber} AND Round={RoundNumber} AND Direction='{Dir}'";
                }
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd2.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd2.Dispose();
                }
            }
        }

        private static string GetNameFromPlayerNamesTable(string DB, int playerNumber)
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