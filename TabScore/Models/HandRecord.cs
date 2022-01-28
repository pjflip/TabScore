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

        public string NorthSpades;
        public string NorthHearts;
        public string NorthDiamonds;
        public string NorthClubs;
        public string EastSpades;
        public string EastHearts;
        public string EastDiamonds;
        public string EastClubs;
        public string SouthSpades;
        public string SouthHearts;
        public string SouthDiamonds;
        public string SouthClubs;
        public string WestSpades;
        public string WestHearts;
        public string WestDiamonds;
        public string WestClubs;

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