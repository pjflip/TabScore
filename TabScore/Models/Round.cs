using System.Data.Odbc;

namespace TabScore.Models
{
    public class Round
    {
        public int Table;
        public int RoundNumber;
        public int PairNS;   // Doubles as North player number for individuals
        public int PairEW;   // Doubles as East player number for individuals
        public int LowBoard;
        public int HighBoard;
        public int South;
        public int West;

        public Round()   // Default constructor
        {
        }

        public Round(string DB, int sectionID, int table, int round, bool individual)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                PairNS = 0;
                connection.Open();
                if (individual)
                {
                    string SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={table} AND Round={round}";
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
                    catch (OdbcException)
                    {
                        PairNS = -1;
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={table} AND Round={round}";
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
                    catch (OdbcException)
                    {
                        PairNS = -1;
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
    }
}