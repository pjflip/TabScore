using System.Data.Odbc;

namespace TabScore.Models
{
    public class Settings
    {
        public bool ShowResults { get; private set; }
        public bool ShowPercentage { get; private set; }
        public bool EnterLeadCard { get; private set; }
        public bool ValidateLeadCard { get; private set; }
        public int ShowRanking { get; private set; }
        public int EnterResultsMethod { get; private set; }
        public bool ShowHandRecord { get; private set; }
        public bool NumberEntryEachRound { get; private set; }
        public int NameSource { get; private set; }

        public Settings(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = "SELECT ShowResults, ShowPercentage, LeadCard, BM2ValidateLeadCard, BM2Ranking, EnterResultsMethod, BM2ViewHandRecord, BM2NumberEntryEachRound, BM2NameSource FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ShowResults = reader.GetBoolean(0);
                            ShowPercentage = reader.GetBoolean(1);
                            EnterLeadCard = reader.GetBoolean(2);
                            ValidateLeadCard = reader.GetBoolean(3);
                            ShowRanking = reader.GetInt32(4);
                            EnterResultsMethod = reader.GetInt32(5);
                            ShowHandRecord = reader.GetBoolean(6);
                            NumberEntryEachRound = reader.GetBoolean(7);
                            NameSource = reader.GetInt32(8);
                        }
                    });
                }
                catch
                {
                    ShowResults = true;
                    ShowPercentage = true;
                    EnterLeadCard = true;
                    ValidateLeadCard = true;
                    ShowRanking = 1;
                    EnterResultsMethod = 1;
                    ShowHandRecord = true;
                    NumberEntryEachRound = true;
                    NameSource = 0;
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
        }
    }
}