using System.Data.Odbc;

namespace TabScore.Models
{
    public static class PairNames
    {
        public static NamesClass GetNamesForPairNumber(string DB, string SectionID, string PairNo, string Direction)
        {
            string SQLString;
            object queryResult;
            string Table;
            string StartDirection;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                // First get table at which this pair started
                connection.Open();
                if (Direction == "NS")
                {
                    SQLString = $"SELECT Table FROM RoundData WHERE Section={SectionID} AND NSPair={PairNo} AND Round=1";
                }
                else
                {
                    SQLString = $"SELECT Table FROM RoundData WHERE Section={SectionID} AND EWPair={PairNo} AND Round=1";
                }
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                queryResult = cmd.ExecuteScalar();
                if (queryResult != null)
                {
                    Table = queryResult.ToString();
                    StartDirection = Direction;
                }
                else
                {
                    if (Direction == "NS")
                    {
                        SQLString = $"SELECT Table FROM RoundData WHERE Section={SectionID} AND EWPair={PairNo} AND Round=1";
                    }
                    else
                    {
                        SQLString = $"SELECT Table FROM RoundData WHERE Section={SectionID} AND NSPair={PairNo} AND Round=1";
                    }
                    cmd = new OdbcCommand(SQLString, connection);
                    queryResult = cmd.ExecuteScalar();
                    Table = queryResult.ToString();
                    if (Direction == "NS")
                    {
                        StartDirection = "EW";
                    }
                    else
                    {
                        StartDirection = "NS";
                    }
                }
                cmd.Dispose();
            }
            // Now get names from that starting table
            return GetNamesForStartTableNumber(DB, SectionID, Table, StartDirection);
        }

        public static NamesClass GetNamesForStartTableNumber(string DB, string SectionID, string Table, string StartDirection)
        {
            NamesClass names = new NamesClass();

            string SQLString;
            object queryResult;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                // Get North or East name
                if (StartDirection == "NS")
                {
                    SQLString = $"SELECT Name FROM PlayerNumbers WHERE Section={SectionID} AND Direction='N' AND Table={Table}";
                }
                else
                {
                    SQLString = $"SELECT Name FROM PlayerNumbers WHERE Section={SectionID} AND Direction='E' AND Table={Table}";
                }
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                queryResult = cmd.ExecuteScalar();
                if (queryResult == null)
                {
                    names.NameNE = "";
                }
                else
                {
                    names.NameNE = queryResult.ToString();
                }
                if (names.NameNE == "")  // Blank name, so try using Number instead
                {
                    if (StartDirection == "NS")
                    {
                        SQLString = $"SELECT Number FROM PlayerNumbers WHERE Section={SectionID} AND Direction='N' AND Table={Table}";
                    }
                    else
                    {
                        SQLString = $"SELECT Number FROM PlayerNumbers WHERE Section={SectionID} AND Direction='E' AND Table={Table}";
                    }
                    cmd = new OdbcCommand(SQLString, connection);
                    queryResult = cmd.ExecuteScalar();
                    if (queryResult == null)
                    {
                        names.NameNE = "";
                    }
                    else
                    {
                        names.NameNE = PlayerNumber.GetNameFromPlayerNumber(DB, queryResult.ToString());
                    }
                }

                // Get South or West name
                if (StartDirection == "NS")
                {
                    SQLString = $"SELECT Name FROM PlayerNumbers WHERE Section={SectionID} AND Direction='S' AND Table={Table}";
                }
                else
                {
                    SQLString = $"SELECT Name FROM PlayerNumbers WHERE Section={SectionID} AND Direction='W' AND Table={Table}";
                }
                cmd = new OdbcCommand(SQLString, connection);
                queryResult = cmd.ExecuteScalar();
                if (queryResult == null)
                {
                    names.NameSW = "";
                }
                else
                {
                    names.NameSW = queryResult.ToString();
                }
                if (names.NameSW == "")  // Blank name, so try using Number instead
                {
                    if (StartDirection == "NS")
                    {
                        SQLString = $"SELECT Number FROM PlayerNumbers WHERE Section={SectionID} AND Direction='S' AND Table={Table}";
                    }
                    else
                    {
                        SQLString = $"SELECT Number FROM PlayerNumbers WHERE Section={SectionID} AND Direction='W' AND Table={Table}";
                    }
                    cmd = new OdbcCommand(SQLString, connection);
                    queryResult = cmd.ExecuteScalar();
                    if (queryResult == null)
                    {
                        names.NameSW = "";
                    }
                    else
                    {
                        names.NameSW = PlayerNumber.GetNameFromPlayerNumber(DB, queryResult.ToString());
                    }
                }
                cmd.Dispose();
            }
            return names;
        }
    }
}