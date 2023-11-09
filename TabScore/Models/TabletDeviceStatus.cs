// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Resources;

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
        public bool AtSitoutTable { get; set; }

        public TabletDeviceStatus(int sectionID, int tableNumber, Direction direction, int pairNumber, int roundNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            Direction = direction;
            PairNumber = pairNumber;
            RoundNumber = roundNumber;
            SetLocation();
        }

        public void Update(int tableNumber, Direction direction, int roundNumber)
        {
            TableNumber = tableNumber;
            Direction = direction;
            RoundNumber = roundNumber;
            SetLocation();
        }

        private void SetLocation()
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == SectionID);
            Location = AppData.SectionsList.Find(x => x.SectionID == SectionID).SectionLetter + TableNumber.ToString();
            if (section.TabletDevicesPerTable == 4)
            {
                Location += " ";
                switch (Direction)
                {
                    case Direction.North:
                        Location += Strings.North;
                        break;
                    case Direction.South:
                        Location += Strings.South;
                        break;
                    case Direction.East:
                        Location += Strings.East;
                        break;
                    case Direction.West:
                        Location += Strings.West;
                        break;
                    case Direction.Sitout:
                        Location += Strings.Sitout;
                        break;
                }
                PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.None;
                PerspectiveDirection = Direction;
            }
            else if (section.TabletDevicesPerTable == 2)
            {
                if (Direction == Direction.North)
                {
                    Location += $" {Strings.N}{Strings.S}";
                    PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.NS;
                    if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.South;
                    else PerspectiveDirection = Direction.North;
                }
                else if (Direction == Direction.East)
                {
                    Location += $" {Strings.E}{Strings.W}";
                    PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.EW;
                    if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.West;
                    else PerspectiveDirection = Direction.East;
                }
                else Location += $" {Strings.Sitout}";
            }
            else  // TabletDevicesPerTable == 1
            {
                Location = $"{Strings.Table} " + Location;
                PerspectiveButtonOption = HandRecordPerspectiveButtonOptions.NSEW;
                if (Settings.HandRecordReversePerspective) PerspectiveDirection = Direction.South;
                else PerspectiveDirection = Direction.North;
            }
        }
    }
}
