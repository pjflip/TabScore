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
                    return PlayerFunctions.GetNameIndividual(DB, SectionID, RoundNumber, PairNS);
                }
                else
                {
                    return PlayerFunctions.GetName(DB, SectionID, RoundNumber, PairNS, "N");
                }
            }
        }
        public string NameSouth
        {
            get
            {
                if (Individual)
                {
                    return PlayerFunctions.GetNameIndividual(DB, SectionID, RoundNumber, South);
                }
                else
                {
                    return PlayerFunctions.GetName(DB, SectionID, RoundNumber, PairNS, "S");
                }
            }
        }
        public string NameEast
        {
            get
            {
                if (Individual)
                {
                    return PlayerFunctions.GetNameIndividual(DB, SectionID, RoundNumber, PairEW);
                }
                else
                {
                    return PlayerFunctions.GetName(DB, SectionID, RoundNumber, PairEW, "E");
                }
            }
        }
        public string NameWest
        {
            get
            {
                if (Individual)
                {
                    return PlayerFunctions.GetNameIndividual(DB, SectionID, RoundNumber, West);
                }
                else
                {
                    return PlayerFunctions.GetName(DB, SectionID, RoundNumber, PairEW, "W");
                }
            }
        }
    }
}