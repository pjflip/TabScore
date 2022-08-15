// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class Ranking
    {
        public string Orientation {get; set;}
        public int PairNo {get; set;}  // Doubles as player number for individuals
        public string Score {get; set;}
        public double ScoreDecimal {get; set;}
        public string Rank {get; set;}
        public double MP {get; set;}
        public int MPMax {get; set;}
    }
}