// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Resources;

namespace TabScore.Models
{
    public class PlayerEntry
    {
        public string Name { get; private set; }
        public int Number { get; private set; }
        public Direction Direction { get; private set; }
        public string DisplayDirection { get; private set; }

        public PlayerEntry (Round round, Direction direction)
        {
            Direction = direction;
            if (direction == Models.Direction.North)
            {
                Name = round.NameNorth;
                Number = round.NumberNorth;
                DisplayDirection = Strings.North;
            }
            else if (direction == Models.Direction.East)
            {
                Name = round.NameEast;
                Number = round.NumberEast;
                DisplayDirection = Strings.East;
            }
            else if (direction == Models.Direction.South)
            {
                Name = round.NameSouth;
                Number = round.NumberSouth;
                DisplayDirection = Strings.South;
            }
            else
            {
                Name = round.NameWest;
                Number = round.NumberWest;
                DisplayDirection = Strings.West;
            }
        }
    }
}