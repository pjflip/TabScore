using System.Data.Odbc;
using System.Text;

namespace TabScore.Models
{
    public static class Lead
    {
        public static bool Validate(string DB, int sectionID, int board, string card, string NSEW)
        {
            if (card == "SKIP") return true;
            if (card.Substring(1,1) == "1")    // Account for different representations of '10'
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