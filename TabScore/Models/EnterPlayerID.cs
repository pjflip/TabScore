// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class EnterPlayerID
    {
        public int TabletDeviceNumber { get; set; }
        public Direction Direction { get; set; }
        public string DisplayDirection { get; set; }
    }
}