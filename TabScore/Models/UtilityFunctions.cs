using System;
using System.Data.Odbc;
using System.Text;

namespace TabScore.Models
{
    public static class UtilityFunctions
    {
        // Find out how many rounds there are in the event
        public static int NumberOfRoundsInEvent(int sectionID)
        {
            object queryResult = null;
            using(OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = SQLString = $"SELECT MAX(Round) FROM RoundData WHERE Section={sectionID}";
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
            }
            return Convert.ToInt32(queryResult);
        }

        // Get the last round that has any results entered for it
        public static int GetLastRoundWithResults(int sectionID, int table)
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT MAX(Round) FROM ReceivedData WHERE Section={sectionID} AND [Table]={table}";
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
            }
            if (queryResult == DBNull.Value || queryResult == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(queryResult);
            }
        }

        // Get the dealer based on board number for standard boards
        public static string GetDealerForBoard(int boardNumber)
        {
            switch ((boardNumber - 1) % 4)
            {
                case 0:
                    return "N";
                case 1:
                    return "E";
                case 2:
                    return "S";
                case 3:
                    return "W";
                default:
                    return "#";
            }
        }

        // Used for setting vulnerability by board number
        public static readonly bool[] NSVulnerability = { false, true, false, true, true, false, true, false, false, true, false, true, true, false, true, false };
        public static readonly bool[] EWVulnerability = { false, false, true, true, false, true, true, false, true, true, false, false, true, false, false, true };

        public static string ColourPairByVulnerability(string dir, int boardNo, string pair)
        {
            string PairString;
            if (dir == "NS")
            {
                if (NSVulnerability[(boardNo - 1) % 16])
                {
                    PairString = $"<a style=\"color:red\">{pair}</a>";
                }
                else
                {
                    PairString = $"<a style=\"color:green\">{pair}</a>";
                }
            }
            else
            {
                if (EWVulnerability[(boardNo - 1) % 16])
                {
                    PairString = $"<a style=\"color:red\">{pair}</a>";
                }
                else
                {
                    PairString = $"<a style=\"color:green\">{pair}</a>";
                }
            }
            return PairString;
        }

        // Validate the lead card against the hand record
        public static bool ValidateLead(string DB, int sectionID, int board, string card, string NSEW)
        {
            if (card == "SKIP") return true;
            if (card.Substring(1, 1) == "1")    // Account for different representations of '10'
            {
                card = card.Substring(0, 1) + "T";
            }
            StringBuilder SQLString = new StringBuilder();
            SQLString.Append("SELECT ");
            switch (NSEW)
            {
                case "N":
                    SQLString.Append("East");
                    break;
                case "S":
                    SQLString.Append("West");
                    break;
                case "E":
                    SQLString.Append("South");
                    break;
                case "W":
                    SQLString.Append("North");
                    break;
            }
            switch (card.Substring(0, 1))
            {
                case "S":
                    SQLString.Append("Spades");
                    break;
                case "H":
                    SQLString.Append("Hearts");
                    break;
                case "D":
                    SQLString.Append("Diamonds");
                    break;
                case "C":
                    SQLString.Append("Clubs");
                    break;
            }
            SQLString.Append($" FROM HandRecord WHERE Section={sectionID} AND Board={board}");

            bool validateOk = true;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                OdbcCommand cmd = new OdbcCommand(SQLString.ToString(), connection);
                connection.Open();
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        object queryResult = cmd.ExecuteScalar();
                        if (queryResult == null)
                        {
                            validateOk = true;
                        }
                        else
                        {
                            string suitString = queryResult.ToString();
                            if (suitString.Contains(card.Substring(1, 1)))
                            {
                                validateOk = true;
                            }
                            else
                            {
                                validateOk = false;
                            }
                        }
                    });
                }
                catch (OdbcException)  // An error occurred, even after retries, so no validation possible 
                {
                    return true;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return validateOk;
        }
    }
}