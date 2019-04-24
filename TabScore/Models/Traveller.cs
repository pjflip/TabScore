using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Traveller
    {
        public static List<TravellerResultClass> GetResults(string DB, string sectionID, string board)
        {
            List<TravellerResultClass> trList = new List<TravellerResultClass>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={board}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TravellerResultClass tr = new TravellerResultClass()
                    {
                        PairNS = reader.GetInt32(0).ToString(),
                        PairEW = reader.GetInt32(1).ToString(),
                        NSEW = reader.GetString(2),
                        Contract = reader.GetString(3),
                        LeadCard = reader.GetString(4),
                        TricksTakenSymbol = reader.GetString(5)
                    };
                    if (tr.Contract.Length > 2)  // Testing for corrupt ReceivedData table
                    {
                        trList.Add(tr);
                    }
                }
                reader.Close();
                cmd.Dispose();
            };

            foreach(TravellerResultClass tr in trList)
            {
                // Use ResultClass Score method to calculate score
                ResultClass res = new ResultClass()
                {
                    SectionID = sectionID,
                    Board = board
                };
                if (tr.Contract == "PASS")
                {
                    res.ContractLevel = "PASS";
                    res.NSEW = "";
                    res.TricksTakenSymbol = "";
                }
                else
                {
                    string[] temp = tr.Contract.Split(' ');
                    res.ContractLevel = temp[0];
                    res.ContractSuit = temp[1];
                    if (temp.Length > 2) res.ContractX = temp[2];
                    else res.ContractX = "NONE";
                    res.NSEW = tr.NSEW;
                    res.TricksTakenSymbol = tr.TricksTakenSymbol;
                }
                tr.Score = res.Score();
                tr.ScoreNS = "";
                tr.ScoreEW = "";
                if (tr.Score > 0)
                {
                    tr.ScoreNS = tr.Score.ToString();
                }
                else if (tr.Score < 0)
                {
                    tr.ScoreEW = Convert.ToString(-tr.Score);
                }
            }
            return trList;
        }
    }
}