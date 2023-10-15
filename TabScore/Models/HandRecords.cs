// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using Resources;

namespace TabScore.Models
{
    // HandRecords is a global class that applies accross all sessions
    public static class HandRecords
    {
        private static DateTime UpdateTime;
        private static bool InitialUpdateComplete = false;

        public static List<HandRecord> HandRecordsList = new List<HandRecord>();

        public static void Refresh()
        {
            if (InitialUpdateComplete && DateTime.Now.Subtract(UpdateTime).TotalMinutes < 1.0) return;  // Hand records updated recently, so don't bother
            UpdateTime = DateTime.Now;

            List<HandRecord> TempHandRecordsList = new List<HandRecord>();
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                string SQLString = $"SELECT Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs FROM HandRecord";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                connection.Open();
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            HandRecord handRecord = new HandRecord
                            {
                                SectionID = reader.GetInt16(0),
                                BoardNumber = reader.GetInt16(1),
                                NorthSpadesPBN = reader.GetString(2),
                                NorthHeartsPBN = reader.GetString(3),
                                NorthDiamondsPBN = reader.GetString(4),
                                NorthClubsPBN = reader.GetString(5),
                                EastSpadesPBN = reader.GetString(6),
                                EastHeartsPBN = reader.GetString(7),
                                EastDiamondsPBN = reader.GetString(8),
                                EastClubsPBN = reader.GetString(9),
                                SouthSpadesPBN = reader.GetString(10),
                                SouthHeartsPBN = reader.GetString(11),
                                SouthDiamondsPBN = reader.GetString(12),
                                SouthClubsPBN = reader.GetString(13),
                                WestSpadesPBN = reader.GetString(14),
                                WestHeartsPBN = reader.GetString(15),
                                WestDiamondsPBN = reader.GetString(16),
                                WestClubsPBN = reader.GetString(17)
                            };
                            handRecord.Dealer = Utilities.GetDealerForBoard(handRecord.BoardNumber);
                            TempHandRecordsList.Add(handRecord);
                        }
                        reader.Close();
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S02")  // HandRecord table does not exist
                    {
                        return;
                    }
                    else
                    {
                        throw (e);
                    }
                }

                // Set display versions of hand record strings using language card names
                foreach (HandRecord handRecord in TempHandRecordsList)
                {
                    handRecord.NorthSpadesDisplay = handRecord.NorthSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.NorthHeartsDisplay = handRecord.NorthHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.NorthDiamondsDisplay = handRecord.NorthDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.NorthClubsDisplay = handRecord.NorthClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.EastSpadesDisplay = handRecord.EastSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.EastHeartsDisplay = handRecord.EastHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.EastDiamondsDisplay = handRecord.EastDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.EastClubsDisplay = handRecord.EastClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.SouthSpadesDisplay = handRecord.SouthSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.SouthHeartsDisplay = handRecord.SouthHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.SouthDiamondsDisplay = handRecord.SouthDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.SouthClubsDisplay = handRecord.SouthClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.WestSpadesDisplay = handRecord.WestSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.WestHeartsDisplay = handRecord.WestHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.WestDiamondsDisplay = handRecord.WestDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                    handRecord.WestClubsDisplay = handRecord.WestClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
                }

