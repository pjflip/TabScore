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
            if (AppData.IsIndividual)
            {
                if (round.PairNS != 0)
                {
                    Add(new Move(sessionData, newRoundNumber, round.PairNS, "North"));
                }
                if (round.South != 0)
                {
                    Add(new Move(sessionData, newRoundNumber, round.South, "South"));
                }
                if (round.PairEW != 0)
                {
                    Add(new Move(sessionData, newRoundNumber, round.PairEW, "East"));
                }
                if (round.West != 0)
                {
                    Add(new Move(sessionData, newRoundNumber, round.West, "West"));
                }
            }
            else
            {
                if (round.PairNS != 0 && round.PairNS != sessionData.MissingPair)
                {
                    Add(new Move(sessionData, newRoundNumber, round.PairNS, "NS"));
                }
               if (round.PairEW != 0 && round.PairEW != sessionData.MissingPair)
                {
                    Add(new Move(sessionData, newRoundNumber, round.PairEW, "EW"));
                }
            }
            BoardsNewTable = new BoardMove(sessionData, newRoundNumber, round.LowBoard).NewTableNumber;
        }
    }
}