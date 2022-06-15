// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
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
        public Direction PerspectiveDirection { get; set; }
        public HandRecordPerspectiveButtonOptions PerspectiveButtonOption { get; set; }
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
                PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.None;
                PerspectiveDirection = Direction;
            }
            else if (section.TabletDevicesPerTable == 2)
            {
                if (direction == Direction.North)
                {
                    Location += " NS"; 
                    PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.NS;
                    if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.South;
                    else PerspectiveDirection = Direction.North;
                }
                else
                {
                    Location += " EW"; 
                    PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.EW;
                    if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.West;
                    else PerspectiveDirection = Direction.East;
                }
            }
            else  // TabletDevicesPerTable == 1
            {
                Location = "Table " + Location;
                PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.NSEW;
                if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.South;
                else PerspectiveDirection = Direction.North;
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
                if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.South;
                else PerspectiveDirection = Direction.North;
            }
            else
            {
                if (direction == Direction.North)
                {
                    Location += " NS";
                    PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.NS;
                    if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.South;
                    else PerspectiveDirection = Direction.North;
                }
                else if (direction == Direction.East)
                {
                    Location += " EW";
                    PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.EW;
                    if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.West;
                    else PerspectiveDirection = Direction.East;
                }
                else Location += " Sitout";
            }
        }
    }
}