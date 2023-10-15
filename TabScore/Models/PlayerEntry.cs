// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class PlayerEntry
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public Direction Direction { get; set; }

        public PlayerEntry (Round round, Direction direction)
        {
            Direction = direction;
            if (direction == Models.Direction.North)
            {
                Name = round.NameNorth;
                Number = round.NumberNorth;
            }
            else if (direction == Models.Direction.East)
            {
                Name = round.NameEast;
                Number = round.NumberEast;
            }
            else if (direction == Models.Direction.South)
            {
                Name = round.NameSouth;
                Number = round.NumberSouth;
            }
            else
            {
                Name = round.NameWest;
                Number = round.NumberWest;
            }
        }
    }
}