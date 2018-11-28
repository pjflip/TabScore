using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Traveller
    {
        public static List<TravellerResultClass> GetResults(string DB, string sectionID, string board)
        {
            List<TravellerResultClass> rList = new List<TravellerResultClass>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={sectionID} AND Board={board}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ResultClass res = new ResultClass()
                    {
                        SectionID = sectionID,
                        Board = board
                    };
                    res.PairNS = reader.GetInt32(0).ToString();
                    res.PairEW = reader.GetInt32(1).ToString();
                    string contract = reader.GetString(3);
                    if (contract == "PASS")
                    {
                        res.ContractLevel = "PASS";
                        res.NSEW = "";
                        res.TricksTakenSymbol = "";
                    }
                    else
                    {
                        string[] temp = contract.Split(' ');
                        res.ContractLevel = temp[0];
                        res.ContractSuit = temp[1];
                        if (temp.Length > 2) res.ContractX = temp[2];
                        else res.ContractX = "NONE";
                        res.NSEW = reader.GetString(2);
                        res.LeadCard = reader.GetString(4);
                        res.TricksTakenSymbol = reader.GetString(5);
                    }

                    TravellerResultClass tr = new TravellerResultClass()
                    {
                        PairNS = res.PairNS,
                        PairEW = res.PairEW,
                        NSEW = res.NSEW,
                        Contract = res.DisplayContract(0),
                        TricksTakenSymbol = res.TricksTakenSymbol,
                        LeadCard = res.DisplayLeadCard(),
                        Score = res.Score()
                    };
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

                    rList.Add(tr);
                }
                reader.Close();
                cmd.Dispose();
            }
            return rList;
        }
    }
}