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
            if (tableStatus != null && tableStatus.RoundNumber < newRoundNumber)
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

            MovesList movesList = new MovesList(tabletDeviceNumber, tableStatus, newRoundNumber, tableNotReadyNumber);

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

            // Check if trying to advance to next round too quickly (ie before previous round is complete)
            int minRoundNumber = 999;
            foreach (TabletDeviceStatus iTabletDeviceStatus in AppData.TabletDeviceStatusList)
            {
                if (iTabletDeviceStatus.TableNumber > 0 && iTabletDeviceStatus.RoundNumber < minRoundNumber) minRoundNumber = iTabletDeviceStatus.RoundNumber;
            }
            if (newRoundNumber > minRoundNumber + 1)
            {
                return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber, tableNotReadyNumber = 0 });
            }

            Section section = AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID);
            if (section.TabletDevicesPerTable > 1)  // Tablet devices are moving, so need to check if new table is ready
            {
                // Get the move for this tablet device
                RoundsList roundsList = new RoundsList(tabletDeviceStatus.SectionID, newRoundNumber);
                Move move = roundsList.GetMove(tabletDeviceStatus.TableNumber, tabletDeviceStatus.PairNumber, tabletDeviceStatus.Direction);

                if (move.NewTableNumber == 0)  // Move is to phantom table, so go straight to RoundInfo
                {
                    tabletDeviceStatus.Update(0, "Sitout", newRoundNumber);
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                }

                // Check if move-to table is ready.  Expanded code here to make it easier to understand
                bool newTableReady;
                TableStatus newTableStatus = AppData.TableStatusList.Find(x => x.SectionID == section.SectionID && x.TableNumber == move.NewTableNumber);
                if (newTableStatus == null)
                {
                    newTableReady = false;  // New table not yet registered (unlikely but possible)
                }
                else if (newTableStatus.RoundNumber == newRoundNumber)
                {
                    newTableReady = true;  // Move-to table has already been advanced to next round by another tablet device, so is ready
                }
                else
                {
                    // A table is ready if all tablet device locations are ready.  Sitout locations were set to 'ready' previously 
                    if (section.TabletDevicesPerTable == 2 && newTableStatus.ReadyForNextRoundNorth && newTableStatus.ReadyForNextRoundEast)
                    {
                        newTableReady = true;
                    }
                    else if (section.TabletDevicesPerTable == 4 && newTableStatus.ReadyForNextRoundNorth && newTableStatus.ReadyForNextRoundSouth && newTableStatus.ReadyForNextRoundEast && newTableStatus.ReadyForNextRoundWest)
                    {
                        newTableReady = true;
                    }
                    else
                    {
                        newTableReady = false;
                    }
                }

                if (newTableReady)  // Reset tablet device and table statuses for new round
                {
                    tabletDeviceStatus.Update(move.NewTableNumber, move.NewDirection, newRoundNumber);
                    newTableStatus.Update(move.NewTableNumber, newRoundNumber);
                }
                else  // Go back and wait
                {
                    return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber, tableNotReadyNumber = move.NewTableNumber });
                }
            }
            else  // Tablet device not moving and is the only tablet device at this table
            {
                tabletDeviceStatus.RoundNumber = newRoundNumber;
                TableStatus newTableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
                newTableStatus.Update(tabletDeviceStatus.TableNumber, newRoundNumber);
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