﻿// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class RoundInfo
    {
        public int TabletDeviceNumber { get; private set; }
        public int RoundNumber { get; private set; }
        public Round RoundData { get; private set; }
        public bool NSMissing { get; set; } = false;
        public bool EWMissing { get; set; } = false;
        public int BoardsFromTable { get; set; } = -1;

        public RoundInfo(int tabletDeviceNumber, TableStatus tableStatus)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            RoundNumber = tabletDeviceStatus.RoundNumber;
            RoundData = tableStatus.RoundData;

            if (RoundNumber > 1)
            {
                RoundsList roundsList = new RoundsList(tabletDeviceStatus.SectionID, RoundNumber - 1);
                BoardsFromTable = roundsList.GetBoardsFromTableNumber(tabletDeviceStatus.RoundNumber, RoundData.LowBoard);
            }
        }
    }
}