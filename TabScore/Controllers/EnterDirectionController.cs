// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterDirectionController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, string direction = "", bool confirm = false) 
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            Round round = tableStatus.RoundData;
            EnterTableNumberDirection tableNumberData = new EnterTableNumberDirection
            {
                SectionID = sectionID,
                TableNumber = tableNumber,
                Direction = direction,
                Confirm = confirm
            };
            if (round.NumberNorth == 0 || round.NumberNorth == section.MissingPair)
            {
                tableNumberData.NorthMissing = true;
                tableNumberData.SouthMissing = true;
            }
            else if (round.NumberEast == 0 || round.NumberEast == section.MissingPair)
            {
                tableNumberData.EastMissing = true;
                tableNumberData.WestMissing = true;
            }
            if (section.TabletDevicesPerTable == 2)
            {
                tableNumberData.SouthMissing = true;
                tableNumberData.WestMissing = true;
            }

            ViewData["Title"] = $"Enter Direction - {section.SectionLetter}{tableNumber}";
            ViewData["Header"] = $"Table {section.SectionLetter}{tableNumber}";
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            return View("Index", tableNumberData);
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, string direction, bool confirm)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);

            // Check if tablet device is already registered for this location, and if so confirm
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber && x.Direction == direction);
            if (tabletDeviceStatus != null && !confirm)
            {
                return RedirectToAction("Index", "EnterDirection", new { sectionID, tableNumber, direction, confirm = true });
            }
            else if (tabletDeviceStatus == null)
            {
                // Not on list, so need to add it
                int pairNumber = 0;
                if (direction == "North")
                {
                    pairNumber = tableStatus.RoundData.NumberNorth;
                }
                else if (direction == "East")
                {
                    pairNumber = tableStatus.RoundData.NumberEast;
                }
                else if (direction == "South")
                {
                    pairNumber = tableStatus.RoundData.NumberSouth;
                }
                else if (direction == "West")
                {
                    pairNumber = tableStatus.RoundData.NumberWest;
                }
                tabletDeviceStatus = new TabletDeviceStatus(sectionID, tableNumber, direction, pairNumber, tableStatus.RoundData.RoundNumber);
                AppData.TabletDeviceStatusList.Add(tabletDeviceStatus);
            }

            // tabletDeviceNumber is the key for identifying this particular tablet device and is used throughout the rest of the application
            int tabletDeviceNumber = AppData.TabletDeviceStatusList.LastIndexOf(tabletDeviceStatus);

            if ((direction == "North" && tableStatus.ReadyForNextRoundNorth) || (direction == "South" && tableStatus.ReadyForNextRoundSouth) || (direction == "East" && tableStatus.ReadyForNextRoundEast) || (direction == "West" && tableStatus.ReadyForNextRoundWest))
            {
                return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = tableStatus.RoundData.RoundNumber + 1 });
            }
            else
            {
                return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber });
            }
        }
    }
}