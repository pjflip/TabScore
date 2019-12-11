using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Round
    {
        private readonly string DB;
        private readonly int SectionID;
        private readonly bool Individual;
        
        public int RoundNumber { get; private set; }
        public int PairNS { get; private set; }   // Doubles as North player number for individuals
        public int PairEW { get; private set; }   // Doubles as East player number for individuals
        public int LowBoard { get; private set; }
        public int HighBoard { get; private set; }
        public int South { get; private set; }
        public int West { get; private set; }

        public Round(string dB, int sectionID, int tableNumber, int roundNumber, bool individual)
        {
            DB = dB;
            SectionID = sectionID;
            RoundNumber = roundNumber;
            Individual = individual;

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                PairNS = 0;
                connection.Open();
                if (individual)
                {
                    string SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
                                South = reader.GetInt32(2);
                                West = reader.GetInt32(3);
                                LowBoard = reader.GetInt32(4);
                                HighBoard = reader.GetInt32(5);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
                                LowBoard = reader.GetInt32(2);
                                HighBoard = reader.GetInt32(3);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
            }
            return;
        }

        public string NameNorth
        {
            get
            {
                if (Individual)
                {
                    return GetNameIndividual(DB, SectionID, RoundNumber, PairNS);
                }
                else
                {
                    return GetName(DB, SectionID, RoundNumber, PairNS, "N");
                }
            }
        }
        public string NameSouth
        {
            get
            {
                if (Individual)
                {
                    return GetNameIndividual(DB, SectionID, RoundNumber, South);
                }
                else
                {
                    return GetName(DB, SectionID, RoundNumber, PairNS, "S");
                }
            }
        }
        public string NameEast
        {
            get
            {
                if (Individual)
                {
                    return GetNameIndividual(DB, SectionID, RoundNumber, PairEW);
                }
                else
                {
                    return GetName(DB, SectionID, RoundNumber, PairEW, "E");
                }
            }
        }
        public string NameWest
        {
            get
            {
                if (Individual)
                {
                    return GetNameIndividual(DB, SectionID, RoundNumber, West);
                }
                else
                {
                    return GetName(DB, SectionID, RoundNumber, PairEW, "W");
                }
            }
        }

        private static string GetName(string DB, int sectionID, int round, int pairNo, string direction)
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

        private static string GetNameIndividual(string DB, int sectionID, int round, int playerNo)
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
