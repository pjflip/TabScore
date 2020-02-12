using System.Data.Odbc;

namespace TabScore.Models
{
    public class HandRecord
    {
        public int BoardNumber { get; private set; }
        public string Dealer { get; private set; }
        public string NorthSpades { get; private set; }
        public string NorthHearts {get; private set;}
        public string NorthDiamonds {get; private set;}
        public string NorthClubs {get; private set;}
        public string EastSpades {get; private set;}
        public string EastHearts {get; private set;}
        public string EastDiamonds {get; private set;}
        public string EastClubs {get; private set;}
        public string SouthSpades {get; private set;}
        public string SouthHearts {get; private set;}
        public string SouthDiamonds {get; private set;}
        public string SouthClubs {get; private set;}
        public string WestSpades {get; private set;}
        public string WestHearts {get; private set;}
        public string WestDiamonds {get; private set;}
        public string WestClubs {get; private set;}

        public string EvalNorthNT {get; private set;}
        public string EvalNorthSpades {get; private set;}
        public string EvalNorthHearts {get; private set;}
        public string EvalNorthDiamonds {get; private set;}
        public string EvalNorthClubs {get; private set;}
        public string EvalEastNT {get; private set;}
        public string EvalEastSpades {get; private set;}
        public string EvalEastHearts {get; private set;}
        public string EvalEastDiamonds {get; private set;}
        public string EvalEastClubs {get; private set;}
        public string EvalSouthNT {get; private set;}
        public string EvalSouthSpades {get; private set;}
        public string EvalSouthHearts {get; private set;}
        public string EvalSouthDiamonds {get; private set;}
        public string EvalSouthClubs {get; private set;}
        public string EvalWestSpades {get; private set;}
        public string EvalWestNT {get; private set;}
        public string EvalWestHearts {get; private set;}
        public string EvalWestDiamonds {get; private set;}
        public string EvalWestClubs {get; private set;}

        public string HCPNorth {get; private set;}
        public string HCPSouth {get; private set;}
        public string HCPEast {get; private set;}
        public string HCPWest {get; private set;}

        public HandRecord(int sectionID, int boardNumber)
        {
            BoardNumber = boardNumber;
            Dealer = UtilityFunctions.GetDealerForBoard(boardNumber);
            NorthSpades = "###";
            EvalNorthSpades = "###";

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                string SQLString = $"SELECT NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs FROM HandRecord WHERE Section={sectionID} AND Board={boardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                connection.Open();
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            NorthSpades = reader.GetString(0);
                            NorthHearts = reader.GetString(1);
                            NorthDiamonds = reader.GetString(2);
                            NorthClubs = reader.GetString(3);
                            EastSpades = reader.GetString(4);
                            EastHearts = reader.GetString(5);
                            EastDiamonds = reader.GetString(6);
                            EastClubs = reader.GetString(7);
                            SouthSpades = reader.GetString(8);
                            SouthHearts = reader.GetString(9);
                            SouthDiamonds = reader.GetString(10);
                            SouthClubs = reader.GetString(11);
                            WestSpades = reader.GetString(12);
                            WestHearts = reader.GetString(13);
                            WestDiamonds = reader.GetString(14);
                            WestClubs = reader.GetString(15);
                        }
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than HandRecord table does not exist
                    {
                        throw (e);
                    }
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }

                SQLString = $"SELECT NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, NorthNotrump, EastSpades, EastHearts, EastDiamonds, EastClubs, EastNotrump, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, SouthNotrump, WestSpades, WestHearts, WestDiamonds, WestClubs, WestNoTrump, NorthHcp, EastHcp, SouthHcp, WestHcp FROM HandEvaluation WHERE Section={sectionID} AND Board={boardNumber}";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader.GetInt16(0) > 6) EvalNorthSpades = (reader.GetInt16(0) - 6).ToString(); else EvalNorthSpades = "";
                            if (reader.GetInt16(1) > 6) EvalNorthHearts = (reader.GetInt16(1) - 6).ToString(); else EvalNorthHearts = "";
                            if (reader.GetInt16(2) > 6) EvalNorthDiamonds = (reader.GetInt16(2) - 6).ToString(); else EvalNorthDiamonds = "";
                            if (reader.GetInt16(3) > 6) EvalNorthClubs = (reader.GetInt16(3) - 6).ToString(); else EvalNorthClubs = "";
                            if (reader.GetInt16(4) > 6) EvalNorthNT = (reader.GetInt16(4) - 6).ToString(); else EvalNorthNT = "";
                            if (reader.GetInt16(5) > 6) EvalEastSpades = (reader.GetInt16(5) - 6).ToString(); else EvalEastSpades = "";
                            if (reader.GetInt16(6) > 6) EvalEastHearts = (reader.GetInt16(6) - 6).ToString(); else EvalEastHearts = "";
                            if (reader.GetInt16(7) > 6) EvalEastDiamonds = (reader.GetInt16(7) - 6).ToString(); else EvalEastDiamonds = "";
                            if (reader.GetInt16(8) > 6) EvalEastClubs = (reader.GetInt16(8) - 6).ToString(); else EvalEastClubs = "";
                            if (reader.GetInt16(9) > 6) EvalEastNT = (reader.GetInt16(9) - 6).ToString(); else EvalEastNT = "";
                            if (reader.GetInt16(10) > 6) EvalSouthSpades = (reader.GetInt16(10) - 6).ToString(); else EvalSouthSpades = "";
                            if (reader.GetInt16(11) > 6) EvalSouthHearts = (reader.GetInt16(11) - 6).ToString(); else EvalSouthHearts = "";
                            if (reader.GetInt16(12) > 6) EvalSouthDiamonds = (reader.GetInt16(12) - 6).ToString(); else EvalSouthDiamonds = "";
                            if (reader.GetInt16(13) > 6) EvalSouthClubs = (reader.GetInt16(13) - 6).ToString(); else EvalSouthClubs = "";
                            if (reader.GetInt16(14) > 6) EvalSouthNT = (reader.GetInt16(14) - 6).ToString(); else EvalSouthNT = "";
                            if (reader.GetInt16(15) > 6) EvalWestSpades = (reader.GetInt16(15) - 6).ToString(); else EvalWestSpades = "";
                            if (reader.GetInt16(16) > 6) EvalWestHearts = (reader.GetInt16(16) - 6).ToString(); else EvalWestHearts = "";
                            if (reader.GetInt16(17) > 6) EvalWestDiamonds = (reader.GetInt16(17) - 6).ToString(); else EvalWestDiamonds = "";
                            if (reader.GetInt16(18) > 6) EvalWestClubs = (reader.GetInt16(18) - 6).ToString(); else EvalWestClubs = "";
                            if (reader.GetInt16(19) > 6) EvalWestNT = (reader.GetInt16(19) - 6).ToString(); else EvalWestNT = "";

                            HCPNorth = reader.GetInt16(20).ToString();
                            HCPEast = reader.GetInt16(21).ToString();
                            HCPSouth = reader.GetInt16(22).ToString();
                            HCPWest = reader.GetInt16(23).ToString();
                        }
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than HandEvaluation table does not exist
                    {
                        throw (e);
                    }
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
            return;
        }
    }
}
