using System.Collections.Generic;

namespace TabScore.Models
{
    public class MovesList : List<Move>
    {
        public int NewRoundNumber { get; private set; }
        public int LowBoard { get; private set; }
        public int HighBoard { get; private set; }
        public int BoardsNewTable { get; private set; }

        public MovesList(string DB, int sectionID, Round round, int newRoundNumber, int tableNumber, int missingPair, bool individual)
        {
            NewRoundNumber = newRoundNumber;
            LowBoard = round.LowBoard;
            HighBoard = round.HighBoard;
            if (individual)
            {
                if (round.PairNS != 0)
                {
                    Add(new Move(DB, sectionID, newRoundNumber, tableNumber, round.PairNS, "North"));
                }
                if (round.South != 0)
                {
                    Add(new Move(DB, sectionID, newRoundNumber, tableNumber, round.South, "South"));
                }
                if (round.PairEW != 0)
                {
                    Add(new Move(DB, sectionID, newRoundNumber, tableNumber, round.PairEW, "East"));
                }
                if (round.West != 0)
                {
                    Add(new Move(DB, sectionID, newRoundNumber, tableNumber, round.West, "West"));
                }
            }
            else
            {
                if (round.PairNS != 0 && round.PairNS != missingPair)
                {
                    Add(new Move(DB, sectionID, newRoundNumber, tableNumber, round.PairNS, "NS"));
                }
               if (round.PairEW != 0 && round.PairEW != missingPair)
                {
                    Add(new Move(DB, sectionID, newRoundNumber, tableNumber, round.PairEW, "EW"));
                }
            }
            BoardsNewTable = new BoardMove(DB, sectionID, newRoundNumber, tableNumber, round.LowBoard).TableNumber;
        }
    }
}