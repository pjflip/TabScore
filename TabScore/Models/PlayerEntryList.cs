// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
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

        public PlayerEntryList(TableStatus tableStatus, int tabletDeviceNumber)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            int tabletDevicesPerTable = AppData.SectionsList.Find(x => x.SectionID == AppData.TabletDeviceStatusList[tabletDeviceNumber].SectionID).TabletDevicesPerTable;
            string direction = AppData.TabletDeviceStatusList[tabletDeviceNumber].Direction;
            int missingPair = AppData.SectionsList.Find(x => x.SectionID == tableStatus.SectionID).MissingPair;
            
            if (tabletDevicesPerTable == 1)
            {
                if (tableStatus.RoundData.NumberNorth != 0 && tableStatus.RoundData.NumberNorth != missingPair)
                {
                    Add(new PlayerEntry(tableStatus.RoundData, "North"));
                    Add(new PlayerEntry(tableStatus.RoundData, "South"));
                }
                if (tableStatus.RoundData.NumberEast != 0 && tableStatus.RoundData.NumberEast != missingPair)
                {
                    Add(new PlayerEntry(tableStatus.RoundData, "East"));
                    Add(new PlayerEntry(tableStatus.RoundData, "West"));
                }
            }
            else if (tabletDevicesPerTable == 2)
            {
                if (direction == "North")
                {
                    Add(new PlayerEntry(tableStatus.RoundData, "North"));
                    Add(new PlayerEntry(tableStatus.RoundData, "South"));
                }
                else
                {
                    Add(new PlayerEntry(tableStatus.RoundData, "East"));
                    Add(new PlayerEntry(tableStatus.RoundData, "West"));
                }
            }
            else  // tabletDevicesPerTable == 4
            {
                Add(new PlayerEntry(tableStatus.RoundData, direction));
            }

            NumberOfBlankEntries = FindAll(x => x.Name == "").Count;
            ShowMessage = tabletDevicesPerTable == 4 || (tabletDevicesPerTable == 2 && Count == 4);
        }
    }
}