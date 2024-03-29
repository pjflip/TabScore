﻿// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    // Settings is a global class that applies accross all sessions
    public static class Settings
    {
        public static bool ShowResults { get; private set; }
        public static bool ShowPercentage { get; private set; }
        public static bool EnterLeadCard { get; private set; }
        public static bool ValidateLeadCard { get; private set; }
        public static int ShowRanking { get; private set; }
        public static int EnterResultsMethod { get; private set; }
        public static bool ShowHandRecord { get; private set; }
        public static bool HandRecordReversePerspective { get; private set; }
        public static bool NumberEntryEachRound { get; private set; }
        public static int NameSource { get; private set; }
        public static bool TabletDevicesMove { get; private set; }
        public static bool ShowTimer { get; private set; }
        public static int SecondsPerBoard { get; private set; }
        public static int AdditionalSecondsPerRound { get; private set; }
        public static bool ManualHandRecordEntry { get; private set; }
        public static bool DoubleDummy { get; private set; }

        private static DateTime UpdateTime;

        public static void Refresh()
        {
            if (DateTime.Now.Subtract(UpdateTime).TotalMinutes < 1.0) return;  // Settings updated recently, so don't bother
            UpdateTime = DateTime.Now;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = "SELECT ShowResults, ShowPercentage, LeadCard, BM2ValidateLeadCard, BM2Ranking, EnterResultsMethod, BM2ViewHandRecord, BM2NumberEntryEachRound, BM2NameSource, TabletsMove, HandRecordReversePerspective, ShowTimer, SecondsPerBoard, AdditionalSecondsPerRound, BM2EnterHandRecord, DoubleDummy FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ShowResults = reader.GetBoolean(0);
                            ShowPercentage = reader.GetBoolean(1);
                            EnterLeadCard = reader.GetBoolean(2);
                            ValidateLeadCard = reader.GetBoolean(3);
                            ShowRanking = reader.GetInt32(4);
                            EnterResultsMethod = reader.GetInt32(5);
                            ShowHandRecord = reader.GetBoolean(6);
                            NumberEntryEachRound = reader.GetBoolean(7);
                            NameSource = reader.GetInt32(8);
                            TabletDevicesMove = reader.GetBoolean(9);
                            HandRecordReversePerspective = reader.GetBoolean(10);
                            ShowTimer = reader.GetBoolean(11);
                            SecondsPerBoard = reader.GetInt32(12);
                            AdditionalSecondsPerRound = reader.GetInt32(13);
                            ManualHandRecordEntry = reader.GetBoolean(14);
                            DoubleDummy = reader.GetBoolean(15);
                        }
                        reader.Close();
                    });
                }
                catch
                {
                    ShowResults = true;
                    ShowPercentage = true;
                    EnterLeadCard = true;
                    ValidateLeadCard = true;
                    ShowRanking = 1;
                    EnterResultsMethod = 1;
                    ShowHandRecord = true;
                    NumberEntryEachRound = true;
                    NameSource = 0;
                    TabletDevicesMove = false;
                    HandRecordReversePerspective = true;
                    ShowTimer = false;
                    SecondsPerBoard = 390;
                    AdditionalSecondsPerRound = 60;
                    ManualHandRecordEntry = false;
                    DoubleDummy = false;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
    }
}