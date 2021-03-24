// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class ResultsList : List<Result>
    {
        public int TabletDeviceNumber { get; set; }
        public int RoundNumber { get; set; }
        public bool GotAllResults { get; private set; }
        public bool ShowViewButton { get; private set; }
        public string Message { get; set; } = "";

        public ResultsList(TableStatus tableStatus, int tabletDeviceNumber)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            RoundNumber = tableStatus.RoundData.RoundNumber;
            GotAllResults = true;
            ShowViewButton = Settings.ShowResults;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString;
                OdbcCommand cmd = null;
                OdbcDataReader reader = null;
                try
                {
                    SQLString = $"SELECT Board, [NS/EW], Contract, Result, Remarks FROM ReceivedData WHERE Section={tableStatus.SectionID} AND [Table]={tableStatus.TableNumber} AND Round={RoundNumber} AND Board>={tableStatus.RoundData.LowBoard} AND Board<={tableStatus.RoundData.HighBoard}";
                    cmd = new OdbcCommand(SQLString, connection);
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Result result = new Result() { BoardNumber = reader.GetInt32(0) };
                            if (reader.GetString(4) == "Not played")
                            {
                                result.ContractLevel = -1;
                                result.ContractSuit = "";
                                result.ContractX = "";
                                result.DeclarerNSEW = "";
                                result.TricksTakenNumber = -1;
                            }
                            else
                            {
                                string temp = reader.GetString(2);
                                result.Contract = temp;   // Sets ContractLevel, etc
                                if (result.ContractLevel == 0)  // Passed out
                                {
                                    result.DeclarerNSEW = "";
                                    result.TricksTakenNumber = -1;
                                }
                                else
                                {
                                    result.DeclarerNSEW = reader.GetString(1);
                                    result.TricksTakenSymbol = reader.GetString(3);
                                }
                            }
                            Add(result);
                        }
                    }); 
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }

                // Check to see if any boards don't have a result, and add dummies to the list
                for (int iBoard = tableStatus.RoundData.LowBoard; iBoard <= tableStatus.RoundData.HighBoard; iBoard++)
                {
                    if (Find(x => x.BoardNumber == iBoard) == null)
                    {
                        GotAllResults = false;
                        Result result = new Result
                        {
                            BoardNumber = iBoard,
                            ContractLevel = -999,
                            ContractSuit = "",
                            ContractX = "",
                            DeclarerNSEW = "",
                            TricksTakenNumber = -1,
                            LeadCard = ""
                        };
                        Add(result);
                    }
                }
                Sort((x, y) => x.BoardNumber.CompareTo(y.BoardNumber));
            }
        }
    }
}