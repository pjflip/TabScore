using System.Data.Odbc;
using System.Text;

namespace TabScore.Models
{
    public static class HandRecord
    {
        public static HandRecordClass GetHandRecord(string DB, string sectionID, string board)
        {
            HandRecordClass hr = new HandRecordClass()
            {
                NorthSpades = "###",
                EvalNorthSpades = -1
            };

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs FROM HandRecord WHERE Section={sectionID} AND Board={board}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    OdbcDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        hr.NorthSpades = reader.GetString(0);
                        hr.NorthHearts = reader.GetString(1);
                        hr.NorthDiamonds = reader.GetString(2);
                        hr.NorthClubs = reader.GetString(3);
                        hr.EastSpades = reader.GetString(4);
                        hr.EastHearts = reader.GetString(5);
                        hr.EastDiamonds = reader.GetString(6);
                        hr.EastClubs = reader.GetString(7);
                        hr.SouthSpades = reader.GetString(8);
                        hr.SouthHearts = reader.GetString(9);
                        hr.SouthDiamonds = reader.GetString(10);
                        hr.SouthClubs = reader.GetString(11);
                        hr.WestSpades = reader.GetString(12);
                        hr.WestHearts = reader.GetString(13);
                        hr.WestDiamonds = reader.GetString(14);
                        hr.WestClubs = reader.GetString(15);
                    }
                    reader.Close();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState != "42S02")  // HandRecord table does not exist
                    {
                        cmd.Dispose();
                        return hr;
                    }
                    else
                    {
                        throw e;
                    }
                }

                SQLString = $"SELECT NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, NorthNotrump, EastSpades, EastHearts, EastDiamonds, EastClubs, EastNotrump, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, SouthNotrump, WestSpades, WestHearts, WestDiamonds, WestClubs, WestNoTrump, NorthHcp, EastHcp, SouthHcp, WestHcp FROM HandEvaluation WHERE Section={sectionID} AND Board={board}";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    OdbcDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        hr.EvalNorthSpades = reader.GetInt16(0);
                        hr.EvalNorthHearts = reader.GetInt16(1);
                        hr.EvalNorthDiamonds = reader.GetInt16(2);
                        hr.EvalNorthClubs = reader.GetInt16(3);
                        hr.EvalNorthNT = reader.GetInt16(4);
                        hr.EvalEastSpades = reader.GetInt16(5);
                        hr.EvalEastHearts = reader.GetInt16(6);
                        hr.EvalEastDiamonds = reader.GetInt16(7);
                        hr.EvalEastClubs = reader.GetInt16(8);
                        hr.EvalEastNT = reader.GetInt16(9);
                        hr.EvalSouthSpades = reader.GetInt16(10);
                        hr.EvalSouthHearts = reader.GetInt16(11);
                        hr.EvalSouthDiamonds = reader.GetInt16(12);
                        hr.EvalSouthClubs = reader.GetInt16(13);
                        hr.EvalSouthNT = reader.GetInt16(14);
                        hr.EvalWestSpades = reader.GetInt16(15);
                        hr.EvalWestHearts = reader.GetInt16(16);
                        hr.EvalWestDiamonds = reader.GetInt16(17);
                        hr.EvalWestClubs = reader.GetInt16(18);
                        hr.EvalWestNT = reader.GetInt16(19);

                        hr.HCPNorth = reader.GetInt16(20);
                        hr.HCPEast = reader.GetInt16(21);
                        hr.HCPSouth = reader.GetInt16(22);
                        hr.HCPWest = reader.GetInt16(23);
                    }
                    reader.Close();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than HandEvaluation table does not exist
                    {
                        throw e;
                    }
                }
                cmd.Dispose();
            }
            return hr;
        }

        public static bool ValidateLead(string DB, string sectionID, string board, string card, string NSEW)
        {
            if (card == "SKIP") return true;
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

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                OdbcCommand cmd = new OdbcCommand(SQLString.ToString(), connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult == null)
                    {
                        return true;
                    }
                    else
                    {
                        string suitString = queryResult.ToString();
                        if (suitString.Contains(card.Substring(1, 1)))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState != "42S02")  // HandRecord table does not exist so no validation possible
                    {
                        cmd.Dispose();
                        return true;  
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }
    }
}