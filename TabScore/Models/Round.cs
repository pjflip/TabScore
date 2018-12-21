using System.Data.Odbc;

namespace TabScore.Models
{
    public class Round
    {
        public static RoundClass GetRoundInfo(string DB, string sectionID, string table, string round)
        {
            RoundClass rd = new RoundClass();
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={table} AND Round={round}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rd.PairNS = reader.GetInt32(0);
                    rd.PairEW = reader.GetInt32(1);
                    rd.LowBoard = reader.GetInt32(2);
                    rd.HighBoard = reader.GetInt32(3);
                }
                reader.Close();
                cmd.Dispose();
            }
            return rd;
        }

        public static int GetLastEnteredRound(string DB, string sectionID, string table)
        {
            int maxRound = 1;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT Round FROM ReceivedData WHERE Section={sectionID} AND [Table]={table}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int thisRound = reader.GetInt32(0);
                    if (thisRound > maxRound)
                    {
                        maxRound = thisRound;
                    }
                }
                reader.Close();
                cmd.Dispose();
            }
            return maxRound;
        }
    }
}