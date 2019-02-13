using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class RankingList
    {
        public static List<RankingListClass> GetRankingList(string DB, string sectionID)
        {
            List<RankingListClass> rList = new List<RankingListClass>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT Orientation, Number, Score, Rank FROM Results WHERE Section={sectionID}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        RankingListClass rl = new RankingListClass();
                        rl.Orientation = reader.GetString(0);
                        rl.PairNo = reader.GetInt32(1);
                        rl.Score = reader.GetString(2);
                        rl.Rank = reader.GetString(3);

                        rList.Add(rl);
                    }
                    reader.Close();
                    cmd.Dispose();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S02")  // Results table does not exist
                    {
                        cmd.Dispose();
                        return null;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            return rList;
        }
    }






}