                SQLString = $"SELECT Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, NorthNotrump, EastSpades, EastHearts, EastDiamonds, EastClubs, EastNotrump, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, SouthNotrump, WestSpades, WestHearts, WestDiamonds, WestClubs, WestNoTrump, NorthHcp, EastHcp, SouthHcp, WestHcp FROM HandEvaluation";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            HandRecord handRecord = TempHandRecordsList.Find(x => x.SectionID == reader.GetInt16(0) && x.BoardNumber == reader.GetInt16(1));
                            if (handRecord != null)
                            {
                                if (reader.GetInt16(2) > 6) handRecord.EvalNorthSpades = (reader.GetInt16(2) - 6).ToString(); else handRecord.EvalNorthSpades = "";
                                if (reader.GetInt16(3) > 6) handRecord.EvalNorthHearts = (reader.GetInt16(3) - 6).ToString(); else handRecord.EvalNorthHearts = "";
                                if (reader.GetInt16(4) > 6) handRecord.EvalNorthDiamonds = (reader.GetInt16(4) - 6).ToString(); else handRecord.EvalNorthDiamonds = "";
                                if (reader.GetInt16(5) > 6) handRecord.EvalNorthClubs = (reader.GetInt16(5) - 6).ToString(); else handRecord.EvalNorthClubs = "";
                                if (reader.GetInt16(6) > 6) handRecord.EvalNorthNT = (reader.GetInt16(6) - 6).ToString(); else handRecord.EvalNorthNT = "";
                                if (reader.GetInt16(7) > 6) handRecord.EvalEastSpades = (reader.GetInt16(7) - 6).ToString(); else handRecord.EvalEastSpades = "";
                                if (reader.GetInt16(8) > 6) handRecord.EvalEastHearts = (reader.GetInt16(8) - 6).ToString(); else handRecord.EvalEastHearts = "";
                                if (reader.GetInt16(9) > 6) handRecord.EvalEastDiamonds = (reader.GetInt16(9) - 6).ToString(); else handRecord.EvalEastDiamonds = "";
                                if (reader.GetInt16(10) > 6) handRecord.EvalEastClubs = (reader.GetInt16(10) - 6).ToString(); else handRecord.EvalEastClubs = "";
                                if (reader.GetInt16(11) > 6) handRecord.EvalEastNT = (reader.GetInt16(11) - 6).ToString(); else handRecord.EvalEastNT = "";
                                if (reader.GetInt16(12) > 6) handRecord.EvalSouthSpades = (reader.GetInt16(12) - 6).ToString(); else handRecord.EvalSouthSpades = "";
                                if (reader.GetInt16(13) > 6) handRecord.EvalSouthHearts = (reader.GetInt16(13) - 6).ToString(); else handRecord.EvalSouthHearts = "";
                                if (reader.GetInt16(14) > 6) handRecord.EvalSouthDiamonds = (reader.GetInt16(14) - 6).ToString(); else handRecord.EvalSouthDiamonds = "";
                                if (reader.GetInt16(15) > 6) handRecord.EvalSouthClubs = (reader.GetInt16(15) - 6).ToString(); else handRecord.EvalSouthClubs = "";
                                if (reader.GetInt16(16) > 6) handRecord.EvalSouthNT = (reader.GetInt16(16) - 6).ToString(); else handRecord.EvalSouthNT = "";
                                if (reader.GetInt16(17) > 6) handRecord.EvalWestSpades = (reader.GetInt16(17) - 6).ToString(); else handRecord.EvalWestSpades = "";
                                if (reader.GetInt16(18) > 6) handRecord.EvalWestHearts = (reader.GetInt16(18) - 6).ToString(); else handRecord.EvalWestHearts = "";
                                if (reader.GetInt16(19) > 6) handRecord.EvalWestDiamonds = (reader.GetInt16(19) - 6).ToString(); else handRecord.EvalWestDiamonds = "";
                                if (reader.GetInt16(20) > 6) handRecord.EvalWestClubs = (reader.GetInt16(20) - 6).ToString(); else handRecord.EvalWestClubs = "";
                                if (reader.GetInt16(21) > 6) handRecord.EvalWestNT = (reader.GetInt16(21) - 6).ToString(); else handRecord.EvalWestNT = "";

                                handRecord.HCPNorth = reader.GetInt16(22).ToString();
                                handRecord.HCPEast = reader.GetInt16(23).ToString();
                                handRecord.HCPSouth = reader.GetInt16(24).ToString();
                                handRecord.HCPWest = reader.GetInt16(25).ToString();
                            }
                        }
                        reader.Close();
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than HandEvaluation table does not exist
                    {
                        throw (e);
                    }
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            HandRecordsList = TempHandRecordsList;
            InitialUpdateComplete = true;
            return;
        }
    }
}
