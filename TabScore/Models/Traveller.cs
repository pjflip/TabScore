using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Traveller
    {
        public static List<Result> GetResults(string DB, int sectionID, int board, bool individual)
        {
            List<Result> resList = new List<Result>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString;
                OdbcCommand cmd = null;
                OdbcDataReader reader = null;
                try
                {
                    if (individual)
                    {
                        SQLString = $"SELECT PairNS, PairEW, South, West, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={board}";
                    }
                    else
                    {
                        SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={board}";
                    }
                    cmd = new OdbcCommand(SQLString, connection);
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Result res = null;
                            if (individual)
                            {
                                res = new Result()
                                {
                                    Board = board,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    South = reader.GetInt32(2),
                                    West = reader.GetInt32(3),
                                    NSEW = reader.GetString(4),
                                    Contract = reader.GetString(5),
                                    LeadCard = reader.GetString(6),
                                    TricksTakenSymbol = reader.GetString(7)
                                };
                            }
                            else
                            {
                                res = new Result()
                                {
                                    Board = board,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    NSEW = reader.GetString(2),
                                    Contract = reader.GetString(3),
                                    LeadCard = reader.GetString(4),
                                    TricksTakenSymbol = reader.GetString(5)
                                };
                            }
                            if (res.Contract.Length > 2)  // Testing for corrupt ReceivedData table
                            {
                                res.CalculateScore();
                                resList.Add(res);
                            }
                        }
                    });
                }
                catch (OdbcException)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            };

            return resList;
        }
    }
}