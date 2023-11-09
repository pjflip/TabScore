// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Microsoft.Ajax.Utilities;
using Resources;
using System.Xml.Linq;

namespace TabScore.Models
{
    public class EnterPlayerID
    {
        public int TabletDeviceNumber { get; set; }
        public Direction Direction { get; set; }
        public string DisplayDirection { get; set; }

        public EnterPlayerID(int tabletDeviceNumber, Direction direction)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            Direction = direction;
            if (direction == Direction.North)
            {
                DisplayDirection = Strings.North;
            }
            else if (direction == Direction.East)
            {
                DisplayDirection = Strings.East;
            }
            else if (direction == Direction.South)
            {
                DisplayDirection = Strings.South;
            }
            else
            {
                DisplayDirection = Strings.West;
            }
        }

    }
}