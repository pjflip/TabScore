// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;

namespace TabScore.Models
{
    public class MovesList : List<Move>
    {
        public int NewRoundNumber { get; private set; }
        public int LowBoard { get; private set; }
        public int HighBoard { get; private set; }
        public int BoardsNewTable { get; private set; }

        public MovesList(SessionData sessionData, Round round, int newRoundNumber)
        {
            NewRoundNumber = newRoundNumber;
            LowBoard = round.LowBoard;
            HighBoard = round.HighBoard;
            RoundsList roundsList = new RoundsList(sessionData.SectionID, newRoundNumber);
            if (AppData.IsIndividual)
            {
                if (round.PairNS != 0)
                {
                    Add(roundsList.GetMove(sessionData.TableNumber, round.PairNS, "North"));
                }
                if (round.South != 0)
                {
                    Add(roundsList.GetMove(sessionData.TableNumber, round.South, "South"));
                }
                if (round.PairEW != 0)
                {
                    Add(roundsList.GetMove(sessionData.TableNumber, round.PairEW, "East"));
                }
                if (round.West != 0)
                {
                    Add(roundsList.GetMove(sessionData.TableNumber, round.West, "West"));
                }
            }
            else
            {
                if (round.PairNS != 0 && round.PairNS != sessionData.MissingPair)
                {
                    Add(roundsList.GetMove(sessionData.TableNumber, round.PairNS, "NS"));
                }
               if (round.PairEW != 0 && round.PairEW != sessionData.MissingPair)
                {
                    Add(roundsList.GetMove(sessionData.TableNumber, round.PairEW, "EW"));
                }
            }
            BoardsNewTable = roundsList.GetBoardsNewTableNumber(sessionData.TableNumber, round.LowBoard);
        }
    }
}