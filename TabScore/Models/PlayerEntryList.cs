// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;

namespace TabScore.Models
{
    public class PlayerEntryList : List<PlayerEntry>
    {
        public int TabletDeviceNumber { get; private set; }
        public int NumberOfBlankEntries { get; private set; }
        public bool ShowWarning { get; set; } = false;
        public bool ShowMessage { get; private set; }

        public PlayerEntryList(int tabletDeviceNumber, TableStatus tableStatus)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Section section = AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID);
            
            if (section.TabletDevicesPerTable == 1)
            {
                if (tableStatus.RoundData.NumberNorth != 0 && tableStatus.RoundData.NumberNorth != section.MissingPair)
                {
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.North));
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.South));
                }
                if (tableStatus.RoundData.NumberEast != 0 && tableStatus.RoundData.NumberEast != section.MissingPair)
                {
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.East));
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.West));
                }
            }
            else if (section.TabletDevicesPerTable == 2)
            {
                if (tabletDeviceStatus.Direction == Direction.North)
                {
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.North));
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.South));
                }
                else
                {
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.East));
                    Add(new PlayerEntry(tableStatus.RoundData, Direction.West));
                }
            }
            else  // tabletDevicesPerTable == 4
            {
                Add(new PlayerEntry(tableStatus.RoundData, tabletDeviceStatus.Direction));
            }

            NumberOfBlankEntries = FindAll(x => x.Name == "").Count;
            ShowMessage = section.TabletDevicesPerTable == 4 || (section.TabletDevicesPerTable == 2 && Count == 4);
        }
    }
}