// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class TableStatus
    {
        public int SectionID { get; set; }
        public int TableNumber { get; set; }
        public int RoundNumber { get; set; }
        public Round RoundData { get; set; }
        public Result ResultData { get; set; }
        public bool ReadyForNextRoundNorth { get; set; } = false;
        public bool ReadyForNextRoundSouth { get; set; } = false;
        public bool ReadyForNextRoundEast { get; set; } = false;
        public bool ReadyForNextRoundWest { get; set; } = false;

        public TableStatus(int sectionID, int tableNumber, int roundNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            RoundNumber = roundNumber;
            RoundData = new Round(this);
        }

        public void Update(int tableNumber, int roundNumber)
        {
            TableNumber = tableNumber;
            RoundNumber = roundNumber;
            RoundData = new Round(this);
            ReadyForNextRoundNorth = false;
            ReadyForNextRoundSouth = false;
            ReadyForNextRoundEast = false;
            ReadyForNextRoundWest = false;
        }
    }
}