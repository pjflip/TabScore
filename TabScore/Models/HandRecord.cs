// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Resources;
using System.Data.Odbc;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace TabScore.Models
{
    public class HandRecord
    {
        public int SectionID;
        public int BoardNumber;
        public int TabletDeviceNumber;
        public string Dealer;
        public bool FromView = false;
        public string PerspectiveDirection;
        public HandRecordPerspectiveButtonOptions PerspectiveButtonOption;

        public string NorthSpadesPBN;
        public string NorthHeartsPBN;
        public string NorthDiamondsPBN;
        public string NorthClubsPBN;
        public string EastSpadesPBN;
        public string EastHeartsPBN;
        public string EastDiamondsPBN;
        public string EastClubsPBN;
        public string SouthSpadesPBN;
        public string SouthHeartsPBN;
        public string SouthDiamondsPBN;
        public string SouthClubsPBN;
        public string WestSpadesPBN;
        public string WestHeartsPBN;
        public string WestDiamondsPBN;
        public string WestClubsPBN;

        public string NorthSpadesDisplay;
        public string NorthHeartsDisplay;
        public string NorthDiamondsDisplay;
        public string NorthClubsDisplay;
        public string EastSpadesDisplay;
        public string EastHeartsDisplay;
        public string EastDiamondsDisplay;
        public string EastClubsDisplay;
        public string SouthSpadesDisplay;
        public string SouthHeartsDisplay;
        public string SouthDiamondsDisplay;
        public string SouthClubsDisplay;
        public string WestSpadesDisplay;
        public string WestHeartsDisplay;
        public string WestDiamondsDisplay;
        public string WestClubsDisplay;

        public string EvalNorthNT = "###";
        public string EvalNorthSpades;
        public string EvalNorthHearts;
        public string EvalNorthDiamonds;
        public string EvalNorthClubs;
        public string EvalEastNT;
        public string EvalEastSpades;
        public string EvalEastHearts;
        public string EvalEastDiamonds;
        public string EvalEastClubs;
        public string EvalSouthNT;
        public string EvalSouthSpades;
        public string EvalSouthHearts;
        public string EvalSouthDiamonds;
        public string EvalSouthClubs;
        public string EvalWestSpades;
        public string EvalWestNT;
        public string EvalWestHearts;
        public string EvalWestDiamonds;
        public string EvalWestClubs;

        public string HCPNorth;
        public string HCPSouth;
        public string HCPEast;
        public string HCPWest;

        public void UpdateDisplay()
        {
            NorthSpadesDisplay = NorthSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            NorthHeartsDisplay = NorthHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            NorthDiamondsDisplay = NorthDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            NorthClubsDisplay = NorthClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            EastSpadesDisplay = EastSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            EastHeartsDisplay = EastHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            EastDiamondsDisplay = EastDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            EastClubsDisplay = EastClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            SouthSpadesDisplay = SouthSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            SouthHeartsDisplay = SouthHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            SouthDiamondsDisplay = SouthDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            SouthClubsDisplay = SouthClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            WestSpadesDisplay = WestSpadesPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            WestHeartsDisplay = WestHeartsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            WestDiamondsDisplay = WestDiamondsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);
            WestClubsDisplay = WestClubsPBN.Replace("A", Strings.A).Replace("K", Strings.K).Replace("Q", Strings.Q).Replace("J", Strings.J).Replace("T", Strings.TenShorthand);

            int northHcp = 0;
            int southHcp = 0;
            int eastHcp = 0;
            int westHcp = 0;
            if (NorthSpadesPBN.Contains("A")) northHcp += 4;
            if (NorthHeartsPBN.Contains("A")) northHcp += 4;
            if (NorthDiamondsPBN.Contains("A")) northHcp += 4;
            if (NorthClubsPBN.Contains("A")) northHcp += 4;
            if (EastSpadesPBN.Contains("A")) eastHcp += 4;
            if (EastHeartsPBN.Contains("A")) eastHcp += 4;
            if (EastDiamondsPBN.Contains("A")) eastHcp += 4;
            if (EastClubsPBN.Contains("A")) eastHcp += 4;
            if (SouthSpadesPBN.Contains("A")) southHcp += 4;
            if (SouthHeartsPBN.Contains("A")) southHcp += 4;
            if (SouthDiamondsPBN.Contains("A")) southHcp += 4;
            if (SouthClubsPBN.Contains("A")) southHcp += 4;
            if (WestSpadesPBN.Contains("A")) westHcp += 4;
            if (WestHeartsPBN.Contains("A")) westHcp += 4;
            if (WestDiamondsPBN.Contains("A")) westHcp += 4;
            if (WestClubsPBN.Contains("A")) westHcp += 4;
            if (NorthSpadesPBN.Contains("K")) northHcp += 3;
            if (NorthHeartsPBN.Contains("K")) northHcp += 3;
            if (NorthDiamondsPBN.Contains("K")) northHcp += 3;
            if (NorthClubsPBN.Contains("K")) northHcp += 3;
            if (EastSpadesPBN.Contains("K")) eastHcp += 3;
            if (EastHeartsPBN.Contains("K")) eastHcp += 3;
            if (EastDiamondsPBN.Contains("K")) eastHcp += 3;
            if (EastClubsPBN.Contains("K")) eastHcp += 3;
            if (SouthSpadesPBN.Contains("K")) southHcp += 3;
            if (SouthHeartsPBN.Contains("K")) southHcp += 3;
            if (SouthDiamondsPBN.Contains("K")) southHcp += 3;
            if (SouthClubsPBN.Contains("K")) southHcp += 3;
            if (WestSpadesPBN.Contains("K")) westHcp += 3;
            if (WestHeartsPBN.Contains("K")) westHcp += 3;
            if (WestDiamondsPBN.Contains("K")) westHcp += 3;
            if (WestClubsPBN.Contains("K")) westHcp += 3;
            if (NorthSpadesPBN.Contains("Q")) northHcp += 2;
            if (NorthHeartsPBN.Contains("Q")) northHcp += 2;
            if (NorthDiamondsPBN.Contains("Q")) northHcp += 2;
            if (NorthClubsPBN.Contains("Q")) northHcp += 2;
            if (EastSpadesPBN.Contains("Q")) eastHcp += 2;
            if (EastHeartsPBN.Contains("Q")) eastHcp += 2;
            if (EastDiamondsPBN.Contains("Q")) eastHcp += 2;
            if (EastClubsPBN.Contains("Q")) eastHcp += 2;
            if (SouthSpadesPBN.Contains("Q")) southHcp += 2;
            if (SouthHeartsPBN.Contains("Q")) southHcp += 2;
            if (SouthDiamondsPBN.Contains("Q")) southHcp += 2;
            if (SouthClubsPBN.Contains("Q")) southHcp += 2;
            if (WestSpadesPBN.Contains("Q")) westHcp += 2;
            if (WestHeartsPBN.Contains("Q")) westHcp += 2;
            if (WestDiamondsPBN.Contains("Q")) westHcp += 2;
            if (WestClubsPBN.Contains("Q")) westHcp += 2;
            if (NorthSpadesPBN.Contains("J")) northHcp += 1;
            if (NorthHeartsPBN.Contains("J")) northHcp += 1;
            if (NorthDiamondsPBN.Contains("J")) northHcp += 1;
            if (NorthClubsPBN.Contains("J")) northHcp += 1;
            if (EastSpadesPBN.Contains("J")) eastHcp += 1;
            if (EastHeartsPBN.Contains("J")) eastHcp += 1;
            if (EastDiamondsPBN.Contains("J")) eastHcp += 1;
            if (EastClubsPBN.Contains("J")) eastHcp += 1;
            if (SouthSpadesPBN.Contains("J")) southHcp += 1;
            if (SouthHeartsPBN.Contains("J")) southHcp += 1;
            if (SouthDiamondsPBN.Contains("J")) southHcp += 1;
            if (SouthClubsPBN.Contains("J")) southHcp += 1;
            if (WestSpadesPBN.Contains("J")) westHcp += 1;
            if (WestHeartsPBN.Contains("J")) westHcp += 1;
            if (WestDiamondsPBN.Contains("J")) westHcp += 1;
            if (WestClubsPBN.Contains("J")) westHcp += 1;
            HCPNorth = northHcp.ToString();
            HCPEast = eastHcp.ToString();
            HCPSouth = southHcp.ToString();
            HCPWest = westHcp.ToString();

            // Set dealer based on board number
            switch ((BoardNumber - 1) % 4)
            {
                case 0:
                    Dealer = Strings.N;
                    break;
                case 1:
                    Dealer = Strings.E;
                    break;
                case 2:
                    Dealer = Strings.S;
                    break;
                case 3:
                    Dealer = Strings.W;
                    break;
                default:
                    Dealer = "#";
                    break;
            }

        }

        public void UpdateDoubleDummy()
        {
            StringBuilder PBNString = new StringBuilder();
            switch ((BoardNumber - 1) % 4)
            {
                case 0:
                    PBNString.Append("N:");
                    PBNString.Append(NorthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(EastSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(SouthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(WestSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestClubsPBN);
                    break;
                case 1:
                    PBNString.Append("E:");
                    PBNString.Append(EastSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(SouthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(WestSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(NorthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthClubsPBN);
                    break;
                case 2:
                    PBNString.Append("S:");
                    PBNString.Append(SouthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(WestSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(NorthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(EastSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastClubsPBN);
                    break;
                case 3:
                    PBNString.Append("W:");
                    PBNString.Append(WestSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(WestClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(NorthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(NorthClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(EastSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(EastClubsPBN);
                    PBNString.Append(" ");
                    PBNString.Append(SouthSpadesPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthHeartsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthDiamondsPBN);
                    PBNString.Append(".");
                    PBNString.Append(SouthClubsPBN);
                    break;
            }

            int[] ddTable = new int[20];
            ddTable = DoubleDummySolver.CalcTable(PBNString.ToString());
            if (ddTable[0] > 6) EvalNorthSpades = (ddTable[0] - 6).ToString(); else EvalNorthSpades = "";
            if (ddTable[1] > 6) EvalEastSpades = (ddTable[1] - 6).ToString(); else EvalEastSpades = "";
            if (ddTable[2] > 6) EvalSouthSpades = (ddTable[2] - 6).ToString(); else EvalSouthSpades = "";
            if (ddTable[3] > 6) EvalWestSpades = (ddTable[3] - 6).ToString(); else EvalWestSpades = "";
            if (ddTable[4] > 6) EvalNorthHearts = (ddTable[4] - 6).ToString(); else EvalNorthHearts = "";
            if (ddTable[5] > 6) EvalEastHearts = (ddTable[5] - 6).ToString(); else EvalEastHearts = "";
            if (ddTable[6] > 6) EvalSouthHearts = (ddTable[6] - 6).ToString(); else EvalSouthHearts = "";
            if (ddTable[7] > 6) EvalWestHearts = (ddTable[7] - 6).ToString(); else EvalWestHearts = "";
            if (ddTable[8] > 6) EvalNorthDiamonds = (ddTable[8] - 6).ToString(); else EvalNorthDiamonds = "";
            if (ddTable[9] > 6) EvalEastDiamonds = (ddTable[9] - 6).ToString(); else EvalEastDiamonds = "";
            if (ddTable[10] > 6) EvalSouthDiamonds = (ddTable[10] - 6).ToString(); else EvalSouthDiamonds = "";
            if (ddTable[11] > 6) EvalWestDiamonds = (ddTable[11] - 6).ToString(); else EvalWestDiamonds = "";
            if (ddTable[12] > 6) EvalNorthClubs = (ddTable[12] - 6).ToString(); else EvalNorthClubs = "";
            if (ddTable[13] > 6) EvalEastClubs = (ddTable[13] - 6).ToString(); else EvalEastClubs = "";
            if (ddTable[14] > 6) EvalSouthClubs = (ddTable[14] - 6).ToString(); else EvalSouthClubs = "";
            if (ddTable[15] > 6) EvalWestClubs = (ddTable[15] - 6).ToString(); else EvalWestClubs = "";
            if (ddTable[16] > 6) EvalNorthNT = (ddTable[16] - 6).ToString(); else EvalNorthNT = "";
            if (ddTable[17] > 6) EvalEastNT = (ddTable[17] - 6).ToString(); else EvalEastNT = "";
            if (ddTable[18] > 6) EvalSouthNT = (ddTable[18] - 6).ToString(); else EvalSouthNT = "";
            if (ddTable[19] > 6) EvalWestNT = (ddTable[19] - 6).ToString(); else EvalWestNT = "";
        }

        public void UpdateDB() { 
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                // Delete any previous hand record just in case (but there shouldn't be one)
                connection.Open();
                string SQLString = $"DELETE FROM HandRecord WHERE Section={SectionID} AND Board={BoardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                catch { }

                SQLString = $"INSERT INTO HandRecord (Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs) VALUES ('{SectionID}', '{BoardNumber}', '{NorthSpadesPBN}', '{NorthHeartsPBN}', '{NorthDiamondsPBN}', '{NorthClubsPBN}', '{EastSpadesPBN}', '{EastHeartsPBN}', '{EastDiamondsPBN}', '{EastClubsPBN}', '{SouthSpadesPBN}', '{SouthHeartsPBN}', '{SouthDiamondsPBN}', '{SouthClubsPBN}', '{WestSpadesPBN}', '{WestHeartsPBN}', '{WestDiamondsPBN}', '{WestClubsPBN}')";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                catch { }
                cmd.Dispose();
            }
        }
    }
}