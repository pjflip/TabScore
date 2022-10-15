// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

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

        public string EvalNorthNT;
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
    }
}