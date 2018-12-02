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
                string SQLString = $"SELECT ShowResults, ShowPercentage, LeadCard FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    OdbcDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            sc.ShowResults = reader.GetBoolean(0);
                        }
                        if (!reader.IsDBNull(1))
                        {
                            sc.ShowPercentage = reader.GetBoolean(1);
                        }
                        if (!reader.IsDBNull(2))
                        {
                            sc.EnterLeadCard = reader.GetBoolean(2);
                        }
                    }
                    reader.Close();
                }
                catch
                {
                }

                SQLString = $"SELECT BM2ValidateLeadCard, BM2Ranking, BM2ViewHandRecord FROM Settings";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    OdbcDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            sc.ValidateLeadCard = reader.GetBoolean(0);
                        }
                        if (!reader.IsDBNull(1))
                        {
                            sc.ShowRanking = reader.GetInt16(1);
                        }
                        if (!reader.IsDBNull(2))
                        {
                            sc.ShowHandRecord = reader.GetBoolean(2);
                        }
                    }
                    reader.Close();
                }
                catch
                {
                }
                cmd.Dispose();
                return sc;
            }
        }
    }
}