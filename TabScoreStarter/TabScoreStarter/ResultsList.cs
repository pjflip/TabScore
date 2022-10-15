// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScoreStarter
{
    public class ResultsList : List<Result>
    {
        public bool IsIndividual = true;
        
        public ResultsList(string connectionString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT Section, [Table], Round, Board, PairNS, PairEW, South, West, Contract, [NS/EW], LeadCard, Result, Remarks FROM ReceivedData";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result result = new Result()
                        {
                            Section = reader.GetInt32(0),
                            Table = reader.GetInt32(1),
                            Round = reader.GetInt32(2),
                            Board = reader.GetInt32(3),
                            PairNS = reader.GetInt32(4),
                            PairEW = reader.GetInt32(5),
                            South = reader.GetInt32(6),
                            West = reader.GetInt32(7),
                            Contract = reader.GetString(8),
                            DeclarerNSEW = reader.GetString(9),
                            LeadCard = reader.GetString(10),
                            TricksTaken = reader.GetString(11),
                            Remarks = reader.GetString(12)
                        };
                        Add(result);
                    }
                    reader.Close();
                    cmd.Dispose();
                }
                else
                {
                    string SQLString = $"SELECT Section, [Table], Round, Board, PairNS, PairEW, Contract, [NS/EW], LeadCard, Result, Remarks FROM ReceivedData";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result result = new Result()
                        {
                            Section = reader.GetInt32(0),
                            Table = reader.GetInt32(1),
                            Round = reader.GetInt32(2),
                            Board = reader.GetInt32(3),
                            PairNS = reader.GetInt32(4),
                            PairEW = reader.GetInt32(5),
                            Contract = reader.GetString(6),
                            DeclarerNSEW = reader.GetString(7),
                            LeadCard = reader.GetString(8),
                            TricksTaken = reader.GetString(9),
                            Remarks = reader.GetString(10)
                        };
                        Add(result);
                    }
                    reader.Close();
                    cmd.Dispose();
                }
            }

            foreach (Result result in this)
            {
                if (result.Remarks == "" || result.Remarks == "Wrong direction")
                {
                    if (result.Contract == "PASS")
                    {
                        result.ContractLevel = 0;
                        result.ContractSuit = "";
                        result.ContractX = "";
                    }
                    else  // Contract (hopefully) contains a valid contract
                    {
                        string[] temp = result.Contract.Split(' ');
                        result.ContractLevel = Convert.ToInt32(temp[0]);
                        result.ContractSuit = temp[1];
                        if (temp.Length > 2) result.ContractX = temp[2];
                        else result.ContractX = "";
                    }
                }
                else  // Either 'Not played' or arbitral result
                {
                    result.ContractLevel = -1;
                    result.ContractSuit = "";
                    result.ContractX = "";
                }
            }
/*
            Sort();  // Using Result CompareTo comparer
*/
        }
    }
}