using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    // Settings is a global class that applies accross all sessions
    public static class Settings
    {
        public static bool ShowResults { get; private set; }
        public static bool ShowPercentage { get; private set; }
        public static bool EnterLeadCard { get; private set; }
        public static bool ValidateLeadCard { get; private set; }
        public static int ShowRanking { get; private set; }
        public static int EnterResultsMethod { get; private set; }
        public static bool ShowHandRecord { get; private set; }
        public static bool NumberEntryEachRound { get; private set; }
        public static int NameSource { get; private set; }

        private static DateTime UpdateTime;

        public static void Refresh()
        {
            if (DateTime.Now.Subtract(UpdateTime).TotalMinutes < 1.0) return;  // Settings updated recently, so don't bother
            UpdateTime = DateTime.Now;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
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