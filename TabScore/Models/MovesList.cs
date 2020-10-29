// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;

namespace TabScore.Models
{
    public class MovesList : List<Move>
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; private set; }
        public int NewRoundNumber { get; private set; }
        public int LowBoard { get; private set; }
        public int HighBoard { get; private set; }
        public int BoardsNewTable { get; private set; }

        public MovesList(TableStatus tableStatus, int newRoundNumber)
        {
            SectionID = tableStatus.SectionID;
            TableNumber = tableStatus.TableNumber;
            NewRoundNumber = newRoundNumber;
            LowBoard = tableStatus.RoundData.LowBoard;
            HighBoard = tableStatus.RoundData.HighBoard;

            RoundsList roundsList = new RoundsList(SectionID, newRoundNumber);
            if (AppData.IsIndividual)
            {
                if (tableStatus.RoundData.PairNS != 0)
                {
                    Add(roundsList.GetMove(TableNumber, tableStatus.RoundData.PairNS, "North"));
                }
                if (tableStatus.RoundData.South != 0)
                {
                    Add(roundsList.GetMove(TableNumber, tableStatus.RoundData.South, "South"));
                }
                if (tableStatus.RoundData.PairEW != 0)
                {
                    Add(roundsList.GetMove(TableNumber, tableStatus.RoundData.PairEW, "East"));
                }
                if (tableStatus.RoundData.West != 0)
                {
                    Add(roundsList.GetMove(TableNumber, tableStatus.RoundData.West, "West"));
                }
            }
            else
            {
                int missingPair = (AppData.SectionsList.Find(x => x.SectionID == SectionID)).MissingPair;
                if (tableStatus.RoundData.PairNS != 0 && tableStatus.RoundData.PairNS != missingPair)
                {
                    Add(roundsList.GetMove(TableNumber, tableStatus.RoundData.PairNS, "NS"));
                }
               if (tableStatus.RoundData.PairEW != 0 && tableStatus.RoundData.PairEW != missingPair)
                {
                    Add(roundsList.GetMove(TableNumber, tableStatus.RoundData.PairEW, "EW"));
                }
            }
            BoardsNewTable = roundsList.GetBoardsNewTableNumber(TableNumber, tableStatus.RoundData.LowBoard);
        }
    }
}