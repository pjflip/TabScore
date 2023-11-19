// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Data.Odbc;
using System.Text;

namespace TabScore2Starter
{
    class Options
    {
        public bool ShowTraveller { get; set; }
        public bool ShowPercentage { get; set; }
        public bool EnterLeadCard { get; set; }
        public bool ValidateLeadCard { get; set; }
        public int ShowRanking { get; set; }
        public bool ShowHandRecord { get; set; }
        public bool NumberEntryEachRound { get; set; }
        public int NameSource { get; set; }
        public int EnterResultsMethod { get; set; }
        public bool TabletsMove { get; set; }
        public bool HandRecordReversePerspective { get; set; }
        public bool ShowTimer { get; set; }
        public int SecondsPerBoard { get; set; }
        public int AdditionalSecondsPerRound { get; set; }
        public bool ManualHandRecordEntry { get; set; }
        public bool DoubleDummy { get; set; }

        public Options() { }

        public Options(string connectionString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();
                string SQLString = $"SELECT ShowResults, ShowPercentage, LeadCard, BM2ValidateLeadCard, BM2Ranking, BM2ViewHandRecord, BM2NumberEntryEachRound, BM2NameSource, EnterResultsMethod, TabletsMove, HandRecordReversePerspective, ShowTimer, SecondsPerBoard, AdditionalSecondsPerRound, BM2EnterHandRecord, DoubleDummy FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = cmd.ExecuteReader();
                reader.Read();
                ShowTraveller = reader.GetBoolean(0);
                ShowPercentage = reader.GetBoolean(1);
                EnterLeadCard = reader.GetBoolean(2);
                ValidateLeadCard = reader.GetBoolean(3);
                ShowRanking = reader.GetInt32(4);
                ShowHandRecord = reader.GetBoolean(5);
                NumberEntryEachRound = reader.GetBoolean(6);
                NameSource = reader.GetInt32(7);
                EnterResultsMethod = reader.GetInt32(8);
                if (EnterResultsMethod != 1) EnterResultsMethod = 0;
                TabletsMove = reader.GetBoolean(9);
                HandRecordReversePerspective = reader.GetBoolean(10);
                ShowTimer = reader.GetBoolean(11);
                SecondsPerBoard = reader.GetInt32(12);
                AdditionalSecondsPerRound = reader.GetInt32(13);
                ManualHandRecordEntry = reader.GetBoolean(14);
                DoubleDummy = reader.GetBoolean(15);
                reader.Close();
                cmd.Dispose();
            }
        }

        public void UpdateDB(string connectionString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                StringBuilder SQLString = new StringBuilder();
                SQLString.Append($"UPDATE Settings SET");
                if (ShowTraveller)
                {
                    SQLString.Append(" ShowResults=YES,");
                }
                else
                {
                    SQLString.Append(" ShowResults=NO,");
                }
                if (ShowPercentage)
                {
                    SQLString.Append(" ShowPercentage=YES,");
                }
                else
                {
                    SQLString.Append(" ShowPercentage=NO,");
                }
                if (EnterLeadCard)
                {
                    SQLString.Append(" LeadCard=YES,");
                }
                else
                {
                    SQLString.Append(" LeadCard=NO,");
                }
                if (ValidateLeadCard)
                {
                    SQLString.Append(" BM2ValidateLeadCard=YES,");
                }
                else
                {
                    SQLString.Append(" BM2ValidateLeadCard=NO,");
                }
                SQLString.Append($" BM2Ranking={ShowRanking},");
                if (ShowHandRecord)
                {
                    SQLString.Append(" BM2ViewHandRecord=YES,");
                }
                else
                {
                    SQLString.Append(" BM2ViewHandRecord=NO,");
                }
                if (NumberEntryEachRound)
                {
                    SQLString.Append(" BM2NumberEntryEachRound=YES,");
                }
                else
                {
                    SQLString.Append(" BM2NumberEntryEachRound=NO,");
                }
                SQLString.Append($" BM2NameSource={NameSource},");
                SQLString.Append($" EnterResultsMethod={EnterResultsMethod},");
                if (TabletsMove)
                {
                    SQLString.Append(" TabletsMove=YES,");
                }
                else
                {
                    SQLString.Append(" TabletsMove=NO,");
                }
                if (HandRecordReversePerspective)
                {
                    SQLString.Append(" HandRecordReversePerspective=YES,");
                }
                else
                {
                    SQLString.Append(" HandRecordReversePerspective=NO,");
                }
                if (ShowTimer)
                {
                    SQLString.Append(" ShowTimer=YES,");
                }
                else
                {
                    SQLString.Append(" ShowTimer=NO,");
                }
                SQLString.Append($" SecondsPerBoard={SecondsPerBoard},");
                SQLString.Append($" AdditionalSecondsPerRound={AdditionalSecondsPerRound},");
                if (ManualHandRecordEntry)
                {
                    SQLString.Append(" BM2EnterHandRecord=YES,");
                }
                else
                {
                    SQLString.Append(" BM2EnterHandRecord=NO,");
                }
                if (DoubleDummy)
                {
                    SQLString.Append(" DoubleDummy=YES");
                }
                else
                {
                    SQLString.Append(" DoubleDummy=NO");
                }

                connection.Open();
                OdbcCommand cmd = new OdbcCommand(SQLString.ToString(), connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}