// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;

namespace TabScore.Models
{
    public class TabletDeviceStatus
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; set; }
        public int PairNumber { get; set; }
        public Direction Direction { get; set; }
        public string Location { get; set; }
        public int RoundNumber { get; set; }
        public bool NamesUpdateRequired { get; set; } = true;

        public TabletDeviceStatus(int sectionID, int tableNumber, Direction direction, int pairNumber, int roundNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            Direction = direction;
            PairNumber = pairNumber;
            RoundNumber = roundNumber;
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            Location = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter + tableNumber.ToString();
            if (section.TabletDevicesPerTable == 4)
            {
                Location += " " + Enum.GetName(typeof(Direction), direction);
            }
            else if (section.TabletDevicesPerTable == 2)
            {
                if (direction == Direction.North) Location += " NS";
                else Location += " EW";
            }
            else
            {
                Location = "Table " + Location;
            }
        }

        public void Update(int tableNumber, Direction direction, int roundNumber)
        {
            TableNumber = tableNumber;
            Direction = direction;
            RoundNumber = roundNumber;
            Section section = AppData.SectionsList.Find(x => x.SectionID == SectionID);
            Location = AppData.SectionsList.Find(x => x.SectionID == SectionID).SectionLetter + tableNumber.ToString();
            if (section.TabletDevicesPerTable == 4)
            {
                Location += " " + Enum.GetName(typeof(Direction), direction);
            }
            else
            {
                if (direction == Direction.North) Location += " NS";
                else if (direction == Direction.East) Location += " EW";
                else Location += " Sitout";
            }
        }
    }
}