// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;

namespace TabScore.Models
{
    public class MovesList : List<Move>
    {
        public int TabletDeviceNumber { get; set; }
        public Direction Direction { get; private set; }
        public int NewRoundNumber { get; private set; }
        public int LowBoard { get; private set; }
        public int HighBoard { get; private set; }
        public int BoardsNewTable { get; private set; }
        public int TabletDevicesPerTable { get; private set; }
        public int TableNotReadyNumber { get; private set; }

        public MovesList(int tabletDeviceNumber, TableStatus tableStatus, int newRoundNumber, int tableNotReadyNumber)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            TabletDeviceStatus tabletDeviceStatus= AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Direction = tabletDeviceStatus.Direction;
            NewRoundNumber = newRoundNumber;
            TableNotReadyNumber = tableNotReadyNumber;
            Section section = AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID);
            TabletDevicesPerTable = section.TabletDevicesPerTable;
            int missingPair = section.MissingPair;

            RoundsList roundsList = new RoundsList(tabletDeviceStatus.SectionID, newRoundNumber);
            if (TabletDevicesPerTable == 1)  // tableStatus cannot be null
            {
                if (AppData.IsIndividual)
                {
                    if (tableStatus.RoundData.NumberNorth != 0)
                    {
                        Add(roundsList.GetMove(tableStatus.TableNumber, tableStatus.RoundData.NumberNorth, Direction.North));
                    }
                    if (tableStatus.RoundData.NumberSouth != 0)
                    {
                        Add(roundsList.GetMove(tableStatus.TableNumber, tableStatus.RoundData.NumberSouth, Direction.South));
                    }
                    if (tableStatus.RoundData.NumberEast != 0)
                    {
                        Add(roundsList.GetMove(tableStatus.TableNumber, tableStatus.RoundData.NumberEast, Direction.East));
                    }
                    if (tableStatus.RoundData.NumberWest != 0)
                    {
                        Add(roundsList.GetMove(tableStatus.TableNumber, tableStatus.RoundData.NumberWest, Direction.West));
                    }
                }
                else  // Not individual
                {
                    if (tableStatus.RoundData.NumberNorth != 0 && tableStatus.RoundData.NumberNorth != missingPair)
                    {
                        Add(roundsList.GetMove(tableStatus.TableNumber, tableStatus.RoundData.NumberNorth, Direction.North));
                    }
                    if (tableStatus.RoundData.NumberEast != 0 && tableStatus.RoundData.NumberEast != missingPair)
                    {
                        Add(roundsList.GetMove(tableStatus.TableNumber, tableStatus.RoundData.NumberEast, Direction.East));
                    }
                }
            }
            else  // TabletDevicesPerTable > 1, so only need move for single player/pair.  tableStatus could be null (at phantom table), so use tabletDeviceStatus
            {
                Add(roundsList.GetMove(tabletDeviceStatus.TableNumber, tabletDeviceStatus.PairNumber, Direction));
            }

            BoardsNewTable = -999;
            if (tableStatus != null)  // tableStatus==null => phantom table, so no boards to worry about
            {
                // Show boards move only to North (or North/South) unless missing, in which case only show to East (or East/West)
                LowBoard = tableStatus.RoundData.LowBoard;
                HighBoard = tableStatus.RoundData.HighBoard;
                if (Direction == Direction.North || ((tableStatus.RoundData.NumberNorth == 0 || tableStatus.RoundData.NumberNorth == missingPair) && Direction == Direction.East))
                {
                    BoardsNewTable = roundsList.GetBoardsNewTableNumber(tableStatus.TableNumber, tableStatus.RoundData.LowBoard);
                }
            }
        }
    }
}