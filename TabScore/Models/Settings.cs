using System.Data.Odbc;

namespace TabScore.Models
{
    public class Settings
    {
        public static SettingsClass GetSettings(string DB)
        {
            SettingsClass sc = new SettingsClass();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT ShowResults, ShowPercentage, LeadCard, BM2ValidateLeadCard, BM2Ranking, BM2ViewHandRecord FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        sc.ShowResults = reader.GetBoolean(0);
                    }
                    else
                    {
                        sc.ShowResults = true;
                    }
                    if (!reader.IsDBNull(1))
                    {
                        sc.ShowPercentage = reader.GetBoolean(1);
                    }
                    else
                    {
                        sc.ShowPercentage = true;
                    }
                    if (!reader.IsDBNull(2))
                    {
                        sc.EnterLeadCard = reader.GetBoolean(2);
                    }
                    else
                    {
                        sc.EnterLeadCard = true;
                    }
                    if (!reader.IsDBNull(3))
                    {
                        sc.ValidateLeadCard = reader.GetBoolean(3);
                    }
                    else
                    {
                        sc.ValidateLeadCard = true;
                    }
                    if (!reader.IsDBNull(4))
                    {
                        sc.ShowRanking = reader.GetInt16(4);
                    }
                    else
                    {
                        sc.ShowRanking = 1;
                    }
                    if (!reader.IsDBNull(5))
                    {
                        sc.ShowHandRecord = reader.GetBoolean(5);
                    }
                    else
                    {
                        sc.ShowHandRecord = true;
                    }
                }
                reader.Close();
                cmd.Dispose();
                return sc;
            }
        }
    }
}