// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScoreStarter
{
    public class Result
    {
        public int Section { get; set; }
        public int Table { get; set; }
        public int Round { get; set; }
        public int Board { get; set; }
        public int PairNS { get; set; }
        public int PairEW { get; set; }
        public int South { get; set; }
        public int West { get; set; }
        public string Contract { get; set; }
        public int ContractLevel { get; set; }
        public string ContractSuit { get; set; }
        public string ContractX { get; set; }
        public string LeadCard { get; set; }
        public string DeclarerNSEW { get; set; }
        public string TricksTaken { get; set; }
        public bool NotPlayed { get; private set; }
        public bool AdjustedScore { get; set; }
        public string Remarks { get; set; }

        public void UpdateDB(string connectionString)
        {
            int declarer;
            if (ContractLevel <= 0)
            {
                declarer = 0;
            }
            else
            {
                if (DeclarerNSEW == "N" || DeclarerNSEW == "S")
                {
                    declarer = PairNS;
                }
                else
                {
                    declarer = PairEW;
                }
            }

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                // Delete any previous result
                connection.Open();
                string SQLString = $"DELETE FROM ReceivedData WHERE Section={Section} AND [Table]={Table} AND Round={Round} AND Board={Board}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                if (AppData.IsIndividual)
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, South, West, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({Section}, {Table}, {Round}, {Board}, {PairNS}, {PairEW}, {South}, {West}, {declarer}, '{DeclarerNSEW}', '{Contract}', '{TricksTaken}', '{LeadCard}', '{Remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                else
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({Section}, {Table}, {Round}, {Board}, {PairNS}, {PairEW}, {declarer}, '{DeclarerNSEW}', '{Contract}', '{TricksTaken}', '{LeadCard}', '{Remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}
