// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class TravellerResult
    {
        public int NumberNorth { get; set; }
        public int NumberEast { get; set; }
        public int NumberSouth { get; set; }
        public int NumberWest { get; set; }
        public string Contract { get; set; }    
        public string DeclarerNSEW { get; set; }
        public string LeadCard { get; set; }
        public string TricksTaken { get; set; }
        public string ScoreNS { get; set; }
        public string ScoreEW { get; set; }
        public double SortPercentage { get; set; }
        public bool Highlight { get; set; } = false;
    }
}
