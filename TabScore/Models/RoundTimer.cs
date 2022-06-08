// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class RoundTimer
    {
        public int SectionID { get; set; }
        public int RoundNumber { get; set; }
        public System.DateTime StartTime { get; set; }
        public int SecondsPerRound { get; set; }
    }
}