﻿// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;
using Resources;
using System.Web;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, int newRoundNumber, int tableNotReadyNumber = -1)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            if (newRoundNumber > Utilities.GetNumberOfRoundsInSection(tabletDeviceStatus.SectionID))  // Session complete
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
                if (tabletDeviceStatus.Direction == Direction.North)
                {
                    tableStatus.ReadyForNextRoundNorth = true;
                }
                else if (tabletDeviceStatus.Direction == Direction.East)
                {
                    tableStatus.ReadyForNextRoundEast = true;
                }
                else if (tabletDeviceStatus.Direction == Direction.South)
                {
                    tableStatus.ReadyForNextRoundSouth = true;
                }
                else if (tabletDeviceStatus.Direction == Direction.West)
                {
                    tableStatus.ReadyForNextRoundWest = true;
                }
            }

            MovesList movesList = new MovesList(tabletDeviceNumber, tableStatus, newRoundNumber, tableNotReadyNumber);

            ViewData["Header"] = $"{tabletDeviceStatus.Location}";
            ViewData["Title"] = $"{Strings.ShowMove} - {tabletDeviceStatus.Location}";
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);

            return View(movesList);
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber, int newRoundNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Section section = AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID);
            if (section.TabletDevicesPerTable > 1)  // Tablet devices are moving, so need to check if new table is ready
            {
                // Get the move for this tablet device
                RoundsList roundsList = new RoundsList(tabletDeviceStatus.SectionID, newRoundNumber);
                Move move = roundsList.GetMove(tabletDeviceStatus.TableNumber, tabletDeviceStatus.PairNumber, tabletDeviceStatus.Direction);

                if (move.NewTableNumber == 0)  // Move is to phantom table, so go straight to RoundInfo
                {
                    tabletDeviceStatus.Update(0, Direction.Sitout, newRoundNumber);
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                }

                // Check if the new table (the one we're trying to move to) is ready.  Expanded code here to make it easier to understand
                bool newTableReady;
                TableStatus newTableStatus = AppData.TableStatusList.Find(x => x.SectionID == section.SectionID && x.TableNumber == move.NewTableNumber);
                if (newTableStatus == null)
                {
                    newTableReady = false;  // New table not yet registered (unlikely but possible)
                }
                else if (newTableStatus.RoundNumber == newRoundNumber)
                {
                    newTableReady = true;  // New table has already been advanced to next round by another tablet device, so is ready
                }
                else if (newTableStatus.RoundNumber < newRoundNumber - 1)
                {
                    newTableReady = false;  // New table hasn't yet reached the previous round (unlikely but possible)
                }
                else
                {
                    // New table is on the previous round
                    // It is ready for the move if all tablet device locations are ready.  Sitout locations were set to 'ready' previously 
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

                if (newTableReady)  // Reset tablet device and table statuses for new round, and update cookie
                {
                    tabletDeviceStatus.Update(move.NewTableNumber, move.NewDirection, newRoundNumber);
                    newTableStatus.Update(move.NewTableNumber, newRoundNumber);
                    SetCookie(section.SectionID, move.NewTableNumber, move.NewDirection);
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
            return RedirectToAction("Index", "ShowPlayerIDs", new { tabletDeviceNumber });
        }

        // Set a cookie for this device
        private void SetCookie(int sectionID, int tableNumber, Direction direction)
        {
            HttpCookie tabScoreCookie = new HttpCookie("tabScore");
            tabScoreCookie.Values["sectionID"] = sectionID.ToString();
            tabScoreCookie.Values["tableNumber"] = tableNumber.ToString();
            tabScoreCookie.Values["direction"] = Enum.GetName(typeof(Direction), direction);
            Response.Cookies.Add(tabScoreCookie);
        }
    }
}