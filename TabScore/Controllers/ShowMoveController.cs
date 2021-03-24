// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, int newRoundNumber, int tableNotReadyNumber = -1)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            if (newRoundNumber > Utilities.NumberOfRoundsInEvent(tabletDeviceStatus.SectionID))  // Session complete
            {
                if (Settings.ShowRanking == 2)
                {
                    return RedirectToAction("Final", "ShowRankingList", new { tabletDeviceNumber });
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen", new { tabletDeviceNumber });
                }
            }

            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            if (tableStatus != null && tableStatus.RoundData.RoundNumber < newRoundNumber)
            {
                // No tablet device has yet advanced this table to the next round, so show that this one is ready to do so
                if (tabletDeviceStatus.Direction == "North")
                {
                    tableStatus.ReadyForNextRoundNorth = true;
                }
                else if (tabletDeviceStatus.Direction == "East")
                {
                    tableStatus.ReadyForNextRoundEast = true;
                }
                else if (tabletDeviceStatus.Direction == "South")
                {
                    tableStatus.ReadyForNextRoundSouth = true;
                }
                else if (tabletDeviceStatus.Direction == "West")
                {
                    tableStatus.ReadyForNextRoundWest = true;
                }
            }

            MovesList movesList = new MovesList(tabletDeviceNumber, newRoundNumber, tableNotReadyNumber);

            ViewData["Header"] = $"{tabletDeviceStatus.Location}";
            ViewData["Title"] = $"Show Move - {tabletDeviceStatus.Location}";
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;

            if (AppData.IsIndividual)
            {
                return View("Individual", movesList);
            }
            else
            {
                return View("Pair", movesList);
            }
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber, int newRoundNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Section section = AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID);

            // Check if trying to advance to next round too quickly (ie before previous round is complete)
            int minRoundNumber = 999;
            foreach (TableStatus iTableStatus in AppData.TableStatusList)
            {
                if (iTableStatus.TableNumber > 0 && iTableStatus.RoundData.RoundNumber < minRoundNumber) minRoundNumber = iTableStatus.RoundData.RoundNumber;
            }
            if (newRoundNumber > minRoundNumber + 1)
            {
                return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber, tableNotReadyNumber = 0 });
            }

            Move move = null;
            if (section.TabletDevicesPerTable > 1)  // Tablet devices are moving, so need to check if new table is ready
            {
                // Get the move for this tablet device
                RoundsList roundsList = new RoundsList(tabletDeviceStatus.SectionID, newRoundNumber);
                move = roundsList.GetMove(tabletDeviceStatus.TableNumber, tabletDeviceStatus.PairNumber, tabletDeviceStatus.Direction);
                TableStatus newTableStatus = AppData.TableStatusList.Find(x => x.SectionID == section.SectionID && x.TableNumber == move.NewTableNumber);

                // Check if move-to table is ready
                bool newTableNotReady = true;
                if (move.NewTableNumber == 0)
                {
                    newTableNotReady = false;   // Sit-out table is automatically ready
                }
                else if (newTableStatus != null)   // Check that move-to table has indeed been registered 
                {
                    if (newTableStatus.RoundData.RoundNumber == newRoundNumber)
                    {
                        // Move-to table has already been advanced to next round by another tablet device, so is obviously ready
                        newTableNotReady = false;
                    }
                    else
                    {
                        // A table is ready if all non-sitout tablet device locations are ready
                        bool newTableNSMissing = newTableStatus.RoundData.NumberNorth == 0 || newTableStatus.RoundData.NumberNorth == section.MissingPair;
                        bool newTableEWMissing = newTableStatus.RoundData.NumberEast == 0 || newTableStatus.RoundData.NumberEast == section.MissingPair;
                        if (section.TabletDevicesPerTable == 2 && (newTableNSMissing || newTableStatus.ReadyForNextRoundNorth) && (newTableEWMissing || newTableStatus.ReadyForNextRoundEast))
                        {
                            newTableNotReady = false;
                        }
                        else if (section.TabletDevicesPerTable == 4 && (newTableNSMissing || (newTableStatus.ReadyForNextRoundNorth && newTableStatus.ReadyForNextRoundSouth)) && (newTableEWMissing || (newTableStatus.ReadyForNextRoundEast && newTableStatus.ReadyForNextRoundWest)))
                        {
                            newTableNotReady = false;
                        }
                    }
                }
                if (newTableNotReady)  // Go back and wait
                {
                    return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber, tableNotReadyNumber = move.NewTableNumber });
                }

                if (newTableStatus != null && newTableStatus.RoundData.RoundNumber != newRoundNumber)
                {
                    // No device has yet updated the new table status, so do it now
                    // Use Round constructor to get player names, and reset table ready status
                    newTableStatus.RoundData = new Round(tabletDeviceStatus.SectionID, move.NewTableNumber, newRoundNumber);
                    newTableStatus.ReadyForNextRoundNorth = false;
                    newTableStatus.ReadyForNextRoundSouth = false;
                    newTableStatus.ReadyForNextRoundEast = false;
                    newTableStatus.ReadyForNextRoundWest = false;
                }
                tabletDeviceStatus.Update(section.SectionID, move.NewTableNumber, move.NewDirection, newRoundNumber);
            }
            else  // Tablet device not moving and is the only tablet device at this table
            {
                TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
                tableStatus.RoundData = new Round(tabletDeviceStatus.SectionID, tabletDeviceStatus.TableNumber, newRoundNumber);
                tabletDeviceStatus.RoundNumber = newRoundNumber;
            }

            // Refresh settings and hand records for the start of the round
            Settings.Refresh();
            if (Settings.ShowHandRecord || Settings.ValidateLeadCard)
            {
                HandRecords.Refresh();
            }
            return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber });
        }
    }
}