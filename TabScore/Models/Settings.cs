using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class Settings
    {
        public static bool ShowResults(string DB)
        {
            bool showResults = true;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT ShowResults FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        showResults = Convert.ToBoolean(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return showResults;
        }

        public static bool ShowPercentage(string DB)
        {
            bool showPercentage = true;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT ShowPercentage FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        showPercentage = Convert.ToBoolean(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return showPercentage;
        }

        public static bool EnterLeadCard(string DB)
        {
            bool enterLeadCard = true;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT LeadCard FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        enterLeadCard = Convert.ToBoolean(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return enterLeadCard;
        }

        public static bool ValidateLeadCard(string DB)
        {
            bool validateLeadCard = true;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT BM2ValidateLeadCard FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        validateLeadCard = Convert.ToBoolean(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return validateLeadCard;
        }

        public static int ShowRanking(string DB)
        {
            int showRanking = 1;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT BM2Ranking FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        showRanking = Convert.ToInt32(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return showRanking;
        }

        public static bool ShowHandRecord(string DB)
        {
            bool showHandRecord = true;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT BM2ViewHandRecord FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        showHandRecord = Convert.ToBoolean(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return showHandRecord;
        }

        public static bool NumberEntryEachRound(string DB)
        {
            bool numberEntryEachRound = false;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT BM2NumberEntryEachRound FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        numberEntryEachRound = Convert.ToBoolean(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return numberEntryEachRound;
        }

        public static int NameSource(string DB)
        {
            int nameSource = 0;
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT BM2NameSource FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult != null)
                    {
                        nameSource = Convert.ToInt32(queryResult);
                    }
                }
                catch
                {
                }
                cmd.Dispose();
            }
            return nameSource;
        }
    }
}

