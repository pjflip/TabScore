// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterDirectionController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, Direction direction = Direction.Null, bool confirm = false) 
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            EnterDirection enterDirection = new EnterDirection(section, tableNumber, direction, roundNumber, confirm);

            ViewData["Title"] = $"Enter Direction - {section.SectionLetter}{tableNumber}";
            ViewData["Header"] = $"Table {section.SectionLetter}{tableNumber}";
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            ViewData["TimerSeconds"] = -1;
            if (AppData.IsIndividual)
            {
                return View("Individual", enterDirection);
            }
            else
            {
                return View("Pair", enterDirection);
            }
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, Direction direction, int roundNumber, bool confirm)
        {
            // Check if tablet device is already registered for this location, and if so confirm
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber && x.Direction == direction);
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tabletDeviceStatus != null && !confirm)
            {
                return RedirectToAction("Index", "EnterDirection", new { sectionID, tableNumber, roundNumber, direction, confirm = true });
            }
            else if (tabletDeviceStatus == null)
            {
                // Not on list of registered tablet devices, so need to add it
                int pairNumber = 0;
                if (direction == Direction.North)
                {
                    pairNumber = tableStatus.RoundData.NumberNorth;
                }
                else if (direction == Direction.East)
                {
                    pairNumber = tableStatus.RoundData.NumberEast;
                }
                else if (direction == Direction.South)
                {
                    pairNumber = tableStatus.RoundData.NumberSouth;
                }
                else if (direction == Direction.West)
                {
                    pairNumber = tableStatus.RoundData.NumberWest;
                }
                tabletDeviceStatus = new TabletDeviceStatus(sectionID, tableNumber, direction, pairNumber, roundNumber);
                AppData.TabletDeviceStatusList.Add(tabletDeviceStatus);
            }

            // tabletDeviceNumber is the key for identifying this particular tablet device and is used throughout the rest of the application
            int tabletDeviceNumber = AppData.TabletDeviceStatusList.LastIndexOf(tabletDeviceStatus);

            if (((direction == Direction.North) && tableStatus.ReadyForNextRoundNorth) || ((direction == Direction.East) && tableStatus.ReadyForNextRoundEast) || (direction == Direction.South && tableStatus.ReadyForNextRoundSouth) || (direction == Direction.West && tableStatus.ReadyForNextRoundWest))
            {
                return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = roundNumber + 1 });
            }
            else
            {
                return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber });
            }
        }
    }
}