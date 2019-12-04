using System.Data.Odbc;
using System.Text;

namespace TabScoreStarter
{
    class Options
    {
        public bool showTraveller {get; set;}
        public bool showPercentage {get; set;}
        public bool enterLeadCard {get; set;}
        public bool validateLeadCard {get; set;}
        public int showRanking {get; set;}
        public bool showHandRecord {get; set;}
        public bool numberEntryEachRound {get; set;}
        public int nameSource {get; set;}
        public int enterResultsMethod {get; set;}

        private string dbConnectionString;

        public Options(Database db)
        {
            dbConnectionString = db.ConnectionString;
            using (OdbcConnection connection = new OdbcConnection(dbConnectionString))
            {
                string SQLString = $"SELECT ShowResults, ShowPercentage, LeadCard, BM2ValidateLeadCard, BM2Ranking, BM2ViewHandRecord, BM2NumberEntryEachRound, BM2NameSource, EnterResultsMethod FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                reader.Read();
                showTraveller = reader.GetBoolean(0);
                showPercentage = reader.GetBoolean(1);
                enterLeadCard = reader.GetBoolean(2);
                validateLeadCard = reader.GetBoolean(3);
                showRanking = reader.GetInt32(4);
                showHandRecord = reader.GetBoolean(5);
                numberEntryEachRound = reader.GetBoolean(6);
                nameSource = reader.GetInt32(7);
                enterResultsMethod = reader.GetInt32(8);
                if (enterResultsMethod != 1) enterResultsMethod = 0;
                reader.Close();
                cmd.Dispose();
            }
        }

        public void UpdateDB()
        {
            using (OdbcConnection connection = new OdbcConnection(dbConnectionString))
            {
                StringBuilder SQLString = new StringBuilder();
                SQLString.Append($"UPDATE Settings SET");
                if (showTraveller)
                {
                    SQLString.Append(" ShowResults=YES,");
                }
                else
                {
                    SQLString.Append(" ShowResults=NO,");
                }
                if (showPercentage)
                {
                    SQLString.Append(" ShowPercentage=YES,");
                }
                else
                {
                    SQLString.Append(" ShowPercentage=NO,");
                }
                if (enterLeadCard)
                {
                    SQLString.Append(" LeadCard=YES,");
                }
                else
                {
                    SQLString.Append(" LeadCard=NO,");
                }
                if (validateLeadCard)
                {
                    SQLString.Append(" BM2ValidateLeadCard=YES,");
                }
                else
                {
                    SQLString.Append(" BM2ValidateLeadCard=NO,");
                }
                SQLString.Append($" BM2Ranking={showRanking.ToString()},");
                if (showHandRecord)
                {
                    SQLString.Append(" BM2ViewHandRecord=YES,");
                }
                else
                {
                    SQLString.Append(" BM2ViewHandRecord=NO,");
                }
                if (numberEntryEachRound)
                {
                    SQLString.Append(" BM2NumberEntryEachRound=YES,");
                }
                else
                {
                    SQLString.Append(" BM2NumberEntryEachRound=NO,");
                }
                SQLString.Append($" BM2NameSource={nameSource.ToString()},");
                SQLString.Append($" EnterResultsMethod={enterResultsMethod.ToString()}");
                OdbcCommand cmd = new OdbcCommand(SQLString.ToString(), connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}

