// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class TableStatus
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; private set; }
        public string SectionTableString { get; private set; }
        public Round RoundData { get; set; }
        public Result ResultData { get; set; }
        public LeadValidationOptions LeadValidation { get; set; }

        public TableStatus(int sectionID, int tableNumber, Round roundData)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            SectionTableString = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter + TableNumber.ToString();
            RoundData = roundData;
        }

        public void GetDbResult(int boardNumber)
        {
            ResultData = new Result() { BoardNumber = boardNumber };
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT [NS/EW], Contract, Result, LeadCard, Remarks FROM ReceivedData WHERE Section={SectionID} AND [Table]={TableNumber} AND Round={RoundData.RoundNumber} AND Board={boardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader.GetString(4) == "Not played")
                            {
                                ResultData.ContractLevel = -1;
                                ResultData.ContractSuit = "";
                                ResultData.ContractX = "";
                                ResultData.DeclarerNSEW = "";
                                ResultData.TricksTakenNumber = -1;
                                ResultData.LeadCard = "";
                            }
                            else
                            {
                                string temp = reader.GetString(1);
                                ResultData.Contract = temp;   // Sets ContractLevel, etc
                                if (ResultData.ContractLevel == 0)  // Passed out
                                {
                                    ResultData.DeclarerNSEW = "";
                                    ResultData.TricksTakenNumber = -1;
                                    ResultData.LeadCard = "";
                                }
                                else
                                {
                                    ResultData.DeclarerNSEW = reader.GetString(0);
                                    ResultData.TricksTakenSymbol = reader.GetString(2);
                                    ResultData.LeadCard = reader.GetString(3);
                                }
                            }
                        }
                        else  // No result in database
                        {
                            ResultData.ContractLevel = -999;
                            ResultData.ContractSuit = "";
                            ResultData.ContractX = "";
                            ResultData.DeclarerNSEW = "";
                            ResultData.TricksTakenNumber = -1;
                            ResultData.LeadCard = "";
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
        }

        public void UpdateDbResult()
        {
            int declarer;
            if (ResultData.ContractLevel <= 0)
            {
                declarer = 0;
            }
            else
            {
                if (ResultData.DeclarerNSEW == "N" || ResultData.DeclarerNSEW == "S")
                {
                    declarer = RoundData.PairNS;
                }
                else
                {
                    declarer = RoundData.PairEW;
                }
            }
            string leadcard;
            if (ResultData.LeadCard == null || ResultData.LeadCard == "" || ResultData.LeadCard == "SKIP")
            {
                leadcard = "";
            }
            else
            {
                leadcard = ResultData.LeadCard;
            }

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                // Delete any previous result
                connection.Open();
                string SQLString = $"DELETE FROM ReceivedData WHERE Section={SectionID} AND [Table]={TableNumber} AND Round={RoundData.RoundNumber} AND Board={ResultData.BoardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }

                // Add new result
                string remarks = "";
                if (ResultData.ContractLevel == -1)
                {
                    remarks = "Not played";
                }
                if (AppData.IsIndividual)
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, South, West, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({SectionID}, {TableNumber}, {RoundData.RoundNumber}, {ResultData.BoardNumber}, {RoundData.PairNS}, {RoundData.PairEW}, {RoundData.South}, {RoundData.West}, {declarer}, '{ResultData.DeclarerNSEW}', '{ResultData.Contract}', '{ResultData.TricksTakenSymbol}', '{leadcard}', '{remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                else
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({SectionID}, {TableNumber}, {RoundData.RoundNumber}, {ResultData.BoardNumber}, {RoundData.PairNS}, {RoundData.PairEW}, {declarer}, '{ResultData.DeclarerNSEW}', '{ResultData.Contract}', '{ResultData.TricksTakenSymbol}', '{leadcard}', '{remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd2.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd2.Dispose();
                }
            }
        }
    }
}