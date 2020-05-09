// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class Move
    {
        public int NewTableNumber { get; set; }
        public string Direction { get; set; }
        public string NewDirection { get; set; }
        public bool Stay { get; set; }
        public int PairNumber { get; set; }
    }
}
